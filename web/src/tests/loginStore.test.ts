import { afterEach, beforeEach, describe, it, vi } from 'vitest'
import { apiClient, EmailUsedError, UnauthorizedError } from '../api'
import { LoginResponse } from '../model/responses'
import { useLoginStore, UserData } from '../stores/loginStore'

const loginResponse: LoginResponse = {
    accessToken: 'a.b.c',
    userId: 123,
    userStatus: 0,
}
const userData: UserData = {
    isAdmin: false,
    token: loginResponse.accessToken,
    userId: loginResponse.userId,
}

describe('LoginStore', () => {
    afterEach(() => {
        vi.resetAllMocks()
        useLoginStore.getState().logOut()
    })

    it('is initially logged out', () => {
        expect(useLoginStore.getState().isLoggedIn()).toBe(false)
    })

    it('initially has no userData', () => {
        expect(useLoginStore.getState().userData).toBe(undefined)
    })

    it('logout clears the userData', () => {
        useLoginStore.getState().userData = userData

        useLoginStore.getState().logOut()

        expect(useLoginStore.getState().userData).toBe(undefined)
    })

    it('isLoggedIn returns true when a userData is present', () => {
        useLoginStore.getState().userData = userData

        expect(useLoginStore.getState().isLoggedIn()).toBe(true)
    })

    describe('logIn', () => {
        it('returns true on success', async () => {
            ;(apiClient.logIn as jest.Mock).mockImplementationOnce(
                () => loginResponse
            )

            expect(
                await useLoginStore.getState().logIn('email', 'password')
            ).toBe(true)
        })

        it('is logged in after success', async () => {
            ;(apiClient.logIn as jest.Mock).mockImplementationOnce(
                () => loginResponse
            )

            await useLoginStore.getState().logIn('email', 'password')

            expect(useLoginStore.getState().isLoggedIn()).toBe(true)
            expect(useLoginStore.getState().userData).toEqual(userData)
        })

        it('returns false on exception', async () => {
            ;(apiClient.logIn as jest.Mock).mockImplementationOnce(() => {
                throw new UnauthorizedError()
            })

            expect(
                await useLoginStore.getState().logIn('email', 'password')
            ).toBe(false)
        })

        it('is not logged ina fter failure', async () => {
            ;(apiClient.logIn as jest.Mock).mockImplementationOnce(() => {
                throw new UnauthorizedError()
            })

            await useLoginStore.getState().logIn('email', 'password')

            expect(useLoginStore.getState().isLoggedIn()).toBe(false)
        })
    })

    describe('register', () => {
        beforeEach(() => {
            ;(apiClient.logIn as jest.Mock).mockImplementationOnce(
                () => loginResponse
            )
        })

        it('returns true on success', async () => {
            ;(apiClient.register as jest.Mock).mockImplementationOnce(() =>
                Promise.resolve()
            )

            expect(
                await useLoginStore
                    .getState()
                    .register('email', 'password', 21, 'male')
            ).toBe(true)
        })

        it('returns false on used email exception', async () => {
            ;(apiClient.register as jest.Mock).mockImplementationOnce(() => {
                throw new EmailUsedError()
            })

            expect(
                await useLoginStore
                    .getState()
                    .register('email', 'password', 21, 'male')
            ).toBe(false)
        })

        it('logs in after register', async () => {
            ;(apiClient.register as jest.Mock).mockImplementationOnce(() =>
                Promise.resolve()
            )

            await useLoginStore
                .getState()
                .register('email', 'password', 21, 'male')

            expect(useLoginStore.getState().isLoggedIn()).toBe(true)
        })

        it('is not logged in after failure', async () => {
            ;(apiClient.register as jest.Mock).mockImplementationOnce(() => {
                throw new EmailUsedError()
            })

            await useLoginStore
                .getState()
                .register('email', 'password', 21, 'male')

            expect(useLoginStore.getState().isLoggedIn()).toBe(false)
        })
    })
})
