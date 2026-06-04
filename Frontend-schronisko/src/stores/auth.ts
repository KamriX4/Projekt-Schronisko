import { defineStore } from 'pinia'
import { ref } from 'vue'

export const useAuthStore = defineStore('auth', () => {
  const czyZalogowany = ref(false)
  const nazwaUzytkownika = ref('')
  const rola = ref('') // zmienna zapamiętująca rolę użytkownika

  const zaloguj = async (login: string, haslo: string) => {
    try {
      const response = await fetch('https://localhost:7295/api/auth/login', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ login, haslo })
      })

      const data = await response.json()

      if (response.ok) {
        czyZalogowany.value = true
        nazwaUzytkownika.value = data.login
        rola.value = data.rola // przechwytujemy rolę gosc lub pracownik
        return { sukces: true, komunikat: data.komunikat }
      } else {
        return { sukces: false, komunikat: data.komunikat }
      }
    } catch (error) {
      return { sukces: false, komunikat: 'Błąd połączenia z serwerem' }
    }
  }

  const wyloguj = () => {
    czyZalogowany.value = false
    nazwaUzytkownika.value = ''
    rola.value = '' // usuwanie roli po wylogowaniu
  }

  return { czyZalogowany, nazwaUzytkownika, rola, zaloguj, wyloguj }
})
