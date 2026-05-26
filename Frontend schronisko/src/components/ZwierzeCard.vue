<script setup lang="ts">
import type { Zwierze } from '@/types/zwierze'

// Kafelek po prostu przyjmuje obiekt Zwierze z zewnątrz
defineProps<{
  zwierze: Zwierze
}>()

// Domyślne zdjęcie, jeśli w bazie zdjecieUrl to null
const domyslneZdjecie = 'https://placehold.co/400x300?text=Brak+zdjęcia'
</script>

<template>
  <div class="card bg-base-100 w-72 shadow-xl hover:shadow-2xl transition-shadow">
    <figure>
      <img
        :src="zwierze.zdjecieUrl || domyslneZdjecie"
        :alt="`Zdjęcie ${zwierze.imie}`"
        class="h-48 w-full object-cover"
      />
    </figure>
    <div class="card-body">
      <h2 class="card-title text-2xl uppercase">
        {{ zwierze.imie }}
        <div v-if="zwierze.gatunek" class="badge badge-secondary">{{ zwierze.gatunek.nazwa }}</div>
      </h2>

      <p class="text-gray-500">
        Płeć: <span class="font-bold">{{ zwierze.plec }}</span>
      </p>

      <div class="card-actions justify-end mt-4">
        <div
          :class="['badge', zwierze.status === 'Do Adopcji' ? 'badge-success' : 'badge-warning']"
        >
          {{ zwierze.status }}
        </div>
      </div>
    </div>
  </div>
</template>
