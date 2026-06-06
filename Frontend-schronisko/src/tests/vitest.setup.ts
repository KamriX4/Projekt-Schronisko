/*
[1] VITEST — globalna konfiguracja testów jednostkowych.
 */
import { config } from '@vue/test-utils'
import { createI18n } from 'vue-i18n'
import pl from '@/locales/pl.json'

const i18n = createI18n({
  legacy: false,
  locale: 'pl',
  messages: { pl },
})

config.global.plugins = [i18n]
