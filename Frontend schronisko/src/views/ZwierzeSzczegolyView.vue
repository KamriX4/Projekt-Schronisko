<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useZwierzetaStore } from '@/stores/zwierzeta'
import { uploadZdjecia } from '@/stores/pliki'
import type { Zwierze } from '@/types/zwierze'

import ZwierzeForm from '@/components/ZwierzeForm.vue' // Twój nowy klocek!
import ToggleSwitch from 'primevue/toggleswitch'
import Button from 'primevue/button'
import ConfirmDialog from 'primevue/confirmdialog';
import { useConfirm } from 'primevue/useconfirm'

const route = useRoute()
const router = useRouter()
const store = useZwierzetaStore()
const confirm = useConfirm()

// Funkcja cofająca
const wroc = () => {
  // router.back() cofa do poprzedniej strony w historii przeglądarki.
  // Alternatywnie możesz użyć: router.push('/zwierzeta')
  router.back()
}
// Stany widoku
const trybEdycji = ref(false)
const lokalneZwierze = ref<Zwierze | null>(null) // Lokalna kopia do edycji
const wybranyPlikRaw = ref<File | null>(null) // Przechowuje ewentualne nowe zdjęcie

// 1. Pobieranie danych przy wejściu na stronę
onMounted(async () => {
  const idZAdresu = Number(route.params.id)

  if (idZAdresu) {
    await store.pobierzZwierze(idZAdresu)

    if (store.aktualneZwierze) {
      lokalneZwierze.value = {
        ...store.aktualneZwierze,
        // Zamieniamy tekst z bazy na obiekt Date dla kalendarza PrimeVue
        dataPrzyjecia: store.aktualneZwierze.dataPrzyjecia
          ? new Date(store.aktualneZwierze.dataPrzyjecia)
          : null,
        przyblizonaDataUrodzenia: store.aktualneZwierze.przyblizonaDataUrodzenia
          ? new Date(store.aktualneZwierze.przyblizonaDataUrodzenia)
          : null,
      } as unknown as Zwierze
    }
  }
})

// 2. Odbieranie pliku od formularza (identycznie jak w Modalu)
const odbierzPlikZFormularza = (plik: File | null) => {
  wybranyPlikRaw.value = plik
}

// 3. Zapisywanie zmian na serwerze
const zapiszZmiany = async () => {
  if (!lokalneZwierze.value) return

  // Formatowanie numeru
  if (lokalneZwierze.value.numerEwidencyjny) {
    lokalneZwierze.value.numerEwidencyjny = lokalneZwierze.value.numerEwidencyjny.toUpperCase()
  }

  // Jeśli użytkownik podmienił zdjęcie, wgrywamy najpierw plik
  if (wybranyPlikRaw.value) {
    try {
      const wygenerowanyLink = await uploadZdjecia(wybranyPlikRaw.value)
      lokalneZwierze.value.zdjecieUrl = wygenerowanyLink
    } catch (error) {
      console.error('Błąd uploadu:', error)
      alert('Błąd podczas wgrywania nowego zdjęcia na serwer!')
      return
    }
  }

  try {
    // Zakładam, że w pliku zwierzeta.ts masz funkcję do edycji (PUT/PATCH).
    // Jeśli się nazywa inaczej, podmień poniższą linijkę:
    await store.edytujZwierze(lokalneZwierze.value.id, lokalneZwierze.value)

    trybEdycji.value = false // Wyłączamy tryb edycji
    wybranyPlikRaw.value = null // Czyścimy pamięć pliku

    // (Opcjonalnie) aktualizujemy Store, żeby miał nowe dane
    store.aktualneZwierze = { ...lokalneZwierze.value }
  } catch (error) {
    console.error('Błąd zapisu:', error)
    alert('Wystąpił błąd podczas zapisywania danych w bazie.')
  }
}

// 4. Resetowanie formularza przy anulowaniu
const anulujEdycje = () => {
  trybEdycji.value = false
  wybranyPlikRaw.value = null

  if (store.aktualneZwierze) {
    lokalneZwierze.value = {
      ...store.aktualneZwierze,
      // Tutaj też robimy konwersję przy resecie!
      dataPrzyjecia: store.aktualneZwierze.dataPrzyjecia
        ? new Date(store.aktualneZwierze.dataPrzyjecia)
        : null,
      przyblizonaDataUrodzenia: store.aktualneZwierze.przyblizonaDataUrodzenia
        ? new Date(store.aktualneZwierze.przyblizonaDataUrodzenia)
        : null,
    } as unknown as Zwierze
  }
}

