<script setup lang="ts">
import type { Zwierze } from '@/types/zwierze'
import type { FileUploadSelectEvent } from 'primevue/fileupload'

import InputText from 'primevue/inputtext'
import SelectButton from 'primevue/selectbutton'
import Select from 'primevue/select'
import FileUpload from 'primevue/fileupload'
import InputMask from 'primevue/inputmask'
import DatePicker from 'primevue/datepicker'
import Checkbox from 'primevue/checkbox'

const modelValue = defineModel<Partial<Zwierze>>({ required: true })
// Props: Dane z zewnątrz i flaga blokady
defineProps<{
  isReadonly: boolean
}>()

// Emits: Przesyłanie wybranego pliku wyżej
const emit = defineEmits<{
  (e: 'fileSelected', plik: File | null): void
}>()

// Lokalne słowniki opcji
const gatunki = [
  { id: 1, nazwa: 'Pies' },
  { id: 2, nazwa: 'Kot' },
]

const plec = [
  { id: 'Samiec', nazwa: 'Samiec' },
  { id: 'Samica', nazwa: 'Samica' },
]

const statusy = [
  { id: 'Do Adopcji', nazwa: 'Do Adopcji' },
  { id: 'W kwarantannie', nazwa: 'W kwarantannie' },
  { id: 'Adoptowany', nazwa: 'Adoptowany' },
  { id: 'Zarezerwowany', nazwa: 'Zarezerwowany' },
  { id: 'W leczeniu', nazwa: 'W leczeniu' },
]

// Obsługa plików wizualna + przekazanie do rodzica
const onFileSelect = (event: FileUploadSelectEvent) => {
  const plik = event.files[0]
  if (!plik) return

  if (modelValue.value.zdjecieUrl && modelValue.value.zdjecieUrl.startsWith('blob:')) {
    URL.revokeObjectURL(modelValue.value.zdjecieUrl)
  }

  // 2. Bezpośrednie, w 100% legalne przypisanie
  modelValue.value.zdjecieUrl = URL.createObjectURL(plik)

  // 3. Wysyłamy fizyczny plik wyżej
  emit('fileSelected', plik)
}

const onFileClear = () => {
  if (modelValue.value.zdjecieUrl && modelValue.value.zdjecieUrl.startsWith('blob:')) {
    URL.revokeObjectURL(modelValue.value.zdjecieUrl)
  }
  modelValue.value.zdjecieUrl = ''
  emit('fileSelected', null) // Informujemy, że anulowano plik
}

</script>

