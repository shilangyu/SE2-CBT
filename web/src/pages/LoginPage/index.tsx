import {
    Button,
    CircularProgress,
    Grid,
    TextField,
    Typography,
    useTheme,
} from '@mui/material'
import { useState } from 'react'
import * as React from 'react'
import { useSnackbar } from 'notistack'
import { useLoginStore } from '../../stores/loginStore'
import { useAsync } from '../../hooks/useAsync'

function LoginPage() {
    const theme = useTheme()
    const loginStore = useLoginStore()
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [credentialsError, setCredentialsError] = useState<
        string | undefined
    >(undefined)
    const { call: logIn, loading, error } = useAsync(loginStore.logIn)
    const { enqueueSnackbar } = useSnackbar()

    async function onSubmit() {
        const success = await logIn(email, password)
        if (success === undefined) return

        if (success) {
            enqueueSnackbar('Login successful', { variant: 'success' })
        } else {
            setCredentialsError('wrong crentials')
        }
    }

    return (
        <Grid
            container
            direction="column"
            rowSpacing={4}
            alignItems="center"
            justifyContent="center"
            style={{ minHeight: '100vh' }}
        >
            <Grid item>
                <TextField
                    label="Email"
                    value={email}
                    error={!!credentialsError}
                    helperText={credentialsError}
                    onChange={e => setEmail(e.target.value)}
                />
            </Grid>
            <Grid item>
                <TextField
                    label="Password"
                    value={password}
                    type="password"
                    error={!!credentialsError}
                    helperText={credentialsError}
                    onChange={e => setPassword(e.target.value)}
                />
            </Grid>
            <Grid item>
                <Button variant="contained" size="large" onClick={onSubmit}>
                    Log in
                </Button>
            </Grid>
            {loading && (
                <Grid item>
                    <CircularProgress />
                </Grid>
            )}
            {error && (
                <Grid item>
                    <Typography color={theme.palette.error.main}>
                        {error.toString()}
                    </Typography>
                </Grid>
            )}
        </Grid>
    )
}

export default LoginPage
