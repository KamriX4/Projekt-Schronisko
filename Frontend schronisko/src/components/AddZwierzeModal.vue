<script setup lang="ts">
import { ref, watch } from 'vue'
import type { NoweZwierze } from '@/types/zwierze'
import { uploadZdjecia } from '@/stores/pliki'
import Dialog from 'primevue/dialog'
import Button from 'primevue/button'
import ZwierzeForm from '@/components/ZwierzeForm.vue'
// import InputNumber from 'primevue/inputnumber'

const props = defineProps<{
  otwarty: boolean
}>()

const emit = defineEmits<{
  (e: 'zamknij'): void
  (e: 'zapisz', noweZwierze: NoweZwierze): void // Tu możesz użyć Partial<Zwierze> docelowo
}>()

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
const wybranyPlikRaw = ref<File | null>(null) // Przechowuje fizyczny plik przekazany z ZwierzeForm

watch(
  () => props.otwarty,
  (czyOtwarty) => {
    if (czyOtwarty) {
      formularzZwierze.value = domyslnyStan()
      wybranyPlikRaw.value = null
    }
  },
)

// Odbiera plik od komponentu ZwierzeForm
const odbierzPlikZFormularza = (plik: File | null) => {
  wybranyPlikRaw.value = plik
}

const handleDodaj = async () => {
  if (!formularzZwierze.value.imie) return alert('Imię jest wymagane!')
  if (formularzZwierze.value.gatunekId === 0) return alert('Wybierz gatunek!')

  if (formularzZwierze.value.numerEwidencyjny) {
    formularzZwierze.value.numerEwidencyjny = formularzZwierze.value.numerEwidencyjny.toUpperCase()
  }

  if (wybranyPlikRaw.value) {
    try {
      const wygenerowanyLink = await uploadZdjecia(wybranyPlikRaw.value)
      formularzZwierze.value.zdjecieUrl = wygenerowanyLink
    } catch (error) {
      console.error('Szczegóły błędu uploadu:', error)
      alert('Błąd podczas wgrywania zdjęcia na serwer!')
      return
    }
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

    <ZwierzeForm
      v-model="formularzZwierze"
      :isReadonly="false"
      @fileSelected="odbierzPlikZFormularza"
    />

    <div class="flex justify-end gap-2">
      <Button type="button" label="Anuluj" severity="secondary" @click="emit('zamknij')"></Button>
      <Button type="button" label="Dodaj" severity="success" @click="handleDodaj"></Button>
    </div>
  </Dialog>
</template>
