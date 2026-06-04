<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useZwierzetaStore } from '@/stores/zwierzeta'
import ZwierzeCard from '@/components/ZwierzeCard.vue'
import AddZwierzeModal from '@/components/AddZwierzeModal.vue'
import type { NoweZwierze } from '@/types/zwierze'
import InputText from 'primevue/inputtext'
import IconField from 'primevue/iconfield'
import InputIcon from 'primevue/inputicon'
import Button from 'primevue/button'
import GenericList from '@/components/GenericList.vue'

// Inicjalizacja Store'a
const store = useZwierzetaStore()

// Lokalny stan UI
const searchInput = ref('')
const czyModalOtwarty = ref(false)

// Obliczana logika wyszukiwarki (korzysta ze Store'a)
const filtrowaneZwierzeta = computed(() => {
  const query = searchInput.value.toLowerCase().trim()
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
          <InputText v-model="searchInput" :placeholder="$t('animals.search')" class="w-full" />
        </IconField>
        <Button
          :label="$t('animals.add')"
          icon="pi pi-plus"
          severity="success"
          @click="czyModalOtwarty = true"
          class="p-4 font-semibold shadow-lg"
        />
      </div>
    </div>

    <GenericList :items="filtrowaneZwierzeta">
      <template #item="{ item }">
        <ZwierzeCard :zwierze="item" />
      </template>
      <template #empty> {{ $t('animals.notFound') }} </template>
    </GenericList>

    <AddZwierzeModal
      :otwarty="czyModalOtwarty"
      @zamknij="czyModalOtwarty = false"
      @zapisz="obsluzDodanie"
    />
  </div>
</template>
