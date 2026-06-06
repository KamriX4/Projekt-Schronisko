<script setup lang="ts">
// Modal do dodawania nowego zwierzęcia. Zawiera formularz ZwierzeForm i obsługę uploadu obrazu.
import { ref, watch } from 'vue'
import type { NoweZwierze } from '@/types/zwierze'
import { uploadZdjecia } from '@/stores/pliki'
import Dialog from 'primevue/dialog'
import Button from 'primevue/button'
import ZwierzeForm from '@/components/ZwierzeForm.vue'
import { useI18n } from 'vue-i18n'

const { t } = useI18n()

const props = defineProps<{
  otwarty: boolean
}>()

const emit = defineEmits<{
  (e: 'zamknij'): void
  (e: 'zapisz', noweZwierze: NoweZwierze): void
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
const wybranyPlikRaw = ref<File | null>(null) // zdjecie wybrane w formularzu
const formularzRef = ref<InstanceType<typeof ZwierzeForm> | null>(null)

watch(
  () => props.otwarty,
  (czyOtwarty) => {
    if (czyOtwarty) {
      formularzZwierze.value = domyslnyStan()
      wybranyPlikRaw.value = null
    }
  },
)

const odbierzPlikZFormularza = (plik: File | null) => {
  wybranyPlikRaw.value = plik
}

// Waliduje formularz, ewentualnie wysyła zdjęcie i emituje dane do rodzica
const handleDodaj = async () => {
  const czyPoprawny = formularzRef.value?.walidujFormularz()
  if (!czyPoprawny) {
    alert(t('animals.alerts.validation_error'))
    return
  }

  if (formularzZwierze.value.numerEwidencyjny) {
    formularzZwierze.value.numerEwidencyjny = formularzZwierze.value.numerEwidencyjny.toUpperCase()
  }

  if (wybranyPlikRaw.value) {
    try {
      const wygenerowanyLink = await uploadZdjecia(wybranyPlikRaw.value)
      formularzZwierze.value.zdjecieUrl = wygenerowanyLink
    } catch (error) {
      console.error('Szczegóły błędu uploadu:', error)
      alert(t('animals.alerts.upload_error'))
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
    :header="$t('animals.add_dialog.header')"
    :style="{ width: '40rem' }"
  >
    <span class="text-surface-500 dark:text-surface-400 block mb-8">
      {{ $t('animals.add_dialog.description') }}
    </span>

    <ZwierzeForm
      ref="formularzRef"
      v-model="formularzZwierze"
      :isReadonly="false"
      @fileSelected="odbierzPlikZFormularza"
    />

    <div class="flex justify-end gap-2">
      <Button
        type="button"
        :label="$t('animals.form.cancel')"
        severity="secondary"
        @click="emit('zamknij')"
      ></Button>
      <Button
        type="button"
        :label="$t('animals.add')"
        severity="success"
        @click="handleDodaj"
      ></Button>
    </div>
  </Dialog>
</template>
