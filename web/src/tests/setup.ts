import '@testing-library/jest-dom'
import { vi } from 'vitest'

vi.mock('../utils/config.ts', () => ({
    apiUrl: 'something',
}))

// @ts-ignore
global.IS_REACT_ACT_ENVIRONMENT = true

vi.mock('../api/ApiClient.ts')

export {}
