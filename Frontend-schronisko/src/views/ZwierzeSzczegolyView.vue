<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useZwierzetaStore } from '@/stores/zwierzeta'
import { uploadZdjecia } from '@/stores/pliki'
import type { Zwierze } from '@/types/zwierze'
import BaseKarta from '@/components/BaseKarta.vue'
import ZwierzeForm from '@/components/ZwierzeForm.vue' // Twój nowy klocek!
import ToggleSwitch from 'primevue/toggleswitch'
import Button from 'primevue/button'
import ConfirmDialog from 'primevue/confirmdialog'
import { useConfirm } from 'primevue/useconfirm'
import SukcesModal from '@/components/SukcesModal.vue'
import { useI18n } from 'vue-i18n' // <-- Import i18n

const { t } = useI18n() // <-- Inicjalizacja i18n

const route = useRoute()
const router = useRouter()
const store = useZwierzetaStore()
const confirm = useConfirm()
const pokazSukcesModal = ref(false)
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

// 1. DODAJ REFERENCJĘ DO FORMULARZA (Tak samo jak w Modalu!)
const formularzRef = ref<InstanceType<typeof ZwierzeForm> | null>(null)

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

  // 2. WYWOŁAJ WALIDACJĘ Z DZIECKA (ZwierzeForm)
  const czyPoprawny = formularzRef.value?.walidujFormularz()

  // 3. ZABLOKUJ ZAPIS JEŚLI SĄ BŁĘDY
  if (!czyPoprawny) {
    alert(t('animals.alerts.edit_validation_error')) // <-- Użycie tłumaczenia
    return // PRZERYWAMY DZIAŁANIE! Zmiany nie zostaną zapisane.
  }

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
      alert(t('animals.alerts.upload_error'))
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
    pokazSukcesModal.value = true // Pokazujemy modal sukcesu
  } catch (error) {
    console.error('Błąd zapisu:', error)
    alert(t('animals.alerts.save_error'))
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
    message: t('animals.delete_dialog.message', { name: lokalneZwierze.value?.imie }),
    header: t('animals.delete_dialog.header'),
    icon: 'pi pi-exclamation-triangle',
    rejectProps: {
      label: t('animals.profile.cancel'),
      severity: 'secondary',
      outlined: true,
    },
    acceptProps: {
      label: t('animals.profile.delete'),
      severity: 'danger',
    },
    accept: async () => {
      // Ta funkcja wykona się tylko, gdy użytkownik kliknie "Usuń"
      await usunZwierzaka()
    },
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
      alert(t('animals.alerts.delete_fail'))
    }
  } catch (error) {
    console.error('Błąd podczas usuwania:', error)
    alert(t('animals.alerts.delete_error'))
  }
}
</script>

<template>
  <div>
    <div class="p-4 sm:p-8 max-w-4xl mx-auto">
      <div v-if="!lokalneZwierze" class="flex justify-center items-center h-64">
        <i class="pi pi-spinner pi-spin text-4xl text-primary mb-4"></i>
      </div>

      <BaseKarta v-else>
        <template #header>
          <div class="mb-6 -mt-2">
            <Button
              icon="pi pi-arrow-left"
              :label="$t('animals.profile.back')"
              text
              severity="secondary"
              class="!px-0 hover:bg-transparent hover:text-primary transition-colors font-semibold"
              @click="wroc"
            />
          </div>
          <div class="flex flex-col sm:flex-row justify-between items-start sm:items-center gap-4">
            <div>
              <p class="text-sm text-gray-400 font-semibold tracking-wider uppercase mb-1">
                {{ $t('animals.profile.subtitle') }}
              </p>
              <h1 class="text-3xl font-bold text-gray-800">{{ lokalneZwierze.imie }}</h1>
            </div>
            <div
              class="flex items-center gap-3 bg-gray-50 px-5 py-3 rounded-xl border border-gray-200 shadow-sm transition-colors"
              :class="{ 'border-yellow-400 bg-primary/5': trybEdycji }"
            >
              <label for="edycja" class="font-semibold text-gray-700 cursor-pointer select-none">
                {{ trybEdycji ? $t('animals.profile.edit_mode_on') : $t('animals.profile.edit_mode_off') }}
              </label>
              <ToggleSwitch
                id="edycja"
                v-model="trybEdycji"
                @change="!trybEdycji && anulujEdycje()"
              />
            </div>
          </div>
        </template>

        <ZwierzeForm
          ref="formularzRef"
          v-model="lokalneZwierze"
          :isReadonly="!trybEdycji"
          @fileSelected="odbierzPlikZFormularza"
        />

        <template #footer>
          <template v-if="trybEdycji">
            <Button
              :label="$t('animals.profile.cancel')"
              severity="secondary"
              icon="pi pi-times"
              outlined
              @click="anulujEdycje"
            />
            <Button
              :label="$t('animals.profile.save_changes')"
              severity="success"
              icon="pi pi-check"
              @click="zapiszZmiany"
            />
          </template>
          <template v-else>
            <Button
              :label="$t('animals.profile.delete')"
              icon="pi pi-trash"
              severity="danger"
              outlined
              @click="potwierdzUsuniecie"
            />
          </template>
        </template>
      </BaseKarta>
    </div>
    <ConfirmDialog />
    <SukcesModal
      :widoczny="pokazSukcesModal"
      :tytul="$t('animals.profile.save_success')"
      @zamknij="pokazSukcesModal = false"
    />
  </div>
</template>
