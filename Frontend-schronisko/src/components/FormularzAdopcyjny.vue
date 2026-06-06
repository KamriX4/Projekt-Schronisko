<template>
  <div class="card bg-base-100 shadow-xl max-w-lg mx-auto mt-10">
    <div class="card-body">
      <h2 class="card-title text-2xl mb-4">Wniosek Adopcyjny</h2>

      <form @submit.prevent="wyslijWniosek" class="space-y-4">
        <div class="form-control">
          <label class="label"><span class="label-text">Imię i Nazwisko</span></label>
          <InputText
            v-model="formularz.imieINazwisko"
            placeholder="np. Jan Kowalski"
            required
            class="w-full"
          />
        </div>

        <div class="form-control">
          <label class="label"><span class="label-text">Numer telefonu</span></label>
          <InputText
            v-model="formularz.telefon"
            placeholder="np. 123456789"
            required
            class="w-full"
          />
        </div>

        <div class="form-control">
          <label class="label"><span class="label-text">Adres E-mail</span></label>
          <InputText
            v-model="formularz.email"
            type="email"
            placeholder="np. jan@example.com"
            required
            class="w-full"
          />
        </div>

        <div class="form-control">
          <label class="label"
            ><span class="label-text">Dlaczego chcesz adoptować tego zwierzaka?</span></label
          >
          <Textarea
            v-model="formularz.uzasadnienie"
            rows="4"
            placeholder="Napisz kilka słów..."
            required
            class="w-full"
          />
        </div>

        <div v-if="wiadomoscS" class="alert alert-success mt-4">
          <span>{{ wiadomoscS }}</span>
        </div>
        <div v-if="wiadomoscE" class="alert alert-error mt-4">
          <span>{{ wiadomoscE }}</span>
        </div>

        <div class="card-actions justify-end mt-6">
          <Button
            type="submit"
            label="Wyślij Wniosek"
            :loading="trwaWysylanie"
            class="p-button-primary"
          />
        </div>
      </form>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue'
import InputText from 'primevue/inputtext'
import Textarea from 'primevue/textarea'
import Button from 'primevue/button'

//IMPORTUJEMY MAGAZYN ZWIERZĄT
import { useZwierzetaStore } from '@/stores/zwierzeta'

// odbieramy ID zwierzaka, z którego karty otworzono formularz
const props = defineProps<{
  zwierzeId: number
}>()

//URUCHAMIAMY MAGAZYN ZWIERZĄT
const zwierzetaStore = useZwierzetaStore()

// formularz
const formularz = ref({
  imieINazwisko: '',
  telefon: '',
  email: '',
  uzasadnienie: '',
})

// Zmienne pomocnicze
const trwaWysylanie = ref(false)
const wiadomoscS = ref('')
const wiadomoscE = ref('')

const baseUrl = import.meta.env.VITE_API_URL
// Funkcja wysyłająca dane do C#
const wyslijWniosek = async () => {
  trwaWysylanie.value = true
  wiadomoscS.value = ''
  wiadomoscE.value = ''

  // Składamy dane do wysyłki
  const daneDoWyslania = {
    imieINazwisko: formularz.value.imieINazwisko,
    telefon: formularz.value.telefon,
    email: formularz.value.email,
    uzasadnienie: formularz.value.uzasadnienie,
    zwierzeId: props.zwierzeId,
  }

  try {
    const odpowiedz = await fetch(`${baseUrl}/api/Wnioski/adopcja`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(daneDoWyslania),
    })

    if (odpowiedz.ok) {
      wiadomoscS.value = 'Wniosek został wysłany!'

      // Czyszczenie formularza po sukcesie
      formularz.value = { imieINazwisko: '', telefon: '', email: '', uzasadnienie: '' }

      // Szukamy w magazynie zwierzaka, którego dotyczy ten wniosek
      const obecneZwierze = zwierzetaStore.zwierzeta.find((z) => z.id === props.zwierzeId)

      if (obecneZwierze) {
        // Zmieniamy jego status na "Zarezerwowany" w bazie danych
        await zwierzetaStore.edytujZwierze(props.zwierzeId, {
          ...obecneZwierze,
          status: 'Zarezerwowany',
        })
      }

      // Na sam koniec zmuszamy przeglądarkę do pobrania świeżej listy z nowym statusem
      await zwierzetaStore.pobierzZwierzeta()
    } else {
      wiadomoscE.value = 'Wystąpił błąd podczas wysyłania. Sprawdź poprawność danych.'
    }
  } catch (error) {
    wiadomoscE.value = 'Błąd połączenia z serwerem. Upewnij się, że backend działa!'
  } finally {
    trwaWysylanie.value = false
  }
}
</script>
