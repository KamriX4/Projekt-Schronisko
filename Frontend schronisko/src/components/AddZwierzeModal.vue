<script setup lang="ts">
import { ref, watch } from 'vue'
import type { NoweZwierze } from '@/types/zwierze'
import { uploadZdjecia } from '@/stores/pliki'
import Dialog from 'primevue/dialog'
import InputText from 'primevue/inputtext'
import Button from 'primevue/button'
// import InputNumber from 'primevue/inputnumber'
import SelectButton from 'primevue/selectbutton'
import Select from 'primevue/select'
import FileUpload from 'primevue/fileupload'
import InputMask from 'primevue/inputmask'
import DatePicker from 'primevue/datepicker'
import Checkbox from 'primevue/checkbox'

// Funkcja obsługująca Twój event @select
const onFileSelect = async (event: { files: File[] }) => {
  const fizycznyPlik = event.files[0]
  if (!fizycznyPlik) return

  try {
    // 1. Wysyłamy plik na backend
    const wygenerowanyUrl = await uploadZdjecia(fizycznyPlik)

    // 2. Przypisujemy wygenerowany link do formularza
    // Dzięki temu aktywuje się v-if="formularzZwierze.zdjecieUrl" i pokaże się obrazek!
    formularzZwierze.value.zdjecieUrl = wygenerowanyUrl
  } catch (error) {
    console.error(error)
    alert('Wystąpił błąd podczas wgrywania zdjęcia.')
  }
}

const props = defineProps<{
  otwarty: boolean
}>()

const emit = defineEmits<{
  (e: 'zamknij'): void
  (e: 'zapisz', noweZwierze: NoweZwierze): void // Tu możesz użyć Partial<Zwierze> docelowo
}>()

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

const domyslnyStan = (): NoweZwierze => ({
  imie: '',
  gatunekId: 1,
  plec: 'Samiec',
  status: '',
  zdjecieUrl: '',
  numerEwidencyjny: '',
  dataPrzyjecia: null,
  przyblizonaDataUrodzenia: null,
  wiekMiesiace: 0,
  czyZachipowany: false,
  czySzczepiony: false,
  czyKastrowanySterylizowany: false,
})

const formularzZwierze = ref<NoweZwierze>(domyslnyStan())

watch(
  () => props.otwarty,
  (czyOtwarty) => {
    if (czyOtwarty) formularzZwierze.value = domyslnyStan()
  },
)

const handleDodaj = () => {
  if (!formularzZwierze.value.imie) return alert('Imię jest wymagane!')
  if (formularzZwierze.value.gatunekId === 0) return alert('Wybierz gatunek!') // <--- Dodane zabezpieczenie
  if (formularzZwierze.value.numerEwidencyjny) {
    formularzZwierze.value.numerEwidencyjny = formularzZwierze.value.numerEwidencyjny.toUpperCase()
  }
  emit('zapisz', formularzZwierze.value)
}
</script>

