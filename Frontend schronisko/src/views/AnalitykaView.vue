<script setup lang="ts">
import Chart from 'primevue/chart'
import { ref, computed, onMounted } from 'vue'
import { useZwierzetaStore } from '@/stores/zwierzeta'

const store = useZwierzetaStore()
const MAKSYMALNA_POJEMNOSC = 100

// 1. Pobieranie danych tylko raz przy montowaniu
onMounted(async () => {
  if (store.zwierzeta.length === 0) {
    await store.pobierzZwierzeta()
  }
})

// 2. Dynamiczne dane wykresu (zawsze aktualne)
const chartData = computed(() => {
  const zajete = store.zwierzeta.length
  const wolne = Math.max(0, MAKSYMALNA_POJEMNOSC - zajete)

  return {
    labels: ['Zajęte miejsca', 'Wolne miejsca'],
    datasets: [
      {
        data: [zajete, wolne],
        backgroundColor: ['#22c55e', '#f3f4f6'],
        hoverBackgroundColor: ['#16a34a', '#e5e7eb'],
        borderWidth: 0,
        borderRadius: 2,
      },
    ],
  }
})

// 3. JEDNA, STATYCZNA KONFIGURACJA OPCJI (Bez wywoływania w onMounted!)
const chartOptions = ref({
  cutout: '70%',
  plugins: {
    legend: {
      position: 'bottom',
      labels: {
        usePointStyle: true,
        boxWidth: 14, // Kontroluje szerokość punktu (kropki)
        boxHeight: 14, // Kontroluje wysokość punktu (kropki)
        padding: 30, // Odstęp między wykresem a legendą
        color: '#1f2937', // Bardzo ciemny, wyraźny szary tekst
        font: {
          size: 13, // KULMINACYJNY PUNKT: Ustawione na 24px dla testu!
          weight: '500', // Pogrubienie tekstu
          family: 'sans-serif', // Wymuszenie czcionki systemowej
        },
      },
    },
    tooltip: {
      callbacks: {
        label: (context: { label: string; raw: number }) => {
          return ` ${context.label}: ${context.raw}`
        },
      },
    },
  },
})
</script>

<template>
  <div class="container mx-auto p-4 sm:p-6">
    <h1 class="text-3xl font-bold mb-2">Analityka</h1>
    <p class="text-gray-500 mb-8">Bieżące statystyki i pojemność schroniska.</p>

    <div class="flex justify-center">
      <div
        class="bg-white p-6 rounded-2xl shadow-sm border border-gray-100 flex flex-col items-center"
      >
        <h2 class="text-lg font-semibold mb-6 w-full text-left">
          Pojemność schroniska
        </h2>

        <div class="relative w-full flex justify-center">
          <Chart
            type="doughnut"
            :data="chartData"
            :options="chartOptions"
            class="w-full max-w-[20rem]"
          />

          <div
            class="absolute inset-0 flex flex-col items-center justify-center pointer-events-none -mt-8"
          >
            <span class="text-4xl font-bold text-gray-800">{{ store.zwierzeta.length }}</span>
            <span class="text-sm text-gray-500">/ {{ MAKSYMALNA_POJEMNOSC }}</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
