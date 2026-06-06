/** [1] VITEST — testy w useWiekZwierzecia.spec.ts */
import { computed, type Ref } from 'vue'
import { useI18n } from 'vue-i18n'

// Eksportujemy funkcję, która przyjmuje datę urodzenia jako reaktywny Ref
// i udostępnia wiek w miesiącach oraz sformatowaną wersję tekstową.
export function useWiekZwierzecia(dataUrodzenia: Ref<Date | string | null | undefined>) {
  const { t } = useI18n()

  const wiekMiesiace = computed(() => {
    if (!dataUrodzenia.value) return t('validation.missing_date')

    const birthDate = new Date(dataUrodzenia.value)
    const dzisiaj = new Date()

    if (birthDate > dzisiaj) return 0

    const roznicaCzasu = dzisiaj.getTime() - birthDate.getTime()
    const roznicaDni = roznicaCzasu / (1000 * 3600 * 24)

    // Przeliczenie dni na miesiące przybliżone średnią długością miesiąca
    return Math.floor(roznicaDni / 30.436875)
  })

  const wiekSformatowany = computed(() => {
    if (typeof wiekMiesiace.value === 'string') return wiekMiesiace.value

    const miesiace = wiekMiesiace.value
    if (miesiace < 12) return `${miesiace} mies.`

    const lata = Math.floor(miesiace / 12)
    const resztaMiesiecy = miesiace % 12
    return resztaMiesiecy > 0 ? `${lata} lat i ${resztaMiesiecy} mies.` : `${lata} lat`
  })

  return {
    wiekMiesiace,
    wiekSformatowany,
  }
}
