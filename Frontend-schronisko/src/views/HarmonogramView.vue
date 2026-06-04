<template>
  <div class="container mx-auto p-6">
    <div class="flex justify-between items-center mb-8">
      <h1 class="text-3xl font-bold">Harmonogram Schroniska</h1>
      <button @click="otworzModalDoDodawania" class="bg-[#22c55e] hover:bg-[#16a34a] text-white font-semibold py-2 px-6 rounded-xl shadow-sm transition-colors text-lg flex items-center gap-2">
        <span class="text-2xl font-light leading-none -mt-1">+</span> Dodaj
      </button>
    </div>

    <div v-if="!zadaniaZaładowane" class="flex justify-center my-10">
      <span class="loading loading-spinner loading-lg text-primary"></span>
    </div>

    <div v-else class="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">

      <div class="bg-base-200 p-4 rounded-xl">
        <h2 class="text-xl font-bold mb-4 border-b-2 border-primary pb-2">Żywienie</h2>
        <div v-if="zywienie.length === 0" class="text-sm text-gray-500">Brak zadań</div>

        <div v-for="zadanie in zywienie" :key="zadanie.id" class="card bg-base-100 shadow-sm mb-3">
          <div class="card-body p-4">
            <h3 class="card-title text-base" :class="{'line-through text-gray-400': zadanie.czyWykonane}">{{ zadanie.tytul }}</h3>
            <p v-if="zadanie.zwierze" class="text-sm font-bold text-primary mt-1">Dla: {{ zadanie.zwierze.imie }}</p>            <p class="text-sm text-gray-500">{{ formatujDate(zadanie.dataCzas) }}</p>
            <p v-if="zadanie.opis" class="text-sm">{{ zadanie.opis }}</p>

            <div class="flex gap-2 mt-4 items-center">
              <button @click="store.przelaczStatus(zadanie.id)" class="btn btn-xs flex-1" :class="zadanie.czyWykonane ? 'btn-success text-white' : 'btn-outline'">
                {{ zadanie.czyWykonane ? 'Zrobione' : 'Oznacz' }}
              </button>
              <button @click="otworzModalDoEdycji(zadanie)" class="btn btn-xs btn-outline btn-info px-2" title="Edytuj">
                ✏️
              </button>
              <button @click="usunZadanie(zadanie.id)" class="btn btn-xs btn-outline btn-error px-2" title="Usuń">
                🗑️
              </button>
            </div>
          </div>
        </div>
      </div>

      <div class="bg-base-200 p-4 rounded-xl">
        <h2 class="text-xl font-bold mb-4 border-b-2 border-info pb-2">Szczepienia</h2>
        <div v-if="szczepienia.length === 0" class="text-sm text-gray-500">Brak zadań</div>

        <div v-for="zadanie in szczepienia" :key="zadanie.id" class="card bg-base-100 shadow-sm mb-3">
          <div class="card-body p-4">
            <h3 class="card-title text-base" :class="{'line-through text-gray-400': zadanie.czyWykonane}">{{ zadanie.tytul }}</h3>
            <p v-if="zadanie.zwierze" class="text-sm font-bold text-info">Dla: {{ zadanie.zwierze.imie }}</p>
            <p class="text-sm text-gray-500">{{ formatujDate(zadanie.dataCzas) }}</p>

            <div class="flex gap-2 mt-4 items-center">
              <button @click="store.przelaczStatus(zadanie.id)" class="btn btn-xs flex-1" :class="zadanie.czyWykonane ? 'btn-success text-white' : 'btn-outline'">
                {{ zadanie.czyWykonane ? 'Zrobione' : 'Oznacz' }}
              </button>
              <button @click="otworzModalDoEdycji(zadanie)" class="btn btn-xs btn-outline btn-info px-2" title="Edytuj">
                ✏️
              </button>
              <button @click="usunZadanie(zadanie.id)" class="btn btn-xs btn-outline btn-error px-2" title="Usuń">
                🗑️
              </button>
            </div>
          </div>
        </div>
      </div>

      <div class="bg-base-200 p-4 rounded-xl">
        <h2 class="text-xl font-bold mb-4 border-b-2 border-error pb-2">Leki</h2>
        <div v-if="leki.length === 0" class="text-sm text-gray-500">Brak zadań</div>

        <div v-for="zadanie in leki" :key="zadanie.id" class="card bg-base-100 shadow-sm mb-3 border-l-4 border-error">
          <div class="card-body p-4">
            <h3 class="card-title text-base" :class="{'line-through text-gray-400': zadanie.czyWykonane}">{{ zadanie.tytul }}</h3>
            <p v-if="zadanie.zwierze" class="text-sm font-bold text-error">Dla: {{ zadanie.zwierze.imie }}</p>
            <p class="text-sm text-gray-500">{{ formatujDate(zadanie.dataCzas) }}</p>
            <p v-if="zadanie.opis" class="text-sm">{{ zadanie.opis }}</p>

            <div class="flex gap-2 mt-4 items-center">
              <button @click="store.przelaczStatus(zadanie.id)" class="btn btn-xs flex-1" :class="zadanie.czyWykonane ? 'btn-success text-white' : 'btn-outline'">
                {{ zadanie.czyWykonane ? 'Podane' : 'Oznacz' }}
              </button>
              <button @click="otworzModalDoEdycji(zadanie)" class="btn btn-xs btn-outline btn-info px-2" title="Edytuj">
                ✏️
              </button>
              <button @click="usunZadanie(zadanie.id)" class="btn btn-xs btn-outline btn-error px-2" title="Usuń">
                🗑️
              </button>
            </div>
          </div>
        </div>
      </div>

      <div class="bg-base-200 p-4 rounded-xl">
        <h2 class="text-xl font-bold mb-4 border-b-2 border-accent pb-2">Sprzątanie kojców</h2>
        <div v-if="sprzatanie.length === 0" class="text-sm text-gray-500">Brak zadań</div>

        <div v-for="zadanie in sprzatanie" :key="zadanie.id" class="card bg-base-100 shadow-sm mb-3">
          <div class="card-body p-4">
            <h3 class="card-title text-base" :class="{'line-through text-gray-400': zadanie.czyWykonane}">{{ zadanie.tytul }}</h3>
            <p v-if="zadanie.zwierze" class="text-sm font-bold text-accent mt-1">Dla: {{ zadanie.zwierze.imie }}</p>            <p class="text-sm text-gray-500">{{ formatujDate(zadanie.dataCzas) }}</p>

            <div class="flex gap-2 mt-4 items-center">
              <button @click="store.przelaczStatus(zadanie.id)" class="btn btn-xs flex-1" :class="zadanie.czyWykonane ? 'btn-success text-white' : 'btn-outline'">
                {{ zadanie.czyWykonane ? 'Zrobione' : 'Oznacz' }}
              </button>
              <button @click="otworzModalDoEdycji(zadanie)" class="btn btn-xs btn-outline btn-info px-2" title="Edytuj">
                ✏️
              </button>
              <button @click="usunZadanie(zadanie.id)" class="btn btn-xs btn-outline btn-error px-2" title="Usuń">
                🗑️
              </button>
            </div>

          </div>
        </div>
      </div>
    </div>

    <AddZadanieModal
      :otwarty="czyModalOtwarty"
      :zadanieDoEdycji="edytowaneZadanie"
      @zamknij="czyModalOtwarty = false"
      @zapisz="zapiszZadanieDoStore"
    />
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { useHarmonogramStore } from '@/stores/harmonogram'
import AddZadanieModal from '@/components/AddZadanieModal.vue'
import type { NoweZadanie } from '@/components/ZadanieForm.vue'

