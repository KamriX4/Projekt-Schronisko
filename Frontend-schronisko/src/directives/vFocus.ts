import type { Directive } from 'vue'

/*
[4] WŁASNA DYREKTYWA — v-focus
Po zamontowaniu elementu ustawia na nim fokus (np. pole wyszukiwania).
 */
export const vFocus: Directive<HTMLElement, boolean | undefined> = {
  mounted(el, binding) {
    if (binding.value === false) return
    const cel =
      el.tagName === 'INPUT' || el.tagName === 'TEXTAREA'
        ? el
        : (el.querySelector('input, textarea') as HTMLElement | null)
    cel?.focus()
  },
}
