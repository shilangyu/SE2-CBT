import { ThemeProvider } from '@emotion/react'
import React from 'react'
import ReactDOM from 'react-dom'
import { ApiClient } from './api/api_client'
import App from './App'
import { theme } from './theme'

const apiClient = new ApiClient(import.meta.env.VITE_API_URL)

ReactDOM.render(
    <React.StrictMode>
        <ThemeProvider theme={theme}>
            <App />
        </ThemeProvider>
    </React.StrictMode>,
    document.getElementById('root')
)
