import * as React from 'react'
import { HashRouter, Navigate, Route, Routes } from 'react-router-dom'
import HomePage from './pages/HomePage'
import LoginPage from './pages/LoginPage'
import RegisterPage from './pages/RegisterPage'
import { routes } from './pages/routes'
import { useLoginStore } from './stores/loginStore'

function App() {
    const isLoggedIn = useLoginStore(s => s.isLoggedIn())

    return (
        <HashRouter>
            {!isLoggedIn ? (
                <Routes>
                    <Route
                        path={routes.login.pattern}
                        element={<LoginPage />}
                    />
                    <Route
                        path={routes.register.pattern}
                        element={<RegisterPage />}
                    />
                    <Route
                        path="*"
                        element={<Navigate to={routes.login()} replace />}
                    />
                </Routes>
            ) : (
                <Routes>
                    <Route path="/*" element={<HomePage />} />
                </Routes>
            )}
        </HashRouter>
    )
}

export default App
