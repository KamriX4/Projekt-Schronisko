<script setup lang="ts">
import type { Pet } from '@/types/pet'
const _props = defineProps<{ pet: Pet }>()
</script>
<template>
  <div v-if="pet.Level" class="card bg-base-100 w-48 shadow-sm">
    <h2 class="text-center text-xl uppercase">{{ pet.Name }}</h2>
    <img :src="`assets/${pet.Picture}`" />
    <div class="card-body">
      <p :class="[pet.Level < 3 ? 'text-red-500' : pet.Level > 3 ? 'text-green-500' : '']">
        Poziom: {{ pet.Level }}
      </p>
      <p v-if="pet.Description">Opis: {{ pet.Description }}</p>
    </div>
  </div>
</template>
<style scoped></style>
