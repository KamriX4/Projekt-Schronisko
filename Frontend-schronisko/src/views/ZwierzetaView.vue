<script setup lang="ts">
import { ref, computed, onMounted, watch } from 'vue'
import { useDebounceFn, useLocalStorage } from '@vueuse/core'
import { useZwierzetaStore } from '@/stores/zwierzeta'

//Importujemy magazyn pamięci, żeby sprawdzić, kto jest zalogowany
import { useAuthStore } from '@/stores/auth'

import ZwierzeCard from '@/components/ZwierzeCard.vue'
import AddZwierzeModal from '@/components/AddZwierzeModal.vue'
import type { NoweZwierze } from '@/types/zwierze'
import InputText from 'primevue/inputtext'
import IconField from 'primevue/iconfield'
import InputIcon from 'primevue/inputicon'
import Button from 'primevue/button'
import GenericList from '@/components/GenericList.vue'

// Inicjalizacja Store'a (magazynu z danymi o zwierzętach)
const store = useZwierzetaStore()

// DODANE: Uruchamiamy magazyn pamięci logowania
const authStore = useAuthStore()

/** [7] VUEUSE — useLocalStorage: zapamiętanie frazy wyszukiwania między wizytami */
const searchInput = useLocalStorage('schronisko-search', '')
const searchDebounced = ref(searchInput.value)

/** [7] VUEUSE — useDebounceFn: opóźnienie filtrowania przy pisaniu (mniej przeliczeń) */
const aktualizujDebounced = useDebounceFn((wartosc: string) => {
  searchDebounced.value = wartosc
}, 300)

watch(searchInput, (v) => aktualizujDebounced(v), { immediate: true })

const czyModalOtwarty = ref(false)

const filtrowaneZwierzeta = computed(() => {
  const query = searchDebounced.value.toLowerCase().trim()
  if (!query) return store.zwierzeta

  return store.zwierzeta.filter((z) => {
    return (
      z.imie?.toLowerCase().includes(query) ||
      z.gatunek?.nazwa?.toLowerCase().includes(query) ||
      z.plec?.toLowerCase().includes(query)
    )
  })
})

const obsluzDodanie = async (noweZwierze: NoweZwierze) => {
  const sukces = await store.dodajZwierze(noweZwierze)
  if (sukces) {
    czyModalOtwarty.value = false
  }
}

onMounted(() => {
  store.pobierzZwierzeta()
})
</script>

<template>
  <div class="container mx-auto p-6 sm:p-6">
    <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-6 mb-8">
      <h1 class="text-3xl font-bold">{{ $t('nav.animals') }}</h1>
      <div class="flex flex-row items-center gap-3 w-full sm:w-auto">
        <IconField class="flex-1">
          <InputIcon class="pi pi-search" />
          <!-- [4] WŁASNA DYREKTYWA v-focus — fokus na polu wyszukiwania po wejściu na widok -->
          <InputText
            v-model="searchInput"
            v-focus
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
      <template #empty> {{ $t('animals.notFound') }} </template>
    </GenericList>

    <AddZwierzeModal :otwarty="czyModalOtwarty"
                     @zamknij="czyModalOtwarty = false"
                     @zapisz="obsluzDodanie" />
  </div>
</template>
