export const mockLogIn = jest.fn()
export const mockRegister = jest.fn()

export const ApiClient = jest.fn().mockImplementation(() => ({
    logIn: mockLogIn,
    register: mockRegister,
}))
