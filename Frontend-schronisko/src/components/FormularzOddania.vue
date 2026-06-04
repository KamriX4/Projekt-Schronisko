<template>
  <div class="card bg-base-100 shadow-xl max-w-xl mx-auto mt-2">
    <div class="card-body">
      <form @submit.prevent="wyslijWniosek" class="space-y-4">
        <h3 class="text-lg font-bold border-b pb-2">Twoje dane</h3>
        <div class="form-control">
          <label class="label"><span class="label-text">Imię i Nazwisko</span></label>
          <InputText
            v-model="formularz.imieINazwiskoOddajacego"
            placeholder="np. Anna Nowak"
            required
            class="w-full"
          />
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div class="form-control">
            <label class="label"><span class="label-text">Telefon</span></label>
            <InputText
              v-model="formularz.telefon"
              placeholder="np. 123456789"
              required
              class="w-full"
            />
          </div>
          <div class="form-control">
            <label class="label"><span class="label-text">E-mail</span></label>
            <InputText
              v-model="formularz.email"
              type="email"
              placeholder="np. anna@example.com"
              required
              class="w-full"
            />
          </div>
        </div>

        <h3 class="text-lg font-bold border-b pb-2 mt-4">Dane zwierzaka</h3>
        <div class="grid grid-cols-2 gap-4">
          <div class="form-control">
            <label class="label"><span class="label-text">Imię zwierzaka</span></label>
            <InputText
              v-model="formularz.imieZwierzaka"
              placeholder="np. Puszek"
              required
              class="w-full"
            />
          </div>
          <div class="form-control">
            <label class="label"><span class="label-text">Gatunek</span></label>
            <InputText
              v-model="formularz.gatunek"
              placeholder="np. Pies, Kot"
              required
              class="w-full"
            />
          </div>
        </div>

        <div class="form-control">
          <label class="label"><span class="label-text">Powód oddania</span></label>
          <Textarea
            v-model="formularz.powodOddania"
            rows="4"
            placeholder="Napisz krótko, dlaczego musisz oddać zwierzaka..."
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
            label="Wyślij formularz"
            icon="pi pi-send"
            :loading="trwaWysylanie"
            severity="danger"
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

// Stan formularza (musi pasować do pól z C#!)
const formularz = ref({
  imieINazwiskoOddajacego: '',
  telefon: '',
  email: '',
  imieZwierzaka: '',
  gatunek: '',
  powodOddania: '',
})

// Zmienne pomocnicze
const trwaWysylanie = ref(false)
const wiadomoscS = ref('')
const wiadomoscE = ref('')

// Funkcja wysyłająca
const wyslijWniosek = async () => {
  trwaWysylanie.value = true
  wiadomoscS.value = ''
  wiadomoscE.value = ''

  try {
    const odpowiedz = await fetch('http://localhost:5145/api/Wnioski/oddanie', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(formularz.value),
    })

    if (odpowiedz.ok) {
      wiadomoscS.value = 'Formularz został wysłany.'
      // Czyszczenie formularza
      formularz.value = {
        imieINazwiskoOddajacego: '',
        telefon: '',
        email: '',
        imieZwierzaka: '',
        gatunek: '',
        powodOddania: '',
      }
    } else {
      wiadomoscE.value = 'Wystąpił błąd. Sprawdź poprawność wpisanych danych (np. format telefonu).'
    }
  } catch (error) {
    wiadomoscE.value = 'Błąd połączenia z serwerem. Upewnij się, że backend działa.'
  } finally {
    trwaWysylanie.value = false
  }
}
</script>
