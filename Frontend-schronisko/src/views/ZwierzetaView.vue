<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted, watch } from 'vue'
import { useDebounceFn, useLocalStorage } from '@vueuse/core'
import { useZwierzetaStore } from '@/stores/zwierzeta'
import { useAuthStore } from '@/stores/auth'
import ZwierzeCard from '@/components/ZwierzeCard.vue'
import AddZwierzeModal from '@/components/AddZwierzeModal.vue'
import type { NoweZwierze } from '@/types/zwierze'
import InputText from 'primevue/inputtext'
import IconField from 'primevue/iconfield'
import InputIcon from 'primevue/inputicon'
import Button from 'primevue/button'
import GenericList from '@/components/GenericList.vue'

// Inicjalizacja magazynów i lokalnego stanu wyszukiwania
const store = useZwierzetaStore()
const authStore = useAuthStore()

const searchInput = useLocalStorage('schronisko-search', '')
const searchDebounced = ref(searchInput.value)

/** Opóźnienie filtrowania przy pisaniu (mniej przeliczeń) */
const aktualizujDebounced = useDebounceFn((wartosc: string) => {
  searchDebounced.value = wartosc
}, 300)
// Za każdym razem, gdy użytkownik wpisze nową literę w polu wyszukiwania, watch to zauważy i wywoła aktualizujDebounced,
// która po 300ms zaktualizuje searchDebounced.
// Dzięki temu filtrowanie będzie się odbywać dopiero po chwili od ostatniego wpisu, co poprawia wydajność.
watch(searchInput, (v) => aktualizujDebounced(v), { immediate: true })

const czyModalOtwarty = ref(false)

// Lista zwierząt przefiltrowana wg wyszukiwania i roli użytkownika
const filtrowaneZwierzeta = computed(() => {
  let lista = store.zwierzeta

  if (searchDebounced.value) {
    const wpisanyTekst = searchDebounced.value.toLowerCase()
    lista = lista.filter((z) => z.imie.toLowerCase().includes(wpisanyTekst))
  }

  if (authStore.rola !== 'pracownik') {
    return lista.filter((z) => {
      if (!z.status) return false

      const czystyStatus = z.status.toLowerCase().trim()

      return (
        czystyStatus === 'do adopcji' ||
        czystyStatus === 'zarezerwowany' ||
        czystyStatus === 'adoptowano'
      )
    })
  }

  return lista
})

// Obsługa dodania nowego zwierzęcia i odświeżenie listy po sukcesie
const obsluzDodanie = async (noweZwierze: NoweZwierze) => {
  const sukces = await store.dodajZwierze(noweZwierze)
  if (sukces) {
    czyModalOtwarty.value = false
    await store.pobierzZwierzeta()
  }
}

let czasomierzOdswiezania: ReturnType<typeof setInterval>

// Ładowanie danych przy wejściu i okresowe odświeżanie listy
onMounted(() => {
  store.pobierzZwierzeta()

  czasomierzOdswiezania = setInterval(() => {
    store.pobierzZwierzeta()
  }, 5000)
})

onUnmounted(() => {
  clearInterval(czasomierzOdswiezania)
})
</script>

<template>
  <div class="container mx-auto p-6 sm:p-6">
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-6 mb-8">
      <h1 class="text-3xl font-bold">{{ $t('nav.animals') }}</h1>
      <div class="flex flex-row items-center gap-3 w-full sm:w-auto">
        <IconField class="flex-1">
          <InputIcon class="pi pi-search" />
          <InputText
            v-model="searchInput"
            :placeholder="$t('animals.search')"
            class="w-full"
          />
        </IconField>
        <Button
          v-if="authStore.rola === 'pracownik'"
          :label="$t('animals.add')"
          icon="pi pi-plus"
          severity="success"
          class="p-4 font-semibold shadow-lg"
          @click="czyModalOtwarty = true"
        />
      </div>
    </div>

    <GenericList :items="filtrowaneZwierzeta">
      <template #item="{ item }">
        <ZwierzeCard :zwierze="item" />
      </template>
      <template #empty>
        {{ $t('animals.notFound') }}
      </template>
    </GenericList>

    <AddZwierzeModal
      :otwarty="czyModalOtwarty"
      @zamknij="czyModalOtwarty = false"
      @zapisz="obsluzDodanie"
    />
  </div>
</template>
