import { ApiClient } from './ApiClient'

export const apiClient = new ApiClient(import.meta.env.VITE_API_URL)

export class UnauthorizedError extends Error {}
export class EmailUsedError extends Error {}
