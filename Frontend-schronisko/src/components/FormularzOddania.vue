<template>
  <div class="card bg-base-100 shadow-xl max-w-xl mx-auto mt-2">
    <div class="card-body">
      <form @submit.prevent="wyslijWniosek" class="space-y-4">
        <h3 class="text-lg font-bold border-b pb-2">{{ $t('submission_form.your_data') }}</h3>
        <div class="form-control">
          <label class="label"><span class="label-text">{{ $t('submission_form.name') }}</span></label>
          <InputText
            v-model="formularz.imieINazwiskoOddajacego"
            :placeholder="t('submission_form.eg_name')"
            required
            class="w-full"
          />
        </div>

        <div class="grid grid-cols-2 gap-4">
          <div class="form-control">
            <label class="label"><span class="label-text">{{ $t('submission_form.phone') }}</span></label>
            <InputText
              v-model="formularz.telefon"
              :placeholder="t('submission_form.eg_phone')"
              required
              class="w-full"
            />
          </div>
          <div class="form-control">
            <label class="label"><span class="label-text">{{ $t('submission_form.email') }}</span></label>
            <InputText
              v-model="formularz.email"
              type="email"
              :placeholder="t('submission_form.eg_email')"
              required
              class="w-full"
            />
          </div>
        </div>

        <h3 class="text-lg font-bold border-b pb-2 mt-4">{{ $t('submission_form.pet_data') }}</h3>
        <div class="grid grid-cols-2 gap-4">
          <div class="form-control">
            <label class="label"><span class="label-text">{{ $t('submission_form.pet_name') }}</span></label>
            <InputText
              v-model="formularz.imieZwierzaka"
              :placeholder="t('submission_form.eg_pet_name')"
              required
              class="w-full"
            />
          </div>
          <div class="form-control">
            <label class="label"><span class="label-text">{{ $t('submission_form.species') }}</span></label>
            <select v-model="formularz.gatunek"
                    required
                    class="p-inputtext w-full">
              <option value="" disabled>{{ $t('submission_form.select') }}</option>
              <option value="Pies">{{ $t('animals.dog') }}</option>
              <option value="Kot">{{ $t('animals.cat') }}</option>
            </select>
          </div>
        </div>

        <div class="form-control">
          <label class="label"><span class="label-text">{{ $t('submission_form.reason') }}</span></label>
          <Textarea
            v-model="formularz.powodOddania"
            rows="4"
            :placeholder="t('submission_form.eg_reason')"
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
            :label="$t('submission_form.submit')"
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
import { useI18n } from 'vue-i18n'

const { t } = useI18n()
// Stan formularza (musi pasować do pól z C#!)
const formularz = ref({
  imieINazwiskoOddajacego: '',
  telefon: '',
  email: '',
  imieZwierzaka: '',
  gatunek: '',
  powodOddania: '',
})

const baseUrl = import.meta.env.VITE_API_URL;
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
    const odpowiedz = await fetch(`${baseUrl}/api/Wnioski/oddanie`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(formularz.value),
    })

    if (odpowiedz.ok) {
      wiadomoscS.value = t('submission_form.alerts.success')
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
      wiadomoscE.value = t('submission_form.alerts.error')
    }
  } catch (error) {
    wiadomoscE.value = t('submission_form.alerts.connection_error')
  } finally {
    trwaWysylanie.value = false
  }
}
</script>
