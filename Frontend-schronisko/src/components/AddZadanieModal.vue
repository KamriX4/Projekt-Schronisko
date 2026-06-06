<script setup lang="ts">
import { ref, watch, onMounted } from 'vue'
import Dialog from 'primevue/dialog'
import Button from 'primevue/button'
import ZadanieForm, { type NoweZadanie } from '@/components/ZadanieForm.vue'
import { useZwierzetaStore } from '@/stores/zwierzeta'

// Props: kontrola widoczności modala i opcjonalne zadanie do edycji
const props = defineProps<{
  otwarty: boolean
  zadanieDoEdycji?: NoweZadanie | null // tryb edycji
}>()

const emit = defineEmits<{
  (e: 'zamknij'): void
  (e: 'zapisz', noweZadanie: NoweZadanie): void
}>()

const zwierzetaStore = useZwierzetaStore()

onMounted(async () => {
  if (zwierzetaStore.zwierzeta.length === 0) await zwierzetaStore.pobierzZwierzeta()
})

const domyslnyStan = (): NoweZadanie => ({
  tytul: '',
  opis: '',
  kategoria: '',
  dataCzas: null,
  zwierzeId: null,
  czyWykonane: false,
})

const formularzZadanie = ref<NoweZadanie>(domyslnyStan())
const formularzRef = ref<InstanceType<typeof ZadanieForm> | null>(null)

// Watcher resetuje formularz przy otwarciu modala, a w trybie edycji wczytuje istniejące dane
watch(
  () => props.otwarty,
  (czyOtwarty) => {
    if (czyOtwarty) {
      if (props.zadanieDoEdycji) {
        formularzZadanie.value = { ...props.zadanieDoEdycji }
        if (typeof formularzZadanie.value.dataCzas === 'string') {
          formularzZadanie.value.dataCzas = new Date(formularzZadanie.value.dataCzas)
        }
      } else {
        formularzZadanie.value = domyslnyStan()
      }
    }
  },
)

// Waliduje i emituje dane zadania do rodzica po kliknięciu "Zapisz"
const handleZapisz = async () => {
  if (!formularzRef.value?.walidujFormularz()) {
    alert('Popraw błędy w formularzu!')
    return
  }
  emit('zapisz', formularzZadanie.value)
}
</script>

<template>
  <Dialog
    :visible="otwarty"
    @update:visible="emit('zamknij')"
    modal
    :header="zadanieDoEdycji ? 'Edytuj zadanie' : 'Zaplanuj nowe zadanie'"
    :style="{ width: '40rem' }"
  >
    <ZadanieForm
      ref="formularzRef"
      v-model="formularzZadanie"
      :isReadonly="false"
      :listaZwierzat="zwierzetaStore.zwierzeta"
    />
    <div class="flex justify-end gap-2 mt-4">
      <Button type="button" label="Anuluj" severity="secondary" @click="emit('zamknij')"></Button>
      <Button
        type="button"
        :label="zadanieDoEdycji ? 'Zapisz zmiany' : 'Dodaj zadanie'"
        severity="success"
        @click="handleZapisz"
      ></Button>
    </div>
  </Dialog>
</template>
