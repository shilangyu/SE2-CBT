import { vi } from 'vitest'

export const mockLogIn = vi.fn()
export const mockRegister = vi.fn()

// user
export const mockGetUser = vi.fn()
export const mockUpdateUser = vi.fn()
export const mockDeleteUser = vi.fn()

export const ApiClient = vi.fn().mockImplementation(() => ({
    logIn: mockLogIn,
    register: mockRegister,
    getUser: mockGetUser,
    updateUser: mockUpdateUser,
    deleteUser: mockDeleteUser,
}))
