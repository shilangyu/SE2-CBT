import create from 'zustand'
import { apiClient, EmailUsedError, UnauthorizedError } from '../api'
import { LoginResponse } from '../model/responses'

const userDataStorageKey = 'userData'

export type UserData = {
    token: string
    userId: number
    isAdmin: boolean
}

type LoginStore = {
    userData?: UserData
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
    userData:
        localStorage.getItem(userDataStorageKey) !== null
            ? JSON.parse(localStorage[userDataStorageKey])
            : undefined,
    isLoggedIn() {
        return get().userData !== undefined
    },
    async logIn(email, password) {
        try {
            const loginResponse = await apiClient.logIn(email, password)
            set(state => ({
                userData: loginResponseToUserData(loginResponse),
            }))

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
        set(state => ({ userData: undefined }))
    },
}))

export function loginResponseToUserData(
    loginResponse: LoginResponse
): UserData {
    return {
        isAdmin: loginResponse.userStatus === 1,
        token: loginResponse.accessToken,
        userId: loginResponse.userId,
    }
}

useLoginStore.subscribe(state => {
    if (state.userData === undefined) {
        localStorage.removeItem(userDataStorageKey)
    } else {
        localStorage[userDataStorageKey] = JSON.stringify(state.userData)
    }
})
