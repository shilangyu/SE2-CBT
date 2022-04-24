import * as React from 'react'
import { apiClient, UnauthorizedError } from '../../api'
import LoginPage from '../../pages/LoginPage'
import { act, fireEvent, render, screen } from '../test-utils'

describe('LoginPage', () => {
    afterEach(() => {
        jest.resetAllMocks()
    })

    it('has all inputs', async () => {
        render(<LoginPage />)

        expect(screen.getByLabelText(/email/i)).toHaveAttribute('type', 'text')
        expect(screen.getByLabelText(/password/i)).toHaveAttribute(
            'type',
            'password'
        )
    })

    it('calls login on submit', async () => {
        const promise = Promise.resolve()
        ;(apiClient.logIn as jest.Mock).mockImplementationOnce(() => promise)

        render(<LoginPage />)

        const submit = screen.getByRole('button')
        fireEvent.click(submit)

        expect(apiClient.logIn).toHaveBeenCalled()

        await act(() => promise)
    })

    it('calls login with input data', async () => {
        const promise = Promise.resolve()
        ;(apiClient.logIn as jest.Mock).mockImplementationOnce(() => promise)

        render(<LoginPage />)

        fireEvent.change(screen.getByLabelText(/email/i), {
            target: { value: 'email' },
        })
        fireEvent.change(screen.getByLabelText(/password/i), {
            target: { value: 'password' },
        })

        fireEvent.click(screen.getByRole('button'))

        expect(apiClient.logIn).toHaveBeenCalledWith('email', 'password')

        await act(() => promise)
    })

    it('displays error on failed login', async () => {
        ;(apiClient.logIn as jest.Mock).mockImplementationOnce(() => {
            throw new UnauthorizedError()
        })

        render(<LoginPage />)

        fireEvent.change(screen.getByLabelText(/email/i), {
            target: { value: 'email' },
        })
        fireEvent.change(screen.getByLabelText(/password/i), {
            target: { value: 'password' },
        })

        fireEvent.click(screen.getByRole('button'))

        expect(await screen.findAllByText(/wrong credentials/i)).toHaveLength(2)
    })

    it('has a register button', async () => {
        render(<LoginPage />)

        expect(screen.getByRole('link')).toHaveTextContent(/create account/i)
    })
})
