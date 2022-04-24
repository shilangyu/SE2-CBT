import { ThemeProvider } from '@emotion/react'
import { SnackbarProvider } from 'notistack'
import React from 'react'
import { createRoot } from 'react-dom/client'
import App from './App'
import { theme } from './theme'

const root = createRoot(document.getElementById('root')!)

root.render(
    <React.StrictMode>
        <ThemeProvider theme={theme}>
            <SnackbarProvider maxSnack={3}>
                <App />
            </SnackbarProvider>
        </ThemeProvider>
    </React.StrictMode>
)
