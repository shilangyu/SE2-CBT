import create from 'zustand'
import { apiClient, UnauthorizedError } from '../api'

type LoginStore = {
    token?: string
    logIn: (email: string, password: string) => Promise<boolean>
    logOut: () => void
}

export const useLoginStore = create<LoginStore>(set => ({
    token: undefined,
    async logIn(email, password) {
        try {
            const token = await apiClient.logIn(email, password)
            set(state => ({ token }))

            return true
        } catch (err) {
            if (err instanceof UnauthorizedError) {
                return false
            }

            throw err
        }
    },
    logOut() {
        set(state => ({ token: undefined }))
    },
}))

export const loginStoreSelects = {
    isLoggedIn: (s: LoginStore) => s.token !== undefined,
}
