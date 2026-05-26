<script setup lang="ts">
import '@/assets/main.css'
import { RouterLink, RouterView } from 'vue-router'
</script>

<template>
  <header>
    <nav>
      <RouterLink to="/">Home</RouterLink> |
      <RouterLink to="/zwierzeta">Zwierzęta</RouterLink>
    </nav>
  </header>

  <RouterView />
</template>

<style scoped></style>
