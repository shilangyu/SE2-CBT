import * as React from 'react'
import { HashRouter, Navigate, Route, Routes } from 'react-router-dom'
import HomePage from './pages/HomePage'
import LoginPage from './pages/LoginPage'
import { useLoginStore } from './stores/loginStore'

function App() {
    const isLoggedIn = useLoginStore(s => s.isLoggedIn())

    return (
        <HashRouter>
            {!isLoggedIn ? (
                <Routes>
                    <Route path="/login" element={<LoginPage />} />
                    <Route
                        path="*"
                        element={<Navigate to="/login" replace />}
                    />
                </Routes>
            ) : (
                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="*" element={<Navigate to="/" replace />} />
                </Routes>
            )}
        </HashRouter>
    )
}

export default App
