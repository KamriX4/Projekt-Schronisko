/**
 * [1] VITEST + [9] komponent funkcyjny — render badge kategorii.
 */
import { describe, it, expect } from 'vitest'
import { mount } from '@vue/test-utils'
import { ZadanieKategoriaBadge } from './ZadanieKategoriaBadge'

describe('ZadanieKategoriaBadge', () => {
  it('[9] renderuje nazwę kategorii jako komponent funkcyjny', () => {
    const wrapper = mount(ZadanieKategoriaBadge, {
      props: { kategoria: 'Leki' },
    })
    expect(wrapper.text()).toBe('Leki')
    expect(wrapper.classes().join(' ')).toMatch(/border/)
  })
})