<template>
  <div class="flex items-center gap-4 mb-4">
    <label for="imie" class="font-semibold w-24">Imię</label>
    <InputText
      id="imie"
      v-model="modelValue.imie"
      required
      :disabled="isReadonly"
      class="flex-auto"
      autocomplete="off"
      placeholder="Wpisz imię..."
    />
  </div>

  <div class="flex items-center gap-4 mb-4">
    <label for="gatunek" class="font-semibold w-24">Gatunek</label>
    <SelectButton
      id="gatunek"
      v-model="modelValue.gatunekId"
      :options="gatunki"
      optionLabel="nazwa"
      optionValue="id"
      :disabled="isReadonly"
      class="flex-auto"
    />
  </div>

  <div class="flex items-center gap-4 mb-4">
    <label for="plec" class="font-semibold w-24">Płeć</label>
    <SelectButton
      id="plec"
      v-model="modelValue.plec"
      :options="plec"
      optionLabel="nazwa"
      optionValue="nazwa"
      :disabled="isReadonly"
      class="flex-auto"
    />
  </div>
  <div class="flex items-center gap-4 mb-4">
    <label for="status" class="font-semibold w-24">Status</label>
    <Select
      id="status"
      v-model="modelValue.status"
      required
      :options="statusy"
      optionLabel="nazwa"
      optionValue="nazwa"
      placeholder="Wybierz status..."
      :disabled="isReadonly"
      class="flex-auto"
    />
  </div>

  <div class="flex items-start gap-4 mb-4">
    <label class="font-semibold w-24 pt-2">Zdjęcie</label>

    <div class="flex-auto flex flex-col gap-4">
      <FileUpload
        v-if="!isReadonly"
        @select="onFileSelect"
        @clear="onFileClear"
        customUpload
        accept="image/*"
        chooseLabel="Wybierz zdjęcie"
        cancelLabel="Anuluj"
        :showUploadButton="false"
        severity="secondary"
        class="p-button-outlined"
      >
        <template #empty>
          <div
            class="flex items-center justify-center flex-col p-8 border-2 border-dashed border-gray-300 rounded-xl bg-gray-50 hover:bg-gray-100 transition-colors"
          >
            <i class="pi pi-cloud-upload text-5xl text-gray-400 mb-4"></i>
            <p class="m-0 text-gray-500 font-medium">Przeciągnij i upuść zdjęcie tutaj</p>
          </div>
        </template>

        <template #content>
          <div class="hidden"></div>
        </template>
      </FileUpload>

      <div v-if="modelValue.zdjecieUrl" class="flex justify-center">
        <img
          :src="modelValue.zdjecieUrl"
          alt="Podgląd zdjęcia"
          class="shadow-md rounded-xl w-full sm:w-72 object-cover"
        />
      </div>
    </div>
  </div>

  <div class="flex items-center gap-4 mb-4">
    <label for="numerEwidencyjny" class="font-semibold w-24">Numer ewidencyjny</label>
    <InputMask
      id="numerEwidencyjny"
      v-model="modelValue.numerEwidencyjny"
      mask="a/9999/999"
      required
      placeholder="P/2026/001"
      :disabled="isReadonly"
      class="flex-auto uppercase"
    />
  </div>

  <div class="flex items-center gap-4 mb-4">
    <label for="dataPrzyjecia" class="font-semibold w-24">Data przyjęcia</label>
    <DatePicker
      id="dataPrzyjecia"
      required
      :model-value="modelValue.dataPrzyjecia ? new Date(modelValue.dataPrzyjecia) : null"
      @update:model-value="
        (val: any) => {
          // Bezpiecznie sprawdzamy co przyszło, niezależnie od tego co mówi TS
          if (Array.isArray(val)) {
            modelValue.dataPrzyjecia = val[0] || null
          } else {
            modelValue.dataPrzyjecia = val
          }
        }
      "
      showIcon
      iconDisplay="input"
      :disabled="isReadonly"
      class="flex-auto"
    />
  </div>

  <div class="flex items-center gap-4 mb-4">
    <label for="przyblizonaDataUrodzenia" class="font-semibold w-24">Data urodzenia</label>
    <DatePicker
      id="przyblizonaDataUrodzenia"
      required
      :model-value="
        modelValue.przyblizonaDataUrodzenia ? new Date(modelValue.przyblizonaDataUrodzenia) : null
      "
      @update:model-value="
        (val: any) => {
          // Bezpiecznie sprawdzamy co przyszło, niezależnie od tego co mówi TS
          if (Array.isArray(val)) {
            modelValue.przyblizonaDataUrodzenia = val[0] || null
          } else {
            modelValue.przyblizonaDataUrodzenia = val
          }
        }
      "
      showIcon
      iconDisplay="input"
      :disabled="isReadonly"
      class="flex-auto"
    />
  </div>

  <div class="flex items-center gap-4 mb-4">
    <label for="czyZachipowany" class="font-semibold w-24">Czy zachipowany?</label>
    <Checkbox
      id="czyZachipowany"
      v-model="modelValue.czyZachipowany"
      :binary="true"
      size="large"
      :disabled="isReadonly"
      class="flex-auto"
    />
  </div>

  <div class="flex items-center gap-4 mb-4">
    <label for="czySzczepiony" class="font-semibold w-24">Czy szczepiony?</label>
    <Checkbox
      id="czySzczepiony"
      v-model="modelValue.czySzczepiony"
      :binary="true"
      size="large"
      :disabled="isReadonly"
      class="flex-auto"
    />
  </div>

  <div class="flex items-center gap-4 mb-4">
    <label for="czyKastrowanySterylizowany" class="font-semibold w-24">Czy kastrowany?</label>
    <Checkbox
      id="czyKastrowanySterylizowany"
      v-model="modelValue.czyKastrowanySterylizowany"
      :binary="true"
      size="large"
      :disabled="isReadonly"
      class="flex-auto"
    />
  </div>
</template>
