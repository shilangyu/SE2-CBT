import { apiClient, EmailUsedError, UnauthorizedError } from '../api'
import { useLoginStore } from '../stores/loginStore'

jest.mock('../utils/config.ts', () => ({
    apiUrl: 'something',
}))

jest.mock('../api/ApiClient.ts')

describe('LoginStore', () => {
    afterEach(() => {
        jest.restoreAllMocks()
        useLoginStore.getState().logOut()
    })

    it('is initially logged out', () => {
        expect(useLoginStore.getState().isLoggedIn()).toBe(false)
    })

    it('initially has no token', () => {
        expect(useLoginStore.getState().token).toBe(undefined)
    })

    it('logout clears the token', () => {
        useLoginStore.getState().token = 'token'

        useLoginStore.getState().logOut()

        expect(useLoginStore.getState().token).toBe(undefined)
    })

    it('isLoggedIn returns true when a token is present', () => {
        useLoginStore.getState().token = 'token'

        expect(useLoginStore.getState().isLoggedIn()).toBe(true)
    })

    describe('logIn', () => {
        it('returns true on success', async () => {
            ;(apiClient.logIn as jest.Mock).mockImplementationOnce(
                () => 'token'
            )

            expect(
                await useLoginStore.getState().logIn('email', 'password')
            ).toBe(true)
        })

        it('is logged in after success', async () => {
            ;(apiClient.logIn as jest.Mock).mockImplementationOnce(
                () => 'token'
            )

            await useLoginStore.getState().logIn('email', 'password')

            expect(useLoginStore.getState().isLoggedIn()).toBe(true)
            expect(useLoginStore.getState().token).toBe('token')
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
                () => 'token'
            )
        })

        it('returns true on success', async () => {
            ;(apiClient.register as jest.Mock).mockImplementationOnce(() => {})

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
            ;(apiClient.register as jest.Mock).mockImplementationOnce(() => {})

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
