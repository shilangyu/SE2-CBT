import { cleanup } from '@testing-library/react'
import * as React from 'react'
import { apiClient, EmailUsedError } from '../../api'
import RegisterPage from '../../pages/RegisterPage'
import { act, fireEvent, render, screen } from '../test-utils'

describe('RegisterPage', () => {
    afterEach(() => {
        jest.resetAllMocks()
        cleanup()
    })

    it('has all inputs', async () => {
        render(<RegisterPage />)

        expect(screen.getByLabelText(/email/i)).toBeTruthy()
        expect(screen.getByLabelText(/password/i)).toBeTruthy()
        expect(screen.getByLabelText(/age/i)).toHaveAttribute('type', 'number')
        expect(screen.getByLabelText(/gender/i)).toBeTruthy()
    })

    it('does not call register with invalid data', async () => {
        const promise = Promise.resolve()
        ;(apiClient.register as jest.Mock).mockImplementationOnce(() => promise)

        render(<RegisterPage />)

        fireEvent.click(screen.getByRole('button'))

        expect(apiClient.register).not.toHaveBeenCalled()

        await act(() => promise)
    })

    it('displays email taken error', async () => {
        ;(apiClient.register as jest.Mock).mockImplementationOnce(() => {
            throw new EmailUsedError()
        })

        render(<RegisterPage />)

        fireEvent.change(screen.getByLabelText(/email/i), {
            target: { value: 'email@asd' },
        })
        fireEvent.change(screen.getByLabelText(/password/i), {
            target: { value: 'password' },
        })
        fireEvent.change(screen.getByLabelText(/age/i), {
            target: { value: 21 },
        })
        fireEvent.change(screen.getByLabelText(/gender/i), {
            target: { value: 'male' },
        })

        fireEvent.click(screen.getByRole('button'))

        expect(await screen.findByText(/email taken/i)).toBeTruthy()
    })

    it('calls register with valid input data', async () => {
        const promise = Promise.resolve()
        ;(apiClient.register as jest.Mock).mockImplementationOnce(() => promise)

        render(<RegisterPage />)

        act(() => {
            fireEvent.change(screen.getByLabelText(/email/i), {
                target: { value: 'email@asd' },
            })
            fireEvent.change(screen.getByLabelText(/password/i), {
                target: { value: 'password' },
            })
            fireEvent.change(screen.getByLabelText(/age/i), {
                target: { value: 21 },
            })
            fireEvent.change(screen.getByLabelText(/gender/i), {
                target: { value: 'male' },
            })

            fireEvent.click(screen.getByRole('button'))
        })

        expect(apiClient.register).toHaveBeenCalledWith(
            'email@asd',
            'password',
            21,
            'male'
        )

        await act(() => promise)
    })

    it('has a log in button', async () => {
        render(<RegisterPage />)

        expect(screen.getByRole('link')).toHaveTextContent(/log in/i)
    })
})
