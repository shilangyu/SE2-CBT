import { vi } from 'vitest'
import { User } from '../../model/user'
import { loginResponseToUserData, useLoginStore } from '../../stores/loginStore'

const testUrl = import.meta.env['TEST_URL']

const { ApiClient } = await vi.importActual<
    typeof import('../../api/ApiClient')
>('../../api/ApiClient')

export const apiClient = new ApiClient(testUrl)

export const testUrlDescribe = testUrl ? describe : describe.skip

/// Creates a random test user in the remote server, logs this user in, and fetches its info.
export async function createTestUser(): Promise<{ user: User; token: string }> {
    const email = `user_${+new Date() % Math.random()}@google.com`
    const password = 'Qweqweqwe$3'
    const age = 21
    const gender = 'male'

    await apiClient.register(email, password, age, gender)
    const loginResult = await apiClient.logIn(email, password)
    useLoginStore.setState({ userData: loginResponseToUserData(loginResult) })

    const user = await apiClient.getUser(loginResult.userId)

    return { user, token: loginResult.accessToken }
}
