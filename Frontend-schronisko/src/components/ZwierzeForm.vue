<script setup lang="ts">
import type { Zwierze } from '@/types/zwierze'
import type { FileUploadSelectEvent } from 'primevue/fileupload'
import { useWiekZwierzecia } from '@/composables/useWiekZwierzecia'
import { computed } from 'vue'
import { ref } from 'vue'
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

// 2. Wyciągasz datę do osobnej, reaktywnej zmiennej, żeby Composable mogło ją śledzić
const dataUrodzeniaRef = computed(() => modelValue.value.przyblizonaDataUrodzenia)
// 3. Destrukturyzujesz wynik z Composable!
const { wiekMiesiace } = useWiekZwierzecia(dataUrodzeniaRef)

// Stan błędów
const bledy = ref({
  imie: '',
  numerEwidencyjny: '',
  status: '',
  dataPrzyjecia: '',
  przyblizonaDataUrodzenia: ''
})

// Funkcja walidująca - możesz ją wywołać w komponencie-rodzicu
// lub przypiąć pod przycisk @click="zapisz"
const walidujFormularz = () => {
  let czyPoprawny = true

  // Czyszczenie starych błędów
  bledy.value = { imie: '', numerEwidencyjny: '', status: '', dataPrzyjecia: '', przyblizonaDataUrodzenia: '' }

  // 1. Walidacja imienia
  if (!modelValue.value.imie || modelValue.value.imie.trim() === '') {
    bledy.value.imie = 'Imię jest wymagane.'
    czyPoprawny = false
  } else if (modelValue.value.imie.length < 2) {
    bledy.value.imie = 'Imię musi mieć co najmniej 2 znaki.'
    czyPoprawny = false
  }

  // 2. Walidacja numeru (np. by miał dokładny format z maski)
  if (!modelValue.value.numerEwidencyjny || modelValue.value.numerEwidencyjny.includes('_')) {
    bledy.value.numerEwidencyjny = 'Podaj pełny numer ewidencyjny.'
    czyPoprawny = false
  }

  // 3. Walidacja statusu
  if (!modelValue.value.status) {
    bledy.value.status = 'Wybierz status.'
    czyPoprawny = false
  }

  // 4. Walidacja daty przyjęcia
  if (!modelValue.value.dataPrzyjecia) {
    bledy.value.dataPrzyjecia = 'Wybierz datę przyjęcia.'
    czyPoprawny = false
  }

  // 5. Walidacja przybliżonej daty urodzenia
  if (!modelValue.value.przyblizonaDataUrodzenia) {
    bledy.value.przyblizonaDataUrodzenia = 'Wybierz przybliżoną datę urodzenia.'
    czyPoprawny = false
  }

  // 6. Walidacja daty urodzenia (wymagana + logika logiczna)
  if (!modelValue.value.przyblizonaDataUrodzenia) {
    bledy.value.przyblizonaDataUrodzenia = 'Data urodzenia jest wymagana.'
    czyPoprawny = false
  } else {
    // Sprawdzanie logicznych zależności między datami
    const dataUr = new Date(modelValue.value.przyblizonaDataUrodzenia)
    const dzisiaj = new Date()
    const dataPrzyj = modelValue.value.dataPrzyjecia ? new Date(modelValue.value.dataPrzyjecia) : null

    if (dataUr > dzisiaj) {
      bledy.value.przyblizonaDataUrodzenia = 'Data urodzenia nie może być z przyszłości.'
      czyPoprawny = false
    } else if (dataPrzyj && dataUr > dataPrzyj) {
      bledy.value.przyblizonaDataUrodzenia = 'Zwierzak nie mógł urodzić się po dacie przyjęcia.'
      czyPoprawny = false
    }
  }

  return czyPoprawny
}

// Udostępniamy funkcję na zewnątrz (jeśli przycisk Zapisu jest w innym pliku)
defineExpose({
  walidujFormularz
})




</script>

