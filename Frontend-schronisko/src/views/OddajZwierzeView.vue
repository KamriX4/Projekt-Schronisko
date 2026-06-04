<script setup lang="ts">
import FormularzOddania from '@/components/FormularzOddania.vue'
</script>

<template>
  <div class="container mx-auto max-w-2xl px-4 py-10">
    <div class="text-center mb-6">
      <h1 class="text-3xl font-bold uppercase tracking-wide text-slate-800">
        Zgłoszenie oddania zwierzaka
      </h1>
      <p class="text-gray-500 mt-2">
        Rozumiemy, że sytuacje życiowe bywają trudne. Wypełnij poniższy formularz, aby zgłosić pupila pod opiekę naszego schroniska.
      </p>
    </div>

    <FormularzOddania />
  </div>
</template>