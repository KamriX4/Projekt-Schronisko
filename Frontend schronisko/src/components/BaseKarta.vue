<template>
  <div class="bg-white p-6 sm:p-8 rounded-xl shadow-lg border border-gray-100">

    <div v-if="$slots.header" class="border-b border-gray-100 pb-4 mb-6">
      <slot name="header"></slot>
    </div>

    <div>
      <slot></slot>
    </div>

    <div v-if="$slots.footer" class="mt-10 flex justify-end gap-3 border-t border-gray-100 pt-6">
      <slot name="footer"></slot>
    </div>

  </div>
</template>

<script setup lang="ts">
// Ten komponent nie potrzebuje skomplikowanej logiki, służy wyłącznie do ułożenia HTML-a!
</script>
