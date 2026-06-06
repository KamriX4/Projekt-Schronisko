<template>
  <!-- Modal pojawiający się na środku ekranu, renderowany do <body> dzięki Teleport -->
  <Teleport to="body">
    <div
      v-if="widoczny"
      class="fixed inset-0 z-[1000] flex items-center justify-center bg-black/40 backdrop-blur-sm"
    >
      <div
        class="bg-white p-6 rounded-2xl shadow-2xl max-w-sm w-full mx-4 transform transition-all"
      >
        <div class="flex items-center gap-3 mb-4">
          <div class="bg-green-100 p-2 rounded-full">
            <i class="pi pi-check text-green-600 text-xl"></i>
          </div>
          <h3 class="text-xl font-bold text-gray-800">{{ tytul }}</h3>
        </div>

        <p class="text-gray-600 mb-6 font-medium">
          <slot></slot>
        </p>

        <div class="flex justify-end">
          <button
            @click="$emit('zamknij')"
            class="bg-green-500 text-white px-5 py-2 rounded-lg font-semibold hover:bg-green-600 transition-colors"
          >
            Super!
          </button>
        </div>
      </div>
    </div>
  </Teleport>
</template>

<script setup lang="ts">
// Komponent przyjmuje flagę czy ma być widoczny oraz własny tytuł
defineProps<{
  widoczny: boolean
  tytul: string
}>()

// Informuje rodzica o kliknięciu przycisku zamknięcia
defineEmits(['zamknij'])
</script>
