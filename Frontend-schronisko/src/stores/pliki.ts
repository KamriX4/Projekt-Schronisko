// src/services/plikiService.ts

const API_BASE_URL = 'http://localhost:5145/api' // Docelowo pobierane z pliku .env

export async function uploadZdjecia(fizycznyPlik: File): Promise<string> {
  const formData = new FormData()
  formData.append('plik', fizycznyPlik)

  const odpowiedz = await fetch(`${API_BASE_URL}/pliki/upload`, {
    method: 'POST',
    body: formData,
  })

  if (!odpowiedz.ok) {
    throw new Error('Błąd serwera podczas wgrywania pliku.')
  }

  const dane = await odpowiedz.json()
  return dane.url // Zwracamy sam wygenerowany string URL
}
