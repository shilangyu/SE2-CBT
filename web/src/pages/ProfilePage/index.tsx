import PersonIcon from '@mui/icons-material/Person'
import {
    Backdrop,
    Button,
    Card,
    CircularProgress,
    Container,
    Divider,
    Grid,
    Stack,
    TextField,
    Typography,
    useTheme,
} from '@mui/material'
import { useSnackbar } from 'notistack'
import * as React from 'react'
import { apiClient } from '../../api'
import { User } from '../../model/user'
import { useLoginStore } from '../../stores/loginStore'
import { parseJwt } from '../../utils/jwt'

function ProfilePage() {
    const theme = useTheme()
    const loginStore = useLoginStore()

    const [userLogin, setUserLogin] = React.useState<string>('...')
    const [userData, setUserData] = React.useState<User | undefined>(undefined)
    const [editMode, setEditMode] = React.useState(false)
    const [backdropOpen, setBackdropOpen] = React.useState(false)

    const {
        email,
        setEmail,
        age,
        setAge,
        gender,
        setGender,
        validate,
        emailError,
        setEmailError,
        ageError,
        genderError,
    } = useUserEditorForm()

    const { enqueueSnackbar } = useSnackbar()

    const loadFromToken = () => {
        // assume token is not null
        const tokenClaims = parseJwt(loginStore.token! as string)
        const tokenUserLogin = tokenClaims.unique_name

        setUserLogin(tokenUserLogin)
        setUserData(undefined)

        apiClient
            .getUser(tokenUserLogin)
            .then(currentUser => {
                setUserData(currentUser)
            })
            .catch(error => {
                console.error(`failed to fetch user data: ${error}`)
                enqueueSnackbar(
                    'failed to fetch user data (details in console)',
                    { variant: 'error' }
                )
            })
    }

    React.useEffect(() => {
        loadFromToken()
    }, [])

    const onSubmit = () => {
        if (validate()) {
            setBackdropOpen(true)
            apiClient
                .updateUser(userLogin, {
                    age,
                    email,
                    gender,
                })
                .then(() => {
                    setBackdropOpen(false)
                    setEditMode(false)
                    loadFromToken()
                })
                .catch(error => {
                    setBackdropOpen(false)
                    console.error(error)
                    enqueueSnackbar(
                        'failed to update user data (details in console)',
                        { variant: 'error' }
                    )
                })
        }
    }

    const renderUserEditor = () => {
        return (
            <>
                <Backdrop
                    sx={{
                        color: '#fff',
                        zIndex: theme => theme.zIndex.drawer + 1,
                    }}
                    open={backdropOpen}
                >
                    <CircularProgress color="inherit" />
                </Backdrop>
                <Grid item xs={2}>
                    <Typography sx={{ fontWeight: 'bold' }}>login</Typography>
                </Grid>
                <Grid item xs={10}>
                    <TextField
                        size="small"
                        value={email}
                        error={!!emailError}
                        helperText={emailError}
                        onChange={e => setEmail(e.target.value)}
                    />
                </Grid>
                <Grid item xs={2}>
                    <Typography sx={{ fontWeight: 'bold' }}>
                        password
                    </Typography>
                </Grid>
                <Grid item xs={10}>
                    <Typography>
                        protected and definitely not exposed through api :)
                    </Typography>
                </Grid>
                <Grid item xs={2}>
                    <Typography sx={{ fontWeight: 'bold' }}>age</Typography>
                </Grid>
                <Grid item xs={10}>
                    <TextField
                        size="small"
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
                <Grid item xs={2}>
                    <Typography sx={{ fontWeight: 'bold' }}>gender</Typography>
                </Grid>
                <Grid item xs={10}>
                    <TextField
                        size="small"
                        value={gender}
                        error={!!genderError}
                        helperText={genderError}
                        onChange={e => setGender(e.target.value)}
                    />
                </Grid>
                <Grid item xs={12}>
                    <Stack direction="row" spacing={2}>
                        <Button
                            variant="contained"
                            disableElevation
                            onClick={onSubmit}
                        >
                            save
                        </Button>
                        <Button
                            variant="outlined"
                            disableElevation
                            onClick={() => setEditMode(false)}
                        >
                            cancel
                        </Button>
                    </Stack>
                </Grid>
            </>
        )
    }

    const renderUserProfile = () => {
        if (userData) {
            return (
                <Grid container spacing={2}>
                    <Grid item xs={12}>
                        <Typography variant="h5">information</Typography>
                        <Typography color={theme.palette.text.secondary}>
                            here you can edit info about your account
                        </Typography>
                    </Grid>
                    <Grid item xs={12}>
                        <Divider />
                    </Grid>
                    <>
                        {(() =>
                            editMode ? (
                                renderUserEditor()
                            ) : (
                                <>
                                    <>{renderUserInfo(userData)}</>
                                    <Grid item xs={12}>
                                        <Button
                                            variant="contained"
                                            disableElevation
                                            onClick={() => {
                                                setEmail(userData.login)
                                                setAge(userData.age)
                                                setGender(userData.gender)
                                                setEditMode(true)
                                            }}
                                        >
                                            edit
                                        </Button>
                                    </Grid>
                                </>
                            ))()}
                    </>
                </Grid>
            )
        } else {
            return (
                <Container
                    sx={{ justifyContent: 'center', textAlign: 'center' }}
                >
                    <CircularProgress />
                </Container>
            )
        }
    }

    return (
        <>
            <Container maxWidth="xl">
                <Grid
                    container
                    spacing={2}
                    sx={{ width: '100%', display: 'block' }}
                >
                    <Grid item xs={12}>
                        <Card variant="outlined" elevation={0}>
                            <Grid container>
                                <Grid
                                    item
                                    display={{ md: 'block', xs: 'none' }}
                                    md={2}
                                    lg={1}
                                >
                                    <PersonIcon
                                        style={{
                                            width: '100%',
                                            height: '100%',
                                        }}
                                    />
                                </Grid>
                                <Grid item sm={8} sx={{ padding: 3 }}>
                                    <Typography variant="h3">
                                        {userLogin}
                                    </Typography>
                                    <Typography
                                        variant="h5"
                                        color={theme.palette.text.secondary}
                                    >
                                        user profile page
                                    </Typography>
                                </Grid>
                            </Grid>
                        </Card>
                    </Grid>
                    <Grid item xs={12}>
                        <Card
                            variant="outlined"
                            elevation={0}
                            sx={{ padding: 3 }}
                        >
                            {renderUserProfile()}
                        </Card>
                    </Grid>
                </Grid>
            </Container>
        </>
    )
}

