export const mockLogIn = jest.fn()
export const mockRegister = jest.fn()

// user
export const mockGetUser = jest.fn()
export const mockUpdateUser = jest.fn()
export const mockDeleteUser = jest.fn()

export const ApiClient = jest.fn().mockImplementation(() => ({
    logIn: mockLogIn,
    register: mockRegister,
    getUser: mockGetUser,
    updateUser: mockUpdateUser,
    deleteUser: mockDeleteUser
}))
