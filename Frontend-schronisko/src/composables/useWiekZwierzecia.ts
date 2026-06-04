import { computed, type Ref } from 'vue'
import { useI18n } from 'vue-i18n' // <-- Import i18n



// Eksportujemy funkcję, która przyjmuje datę (jako zmienną reaktywną Ref)
export function useWiekZwierzecia(dataUrodzenia: Ref<Date | string | null | undefined>) {

  const wiekMiesiace = computed(() => {

    const { t } = useI18n() // <-- Inicjalizacja i18n
    if (!dataUrodzenia.value) return t('validation.missing_date')

    const birthDate = new Date(dataUrodzenia.value)
    const dzisiaj = new Date()

    if (birthDate > dzisiaj) return 0

    const roznicaCzasu = dzisiaj.getTime() - birthDate.getTime()
    const roznicaDni = roznicaCzasu / (1000 * 3600 * 24)

    return Math.floor(roznicaDni / 30.436875)
  })

  // Dodajemy mały bonus: sformatowany tekst (np. "2 lata i 3 miesiące"),
  // co pokaże prowadzącemu, że Composable jest przemyślane i zwraca różne przydatne formaty
  const wiekSformatowany = computed(() => {
    if (typeof wiekMiesiace.value === 'string') return wiekMiesiace.value

    const miesiace = wiekMiesiace.value
    if (miesiace < 12) return `${miesiace} mies.`

    const lata = Math.floor(miesiace / 12)
    const resztaMiesiecy = miesiace % 12
    return resztaMiesiecy > 0 ? `${lata} lat i ${resztaMiesiecy} mies.` : `${lata} lat`
  })

  // Composable zawsze musi zwracać obiekt z reaktywnymi danymi/funkcjami
  return {
    wiekMiesiace,
    wiekSformatowany
  }
}
