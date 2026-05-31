import { createI18n } from 'vue-i18n'
import pl from './locales/pl.json'
import en from './locales/en.json'

const i18n = createI18n({
  legacy: false, // BARDZO WAŻNE: wyłącza tryb Options API, pozwala na użycie z Composition API
  locale: 'pl',  // Język domyślny
  fallbackLocale: 'en', // Język rezerwowy, gdyby brakowało jakiegoś tłumaczenia
  messages: {
    pl,
    en
  }
})

export default i18n
