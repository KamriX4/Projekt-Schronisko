<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'

// Odbieramy polecenie otwarcia od pliku App.vue
const props = defineProps<{ otwarty: boolean }>()
const emit = defineEmits(['zamknij'])

const authStore = useAuthStore()

const login = ref('')
const haslo = ref('')
const komunikatBledu = ref('')
const trwaLogowanie = ref(false)

// Funkcja, która czyści pola i zamyka okienko
const zamknij = () => {
  komunikatBledu.value = ''
  login.value = ''
  haslo.value = ''
  emit('zamknij')
}

const zaloguj = async () => {
  if (!login.value || !haslo.value) {
    komunikatBledu.value = 'Wpisz login i hasło.'
    return
  }

  trwaLogowanie.value = true
  komunikatBledu.value = ''

  const odpowiedz = await authStore.zaloguj(login.value, haslo.value)
  trwaLogowanie.value = false

  if (odpowiedz.sukces) {
    zamknij() // Zamykamy okienko, logowanie się udało!
  } else {
    komunikatBledu.value = odpowiedz.komunikat
  }
}
</script>

<template>
  <!--DaisyUI -->
  <div class="modal" :class="{ 'modal-open': otwarty }">
    <div class="modal-box relative">
      <!-- Krzyżyk w rogu -->
      <button class="btn btn-sm btn-circle btn-ghost absolute right-2 top-2" @click="zamknij">✕</button>

      <h3 class="font-bold text-2xl mb-4 text-center">Panel Logowania</h3>

      <div class="form-control w-full">
        <label class="label"><span class="label-text font-semibold">Twój login</span></label>
        <InputText v-model="login" placeholder="Wpisz login..." class="w-full" />
      </div>

      <div class="form-control w-full mt-3">
        <label class="label"><span class="label-text font-semibold">Hasło</span></label>
        <InputText type="password" v-model="haslo" placeholder="Wpisz hasło..." class="w-full" @keyup.enter="zaloguj" />
      </div>

      <p v-if="komunikatBledu" class="text-error text-sm mt-3 text-center">
        {{ komunikatBledu }}
      </p>

      <div class="modal-action mt-6">
        <Button label="Zaloguj się" icon="pi pi-sign-in" class="w-full p-3 font-bold" :loading="trwaLogowanie" @click="zaloguj" />
      </div>
    </div>
    <!-- Kliknięcie w ciemne tło zamyka okienko -->
    <form method="dialog" class="modal-backdrop" @click="zamknij">
      <button>zamknij</button>
    </form>
  </div>
</template>
