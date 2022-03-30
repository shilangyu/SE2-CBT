import create from 'zustand'
import { apiClient, EmailUsedError, UnauthorizedError } from '../api'

const tokenStorageKey = 'token'

type LoginStore = {
    token?: string
    isLoggedIn: () => boolean
    logIn: (email: string, password: string) => Promise<boolean>
    register: (
        email: string,
        password: string,
        age: number,
        gender: string
    ) => Promise<boolean>
    logOut: () => void
}

export const useLoginStore = create<LoginStore>((set, get) => ({
    token: localStorage[tokenStorageKey],
    isLoggedIn() {
        return get().token !== undefined
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
    async register(
        email: string,
        password: string,
        age: number,
        gender: string
    ) {
        try {
            await apiClient.register(email, password, age, gender)
            return await get().logIn(email, password)
        } catch (err) {
            if (err instanceof EmailUsedError) {
                return false
            }

            throw err
        }
    },
    logOut() {
        set(state => ({ token: undefined }))
    },
}))

useLoginStore.subscribe(state => {
    if (state.token === undefined) {
        localStorage.removeItem(tokenStorageKey)
    } else {
        localStorage[tokenStorageKey] = state.token
    }
})
