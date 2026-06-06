import { h, type FunctionalComponent } from 'vue'

/*
[9] KOMPONENT FUNKCYJNY — renderuje tylko badge kategorii (bez stanu wewnętrznego).
FunctionalComponent w Vue 3 = funkcja zwracająca VNode przez h().
 */
export interface BadgeProps {
  kategoria: string
  kolorRamki?: string
}

const koloryKategorii: Record<string, string> = {
  Zywienie: 'border-primary',
  Szczepienia: 'border-info',
  Leki: 'border-error',
  Sprzatanie: 'border-accent',
}

export const ZadanieKategoriaBadge: FunctionalComponent<BadgeProps> = (props) => {
  const klasaRamki = koloryKategorii[props.kategoria] ?? 'border-gray-300'
  return h(
    'span',
    {
      class: [
        'inline-block text-xs font-semibold uppercase tracking-wide px-2 py-0.5 rounded border',
        klasaRamki,
      ],
      style: props.kolorRamki ? { borderColor: props.kolorRamki } : undefined,
    },
    props.kategoria,
  )
}

ZadanieKategoriaBadge.props = ['kategoria', 'kolorRamki']
