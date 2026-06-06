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
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

// Generyczny formularz zwierzęcia używany do dodawania i edycji danych
const modelValue = defineModel<Partial<Zwierze>>({ required: true })
// Props: zewnętrzny stan tylko do odczytu lub edytowalny formularz
defineProps<{
  isReadonly: boolean
}>()

// Emits: Przesyłanie wybranego pliku wyżej
const emit = defineEmits<{
  (e: 'fileSelected', plik: File | null): void
}>()

// Lokalne słowniki opcji
const gatunki = [
  { id: 1, nazwa: t('animals.dog') },
  { id: 2, nazwa: t('animals.cat') },
]

const plec = [
  { id: 'Samiec', nazwa: t('animals.male') },
  { id: 'Samica', nazwa: t('animals.female') },
]

const statusy = [
  { id: 'Do Adopcji', nazwa: t('statuses.available') },
  { id: 'W kwarantannie', nazwa: t('statuses.quarantined') },
  { id: 'Adoptowany', nazwa: t('statuses.adopted') },
  { id: 'Zarezerwowany', nazwa: t('statuses.reserved') },
  { id: 'W leczeniu', nazwa: t('statuses.in_treatment') },
]

// Obsługa wczytywania zdjęcia i przekazywanie pliku w górę do rodzica
const onFileSelect = (event: FileUploadSelectEvent) => {
  const plik = event.files[0]
  if (!plik) return

  if (modelValue.value.zdjecieUrl && modelValue.value.zdjecieUrl.startsWith('blob:')) {
    URL.revokeObjectURL(modelValue.value.zdjecieUrl)
  }

  modelValue.value.zdjecieUrl = URL.createObjectURL(plik)

  emit('fileSelected', plik)
}

const onFileClear = () => {
  if (modelValue.value.zdjecieUrl && modelValue.value.zdjecieUrl.startsWith('blob:')) {
    URL.revokeObjectURL(modelValue.value.zdjecieUrl)
  }
  modelValue.value.zdjecieUrl = ''
  emit('fileSelected', null) // Informujemy, że anulowano plik
}

const dataUrodzeniaRef = computed(() => modelValue.value.przyblizonaDataUrodzenia)

const { wiekMiesiace } = useWiekZwierzecia(dataUrodzeniaRef)

// Stan błędów
const bledy = ref({
  imie: '',
  numerEwidencyjny: '',
  status: '',
  dataPrzyjecia: '',
  przyblizonaDataUrodzenia: '',
})

const walidujFormularz = () => {
  let czyPoprawny = true

  // Czyszczenie poprzednich komunikatów błędów
  bledy.value = {
    imie: '',
    numerEwidencyjny: '',
    status: '',
    dataPrzyjecia: '',
    przyblizonaDataUrodzenia: '',
  }

  // 1. Walidacja imienia
  if (!modelValue.value.imie || modelValue.value.imie.trim() === '') {
    bledy.value.imie = t('validation.name_required')
    czyPoprawny = false
  } else if (modelValue.value.imie.length < 2) {
    bledy.value.imie = t('validation.name_min_length')
    czyPoprawny = false
  }

  // 2. Walidacja numeru (np. by miał dokładny format z maski)
  if (!modelValue.value.numerEwidencyjny || modelValue.value.numerEwidencyjny.includes('_')) {
    bledy.value.numerEwidencyjny = t('validation.registry_number_required')
    czyPoprawny = false
  }

  // 3. Walidacja statusu
  if (!modelValue.value.status) {
    bledy.value.status = t('validation.status_required')
    czyPoprawny = false
  }

  // 4. Walidacja daty przyjęcia
  if (!modelValue.value.dataPrzyjecia) {
    bledy.value.dataPrzyjecia = t('validation.admission_date_required')
    czyPoprawny = false
  }

  // 5. Walidacja przybliżonej daty urodzenia
  if (!modelValue.value.przyblizonaDataUrodzenia) {
    bledy.value.przyblizonaDataUrodzenia = t('validation.birth_date_required')
    czyPoprawny = false
  }

  // 6. Walidacja daty urodzenia (wymagana + logika logiczna)
  if (!modelValue.value.przyblizonaDataUrodzenia) {
    bledy.value.przyblizonaDataUrodzenia = t('validation.birth_date_required')
    czyPoprawny = false
  } else {
    // Sprawdzanie logicznych zależności między datami
    const dataUr = new Date(modelValue.value.przyblizonaDataUrodzenia)
    const dzisiaj = new Date()
    const dataPrzyj = modelValue.value.dataPrzyjecia
      ? new Date(modelValue.value.dataPrzyjecia)
      : null

    if (dataUr > dzisiaj) {
      bledy.value.przyblizonaDataUrodzenia = t('validation.birth_date_future')
      czyPoprawny = false
    } else if (dataPrzyj && dataUr > dataPrzyj) {
      bledy.value.przyblizonaDataUrodzenia = t('validation.birth_date_after_admission')
      czyPoprawny = false
    }
  }

  return czyPoprawny
}

defineExpose({
  walidujFormularz,
})
</script>

