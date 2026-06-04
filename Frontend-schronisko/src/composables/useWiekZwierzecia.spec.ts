/**
 * [1] VITEST — testy jednostkowe composable useWiekZwierzecia.
 */
import { describe, it, expect } from 'vitest'
import { defineComponent, ref, nextTick } from 'vue'
import { mount } from '@vue/test-utils'
import { useWiekZwierzecia } from './useWiekZwierzecia'

function mountComposable(data: Date | string | null) {
  return mount(
    defineComponent({
      setup() {
        const dataRef = ref(data)
        const wynik = useWiekZwierzecia(dataRef)
        return { ...wynik, dataRef }
      },
      template: '<div />',
    }),
  )
}

describe('useWiekZwierzecia', () => {
  it('zwraca komunikat gdy brak daty urodzenia', async () => {
    const wrapper = mountComposable(null)
    await nextTick()
    expect(wrapper.vm.wiekMiesiace).toBeTruthy()
  })

  it('oblicza wiek w miesiącach dla poprawnej daty', async () => {
    const rokTemu = new Date()
    rokTemu.setFullYear(rokTemu.getFullYear() - 1)
    const wrapper = mountComposable(rokTemu.toISOString())
    await nextTick()
    expect(typeof wrapper.vm.wiekMiesiace).toBe('number')
    expect((wrapper.vm.wiekMiesiace as number) >= 11).toBe(true)
    expect(wrapper.vm.wiekSformatowany).toMatch(/lat|mies/)
  })

  it('zwraca 0 dla daty z przyszłości', async () => {
    const jutro = new Date()
    jutro.setDate(jutro.getDate() + 30)
    const wrapper = mountComposable(jutro.toISOString())
    await nextTick()
    expect(wrapper.vm.wiekMiesiace).toBe(0)
  })
})
