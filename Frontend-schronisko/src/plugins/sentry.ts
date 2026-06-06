import type { App } from 'vue'
import * as Sentry from '@sentry/vue'

/*
[8] SENTRY — monitorowanie błędów i wydajności w Vue.
DSN z .env (VITE_SENTRY_DSN); bez DSN inicjalizacja jest pomijana (dev lokalny).
 */
export function initSentry(app: App): void {
  const dsn = import.meta.env.VITE_SENTRY_DSN as string | undefined

  if (!dsn) {
    console.info('[Sentry] Pominięto — brak VITE_SENTRY_DSN w .env')
    return
  }

  Sentry.init({
    app,
    dsn,
    environment: import.meta.env.MODE,
    integrations: [Sentry.browserTracingIntegration()],
    tracesSampleRate: import.meta.env.PROD ? 0.2 : 1.0,
  })
}
