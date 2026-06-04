<script setup lang="ts">
import { ref } from 'vue'
import InputText from 'primevue/inputtext'
import Select from 'primevue/select'
import DatePicker from 'primevue/datepicker'
import Textarea from 'primevue/textarea'

// Typ dla formularza (najlepiej docelowo przenieść do '@/types/harmonogram.ts')
export interface NoweZadanie {
  id?: number;
  tytul: string;
  opis: string;
  kategoria: string;
  dataCzas: Date | null;
  zwierzeId: number | null;
  czyWykonane: boolean;
}

const modelValue = defineModel<NoweZadanie>({ required: true })

defineProps<{
  isReadonly: boolean
  // Lista zwierząt pobrana ze store'a, żeby móc przypisać zadanie do konkretnego podopiecznego
  listaZwierzat: { id: number, imie: string, numerEwidencyjny: string }[]
}>()

// Słownik kategorii (zgodny z backendem)
const kategorie = [
  { id: 'Zywienie', nazwa: '🍲 Żywienie' },
  { id: 'Leki', nazwa: '💊 Leki' },
  { id: 'Szczepienia', nazwa: '💉 Szczepienia' },
  { id: 'Sprzatanie', nazwa: '🧹 Sprzątanie kojców' },
]

// Stan błędów
const bledy = ref({
  tytul: '',
  kategoria: '',
  dataCzas: ''
})

const walidujFormularz = () => {
  let czyPoprawny = true
  bledy.value = { tytul: '', kategoria: '', dataCzas: '' }

  if (!modelValue.value.tytul || modelValue.value.tytul.trim() === '') {
    bledy.value.tytul = 'Tytuł zadania jest wymagany.'
    czyPoprawny = false
  }

  if (!modelValue.value.kategoria) {
    bledy.value.kategoria = 'Wybierz kategorię zadania.'
    czyPoprawny = false
  }

  if (!modelValue.value.dataCzas) {
    bledy.value.dataCzas = 'Wybierz datę i godzinę.'
    czyPoprawny = false
  }

  return czyPoprawny
}

defineExpose({
  walidujFormularz
})
</script>

<template>
  <div class="flex flex-col gap-1 mb-4">

    <div class="flex items-center gap-4 mb-4">
      <label for="tytul" class="font-semibold w-36">Tytuł zadania</label>
      <InputText
        id="tytul"
        v-model="modelValue.tytul"
        required
        :disabled="isReadonly"
        :invalid="bledy.tytul !== ''"
        @blur="walidujFormularz"
        class="flex-auto"
        placeholder="np. Podanie karmy suchej"
        autocomplete="off"
      />
    </div>
    <small v-if="bledy.tytul" class="text-red-500 font-medium ml-40 -mt-4 mb-4">{{ bledy.tytul }}</small>

    <div class="flex items-center gap-4 mb-4">
      <label for="kategoria" class="font-semibold w-36">Kategoria</label>
      <Select
        id="kategoria"
        v-model="modelValue.kategoria"
        required
        :options="kategorie"
        optionLabel="nazwa"
        optionValue="id"
        placeholder="Wybierz kategorię..."
        :disabled="isReadonly"
        :invalid="bledy.kategoria !== ''"
        @blur="walidujFormularz"
        class="flex-auto"
      />
    </div>
    <small v-if="bledy.kategoria" class="text-red-500 font-medium ml-40 -mt-4 mb-4">{{ bledy.kategoria }}</small>

    <div class="flex items-center gap-4 mb-4">
      <label for="dataCzas" class="font-semibold w-36">Data i godzina</label>
      <DatePicker
        id="dataCzas"
        v-model="modelValue.dataCzas"
        required
        showTime
        hourFormat="24"
        dateFormat="dd.mm.yy"
        showIcon
        iconDisplay="input"
        :disabled="isReadonly"
        :invalid="bledy.dataCzas !== ''"
        @blur="walidujFormularz"
        class="flex-auto"
      />
    </div>
    <small v-if="bledy.dataCzas" class="text-red-500 font-medium ml-40 -mt-4 mb-4">{{ bledy.dataCzas }}</small>

    <div class="flex items-center gap-4 mb-4">
      <label for="zwierzeId" class="font-semibold w-36">Zwierzak (opcjonalnie)</label>
      <Select
        id="zwierzeId"
        v-model="modelValue.zwierzeId"
        :options="listaZwierzat"
        optionLabel="imie"
        optionValue="id"
        dataKey="id"  placeholder="Dotyczy całego schroniska..."
        :disabled="isReadonly"
        showClear
        class="flex-auto"
      >
        <template #option="slotProps">
          <div class="flex flex-col">
            <span class="font-bold">{{ slotProps.option.imie }}</span>
            <span class="text-sm text-gray-500">{{ slotProps.option.numerEwidencyjny }}</span>
          </div>
        </template>
      </Select>
    </div>

    <div class="flex items-start gap-4 mb-4">
      <label for="opis" class="font-semibold w-36 pt-2">Dodatkowy opis</label>
      <Textarea
        id="opis"
        v-model="modelValue.opis"
        rows="3"
        :disabled="isReadonly"
        class="flex-auto"
        placeholder="Uwagi dot. zadania, dawkowanie leku, itp."
      />
    </div>

  </div>
</template>
