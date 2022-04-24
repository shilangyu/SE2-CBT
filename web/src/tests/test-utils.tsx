import { render, RenderOptions } from '@testing-library/react'
import { SnackbarProvider } from 'notistack'
import React from 'react'
import { HashRouter } from 'react-router-dom'

const AllTheProviders: React.FC<{ children: React.ReactNode }> = ({
    children,
}) => {
    return (
        <HashRouter>
            <SnackbarProvider>{children}</SnackbarProvider>
        </HashRouter>
    )
}

const customRender = (
    ui: React.ReactElement,
    options?: Omit<RenderOptions, 'queries'>
) => render(ui, { wrapper: AllTheProviders, ...options })

export * from '@testing-library/react'
export { customRender as render }
