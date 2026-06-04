<script setup lang="ts">
import { ref } from 'vue'
import { useAuthStore } from '@/stores/auth'
import { useWiadomosciStore } from '@/stores/wiadomosci'
import Button from 'primevue/button'
import Textarea from 'primevue/textarea'

const props = defineProps<{ otwarty: boolean }>()
const emit = defineEmits(['zamknij', 'sukces'])

const authStore = useAuthStore()
const wiadomosciStore = useWiadomosciStore()

const tresc = ref('')
const odbiorca = ref('Wszyscy') // Domyślnie dla wszystkich
const ladowanie = ref(false)

const zamknij = () => {
  tresc.value = ''
  odbiorca.value = 'Wszyscy'
  emit('zamknij')
}

const wyslij = async () => {
  if (!tresc.value.trim()) return

  ladowanie.value = true
  const sukces = await wiadomosciStore.wyslijWiadomosc({
    tresc: tresc.value,
    odbiorca: odbiorca.value,
    nadawca: authStore.nazwaUzytkownika
  })
  ladowanie.value = false

  if (sukces) {
    emit('sukces')
    zamknij()
  } else {
    alert('Nie udało się wysłać wiadomości.')
  }
}
</script>

<template>
  <div class="modal" :class="{ 'modal-open': otwarty }">
    <div class="modal-box relative">
      <button class="btn btn-sm btn-circle btn-ghost absolute right-2 top-2" @click="zamknij">✕</button>

      <h3 class="font-bold text-2xl mb-4 text-center">Nowa Wiadomość</h3>

      <div class="form-control w-full mb-4">
        <label class="label"><span class="label-text font-semibold">Adresat wiadomości</span></label>
        <select v-model="odbiorca" class="select select-bordered w-full">
          <option value="Wszyscy">Wszyscy pracownicy</option>
          <option v-if="authStore.nazwaUzytkownika !== 'pracownik1'" value="pracownik1">pracownik1</option>
          <option v-if="authStore.nazwaUzytkownika !== 'pracownik2'" value="pracownik2">pracownik2</option>
        </select>
      </div>

      <div class="form-control w-full">
        <label class="label"><span class="label-text font-semibold">Treść ogłoszenia</span></label>
        <Textarea v-model="tresc" rows="4" placeholder="Wpisz treść komunikatu..." class="w-full" autoResize />
      </div>

      <div class="modal-action mt-6">
        <Button label="Wyślij komunikat" icon="pi pi-send" class="w-full p-3 font-bold" severity="success" :loading="ladowanie" @click="wyslij" />
      </div>
    </div>
    <form method="dialog" class="modal-backdrop" @click="zamknij"><button>zamknij</button></form>
  </div>
</template>
