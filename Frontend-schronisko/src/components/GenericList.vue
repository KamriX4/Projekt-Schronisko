<template>
  <!-- Kontener listy, renderuje wszystkie elementy lub slot pusty jeśli brak danych -->
  <div class="flex flex-wrap gap-6 justify-center">
    <template v-for="(element, index) in items" :key="index">
      <!-- Slot item otrzymuje pojedynczy element listy -->
      <slot name="item" :item="element"></slot>
    </template>

    <div v-if="items.length === 0" class="text-center text-gray-500 mt-8 w-full">
      <!-- Pokazuje treść alternatywną, jeśli lista jest pusta -->
      <slot name="empty">Brak danych do wyświetlenia.</slot>
    </div>
  </div>
</template>

<script setup lang="ts" generic="T">
// Generyczny komponent listy, przyjmuje dowolny typ elementów
defineProps<{
  items: T[]
}>()
</script>
