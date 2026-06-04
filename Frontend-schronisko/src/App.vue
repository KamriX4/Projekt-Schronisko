<script setup lang="ts">
import '@/assets/main.css'
import { RouterLink, RouterView } from 'vue-router'
import { useI18n } from 'vue-i18n'
import Button from 'primevue/button'

const { locale } = useI18n()

// Funkcja zamykająca dropdown z DaisyUI po kliknięciu w link
const zamknijMenu = () => {
  const aktywnyElement = document.activeElement as HTMLElement | null
  if (aktywnyElement) {
    aktywnyElement.blur()
  }
}
const zmienJezyk = (nowyJezyk: string) => {
  locale.value = nowyJezyk // Zmiana tej wartości natychmiast tłumaczy całą stronę!
}
</script>

<template>
  <div class="min-h-screen flex flex-col">
    <header class="navbar bg-base-100 shadow-sm px-4">
      <div class="navbar-start flex-1 lg:w-auto">
        <div class="dropdown">
          <div tabindex="0" role="button" class="btn btn-ghost lg:hidden">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              class="h-5 w-5"
              fill="none"
              viewBox="0 0 24 24"
              stroke="currentColor"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M4 6h16M4 12h8m-8 6h16"
              />
            </svg>
          </div>

          <ul
            tabindex="0"
            class="menu menu-sm dropdown-content bg-base-100 rounded-box z-[1] mt-3 w-52 p-2 shadow"
            @click="zamknijMenu"
          >
            <li><RouterLink to="/">Strona Główna</RouterLink></li>
            <li><RouterLink to="/zwierzeta">Nasze Zwierzaki</RouterLink></li>
            <li><RouterLink to="/analityka">Analityka</RouterLink></li>
            <li><RouterLink to="/harmonogram">Harmonogram</RouterLink></li>
          </ul>
        </div>

        <a class="btn btn-ghost text-lg font-semibold">"Nazwa Schroniska"</a>

        <div class="hidden lg:flex ml-2">
          <ul class="menu menu-horizontal px-1 gap-2">
            <li><RouterLink to="/">Strona Główna</RouterLink></li>
            <li><RouterLink to="/zwierzeta">Nasze Zwierzaki</RouterLink></li>
            <li><RouterLink to="/analityka">Analityka</RouterLink></li>
            <li><RouterLink to="/harmonogram">Harmonogram</RouterLink></li>
          </ul>
        </div>
      </div>

      <div class="navbar-end w-auto">
        <div class="dropdown dropdown-end">
          <div tabindex="0" role="button" class="btn btn-ghost btn-circle avatar">
            <div class="w-10 rounded-full">
              <img
                alt="Avatar użytkownika"
                src="https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.webp"
              />
            </div>
          </div>
          <ul
            tabindex="-1"
            class="menu menu-sm dropdown-content bg-base-100 rounded-box z-[1] mt-3 w-52 p-2 shadow"
            @click="zamknijMenu"
          >
            <li>
              <a class="justify-between">
                Profil
                <span class="badge badge-primary">Nowy</span>
              </a>
            </li>
            <li><a>Ustawienia</a></li>
            <li><a>Wyloguj</a></li>
          </ul>
        </div>
        <div class="flex gap-2 p-4 justify-end">
          <Button
            label="PL"
            :outlined="locale !== 'pl'"
            severity="secondary"
            @click="zmienJezyk('pl')"
          />
          <Button
            label="EN"
            :outlined="locale !== 'en'"
            severity="secondary"
            @click="zmienJezyk('en')"
          />
        </div>
      </div>
    </header>
    <main class="flex-grow">
      <RouterView v-slot="{ Component, route }">
        <Transition name="slide-fade" mode="out-in">
          <component :is="Component" :key="route.path" />
        </Transition>
      </RouterView>
    </main>
    <footer class="footer sm:footer-horizontal footer-center bg-base-300 text-base-content p-4">
      <aside>
        <p>Copyright © {new Date().getFullYear()} - All right reserved by ACME Industries Ltd</p>
      </aside>
    </footer>
  </div>
</template>

<style scoped>
/* Definiujemy czas trwania i rodzaj krzywej przejścia.
  Wartość 0.3s daje płynny, ale nienużący efekt.
*/
.slide-fade-enter-active {
  transition: all 0.3s ease-out;
}

.slide-fade-leave-active {
  transition: all 0.3s cubic-bezier(1, 0.5, 0.8, 1);
}

/* Stan początkowy przy wchodzeniu na stronę (enter-from)
  oraz stan końcowy przy wychodzeniu z niej (leave-to).

  transform: translateY(20px) sprawi, że nowa strona delikatnie
  wjedzie z dołu do góry, przy okazji stając się w pełni widoczna (opacity).
*/
.slide-fade-enter-from,
.slide-fade-leave-to {
  transform: translateY(20px);
  opacity: 0;
}
</style>
