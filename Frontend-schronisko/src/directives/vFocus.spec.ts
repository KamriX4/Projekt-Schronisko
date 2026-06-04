/**
 * [1] VITEST + [4] własna dyrektywa v-focus.
 */
import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import { nextTick } from 'vue'
import { vFocus } from './vFocus'

describe('vFocus', () => {
  it('ustawia fokus na natywnym input', async () => {
    const wrapper = mount(
      {
        template: '<input v-focus data-testid="inp" />',
        directives: { focus: vFocus },
      },
      { attachTo: document.body },
    )
    await nextTick()
    const input = wrapper.get('[data-testid="inp"]').element as HTMLInputElement
    expect(document.activeElement).toBe(input)
    wrapper.unmount()
  })
})
