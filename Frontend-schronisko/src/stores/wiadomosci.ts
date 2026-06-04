import { defineStore } from 'pinia'
import { ref } from 'vue'

export interface Komentarz {
  id?: number
  tresc: string
  autor: string
}

export interface Wiadomosc {
  id?: number
  tresc: string
  dataDodania?: string
  nadawca: string
  odbiorca: string
  komentarze?: Komentarz[]
}

export const useWiadomosciStore = defineStore('wiadomosci', () => {
  const listaWiadomosci = ref<Wiadomosc[]>([])

  const pobierzWiadomosci = async (loginUzytkownika: string) => {
    const response = await fetch(`http://localhost:5145/api/Wiadomosci/${loginUzytkownika}`)
    if (response.ok) listaWiadomosci.value = await response.json()
  }

  const wyslijWiadomosc = async (nowaWiadomosc: Wiadomosc) => {
    const response = await fetch('http://localhost:5145/api/Wiadomosci', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(nowaWiadomosc)
    })
    return response.ok
  }

  // DODANE: brakujące funkcje
  const usunWiadomosc = async (id: number, login: string) => {
    const response = await fetch(`http://localhost:5145/api/Wiadomosci/${id}?loginUzytkownika=${login}`, {
      method: 'DELETE'
    })
    return response.ok
  }

  const dodajKomentarz = async (id: number, komentarz: Komentarz) => {
    const response = await fetch(`http://localhost:5145/api/Wiadomosci/${id}/komentarz`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(komentarz)
    })
    return response.ok
  }

  return { listaWiadomosci, pobierzWiadomosci, wyslijWiadomosc, usunWiadomosc, dodajKomentarz }
})
