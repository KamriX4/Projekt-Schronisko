/// <reference types="vite/client" />

interface ImportMetaEnv {
  /** [8] SENTRY — opcjonalny DSN projektu (https://sentry.io) */
  readonly VITE_SENTRY_DSN?: string
}

interface ImportMeta {
  readonly env: ImportMetaEnv
}
