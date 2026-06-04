<script setup lang="ts">
import Chart from 'primevue/chart'
import { ref, computed, onMounted } from 'vue'
import { useZwierzetaStore } from '@/stores/zwierzeta'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
const store = useZwierzetaStore()
const MAKSYMALNA_POJEMNOSC = 100

onMounted(async () => {
  if (store.zwierzeta.length === 0) {
    await store.pobierzZwierzeta()
  }
})

const chartData = computed(() => {
  const zajete = store.zwierzeta.length
  const wolne = Math.max(0, MAKSYMALNA_POJEMNOSC - zajete)

  return {
    labels: [t('analytics.occupied'), t('analytics.available')],
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

const chartOptions = ref({
  cutout: '70%',
  plugins: {
    legend: {
      position: 'bottom',
      labels: {
        usePointStyle: true,
        boxWidth: 14,
        boxHeight: 14,
        padding: 30,
        color: '#1f2937',
        font: {
          size: 13,
          weight: '500',
          family: 'sans-serif',
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
  <div class="relative w-full flex justify-center">
    <Chart type="doughnut" :data="chartData" :options="chartOptions" class="w-full max-w-[20rem]" />

    <div
      class="absolute inset-0 flex flex-col items-center justify-center pointer-events-none -mt-8"
    >
      <span class="text-4xl font-bold text-gray-800">{{ store.zwierzeta.length }}</span>
      <span class="text-sm text-gray-500">/ {{ MAKSYMALNA_POJEMNOSC }}</span>
    </div>
  </div>
</template>
