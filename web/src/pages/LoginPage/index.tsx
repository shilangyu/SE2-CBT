import {
    Button,
    CircularProgress,
    Grid,
    TextField,
    Typography,
    useTheme,
} from '@mui/material'
import { useSnackbar } from 'notistack'
import * as React from 'react'
import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useAsync } from '../../hooks/useAsync'
import { useLoginStore } from '../../stores/loginStore'
import { dataTestAttr, dataTestInputProp } from '../../utils/testing'
import { routes } from '../routes'

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
            setCredentialsError('wrong credentials')
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
                    {...dataTestInputProp('login-email-input')}
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
                    {...dataTestInputProp('login-password-input')}
                />
            </Grid>
            <Grid
                item
                container
                direction="row"
                justifyContent="center"
                gap={4}
            >
                <Button
                    component={Link}
                    to={routes.register()}
                    {...dataTestAttr('login-create-account-button')}
                >
                    Create account
                </Button>
                <Button
                    variant="contained"
                    size="large"
                    onClick={onSubmit}
                    {...dataTestAttr('login-login-button')}
                >
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
