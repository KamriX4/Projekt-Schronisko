import type { InjectionKey, Ref } from 'vue'

/*
[5] PROVIDE / INJECT — klucz i typ kontekstu aplikacji schroniska.
App.vue wywołuje provide(), komponenty potomne inject().
 */
export interface SchroniskoContext {
  /** Kolor akcentu UI (reaktywny, współdzielony w drzewie komponentów) */
  kolorAkcentu: Ref<string>
  ustawKolorAkcentu: (hex: string) => void
}

export const schroniskoContextKey: InjectionKey<SchroniskoContext> = Symbol('schroniskoContext')
