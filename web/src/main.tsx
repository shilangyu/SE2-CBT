import { ThemeProvider } from '@emotion/react'
import { SnackbarProvider } from 'notistack'
import React from 'react'
import ReactDOM from 'react-dom'
import App from './App'
import { theme } from './theme'

ReactDOM.render(
    <React.StrictMode>
        <ThemeProvider theme={theme}>
            <SnackbarProvider maxSnack={3}>
                <App />
            </SnackbarProvider>
        </ThemeProvider>
    </React.StrictMode>,
    document.getElementById('root')
)
