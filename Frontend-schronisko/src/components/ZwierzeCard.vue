<script setup lang="ts">
import { computed, ref } from 'vue'
import type { Zwierze } from '@/types/zwierze'
import { useRouter } from 'vue-router'
import Badge from 'primevue/badge'
import Button from 'primevue/button'
import Dialog from 'primevue/dialog'
import FormularzAdopcyjny from './FormularzAdopcyjny.vue' //korzystanie z formularza
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
//Zdefiniowane właściwości wejściowe komponentów
const { zwierze } = defineProps<{
  zwierze: Zwierze
}>()

const domyslneZdjecie = 'https://placehold.co/400x300?text=Brak+zdjęcia'
const router = useRouter()

// Zmienna sterująca widocznością okienka z formularzem
const pokazFormularz = ref(false)

// Przejście do strony szczegółów zwierzaka po kliknięciu karty
const otworzSzczegoly = (id: number) => {
  router.push(`/zwierze/${id}`)
}

const gatunekLabel = computed(() => {
  const nazwa = zwierze.gatunek?.nazwa || ''
  return nazwa === 'Pies'
    ? t('animals.dog')
    : nazwa === 'Kot'
    ? t('animals.cat')
    : nazwa
})

const statusLabel = computed(() => {
  const status = zwierze.status?.toLowerCase().trim()
  if (!status) return ''

  if (status === 'do adopcji') return t('statuses.available')
  if (status === 'adoptowany') return t('statuses.adopted')
  if (status === 'w kwarantannie') return t('statuses.quarantined')
  if (status === 'zarezerwowany') return t('statuses.reserved')
  if (status === 'w leczeniu') return t('statuses.in_treatment')

  return zwierze.status

})

const plecLabel = computed(() => {
  const plec = zwierze.plec?.toLowerCase().trim()
  if (!plec) return ''
  return plec === 'samiec'
    ? t('animals.male')
    : plec === 'samica'
      ? t('animals.female')
      : zwierze.plec
})
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
            {{ $t('animals.species') }}: <span class="font-bold">{{ gatunekLabel }}</span>
          </p>
          <p class="text-gray-500">
            {{ $t('animals.gender') }}: <span class="font-bold">{{ plecLabel }}</span>
          </p>
        </div>
        <Badge
          size="xlarge"
          :severity="zwierze.status?.toLowerCase() === 'do adopcji' ? 'success' : 'warning'"
          class="p-4 font-semibold shadow-sm"
        >
          {{ statusLabel }}
        </Badge>
      </div>

      <div
        class="card-actions justify-end mt-4"
        v-if="zwierze.status?.toLowerCase() === 'do adopcji'"
      >
        <Button
          :label="t('statuses.adopt')"
          icon="pi pi-heart"
          severity="success"
          @click.stop="pokazFormularz = true"
        />
      </div>
    </div>
  </div>

  <!-- Modal z formularzem adopcyjnym, otwierany po kliknięciu przycisku Adoptuj -->
  <Dialog
    v-model:visible="pokazFormularz"
    modal
    :header="t('adoption_form.title')"
    :style="{ width: '90vw', maxWidth: '500px' }"
  >
    <FormularzAdopcyjny :zwierzeId="zwierze.id" />
  </Dialog>
</template>