const potwierdzUsuniecie = () => {
  confirm.require({
    message: `Czy na pewno chcesz usunąć zwierzaka: ${lokalneZwierze.value?.imie}?`,
    header: 'Potwierdzenie usunięcia',
    icon: 'pi pi-exclamation-triangle',
    rejectProps: {
      label: 'Anuluj',
      severity: 'secondary',
      outlined: true
    },
    acceptProps: {
      label: 'Usuń',
      severity: 'danger'
    },
    accept: async () => {
      // Ta funkcja wykona się tylko, gdy użytkownik kliknie "Usuń"
      await usunZwierzaka()
    }
  })
}

// 5. Usuwanie zwierzaka
const usunZwierzaka = async () => {
  if (!lokalneZwierze.value) return

  try {
    // Zakładam, że w pliku zwierzeta.ts masz funkcję do usuwania (np. DELETE do API)
    const sukces = await store.usunZwierze(lokalneZwierze.value.id)

    if (sukces) {
      // Jeśli się udało, wracamy do głównej listy
      router.push('/zwierzeta')
    } else {
      alert('Nie udało się usunąć zwierzaka. Sprawdź logi konsoli.')
    }
  } catch (error) {
    console.error('Błąd podczas usuwania:', error)
    alert('Wystąpił błąd serwera podczas usuwania.')
  }
}
</script>

<template>
  <div class="p-4 sm:p-8 max-w-4xl mx-auto">
    <div v-if="!lokalneZwierze" class="flex justify-center items-center h-64">
      <i class="pi pi-spinner pi-spin text-4xl text-primary mb-4"></i>
    </div>

    <div v-else class="bg-white p-6 sm:p-8 rounded-xl shadow-lg border border-gray-100">
      <div class="mb-6 -mt-2">
        <Button
          icon="pi pi-arrow-left"
          label="Wróć"
          text
          severity="secondary"
          class="!px-0 hover:bg-transparent hover:text-primary transition-colors font-semibold"
          @click="wroc"
        />
      </div>
      <div
        class="flex flex-col sm:flex-row justify-between items-start sm:items-center mb-8 border-b border-gray-100 pb-6 gap-4"
      >
        <div>
          <p class="text-sm text-gray-400 font-semibold tracking-wider uppercase mb-1">
            Profil Podopiecznego
          </p>
          <h1 class="text-3xl font-bold text-gray-800">
            {{ lokalneZwierze.imie }}
          </h1>
        </div>

        <div
          class="flex items-center gap-3 bg-gray-50 px-5 py-3 rounded-xl border border-gray-200 shadow-sm transition-colors"
          :class="{ 'border-yellow-400 bg-primary/5': trybEdycji }"
        >
          <label for="edycja" class="font-semibold text-gray-700 cursor-pointer select-none">
            {{ trybEdycji ? 'Tryb edycji: Wł.' : 'Tryb edycji: Wył.' }}
          </label>
          <ToggleSwitch id="edycja" v-model="trybEdycji" @change="!trybEdycji && anulujEdycje()" />
        </div>
      </div>

      <ZwierzeForm
        v-model="lokalneZwierze"
        :isReadonly="!trybEdycji"
        @fileSelected="odbierzPlikZFormularza"
      />

      <div v-if="trybEdycji" class="mt-10 flex justify-end gap-3 border-t border-gray-100 pt-6">
        <Button
          label="Anuluj"
          severity="secondary"
          icon="pi pi-times"
          outlined
          @click="anulujEdycje"
        />
        <Button label="Zapisz zmiany" severity="success" icon="pi pi-check" @click="zapiszZmiany" />
      </div>

      <div v-if="!trybEdycji" class="mt-10 flex justify-end gap-3 border-t border-gray-100 pt-6">
        <ConfirmDialog></ConfirmDialog>
        <Button label="Usuń" icon="pi pi-trash" severity="danger" outlined @click="potwierdzUsuniecie" />
      </div>
    </div>
  </div>
</template>
