import create from 'zustand'
import { apiClient, UnauthorizedError } from '../api'

type LoginStore = {
    token?: string
    isLoggedIn: boolean
    logIn: (email: string, password: string) => Promise<boolean>
    logOut: () => void
}

export const useLoginStore = create<LoginStore>(set => ({
    token: undefined,
    get isLoggedIn() {
        return this.token !== undefined
    },
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
