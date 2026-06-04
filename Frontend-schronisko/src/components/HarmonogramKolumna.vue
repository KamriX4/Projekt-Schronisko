<script setup lang="ts">
/**
 * [6] DRAG & DROP — kolumna harmonogramu z Vue Draggable (Sortable.js).
 * Przeciąganie kart zmienia kolejność zadań w danej kategorii (stan Pinia).
 */
import draggable from 'vuedraggable'
import type { ZadanieHarmonogramu } from '@/stores/harmonogram'
import { ZadanieKategoriaBadge } from '@/components/ZadanieKategoriaBadge'

const model = defineModel<ZadanieHarmonogramu[]>({ required: true })

defineProps<{
  tytulKolumny: string
  klasaNaglowka: string
  kategoria: string
  kolorAkcentu?: string
  formatujDate: (data: string) => string
  onPrzelacz: (id: number) => void
  onEdytuj: (zadanie: ZadanieHarmonogramu) => void
  onUsun: (id: number) => void
  etykietaWykonane?: string
  etykietaOznacz?: string
}>()
</script>

<template>
  <div class="bg-base-200 p-4 rounded-xl">
    <div class="flex items-center gap-2 mb-4 border-b-2 pb-2" :class="klasaNaglowka">
      <h2 class="text-xl font-bold">{{ tytulKolumny }}</h2>
      <!-- [9] komponent funkcyjny w kolumnie harmonogramu -->
      <ZadanieKategoriaBadge :kategoria="kategoria" :kolor-ramki="kolorAkcentu" />
    </div>

    <draggable
      v-model="model"
      item-key="id"
      class="min-h-[4rem]"
      handle=".drag-handle"
      animation="200"
      ghost-class="opacity-50"
    >
      <template #item="{ element: zadanie }">
        <div class="card bg-base-100 shadow-sm mb-3">
          <div class="card-body p-4">
            <div class="flex justify-between items-start gap-2">
              <h3
                class="card-title text-base flex-1"
                :class="{ 'line-through text-gray-400': zadanie.czyWykonane }"
              >
                {{ zadanie.tytul }}
              </h3>
              <span
                class="drag-handle cursor-grab active:cursor-grabbing text-gray-400 select-none px-1"
                title="Przeciągnij, aby zmienić kolejność"
                >⋮⋮</span
              >
            </div>
            <p v-if="zadanie.zwierze" class="text-sm font-bold text-primary mt-1">
              Dla: {{ zadanie.zwierze.imie }}
            </p>
            <p class="text-sm text-gray-500">{{ formatujDate(zadanie.dataCzas) }}</p>
            <p v-if="zadanie.opis" class="text-sm">{{ zadanie.opis }}</p>

            <div class="flex gap-2 mt-4 items-center">
              <button
                type="button"
                class="btn btn-xs flex-1"
                :class="zadanie.czyWykonane ? 'btn-success text-white' : 'btn-outline'"
                @click="onPrzelacz(zadanie.id)"
              >
                {{ zadanie.czyWykonane ? (etykietaWykonane ?? 'Zrobione') : (etykietaOznacz ?? 'Oznacz') }}
              </button>
              <button
                type="button"
                class="btn btn-xs btn-outline btn-info px-2"
                title="Edytuj"
                @click="onEdytuj(zadanie)"
              >
                ✏️
              </button>
              <button
                type="button"
                class="btn btn-xs btn-outline btn-error px-2"
                title="Usuń"
                @click="onUsun(zadanie.id)"
              >
                🗑️
              </button>
            </div>
          </div>
        </div>
      </template>
    </draggable>

    <p v-if="model.length === 0" class="text-sm text-gray-500">Brak zadań</p>
  </div>
</template>
