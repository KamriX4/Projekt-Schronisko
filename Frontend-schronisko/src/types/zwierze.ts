export interface Zwierze {
  id: number
  imie: string
  gatunekId: number
  gatunek?: {
    id: number
    nazwa: string
  }
  numerEwidencyjny: string
  status: string
  numerBoksu?: string
  dataPrzyjecia: Date | string | null
  plec: string
  przyblizonaDataUrodzenia: Date | string | null
  wiekMiesiace: number
  rasa?: string
  czyZachipowany: boolean
  numerChip?: number
  umaszczenie?: string
  wielkosc?: string
  czySzczepiony: boolean
  czyKastrowanySterylizowany: boolean
  choroby?: string
  zachowanie?: string
  akceptuje?: string
  zdjecieUrl?: string
}
export type NoweZwierze = Omit<Zwierze, 'id'>;