const renderUserInfo = (userData: User) => {
    return (
        <>
            <Grid item xs={2}>
                <Typography sx={{ fontWeight: 'bold' }}>login</Typography>
            </Grid>
            <Grid item xs={10}>
                <Typography>{userData?.login}</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography sx={{ fontWeight: 'bold' }}>password</Typography>
            </Grid>
            <Grid item xs={10}>
                <Typography>
                    protected and definitely not exposed through api :)
                </Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography sx={{ fontWeight: 'bold' }}>age</Typography>
            </Grid>
            <Grid item xs={10}>
                <Typography>{userData?.age}</Typography>
            </Grid>
            <Grid item xs={2}>
                <Typography sx={{ fontWeight: 'bold' }}>gender</Typography>
            </Grid>
            <Grid item xs={10}>
                <Typography>{userData?.gender}</Typography>
            </Grid>
        </>
    )
}

const useUserEditorForm = () => {
    const [email, setEmail] = React.useState('')
    const [age, setAge] = React.useState<number>(0)
    const [gender, setGender] = React.useState('')

    const [emailError, setEmailError] = React.useState<string | undefined>(
        undefined
    )
    const [passwordError, setPasswordError] = React.useState<
        string | undefined
    >(undefined)
    const [ageError, setAgeError] = React.useState<string | undefined>(
        undefined
    )
    const [genderError, setGenderError] = React.useState<string | undefined>(
        undefined
    )

    React.useEffect(() => {
        setEmailError(undefined)
    }, [email])
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

export default ProfilePage
