<script setup lang="ts">
import { ref, onMounted } from 'vue'
import ZwierzeCard from '@/components/ZwierzeCard.vue'
import type { Zwierze } from '@/types/zwierze'

// 1. Zmienna do przechowywania listy zwierząt z bazy
const zwierzeta = ref<Zwierze[]>([])
const ladowanie = ref(true)
const blad = ref('')

// 2. Funkcja uderzająca do Twojego API C#
const pobierzZwierzeta = async () => {
  try {
    // UWAGA: Zmień port 7295 na ten, który pokazuje Ci Swagger!
    const odpowiedz = await fetch('https://localhost:7295/api/zwierze')

    if (!odpowiedz.ok) throw new Error('Błąd pobierania danych')

    zwierzeta.value = await odpowiedz.json()
  } catch (e) {
    blad.value = 'Nie udało się połączyć z bazą danych.'
    console.error(e)
  } finally {
    ladowanie.value = false
  }
}

// 3. Uruchamiamy pobieranie automatycznie po otwarciu strony
onMounted(() => {
  pobierzZwierzeta()
})
</script>

<template>
  <main class="container mx-auto p-4">
    <h1 class="text-3xl font-bold mb-6">Nasi podopieczni</h1>

    <div v-if="ladowanie" class="text-center py-10">
      <span class="loading loading-spinner loading-lg text-primary"></span>
      <p>Ładowanie zwierzaków z bazy...</p>
    </div>

    <div v-else-if="blad" class="alert alert-error">
      {{ blad }}
    </div>

    <div v-else class="flex flex-wrap gap-6 justify-center">
      <ZwierzeCard v-for="zwierzak in zwierzeta" :key="zwierzak.id" :zwierze="zwierzak" />
    </div>
  </main>
</template>
