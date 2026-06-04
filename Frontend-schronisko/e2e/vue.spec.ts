/**
 * [2] PLAYWRIGHT — testy E2E (end-to-end) aplikacji schroniska.
 */
import { test, expect } from '@playwright/test'

test.describe('Schronisko Nasze Ogonki', () => {
  test('strona główna wyświetla branding schroniska', async ({ page }) => {
    await page.goto('/')
    await expect(page.getByText('Nasze Ogonki').first()).toBeVisible()
    await expect(page.locator('h1')).toBeVisible()
  })

  test('nawigacja do listy zwierząt', async ({ page }) => {
    await page.goto('/')
    await page.getByRole('link', { name: /Nasi podopieczni|animals/i }).first().click()
    await expect(page).toHaveURL(/\/zwierzeta/)
    await expect(page.locator('h1')).toBeVisible()
  })

  test('harmonogram — nagłówek i przycisk dodawania', async ({ page }) => {
    await page.goto('/harmonogram')
    await expect(page.getByRole('heading', { name: /Harmonogram Schroniska/i })).toBeVisible()
    await expect(page.getByRole('button', { name: /Dodaj/i })).toBeVisible()
  })

  test('[4] pole wyszukiwania na liście zwierząt ma fokus (v-focus)', async ({ page }) => {
    await page.goto('/zwierzeta')
    const search = page.getByPlaceholder(/Szukaj zwierzaka/i)
    await expect(search).toBeFocused()
  })
})
