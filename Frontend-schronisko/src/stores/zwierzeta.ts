import { ref } from 'vue'
import { defineStore } from 'pinia'
import type { Zwierze, NoweZwierze } from '@/types/zwierze'

const baseUrl = import.meta.env.VITE_API_URL

export const useZwierzetaStore = defineStore('zwierzeta', () => {

  const zwierzeta = ref<Zwierze[]>([])

  const aktualneZwierze = ref<Zwierze | null>(null)

  const formatujDateLokalnie = (data: Date | string | null): string | null => {
    if (!data) return null

    const d = new Date(data)

    d.setMinutes(d.getMinutes() - d.getTimezoneOffset())

    return d.toISOString().split('T')[0] ?? null
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

  const pobierzZwierze = async (id: number) => {
    try {
      aktualneZwierze.value = null // Czyścimy stan przed pobraniem, żeby nie pokazywać starego zwierzaka podczas ładowania

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
        body: JSON.stringify(payloadDoWyslania),
      })

      if (response.ok) {
        const stworzoneZwierze = await response.json()
        zwierzeta.value.unshift(stworzoneZwierze)
        await pobierzZwierzeta()
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
        body: JSON.stringify(payloadDoWyslania),
      })

      if (response.ok) {
        const index = zwierzeta.value.findIndex((z) => z.id === id)
        if (index !== -1) {
          zwierzeta.value[index] = { ...zwierzeta.value[index], ...payloadDoWyslania } as Zwierze
        }

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
        zwierzeta.value = zwierzeta.value.filter((z) => z.id !== id)

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
