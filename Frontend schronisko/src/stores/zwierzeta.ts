import { ref } from 'vue'
import { defineStore } from 'pinia'
import type { Zwierze, NoweZwierze } from '@/types/zwierze'

export const useZwierzetaStore = defineStore('zwierzeta', () => {
  // Stan (State)
  const zwierzeta = ref<Zwierze[]>([])

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

  return { zwierzeta, pobierzZwierzeta, dodajZwierze }
})
