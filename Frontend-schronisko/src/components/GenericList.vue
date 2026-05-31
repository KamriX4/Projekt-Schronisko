<template>
  <div class="flex flex-wrap gap-6 justify-center">

    <template v-for="(element, index) in items" :key="index">
      <slot name="item" :item="element"></slot>
    </template>

    <div v-if="items.length === 0" class="text-center text-gray-500 mt-8 w-full">
      <slot name="empty">Brak danych do wyświetlenia.</slot>
    </div>

  </div>
</template>

<script setup lang="ts" generic="T">
// Magia Vue 3.3+: Deklarujemy, że komponent przyjmuje właściwość 'items',
// która jest tablicą dowolnego typu (T[])
defineProps<{
  items: T[]
}>()
</script>