<template>
  <div class="flex flex-col gap-1 mb-4">
    <div class="flex items-center gap-4 mb-4">
      <label for="imie" class="font-semibold w-36">{{ t('animals.name') }}</label>
      <InputText
        id="imie"
        v-model="modelValue.imie"
        required
        :disabled="isReadonly"
        :invalid="bledy.imie !== ''"
        @blur="walidujFormularz"
        class="flex-auto"
        autocomplete="off"
        :placeholder="t('animals.form.name_placeholder')"
      />
    </div>
    <small v-if="bledy.imie" class="text-red-500 font-medium ml-40 -mt-4 mb-4">{{
      bledy.imie
    }}</small>

    <div class="flex items-center gap-4 mb-4">
      <label for="gatunek" class="font-semibold w-36">{{ t('animals.species') }}</label>
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
      <label for="plec" class="font-semibold w-36">{{ t('animals.gender') }}</label>
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
      <label for="status" class="font-semibold w-36">{{ t('animals.status') }}</label>
      <Select
        id="status"
        v-model="modelValue.status"
        required
        :options="statusy"
        optionLabel="nazwa"
        optionValue="nazwa"
        :placeholder="t('animals.form.status_placeholder')"
        :disabled="isReadonly"
        :invalid="bledy.status !== ''"
        @blur="walidujFormularz"
        class="flex-auto"
      />
    </div>
    <small v-if="bledy.status" class="text-red-500 font-medium ml-40 -mt-4 mb-4">{{
      bledy.status
    }}</small>

    <div class="flex items-start gap-4 mb-4">
      <label class="font-semibold w-36 pt-2">{{ t('animals.form.photo') }}</label>

      <div class="flex-auto flex flex-col gap-4">
        <FileUpload
          v-if="!isReadonly"
          @select="onFileSelect"
          @clear="onFileClear"
          customUpload
          accept="image/*"
          :chooseLabel="t('animals.form.choose_photo')"
          :cancelLabel="t('animals.form.cancel')"
          :showUploadButton="false"
          severity="secondary"
          class="p-button-outlined"
        >
          <template #empty>
            <div
              class="flex items-center justify-center flex-col p-8 border-2 border-dashed border-gray-300 rounded-xl bg-gray-50 hover:bg-gray-100 transition-colors"
            >
              <i class="pi pi-cloud-upload text-5xl text-gray-400 mb-4"></i>
              <p class="m-0 text-gray-500 font-medium">{{ t('animals.form.drag_drop') }}</p>
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
      <label for="numerEwidencyjny" class="font-semibold w-36">{{
        t('animals.form.registry_number')
      }}</label>
      <InputMask
        id="numerEwidencyjny"
        v-model="modelValue.numerEwidencyjny"
        mask="a/9999/999"
        required
        :placeholder="t('animals.form.registry_number_placeholder')"
        :disabled="isReadonly"
        :invalid="bledy.numerEwidencyjny !== ''"
        @blur="walidujFormularz"
        class="flex-auto uppercase"
      />
    </div>
    <small v-if="bledy.numerEwidencyjny" class="text-red-500 font-medium ml-40 -mt-4 mb-4">{{
      bledy.numerEwidencyjny
    }}</small>

    <div class="flex items-center gap-4 mb-4">
      <label for="dataPrzyjecia" class="font-semibold w-36">{{
        t('animals.form.admission_date')
      }}</label>
      <DatePicker
        id="dataPrzyjecia"
        dateFormat="dd.mm.yy"
        required
        :model-value="modelValue.dataPrzyjecia ? new Date(modelValue.dataPrzyjecia) : null"
        @update:model-value="
          (val: any) => {
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

    <small v-if="bledy.dataPrzyjecia" class="text-red-500 font-medium ml-40 -mt-4 mb-4">{{
      bledy.dataPrzyjecia
    }}</small>

    <div class="flex items-center gap-4 mb-4">
      <label for="przyblizonaDataUrodzenia" class="font-semibold w-36">{{
        t('animals.form.birth_date')
      }}</label>
      <DatePicker
        id="przyblizonaDataUrodzenia"
        dateFormat="dd.mm.yy"
        required
        :model-value="
          modelValue.przyblizonaDataUrodzenia ? new Date(modelValue.przyblizonaDataUrodzenia) : null
        "
        @update:model-value="
          (val: any) => {
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

    <small
      v-if="bledy.przyblizonaDataUrodzenia"
      class="text-red-500 font-medium ml-40 -mt-4 mb-4"
      >{{ bledy.przyblizonaDataUrodzenia }}</small
    >

    <div class="flex items-center gap-4 mb-4">
      <label for="wiekMiesiace" class="font-semibold w-36">{{
        t('animals.form.age_months')
      }}</label>
      <InputText id="wiekMiesiace" :value="wiekMiesiace" disabled class="flex-auto" />
    </div>

    <div class="flex items-center gap-4 mb-4">
      <label for="czyZachipowany" class="font-semibold w-36">{{ t('animals.form.chipped') }}</label>
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
      <label for="czySzczepiony" class="font-semibold w-36">{{
        t('animals.form.vaccinated')
      }}</label>
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
      <label for="czyKastrowanySterylizowany" class="font-semibold w-36">{{
        t('animals.form.castrated')
      }}</label>
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
