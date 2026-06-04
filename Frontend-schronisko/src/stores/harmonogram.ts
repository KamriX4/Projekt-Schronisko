import { ref } from 'vue'
import { defineStore } from 'pinia'

export interface ZadanieHarmonogramu {
  id: number
  tytul: string
  opis: string
  kategoria: string
  dataCzas: string
  czyWykonane: boolean
  zwierzeId?: number | null
  zwierze?: any
}

export const useHarmonogramStore = defineStore('harmonogram', () => {
  const zadania = ref<ZadanieHarmonogramu[]>([])

  const pobierzZadania = async () => {
    try {
      const response = await fetch('https://localhost:7295/api/harmonogram')
      if (response.ok) {
        zadania.value = await response.json()
      }
    } catch (error) {
      console.error('Błąd pobierania harmonogramu:', error)
    }
  }

  const dodajZadanie = async (noweZadanie: any) => {
    try {
      const response = await fetch('https://localhost:7295/api/harmonogram', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(noweZadanie),
      })
      if (response.ok) {
        await pobierzZadania()
        return true
      }
      return false
    } catch (error) {
      console.error('Błąd dodawania zadania:', error)
      return false
    }
  }

  const edytujZadanie = async (id: number, zaktualizowaneZadanie: any) => {
    try {
      const response = await fetch(`https://localhost:7295/api/harmonogram/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(zaktualizowaneZadanie),
      })
      if (response.ok) {
        await pobierzZadania()
        return true
      }
      return false
    } catch (error) {
      console.error('Błąd edycji zadania:', error)
      return false
    }
  }

  const usunZadanie = async (id: number) => {
    try {
      const response = await fetch(`https://localhost:7295/api/harmonogram/${id}`, {
        method: 'DELETE',
      })
      if (response.ok) {
        await pobierzZadania()
        return true
      }
      return false
    } catch (error) {
      console.error('Błąd usuwania zadania:', error)
      return false
    }
  }

  const przelaczStatus = async (id: number) => {
    await fetch(`https://localhost:7295/api/harmonogram/${id}/zrobione`, { method: 'PUT' })
    await pobierzZadania()
  }

  /** [6] DRAG & DROP — lokalna zmiana kolejności zadań w kolumnie (kategoria) */
  const ustawKolejnoscKategorii = (kategoria: string, nowaLista: ZadanieHarmonogramu[]) => {
    const kolejnoscKategorii = ['Zywienie', 'Szczepienia', 'Leki', 'Sprzatanie'] as const
    const wynik: ZadanieHarmonogramu[] = []
    for (const kat of kolejnoscKategorii) {
      if (kat === kategoria) {
        wynik.push(...nowaLista)
      } else {
        wynik.push(...zadania.value.filter((z) => z.kategoria === kat))
      }
    }
    zadania.value = wynik
  }

  return {
    zadania,
    pobierzZadania,
    dodajZadanie,
    edytujZadanie,
    usunZadanie,
    przelaczStatus,
    ustawKolejnoscKategorii,
  }
})
