import { EmailUsedError } from '../../api'
import { loginResponseToUserData, useLoginStore } from '../../stores/loginStore'
import { apiClient, createTestUser, testUrlDescribe } from './setup'

testUrlDescribe('user creation', () => {
    test('happy path', async () => {
        const email = `user_${+new Date()}@google.com`
        const password = 'qweqweqwe'
        const age = 21
        const gender = 'male'

        await apiClient.register(email, password, age, gender)

        const loginResult = await apiClient.logIn(email, password)

        // we got any response
        expect(loginResult).toBeTruthy()

        // we are a normal (not admin) user
        expect(loginResult.userStatus).toBe(0)

        const userId = loginResult.userId

        useLoginStore.setState({
            userData: loginResponseToUserData(loginResult),
        })
        const user = await apiClient.getUser(userId)

        // check if we created a correct user
        expect(user).toEqual({
            userId,
            login: email,
            age,
            gender,
            userStatus: 0,
            banned: false,
        })
    })

    test('user already exists', async () => {
        const email = `user_${+new Date()}@google.com`
        const password = 'qweqweqwe'
        const age = 21
        const gender = 'male'
        await apiClient.register(email, password, age, gender)

        await expect(
            apiClient.register(email, password, age, gender)
        ).rejects.toBeInstanceOf(EmailUsedError)
    })
})

testUrlDescribe('get user', () => {
    test('gets existing user', async () => {
        const { user } = await createTestUser()

        const apiUser = await apiClient.getUser(user.userId)

        expect(apiUser.userId).toBe(user.userId)
    })

    test('fails to get nonexisting user', async () => {
        await createTestUser()

        await expect(apiClient.getUser(19990129)).rejects.toHaveProperty(
            'response.status',
            404
        )
    })
})

testUrlDescribe('update user', () => {
    test('updates user', async () => {
        const { user } = await createTestUser()

        await apiClient.updateUser(user.userId, { age: user.age + 1 })

        const apiUser = await apiClient.getUser(user.userId)

        expect(apiUser.age).toBe(user.age + 1)
    })

    test('fails to update nonexisting user', async () => {
        await createTestUser()

        await expect(apiClient.updateUser(19990129, {})).rejects.toHaveProperty(
            'response.status',
            404
        )
    })

    // TODO: backend needs to add authorizers
    test.skip('cannot update a different user', async () => {
        const { user: user1 } = await createTestUser()
        const { user: _user2 } = await createTestUser()

        // user2 is now logged in

        await expect(
            apiClient.updateUser(user1.userId, { age: 19 })
        ).rejects.toHaveProperty('response.status', 403)
    })
})

testUrlDescribe('delete user', () => {
    test('deletes user', async () => {
        const { user } = await createTestUser()

        await apiClient.deleteUser(user.userId)

        await expect(apiClient.getUser(user.userId)).rejects.toHaveProperty(
            'response.status',
            404
        )
    })

    test('fails to delete nonexisting user', async () => {
        await createTestUser()

        await expect(apiClient.deleteUser(19990129)).rejects.toHaveProperty(
            'response.status',
            404
        )
    })

    // TODO: backend needs to add authorizers
    test.skip('cannot delete a different user', async () => {
        const { user: user1 } = await createTestUser()
        const { user: _user2 } = await createTestUser()

        // user2 is now logged in

        await expect(apiClient.deleteUser(user1.userId)).rejects.toHaveProperty(
            'response.status',
            403
        )
    })
})