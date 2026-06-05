<script setup lang="ts">
import { defineAsyncComponent } from 'vue'

// Komponent zostanie pobrany z sieci przez przeglądarkę dopiero, gdy ten widok się zamontuje
const PojemnoscChart = defineAsyncComponent(() => import('@/components/PojemnoscChart.vue'))
</script>

<template>
  <div class="container mx-auto p-4 sm:p-6">
    <h1 class="text-3xl font-bold mb-2">{{ $t('nav.analytics') }}</h1>

    <p class="text-gray-500 mb-8">{{ $t('analytics.subtitle') }}</p>

    <div class="flex justify-center">
      <div
        class="bg-white p-6 rounded-2xl shadow-sm border border-gray-100 flex flex-col items-center w-full max-w-md"
      >
        <h2 class="text-lg font-semibold mb-6 w-full text-left">{{ $t('analytics.capacity') }}</h2>

        <PojemnoscChart />
      </div>
    </div>
  </div>
</template>
