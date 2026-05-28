<script setup lang="ts">
import { ref, watch } from 'vue'
import type { NoweZwierze } from '@/types/zwierze'

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
  status: 'Do Adopcji',
  zdjecieUrl: '',
  numerEwidencyjny: '',
  dataPrzyjecia: '',
  przyblizonaDataUrodzenia: '',
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
  emit('zapisz', formularzZwierze.value)
}
</script>

<template>
  <dialog :open="otwarty" class="modal modal-bottom sm:modal-middle">
    <div class="modal-box">
      <h3 class="font-bold text-lg mb-4">Dodaj nowego zwierzaka</h3>

      <div class="flex flex-col gap-3">
        <label class="form-control w-full">
          <div class="label"><span class="label-text">Imię zwierzęcia</span></div>
          <input
            v-model="formularzZwierze.imie"
            type="text"
            class="input input-bordered w-full pl-4"
            placeholder="Wpisz imię..."
          />
        </label>

        <label class="form-control w-full">
          <div class="label"><span class="label-text">ID Gatunku</span></div>
          <input
            v-model.number="formularzZwierze.gatunekId"
            type="number"
            class="input input-bordered w-full pl-4"
          />
        </label>

        <fieldset class="fieldset">
          <legend class="fieldset-legend">Płeć</legend>
          <select v-model="formularzZwierze.plec" class="select select-bordered w-full pl-4">
            <option disabled selected>Wybierz płeć</option>
            <option value="Samiec">Samiec</option>
            <option value="Samica">Samica</option>
          </select>
          <span class="label">Optional</span>
        </fieldset>

        <label class="form-control w-full">
          <div class="label"><span class="label-text">Status adopcji</span></div>
          <select v-model="formularzZwierze.status" class="select select-bordered w-full pl-4">
            <option disabled selected>Wybierz status</option>
            <option value="Do Adopcji">Do Adopcji</option>
            <option value="W kwarantannie">W kwarantannie</option>
            <option value="Adoptowany">Adoptowany</option>
          </select>
        </label>
      </div>

      <div class="modal-action">
        <button @click="emit('zamknij')" class="btn btn-ghost">Anuluj</button>
        <button @click="handleDodaj" class="btn bg-success font-semibold p-4">Dodaj</button>
      </div>
    </div>
  </dialog>
</template>
