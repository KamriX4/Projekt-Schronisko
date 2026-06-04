import { ref } from 'vue'
import { defineStore } from 'pinia'

export interface ZadanieHarmonogramu {
  id: number;
  tytul: string;
  opis: string;
  kategoria: string;
  dataCzas: string;
  czyWykonane: boolean;
  zwierzeId?: number | null;
  zwierze?: any;
}

export const useHarmonogramStore = defineStore('harmonogram', () => {
  const zadania = ref<ZadanieHarmonogramu[]>([])

  const pobierzZadania = async () => {
    try {
      const response = await fetch('http://localhost:5145/api/harmonogram')
      if (response.ok) {
        zadania.value = await response.json()
      }
    } catch (error) {
      console.error('Błąd pobierania harmonogramu:', error)
    }
  }

  const dodajZadanie = async (noweZadanie: any) => {
    try {
      const response = await fetch('http://localhost:5145/api/harmonogram', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(noweZadanie)
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
      const response = await fetch(`http://localhost:5145/api/harmonogram/${id}`, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(zaktualizowaneZadanie)
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
      const response = await fetch(`http://localhost:5145/api/harmonogram/${id}`, {
        method: 'DELETE'
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
    await fetch(`http://localhost:5145/api/harmonogram/${id}/zrobione`, { method: 'PUT' })
    await pobierzZadania()
  }

  return {
    zadania,
    pobierzZadania,
    dodajZadanie,
    edytujZadanie,
    usunZadanie,
    przelaczStatus
  }
})
