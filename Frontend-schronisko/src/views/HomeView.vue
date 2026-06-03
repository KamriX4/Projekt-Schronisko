<script setup lang="ts">
import { RouterLink } from 'vue-router'
</script>

<template>
  <div class="flex items-center justify-center min-h-[75vh]">
    <div class="text-center bg-base-100 p-10 rounded-2xl shadow-xl max-w-2xl border border-gray-100">
      <h1 class="text-5xl font-bold text-gray-800 mb-6">Witamy w naszym Schronisku!</h1>
      <p class="text-lg text-gray-600 mb-8">
        Jesteśmy tu po to, aby łączyć wspaniałe zwierzęta z kochającymi ludźmi.
        Możesz swobodnie przeglądać profile naszych podopiecznych.
      </p>

      <RouterLink to="/zwierzeta" class="btn btn-primary btn-lg font-bold shadow-md">
        Poznaj nasze zwierzaki
      </RouterLink>
    </div>
  </div>
</template>
