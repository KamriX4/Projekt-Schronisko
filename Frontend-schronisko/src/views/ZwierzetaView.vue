<script setup lang="ts">
import { ref, computed, onMounted, onUnmounted, watch } from 'vue' // Uporządkowane importy
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

// Inicjalizacja magazynów
const store = useZwierzetaStore()
const authStore = useAuthStore()

/** Zapamiętanie frazy wyszukiwania między wizytami */
const searchInput = useLocalStorage('schronisko-search', '')
const searchDebounced = ref(searchInput.value)

/** Opóźnienie filtrowania przy pisaniu (mniej przeliczeń) */
const aktualizujDebounced = useDebounceFn((wartosc: string) => {
  searchDebounced.value = wartosc
}, 300)

watch(searchInput, (v) => aktualizujDebounced(v), { immediate: true })

const czyModalOtwarty = ref(false)

const filtrowaneZwierzeta = computed(() => {
  let lista = store.zwierzeta

  // 1. Najpierw sprawdzamy wyszukiwarkę
  if (searchDebounced.value) {
    const wpisanyTekst = searchDebounced.value.toLowerCase()
    lista = lista.filter(z => z.imie.toLowerCase().includes(wpisanyTekst))
  }

  // 2. Potem nakładamy filtr statusów (dla gości)
  if (authStore.rola !== 'pracownik') {
    return lista.filter((z) => {
      // Jeśli status nie istnieje, pomijamy
      if (!z.status) return false

      // Zmieniamy status na same małe litery i ucinamy spacje po bokach
      const czystyStatus = z.status.toLowerCase().trim()

      return czystyStatus === 'do adopcji' ||
        czystyStatus === 'zarezerwowany' ||
        czystyStatus === 'adoptowano'
    })
  }

  // Pracownik widzi całą listę
  return lista
})

const obsluzDodanie = async (noweZwierze: NoweZwierze) => {
  const sukces = await store.dodajZwierze(noweZwierze)
  if (sukces) {
    czyModalOtwarty.value = false
    // Od razu pobieramy nową listę, by pracownik widział dodane zwierzę
    await store.pobierzZwierzeta()
  }
}

// Zmienna, w której trzymamy nasz "czasomierz"
let czasomierzOdswiezania: ReturnType<typeof setInterval>

onMounted(() => {
  // 1. Pobieramy listę od razu przy wejściu na stronę
  store.pobierzZwierzeta()

  // 2. Automatyczne odświeżanie w tle - co 5 sekund (5000 milisekund)
  // Odpytujemy serwer bez przeładowywania strony
  czasomierzOdswiezania = setInterval(() => {
    store.pobierzZwierzeta()
  }, 5000)
})

onUnmounted(() => {
  // Zatrzymujemy czasomierz, gdy użytkownik wyjdzie z zakładki "Zwierzęta",
  // aby nie obciążać przeglądarki i serwera w tle.
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
          <InputText v-model="searchInput"
                     v-focus
                     :placeholder="$t('animals.search')"
                     class="w-full" />
        </IconField>
        <Button v-if="authStore.rola === 'pracownik'"
                :label="$t('animals.add')"
                icon="pi pi-plus"
                severity="success"
                class="p-4 font-semibold shadow-lg"
                @click="czyModalOtwarty = true" />
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

    <AddZwierzeModal :otwarty="czyModalOtwarty"
                     @zamknij="czyModalOtwarty = false"
                     @zapisz="obsluzDodanie" />
  </div>
</template>