<template>
  <Dialog
    :visible="otwarty"
    @update:visible="emit('zamknij')"
    modal
    header="Dodaj nowego zwierzaka"
    :style="{ width: '30rem' }"
  >
    <span class="text-surface-500 dark:text-surface-400 block mb-8">
      Wypełnij dane, aby dodać podopiecznego do bazy.
    </span>

    <div class="flex items-center gap-4 mb-4">
      <label for="imie" class="font-semibold w-24">Imię</label>
      <InputText
        id="imie"
        v-model="formularzZwierze.imie"
        class="flex-auto"
        autocomplete="off"
        placeholder="Wpisz imię..."
      />
    </div>

    <div class="flex items-center gap-4 mb-4">
      <label for="gatunek" class="font-semibold w-24">Gatunek</label>
      <SelectButton
        id="gatunek"
        v-model="formularzZwierze.gatunekId"
        :options="gatunki"
        optionLabel="nazwa"
        optionValue="id"
        class="flex-auto"
      />
    </div>

    <div class="flex items-center gap-4 mb-4">
      <label for="plec" class="font-semibold w-24">Płeć</label>
      <SelectButton
        id="plec"
        v-model="formularzZwierze.plec"
        :options="plec"
        optionLabel="nazwa"
        optionValue="nazwa"
        class="flex-auto"
      />
    </div>
    <div class="flex items-center gap-4 mb-4">
      <label for="status" class="font-semibold w-24">Status</label>
      <Select
        id="status"
        v-model="formularzZwierze.status"
        :options="statusy"
        optionLabel="nazwa"
        optionValue="nazwa"
        placeholder="Wybierz status..."
        class="flex-auto"
      />
    </div>

    <div class="flex items-start gap-4 mb-4">
      <label class="font-semibold w-24 pt-2">Zdjęcie</label>

      <div class="flex-auto flex flex-col gap-4">
        <FileUpload
          mode="basic"
          @select="onFileSelect"
          customUpload
          auto
          chooseLabel="Wybierz zdjęcie"
          severity="secondary"
          class="p-button-outlined w-full"
        />

        <div v-if="formularzZwierze.zdjecieUrl" class="flex justify-center">
          <img
            :src="formularzZwierze.zdjecieUrl"
            alt="Podgląd zdjęcia"
            class="shadow-md rounded-xl w-full sm:w-64"
          />
        </div>
      </div>
    </div>

    <div class="flex items-center gap-4 mb-4">
      <label for="numerEwidencyjny" class="font-semibold w-24">Numer ewidencyjny</label>
      <InputMask
        id="numerEwidencyjny"
        v-model="formularzZwierze.numerEwidencyjny"
        mask="a/9999/999"
        placeholder="P/2026/001"
        class="flex-auto uppercase"
      />
    </div>

    <div class="flex items-center gap-4 mb-4">
      <label for="dataPrzyjecia" class="font-semibold w-24">Data przyjęcia</label>
      <DatePicker
        id="dataPrzyjecia"
        v-model="formularzZwierze.dataPrzyjecia"
        showIcon
        iconDisplay="input"
        class="flex-auto"
      />
    </div>

    <div class="flex items-center gap-4 mb-4">
      <label for="przyblizonaDataUrodzenia" class="font-semibold w-24">Data urodzenia</label>
      <DatePicker
        id="przyblizonaDataUrodzenia"
        v-model="formularzZwierze.przyblizonaDataUrodzenia"
        showIcon
        iconDisplay="input"
        class="flex-auto"
      />
    </div>

    <div class="flex items-center gap-4 mb-4">
      <label for="czyZachipowany" class="font-semibold w-24">Czy zachipowany?</label>
      <Checkbox
        id="czyZachipowany"
        v-model="formularzZwierze.czyZachipowany"
        :binary="true"
        size="large"
        class="flex-auto"
      />
    </div>

    <div class="flex items-center gap-4 mb-4">
      <label for="czySzczepiony" class="font-semibold w-24">Czy szczepiony?</label>
      <Checkbox
        id="czySzczepiony"
        v-model="formularzZwierze.czySzczepiony"
        :binary="true"
        size="large"
        class="flex-auto"
      />
    </div>

    <div class="flex items-center gap-4 mb-4">
      <label for="czyKastrowanySterylizowany" class="font-semibold w-24">Czy kastrowany?</label>
      <Checkbox
        id="czyKastrowanySterylizowany"
        v-model="formularzZwierze.czyKastrowanySterylizowany"
        :binary="true"
        size="large"
        class="flex-auto"
      />
    </div>

    <div class="flex justify-end gap-2">
      <Button type="button" label="Anuluj" severity="secondary" @click="emit('zamknij')"></Button>
      <Button type="button" label="Dodaj" severity="success" @click="handleDodaj"></Button>
    </div>
  </Dialog>
</template>
