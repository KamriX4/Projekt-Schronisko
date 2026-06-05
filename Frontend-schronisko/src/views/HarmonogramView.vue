<template>
  <div class="container mx-auto p-6">
    <div class="flex justify-between items-center mb-8">
      <h1 class="text-3xl font-bold">Harmonogram Schroniska</h1>
      <button
        type="button"
        class="bg-[#22c55e] hover:bg-[#16a34a] text-white font-semibold py-2 px-6 rounded-xl shadow-sm transition-colors text-lg flex items-center gap-2"
        @click="otworzModalDoDodawania"
      >
        <span class="text-2xl font-light leading-none -mt-1">+</span> Dodaj
      </button>
    </div>

    <div v-if="!zadaniaZaładowane" class="flex justify-center my-10">
      <span class="loading loading-spinner loading-lg text-primary"></span>
    </div>

    <!-- [6] DRAG & DROP — cztery kolumny z Vue Draggable -->
    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      <HarmonogramKolumna
        v-model="zywienie"
        kategoria="Zywienie"
        tytul-kolumny="Żywienie"
        klasa-naglowka="border-primary"
        :kolor-akcentu="kolorAkcentu"
        :formatuj-date="formatujDate"
        :on-przelacz="store.przelaczStatus"
        :on-edytuj="otworzModalDoEdycji"
        :on-usun="usunZadanie"
      />

      <HarmonogramKolumna
        v-model="szczepienia"
        kategoria="Szczepienia"
        tytul-kolumny="Szczepienia"
        klasa-naglowka="border-info"
        :kolor-akcentu="kolorAkcentu"
        :formatuj-date="formatujDate"
        :on-przelacz="store.przelaczStatus"
        :on-edytuj="otworzModalDoEdycji"
        :on-usun="usunZadanie"
      />

      <HarmonogramKolumna
        v-model="leki"
        kategoria="Leki"
        tytul-kolumny="Leki"
        klasa-naglowka="border-error"
        :kolor-akcentu="kolorAkcentu"
        :formatuj-date="formatujDate"
        :on-przelacz="store.przelaczStatus"
        :on-edytuj="otworzModalDoEdycji"
        :on-usun="usunZadanie"
        etykieta-wykonane="Podane"
        etykieta-oznacz="Oznacz"
      />

      <HarmonogramKolumna
        v-model="sprzatanie"
        kategoria="Sprzatanie"
        tytul-kolumny="Sprzątanie kojców"
        klasa-naglowka="border-accent"
        :kolor-akcentu="kolorAkcentu"
        :formatuj-date="formatujDate"
        :on-przelacz="store.przelaczStatus"
        :on-edytuj="otworzModalDoEdycji"
        :on-usun="usunZadanie"
      />
    </div>

    <AddZadanieModal
      :otwarty="czyModalOtwarty"
      :zadanie-do-edycji="edytowaneZadanie"
      @zamknij="czyModalOtwarty = false"
      @zapisz="zapiszZadanieDoStore"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted, inject } from 'vue'
import { useHarmonogramStore } from '@/stores/harmonogram'
import type { ZadanieHarmonogramu } from '@/stores/harmonogram'
import AddZadanieModal from '@/components/AddZadanieModal.vue'
import HarmonogramKolumna from '@/components/HarmonogramKolumna.vue'
import type { NoweZadanie } from '@/components/ZadanieForm.vue'
import { schroniskoContextKey } from '@/context/schroniskoContext'

const store = useHarmonogramStore()
const zadaniaZaładowane = ref(false)
const czyModalOtwarty = ref(false)
const edytowaneZadanie = ref<NoweZadanie | null>(null)

/** [5] PROVIDE/INJECT — odczyt współdzielonego kontekstu z App.vue */
const schroniskoCtx = inject(schroniskoContextKey)
const kolorAkcentu = computed(() => schroniskoCtx?.kolorAkcentu.value ?? '')

onMounted(async () => {
  await store.pobierzZadania()
  zadaniaZaładowane.value = true
})

function listaKategorii(kategoria: string) {
  return computed({
    get: () => store.zadania.filter((z) => z.kategoria === kategoria),
    set: (nowaLista: ZadanieHarmonogramu[]) => store.ustawKolejnoscKategorii(kategoria, nowaLista),
  })
}

const zywienie = listaKategorii('Zywienie')
const szczepienia = listaKategorii('Szczepienia')
const leki = listaKategorii('Leki')
const sprzatanie = listaKategorii('Sprzatanie')

const formatujDate = (dataStr: string) => {
  const d = new Date(dataStr)
  return d.toLocaleString('pl-PL', { day: '2-digit', month: '2-digit', hour: '2-digit', minute: '2-digit' })
}

const otworzModalDoDodawania = () => {
  edytowaneZadanie.value = null
  czyModalOtwarty.value = true
}

const otworzModalDoEdycji = (zadanie: ZadanieHarmonogramu) => {
  edytowaneZadanie.value = {
    id: zadanie.id,
    tytul: zadanie.tytul,
    opis: zadanie.opis,
    kategoria: zadanie.kategoria,
    dataCzas: new Date(zadanie.dataCzas),
    zwierzeId: zadanie.zwierzeId ?? null,
    czyWykonane: zadanie.czyWykonane,
  }
  czyModalOtwarty.value = true
}

const usunZadanie = async (id: number) => {
  if (confirm('Czy na pewno chcesz usunąć to zadanie?')) {
    await store.usunZadanie(id)
  }
}

const zapiszZadanieDoStore = async (zadanie: NoweZadanie) => {
  let sukces = false
  if (zadanie.id) {
    sukces = await store.edytujZadanie(zadanie.id, zadanie)
  } else {
    sukces = await store.dodajZadanie(zadanie)
  }

  if (sukces) {
    czyModalOtwarty.value = false
  } else {
    alert('Coś poszło nie tak na serwerze!')
  }
}
</script>
