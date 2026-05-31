<script setup lang="ts">
import '@/assets/main.css'
import { RouterLink, RouterView } from 'vue-router'

// Funkcja zamykająca dropdown z DaisyUI po kliknięciu w link
const zamknijMenu = () => {
  const aktywnyElement = document.activeElement as HTMLElement | null
  if (aktywnyElement) {
    aktywnyElement.blur()
  }
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
          </ul>
        </div>

        <a class="btn btn-ghost text-lg font-semibold">"Nazwa Schroniska"</a>

        <div class="hidden lg:flex ml-2">
          <ul class="menu menu-horizontal px-1 gap-2">
            <li><RouterLink to="/">Strona Główna</RouterLink></li>
            <li><RouterLink to="/zwierzeta">Nasze Zwierzaki</RouterLink></li>
            <li><RouterLink to="/analityka">Analityka</RouterLink></li>
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
      </div>
    </header>
    <main class="flex-grow">
      <RouterView />
    </main>
    <footer class="footer sm:footer-horizontal footer-center bg-base-300 text-base-content p-4">
      <aside>
        <p>Copyright © {new Date().getFullYear()} - All right reserved by ACME Industries Ltd</p>
      </aside>
    </footer>
  </div>
</template>

<style scoped></style>
