import { apiUrl } from '../utils/config'
import { ApiClient } from './ApiClient'

export const apiClient = new ApiClient(apiUrl)

export class UnauthorizedError extends Error {}
export class EmailUsedError extends Error {}
