import { cleanup } from '@testing-library/react'
import * as React from 'react'
import { expect, SpyInstance, vi } from 'vitest'
import { apiClient, EmailUsedError } from '../../api'
import RegisterPage from '../../pages/RegisterPage'
import { act, fireEvent, render } from '../test-utils'

describe('RegisterPage', () => {
    afterEach(() => {
        vi.resetAllMocks()
        cleanup()
    })

    it('has all inputs', async () => {
        const { findByLabelText } = render(<RegisterPage />)

        expect(await findByLabelText(/email/i)).toBeTruthy()
        expect(await findByLabelText(/password/i)).toBeTruthy()
        expect(await findByLabelText(/age/i)).toHaveAttribute('type', 'number')
        expect(await findByLabelText(/gender/i)).toBeTruthy()
    })

    it('does not call register with invalid data', async () => {
        const promise = Promise.resolve()
        ;(apiClient.register as unknown as SpyInstance).mockImplementationOnce(
            () => promise
        )

        const { findByRole } = render(<RegisterPage />)

        await act(async () => {
            fireEvent.click(await findByRole('button'))
        })
        expect(apiClient.register).not.toHaveBeenCalled()

        await act(() => promise)
    })

    it('displays email taken error', async () => {
        ;(apiClient.register as unknown as SpyInstance).mockRejectedValueOnce(
            new EmailUsedError()
        )

        const { findByLabelText, findByRole, findByText } = render(
            <RegisterPage />
        )

        fireEvent.change(await findByLabelText(/email/i), {
            target: { value: 'email@asd' },
        })
        fireEvent.change(await findByLabelText(/password/i), {
            target: { value: 'password' },
        })
        fireEvent.change(await findByLabelText(/age/i), {
            target: { value: 21 },
        })
        fireEvent.change(await findByLabelText(/gender/i), {
            target: { value: 'male' },
        })
        await act(async () => {
            fireEvent.click(await findByRole('button'))
        })

        expect(await findByText(/email taken/i)).toBeTruthy()
    })

    it('calls register with valid input data', async () => {
        const promise = Promise.resolve()
        ;(apiClient.register as unknown as SpyInstance).mockImplementationOnce(
            () => promise
        )

        const { findByLabelText, findByRole } = render(<RegisterPage />)

        fireEvent.change(await findByLabelText(/email/i), {
            target: { value: 'email@asd' },
        })
        fireEvent.change(await findByLabelText(/password/i), {
            target: { value: 'password' },
        })
        fireEvent.change(await findByLabelText(/age/i), {
            target: { value: 21 },
        })
        fireEvent.change(await findByLabelText(/gender/i), {
            target: { value: 'male' },
        })

        await act(async () => {
            fireEvent.click(await findByRole('button'))
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
        const { findByRole } = render(<RegisterPage />)

        expect(await findByRole('link')).toHaveTextContent(/log in/i)
    })
})
