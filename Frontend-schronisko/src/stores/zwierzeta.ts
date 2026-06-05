import { ref } from 'vue'
import { defineStore } from 'pinia'
import type { Zwierze, NoweZwierze } from '@/types/zwierze'

const baseUrl = import.meta.env.VITE_API_URL

export const useZwierzetaStore = defineStore('zwierzeta', () => {
  // Stan (State)
  const zwierzeta = ref<Zwierze[]>([])

  // NOWE: Zmienna trzymająca dane tylko tego zwierzaka, którego aktualnie oglądamy
  const aktualneZwierze = ref<Zwierze | null>(null)

  const formatujDateLokalnie = (data: Date | string | null): string | null => {
    if (!data) return null

    const d = new Date(data)

    // Neutralizujemy offset: przesuwamy czas sztucznie do przodu,
    // aby po odcięciu strefy czasowej przez toISOString() data pozostała nienaruszona.
    d.setMinutes(d.getMinutes() - d.getTimezoneOffset())

    return d.toISOString().split('T')[0] ?? null // Zawsze zwróci bezpieczne "YYYY-MM-DD"
  }

  // Akcja: Pobieranie
  const pobierzZwierzeta = async () => {
    try {
      const response = await fetch(`${baseUrl}/api/zwierze`)
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

      const response = await fetch(`${baseUrl}/api/zwierze/${id}`)

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
      // 1. Formatujemy daty przed wysłaniem
      const payloadDoWyslania = {
        ...noweZwierze,
        dataPrzyjecia: formatujDateLokalnie(noweZwierze.dataPrzyjecia),
        przyblizonaDataUrodzenia: formatujDateLokalnie(noweZwierze.przyblizonaDataUrodzenia),
      }

      const response = await fetch(`${baseUrl}/api/zwierze`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payloadDoWyslania), // 2. Wysyłamy bezpieczny ładunek
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
      // 1. Formatujemy daty przed wysłaniem
      const payloadDoWyslania = {
        ...zaktualizowaneDane,
        dataPrzyjecia: formatujDateLokalnie(zaktualizowaneDane.dataPrzyjecia),
        przyblizonaDataUrodzenia: formatujDateLokalnie(zaktualizowaneDane.przyblizonaDataUrodzenia),
      }

      const response = await fetch(`${baseUrl}/api/zwierze/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(payloadDoWyslania), // 2. Wysyłamy bezpieczny ładunek
      })

      if (response.ok) {
        // Opcjonalnie: zaktualizuj zwierzaka na głównej liście w pamięci (jeśli jest załadowana),
        // żeby po powrocie do listy od razu było widać zmiany bez drugiego strzału do API.
        const index = zwierzeta.value.findIndex((z) => z.id === id)
        if (index !== -1) {
          // Aktualizujemy listę DANYMI Z PAYLOADU, żeby Vue miało poprawne daty w pamięci
          zwierzeta.value[index] = { ...zwierzeta.value[index], ...payloadDoWyslania } as Zwierze
        }

        // Zaktualizuj aktualnie oglądanego zwierzaka (też danymi z payloadu)
        aktualneZwierze.value = { ...payloadDoWyslania } as Zwierze

        return true
      }

      console.error('Błąd serwera podczas zapisywania zmian')
      return false
    } catch (error) {
      console.error('Błąd połączenia podczas edytowania zwierzaka:', error)
      return false
    }
  }

  // Akcja: Usuwanie (DELETE)
  const usunZwierze = async (id: number) => {
    try {
      const response = await fetch(`${baseUrl}/api/zwierze/${id}`, {
        method: 'DELETE',
      })

      if (response.ok) {
        // 1. Usuwamy zwierzaka z lokalnej listy, żeby UI od razu się zaktualizowało
        zwierzeta.value = zwierzeta.value.filter((z) => z.id !== id)

        // 2. Jeśli usuwany zwierzak był akurat otwarty w podglądzie, czyścimy stan
        if (aktualneZwierze.value && aktualneZwierze.value.id === id) {
          aktualneZwierze.value = null
        }

        return true
      }

      console.error('Błąd serwera podczas usuwania zwierzaka')
      return false
    } catch (error) {
      console.error('Błąd połączenia podczas usuwania zwierzaka:', error)
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
    usunZwierze,
  }
})
