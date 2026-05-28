<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useZwierzetaStore } from '@/stores/zwierzeta'
import ZwierzeCard from '@/components/ZwierzeCard.vue'
import AddZwierzeModal from '@/components/AddZwierzeModal.vue'
import type { NoweZwierze } from '@/types/zwierze'

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
  <div class="container mx-auto p-6">
    <div class="flex flex-col md:flex-row justify-between items-center gap-4 mb-8">
      <h1 class="text-3xl font-bold">Nasi podopieczni</h1>

      <div class="flex items-center gap-4 w-full md:w-auto">
        <div class="form-control w-full md:w-80">
          <input
            v-model="searchInput"
            type="text"
            placeholder="Szukaj zwierzaka..."
            class="input input-bordered w-full p-4 shadow-lg focus:input-primary"
          />
        </div>

        <button @click="czyModalOtwarty = true" class="btn bg-success p-4 font-semibold shadow-lg">
          Dodaj
        </button>
      </div>
    </div>

    <div class="flex flex-wrap gap-6 justify-center">
      <ZwierzeCard v-for="zwierze in filtrowaneZwierzeta" :key="zwierze.id" :zwierze="zwierze" />

      <div v-if="filtrowaneZwierzeta.length === 0" class="text-center text-gray-500 mt-8 w-full">
        Nie znaleziono zwierząt spełniających kryteria wyszukiwania.
      </div>
    </div>

    <AddZwierzeModal
      :otwarty="czyModalOtwarty"
      @zamknij="czyModalOtwarty = false"
      @zapisz="obsluzDodanie"
    />
  </div>
</template>
