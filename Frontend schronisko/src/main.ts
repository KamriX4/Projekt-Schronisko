import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import PrimeVue from 'primevue/config'
import Aura from '@primeuix/themes/aura'
import App from './App.vue'
import router from './router'
import 'primeicons/primeicons.css'
import { definePreset } from '@primevue/themes'
import ConfirmationService from 'primevue/confirmationservice';

const SzaryMotyw = definePreset(Aura, {
  semantic: {
    primary: {
      50: '#f8fafc',
      100: '#f1f5f9',
      200: '#e2e8f0',
      300: '#cbd5e1',
      400: '#94a3b8',
      500: '#64748b', /* Główny akcent - np. kolor ramki przy kliknięciu */
      600: '#475569',
      700: '#334155',
      800: '#1e293b',
      900: '#0f172a',
      950: '#020617'
    }
  }
});


const app = createApp(App)
app.use(PrimeVue, {
  theme: {
    preset: SzaryMotyw, // <-- Podmieniamy Aura na SzaryMotyw
    options: {
      darkModeSelector: '.p-dark', // Zostawiamy to, żeby nie wymuszał czarnego tła
    }
  },
  locale: {
    // 1 oznacza poniedziałek (0 to niedziela, domyślnie w USA)
    firstDayOfWeek: 1,

    // Dni tygodnia
    dayNames: ['Niedziela', 'Poniedziałek', 'Wtorek', 'Środa', 'Czwartek', 'Piątek', 'Sobota'],
    dayNamesShort: ['Nie', 'Pon', 'Wto', 'Śro', 'Czw', 'Pią', 'Sob'],
    dayNamesMin: ['Nd', 'Pn', 'Wt', 'Śr', 'Cz', 'Pt', 'Sb'],

    // Miesiące
    monthNames: ['Styczeń', 'Luty', 'Marzec', 'Kwiecień', 'Maj', 'Czerwiec', 'Lipiec', 'Sierpień', 'Wrzesień', 'Październik', 'Listopad', 'Grudzień'],
    monthNamesShort: ['Sty', 'Lut', 'Mar', 'Kwi', 'Maj', 'Cze', 'Lip', 'Sie', 'Wrz', 'Paź', 'Lis', 'Gru'],

    // Dodatkowe przydatne słowa w kalendarzu
    today: 'Dzisiaj',
    clear: 'Wyczyść',
    emptyMessage: 'Brak wyników',
    emptyFilterMessage: 'Brak wyników wyszukiwania'
  }
})
app.use(ConfirmationService)
app.use(createPinia())
app.use(router)
;
app.mount('#app')