const store = useHarmonogramStore()
const zadaniaZaładowane = ref(false)
const czyModalOtwarty = ref(false)
const edytowaneZadanie = ref<NoweZadanie | null>(null)

// Kiedy wchodzisz w zakładkę, pobierz dane z bazy
onMounted(async () => {
  await store.pobierzZadania()
  zadaniaZaładowane.value = true
})

// Filtrowanie pobranych zadań po kategoriach
const zywienie = computed(() => store.zadania.filter(z => z.kategoria === 'Zywienie'))
const szczepienia = computed(() => store.zadania.filter(z => z.kategoria === 'Szczepienia'))
const leki = computed(() => store.zadania.filter(z => z.kategoria === 'Leki'))
const sprzatanie = computed(() => store.zadania.filter(z => z.kategoria === 'Sprzatanie'))

const formatujDate = (dataStr: string) => {
  const d = new Date(dataStr)
  return d.toLocaleString('pl-PL', { day: '2-digit', month: '2-digit', hour: '2-digit', minute: '2-digit' })
}

const otworzModalDoDodawania = () => {
  edytowaneZadanie.value = null
  czyModalOtwarty.value = true
}

const otworzModalDoEdycji = (zadanie: any) => {
  edytowaneZadanie.value = { ...zadanie }
  czyModalOtwarty.value = true
}

const usunZadanie = async (id: number) => {
  if(confirm("Czy na pewno chcesz usunąć to zadanie?")) {
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
    czyModalOtwarty.value = false // Zamknij modal po udanym zapisie
  } else {
    alert('Coś poszło nie tak na serwerze!')
  }
}
</script>