<template>
  <div class="flex flex-col gap-1 mb-4">
    <div class="flex items-center gap-4 mb-4">
      <label for="imie" class="font-semibold w-36">Imię</label>
      <InputText
        id="imie"
        v-model="modelValue.imie"
        required
        :disabled="isReadonly"
        :invalid="bledy.imie !== ''"
        @blur="walidujFormularz"
        class="flex-auto"
        autocomplete="off"
        placeholder="Wpisz imię..."
      />
    </div>
    <small v-if="bledy.imie" class="text-red-500 font-medium ml-40 -mt-4 mb-4">{{ bledy.imie }}</small>



    <div class="flex items-center gap-4 mb-4">
      <label for="gatunek" class="font-semibold w-36">Gatunek</label>
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
      <label for="plec" class="font-semibold w-36">Płeć</label>
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
      <label for="status" class="font-semibold w-36">Status</label>
      <Select
        id="status"
        v-model="modelValue.status"
        required
        :options="statusy"
        optionLabel="nazwa"
        optionValue="nazwa"
        placeholder="Wybierz status..."
        :disabled="isReadonly"
        :invalid="bledy.status !== ''"
        @blur="walidujFormularz"
        class="flex-auto"
      />
    </div>
    <small v-if="bledy.status" class="text-red-500 font-medium ml-40 -mt-4 mb-4">{{ bledy.status }}</small>

    <div class="flex items-start gap-4 mb-4">
      <label class="font-semibold w-36 pt-2">Zdjęcie</label>

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
      <label for="numerEwidencyjny" class="font-semibold w-36">Numer ewidencyjny</label>
      <InputMask
        id="numerEwidencyjny"
        v-model="modelValue.numerEwidencyjny"
        mask="a/9999/999"
        required
        placeholder="P/2026/001"
        :disabled="isReadonly"
        :invalid="bledy.numerEwidencyjny !== ''"
        @blur="walidujFormularz"
        class="flex-auto uppercase"
      />
    </div>
    <small v-if="bledy.numerEwidencyjny" class="text-red-500 font-medium ml-40 -mt-4 mb-4">{{ bledy.numerEwidencyjny }}</small>

    <div class="flex items-center gap-4 mb-4">
      <label for="dataPrzyjecia" class="font-semibold w-36">Data przyjęcia</label>
      <DatePicker
        id="dataPrzyjecia"
        dateFormat="dd.mm.yy"
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
        :invalid="bledy.dataPrzyjecia !== ''"
        @blur="walidujFormularz"
        class="flex-auto"
      />
    </div>

    <small v-if="bledy.dataPrzyjecia" class="text-red-500 font-medium ml-40 -mt-4 mb-4">{{ bledy.dataPrzyjecia }}</small>

    <div class="flex items-center gap-4 mb-4">
      <label for="przyblizonaDataUrodzenia" class="font-semibold w-36">Data urodzenia</label>
      <DatePicker
        id="przyblizonaDataUrodzenia"
        dateFormat="dd.mm.yy"
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
        :invalid="bledy.przyblizonaDataUrodzenia !== ''"
        @blur="walidujFormularz"
        class="flex-auto"
      />
    </div>

    <small v-if="bledy.przyblizonaDataUrodzenia" class="text-red-500 font-medium ml-40 -mt-4 mb-4">{{ bledy.przyblizonaDataUrodzenia }}</small>

    <div class="flex items-center gap-4 mb-4">
      <label for="wiekMiesiace" class="font-semibold w-36">Wiek w miesiącach</label>
      <InputText id="wiekMiesiace" :value="wiekMiesiace" disabled class="flex-auto" />
    </div>

    <div class="flex items-center gap-4 mb-4">
      <label for="czyZachipowany" class="font-semibold w-36">Czy zachipowany?</label>
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
      <label for="czySzczepiony" class="font-semibold w-36">Czy szczepiony?</label>
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
      <label for="czyKastrowanySterylizowany" class="font-semibold w-36">Czy kastrowany?</label>
      <Checkbox
        id="czyKastrowanySterylizowany"
        v-model="modelValue.czyKastrowanySterylizowany"
        :binary="true"
        size="large"
        :disabled="isReadonly"
        class="flex-auto"
      />
    </div>
  </div>

</template>
