<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { useWiadomosciStore } from '@/stores/wiadomosci'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import NewMessageModal from '@/components/NewMessageModal.vue'

const authStore = useAuthStore()
const wiadomosciStore = useWiadomosciStore()

const pokazModal = ref(false)
const nowyKomentarz = ref<Record<number, string>>({}) // Przechowuje tekst komentarza dla każdego id wiadomości

const odswiezWiadomosci = () => {
  if (authStore.nazwaUzytkownika) {
    wiadomosciStore.pobierzWiadomosci(authStore.nazwaUzytkownika)
  }
}

const usunWiadomosc = async (id?: number) => {
  if (!id || !confirm('Czy na pewno usunąć tę wiadomość?')) return

  // Zakładając, że masz funkcję 'usunWiadomosc' w wiadomosciStore
  const sukces = await wiadomosciStore.usunWiadomosc(id, authStore.nazwaUzytkownika)
  if (sukces) odswiezWiadomosci()
  else alert('Błąd usuwania!')
}

const dodajKomentarz = async (wiadomoscId: number) => {
  const tresc = nowyKomentarz.value[wiadomoscId]
  if (!tresc?.trim()) return

  const sukces = await wiadomosciStore.dodajKomentarz(wiadomoscId, {
    tresc,
    autor: authStore.nazwaUzytkownika
  })

  if (sukces) {
    nowyKomentarz.value[wiadomoscId] = ''
    odswiezWiadomosci()
  }
}

const sformatujDate = (dataStr?: string) => {
  if (!dataStr) return ''
  return new Date(dataStr).toLocaleString('pl-PL', {
    day: 'numeric',
    month: 'long',
    year: 'numeric',
    hour: '2-digit',
    minute: '2-digit'
  })
}

onMounted(() => {
  odswiezWiadomosci()
})
</script>

<template>
  <div class="container mx-auto p-6 max-w-4xl">
    <div class="flex justify-between items-center mb-8">
      <div>
        <h1 class="text-3xl font-bold text-gray-800">Tablica Wiadomości</h1>
        <p class="text-sm text-gray-500">Komunikaty wewnętrzne zespołu schroniska</p>
      </div>
      <Button label="Napisz wiadomość" icon="pi pi-plus" severity="primary" class="font-semibold" @click="pokazModal = true" />
    </div>

    <div v-if="wiadomosciStore.listaWiadomosci.length === 0" class="text-center py-12 bg-base-100 rounded-xl border border-dashed border-gray-200 text-gray-400">
      <i class="pi pi-envelope text-4xl mb-3"></i>
      <p>Brak nowych wiadomości w skrzynce.</p>
    </div>

    <div v-else class="space-y-4">
      <div v-for="w in wiadomosciStore.listaWiadomosci" :key="w.id" class="card bg-base-100 shadow-sm border border-gray-100">
        <div class="card-body p-5">
          <div class="flex justify-between items-start">
            <span v-if="w.odbiorca === 'Wszyscy'" class="badge badge-secondary text-xs">Ogłoszenie ogólne</span>
            <span v-else class="badge badge-warning text-xs">Wiadomość prywatna</span>

            <Button v-if="w.nadawca === authStore.nazwaUzytkownika"
                    icon="pi pi-trash"
                    severity="danger"
                    text
                    size="small"
                    @click="usunWiadomosc(w.id)" />
          </div>

          <p class="text-gray-700 mt-2">{{ w.tresc }}</p>

          <div class="divider my-2"></div>

          <div class="space-y-2 mb-4">
            <div v-for="k in w.komentarze" :key="k.id" class="text-sm bg-gray-50 p-3 rounded border-l-4 border-primary">
              <div class="flex justify-between items-center mb-1">
                <span class="font-bold text-gray-800">{{ k.autor }}</span>
                <span class="text-[10px] text-gray-400">{{ sformatujDate(k.dataDodania) }}</span>
              </div>
              <p class="text-gray-600">{{ k.tresc }}</p>
            </div>
          </div>

          <div class="flex gap-2">
            <InputText v-model="nowyKomentarz[w.id!]" placeholder="Dodaj komentarz..." class="w-full" />
            <Button label="Dodaj" size="small" @click="dodajKomentarz(w.id!)" />
          </div>

          <div class="flex justify-between items-center text-xs text-gray-400 mt-4">
            <div>Nadawca: <span class="font-bold text-gray-600">{{ w.nadawca }}</span></div>
            <div>{{ sformatujDate(w.dataDodania) }}</div>
          </div>
        </div>
      </div>
    </div>

    <NewMessageModal :otwarty="pokazModal" @zamknij="pokazModal = false" @sukces="odswiezWiadomosci" />
  </div>
</template>
