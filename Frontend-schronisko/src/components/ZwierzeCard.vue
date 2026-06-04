<script setup lang="ts">
import { ref } from 'vue'
import type { Zwierze } from '@/types/zwierze'
import { useRouter } from 'vue-router'
import Badge from 'primevue/badge'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import FormularzAdopcyjny from './FormularzAdopcyjny.vue'

// Kafelek przyjmuje obiekt Zwierze z zewnątrz
defineProps<{
  zwierze: Zwierze
}>()

// Domyślne zdjęcie
const domyslneZdjecie = 'https://placehold.co/400x300?text=Brak+zdjęcia'
const router = useRouter()

// Zmienna sterująca widocznością okienka z formularzem
const pokazFormularz = ref(false)

const otworzSzczegoly = (id: number) => {
  router.push(`/zwierze/${id}`)
}
</script>

<template>
  <div
    @click="otworzSzczegoly(zwierze.id)"
    class="card bg-base-100 w-96 shadow-lg hover:shadow-xl transition-shadow cursor-pointer"
  >
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
          :severity="zwierze.status?.toLowerCase() === 'do adopcji' ? 'success' : 'warning'"
          class="p-4 font-semibold shadow-sm"
        >
          {{ zwierze.status }}
        </Badge>
      </div>

      <div
        class="card-actions justify-end mt-4"
        v-if="zwierze.status?.toLowerCase() === 'do adopcji'"
      >
        <Button
          label="Adoptuj"
          icon="pi pi-heart"
          severity="success"
          @click.stop="pokazFormularz = true"
        />
      </div>
    </div>
  </div>

  <Dialog
    v-model:visible="pokazFormularz"
    modal
    header="Wypełnij Wniosek Adopcyjny"
    :style="{ width: '90vw', maxWidth: '500px' }"
  >
    <FormularzAdopcyjny :zwierzeId="zwierze.id" />
  </Dialog>
</template>
