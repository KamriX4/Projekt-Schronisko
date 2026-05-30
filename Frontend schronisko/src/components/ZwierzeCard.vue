<script setup lang="ts">
import type { Zwierze } from '@/types/zwierze'
import Badge from 'primevue/badge'

// Kafelek po prostu przyjmuje obiekt Zwierze z zewnątrz
defineProps<{
  zwierze: Zwierze
}>()

// Domyślne zdjęcie, jeśli w bazie zdjecieUrl to null
const domyslneZdjecie = 'https://placehold.co/400x300?text=Brak+zdjęcia'
</script>

<template>
  <div class="card bg-base-100 w-96 h-96 shadow-lg hover:shadow-xl transition-shadow">
    <figure>
      <img
        :src="zwierze.zdjecieUrl || domyslneZdjecie"
        :alt="`Zdjęcie ${zwierze.imie}`"
        class="h-72 w-full object-cover"
      />
    </figure>
    <div class="card-body">
      <div class="flex justify-between items-center w-full">
        <div class="flex flex-col">
          <h2 class="card-title text-2xl uppercase font-semibold">
            {{ zwierze.imie }}
          </h2>
          <p class="text-gray-500">
            Gatunek: <span class="font-bold">{{ zwierze.gatunek?.nazwa }}</span>
          </p>
          <p class="text-gray-500">
            Płeć: <span class="font-bold">{{ zwierze.plec }}</span>
          </p>
        </div>
        <Badge
          size="xlarge"
          :severity="zwierze.status === 'Do Adopcji' ? 'success' : 'warning'"
          class="p-4 font-semibold shadow-sm"
        >
          {{ zwierze.status }}
        </Badge>
      </div>
    </div>
  </div>
</template>
