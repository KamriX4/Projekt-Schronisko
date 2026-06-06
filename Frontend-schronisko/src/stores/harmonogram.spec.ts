/*
[1] VITEST — testy store harmonogramu (m.in. drag & drop / kolejność).
 */
import { describe, it, expect, beforeEach } from 'vitest'
import { setActivePinia, createPinia } from 'pinia'
import { useHarmonogramStore, type ZadanieHarmonogramu } from './harmonogram'

function zadanie(id: number, kategoria: string): ZadanieHarmonogramu {
  return {
    id,
    tytul: `Zadanie ${id}`,
    opis: '',
    kategoria,
    dataCzas: '2026-01-01T10:00:00',
    czyWykonane: false,
  }
}

describe('useHarmonogramStore', () => {
  beforeEach(() => {
    setActivePinia(createPinia())
  })

  it('[6] ustawKolejnoscKategorii zmienia kolejność tylko w wybranej kategorii', () => {
    const store = useHarmonogramStore()
    store.zadania = [
      zadanie(1, 'Zywienie'),
      zadanie(2, 'Zywienie'),
      zadanie(3, 'Leki'),
    ]

    store.ustawKolejnoscKategorii('Zywienie', [zadanie(2, 'Zywienie'), zadanie(1, 'Zywienie')])

    const zywienie = store.zadania.filter((z) => z.kategoria === 'Zywienie')
    expect(zywienie.map((z) => z.id)).toEqual([2, 1])
    expect(store.zadania.find((z) => z.id === 3)?.kategoria).toBe('Leki')
  })
})
