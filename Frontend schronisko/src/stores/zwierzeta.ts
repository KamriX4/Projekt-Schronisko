import { ref } from 'vue'
import { defineStore } from 'pinia'
import type { Zwierze, NoweZwierze } from '@/types/zwierze'

export const useZwierzetaStore = defineStore('zwierzeta', () => {
  // Stan (State)
  const zwierzeta = ref<Zwierze[]>([])

  // NOWE: Zmienna trzymająca dane tylko tego zwierzaka, którego aktualnie oglądamy
  const aktualneZwierze = ref<Zwierze | null>(null)

  // Akcja: Pobieranie
  const pobierzZwierzeta = async () => {
    try {
      const response = await fetch('https://localhost:7295/api/zwierze')
      if (response.ok) {
        zwierzeta.value = await response.json()
      }
    } catch (error) {
      console.error('Błąd pobierania danych:', error)
    }
  }

  // NOWE: Akcja pobierania jednego konkretnego zwierzaka po ID
  const pobierzZwierze = async (id: number) => {
    try {
      aktualneZwierze.value = null // Czyścimy stare dane, żeby nie "mignęły" na ekranie

      const response = await fetch(`https://localhost:7295/api/zwierze/${id}`)

      if (response.ok) {
        aktualneZwierze.value = await response.json()
      } else {
        console.error('Serwer zwrócił błąd: Nie znaleziono zwierzaka')
      }
    } catch (error) {
      console.error('Błąd podczas pobierania szczegółów zwierzaka:', error)
    }
  }

  // Akcja: Dodawanie
  const dodajZwierze = async (noweZwierze: NoweZwierze) => {
    try {
      const response = await fetch('https://localhost:7295/api/zwierze', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(noweZwierze),
      })

      if (response.ok) {
        const stworzoneZwierze = await response.json()
        zwierzeta.value.unshift(stworzoneZwierze) // Dodaj na początek listy
        await pobierzZwierzeta() // Pobierz ponownie, aby załadować relacje (Gatunek)
        return true
      }
      return false
    } catch (error) {
      console.error('Błąd dodawania zwierzaka:', error)
      return false
    }
  }

  // Akcja: Edycja (PUT)
  const edytujZwierze = async (id: number, zaktualizowaneDane: Zwierze) => {
    try {
      const response = await fetch(`https://localhost:7295/api/zwierze/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(zaktualizowaneDane),
      })

      if (response.ok) {
        // Opcjonalnie: zaktualizuj zwierzaka na głównej liście w pamięci (jeśli jest załadowana),
        // żeby po powrocie do listy od razu było widać zmiany bez drugiego strzału do API.
        const index = zwierzeta.value.findIndex(z => z.id === id)
        if (index !== -1) {
          zwierzeta.value[index] = { ...zwierzeta.value[index], ...zaktualizowaneDane }
        }

        // Zaktualizuj aktualnie oglądanego zwierzaka
        aktualneZwierze.value = { ...zaktualizowaneDane }

        return true
      }

      console.error('Błąd serwera podczas zapisywania zmian')
      return false
    } catch (error) {
      console.error('Błąd połączenia podczas edytowania zwierzaka:', error)
      return false
    }
  }

  return {
    zwierzeta,
    aktualneZwierze,
    pobierzZwierzeta,
    pobierzZwierze,
    dodajZwierze,
    edytujZwierze,
  }
})
