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
import { routes } from '../routes'

function RegisterPage() {
    const theme = useTheme()
    const loginStore = useLoginStore()
    const {
        email,
        setEmail,
        password,
        setPassword,
        age,
        setAge,
        gender,
        setGender,
        validate,
        emailError,
        passwordError,
        setEmailError,
        ageError,
        genderError,
    } = useRegisterForm()
    const { call: register, loading, error } = useAsync(loginStore.register)
    const { enqueueSnackbar } = useSnackbar()

    async function onSubmit() {
        if (validate()) {
            const success = await register(email, password, age!, gender)
            if (success === undefined) return

            if (success) {
                enqueueSnackbar('Register successful', { variant: 'success' })
            } else {
                setEmailError('Email taken')
            }
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
                    error={!!emailError}
                    helperText={emailError}
                    onChange={e => setEmail(e.target.value)}
                />
            </Grid>
            <Grid item>
                <TextField
                    label="Password"
                    value={password}
                    type="password"
                    error={!!passwordError}
                    helperText={passwordError}
                    onChange={e => setPassword(e.target.value)}
                />
            </Grid>
            <Grid item>
                <TextField
                    label="Age"
                    value={age}
                    type="number"
                    error={!!ageError}
                    helperText={ageError}
                    onChange={e => {
                        if (e.target.value !== '') {
                            setAge(+e.target.value)
                            e.target.value = (+e.target.value).toString()
                        }
                    }}
                />
            </Grid>
            <Grid item>
                <TextField
                    label="Gender"
                    value={gender}
                    error={!!genderError}
                    helperText={genderError}
                    onChange={e => setGender(e.target.value)}
                />
            </Grid>
            <Grid
                item
                container
                direction="row"
                justifyContent="center"
                gap={4}
            >
                <Button component={Link} to={routes.login()}>
                    Log in
                </Button>
                <Button variant="contained" size="large" onClick={onSubmit}>
                    Create account
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

function useRegisterForm() {
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [age, setAge] = useState<number>(0)
    const [gender, setGender] = useState('')

    const [emailError, setEmailError] = useState<string | undefined>(undefined)
    const [passwordError, setPasswordError] = useState<string | undefined>(
        undefined
    )
    const [ageError, setAgeError] = useState<string | undefined>(undefined)
    const [genderError, setGenderError] = useState<string | undefined>(
        undefined
    )

    React.useEffect(() => {
        setEmailError(undefined)
    }, [email])
    React.useEffect(() => {
        setPasswordError(undefined)
    }, [password])
    React.useEffect(() => {
        setAgeError(undefined)
    }, [age])
    React.useEffect(() => {
        setGenderError(undefined)
    }, [gender])

    function validate() {
        let error = false
        if (!/@/.test(email)) {
            setEmailError('Incorrect email')
            error = true
        }
        if (password.length < 8) {
            setPasswordError('Password has to be at least 8 characters')
            error = true
        }
        if (age === undefined) {
            setAgeError('Age is required')
            error = true
        } else if (age !== Math.floor(age)) {
            setAgeError('Age has to be an integer')
            error = true
        } else if (age <= 0 || age >= 150) {
            setAgeError('Please provide valid age')
            error = true
        }
        if (gender.length === 0) {
            setGenderError('Please specify gender')
            error = true
        }

        return !error
    }

    return {
        email,
        setEmail,
        password,
        setPassword,
        age,
        setAge,
        gender,
        setGender,
        validate,

        emailError,
        setEmailError,
        passwordError,
        ageError,
        genderError,
    }
}

export default RegisterPage
