<template>
  <!-- Zawijacz karciany: nagłówek, treść i stopka jako sloty -->
  <div class="bg-white p-6 sm:p-8 rounded-xl shadow-lg border border-gray-100">
    <!-- Opcjonalny nagłówek karty -->
    <div v-if="$slots.header" class="border-b border-gray-100 pb-4 mb-6">
      <slot name="header"></slot>
    </div>

    <!-- Główna zawartość karty -->
    <div>
      <slot></slot>
    </div>

    <!-- Opcjonalna stopka karty -->
    <div v-if="$slots.footer" class="mt-10 flex justify-end gap-3 border-t border-gray-100 pt-6">
      <slot name="footer"></slot>
    </div>
  </div>
</template>

<script setup lang="ts">
// Brak logiki skryptowej; komponent służy jako kontener stylizowany
</script>
