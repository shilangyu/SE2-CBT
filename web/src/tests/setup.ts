import '@testing-library/jest-dom'
import 'isomorphic-fetch'
import { vi } from 'vitest'

vi.mock('../utils/config.ts', () => ({
    apiUrl: 'something',
}))

// @ts-ignore
global.IS_REACT_ACT_ENVIRONMENT = true
process.env.NODE_TLS_REJECT_UNAUTHORIZED = '0'

vi.mock('../api/ApiClient.ts')

export {}
