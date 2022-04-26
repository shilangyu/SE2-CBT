import {
    AppBar,
    Button,
    Grid,
    IconButton,
    Stack,
    TextField,
    Toolbar,
    Typography,
    useTheme,
    Card,
    Container,
    Icon,
    CircularProgress,
    Divider,
} from '@mui/material'
import { useSnackbar } from 'notistack'
import * as React from 'react'
import { useLoginStore } from '../../stores/loginStore'
import LogoutIcon from '@mui/icons-material/Logout'
import AccountCircleIcon from '@mui/icons-material/AccountCircle'
import { parseJwt } from '../../utils/jwt'
import { apiClient } from '../../api'
import { User } from '../../model/user'
import { maxWidth, padding } from '@mui/system'
import PersonIcon from '@mui/icons-material/Person'

function ProfilePage() {
    const theme = useTheme()
    const loginStore = useLoginStore()

    const [userLogin, setUserLogin] = React.useState<string>('...')
    const [userData, setUserData] = React.useState<User | undefined>(undefined)
    const [editMode, setEditMode] = React.useState(false)

    const { enqueueSnackbar } = useSnackbar()

    React.useEffect(() => {
        // assume token is not null
        const tokenClaims = parseJwt(loginStore.token as string)
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
    }, [])

    const renderUserEditor = () => {
        return (
            <>
                <Grid item xs={2}>
                    <Typography sx={{ fontWeight: 'bold' }}>login</Typography>
                </Grid>
                <Grid item xs={10}>
                    <Typography>{userData?.login}</Typography>
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
                    <Typography>{userData?.age}</Typography>
                </Grid>
                <Grid item xs={2}>
                    <Typography sx={{ fontWeight: 'bold' }}>gender</Typography>
                </Grid>
                <Grid item xs={10}>
                    <Typography>{userData?.gender}</Typography>
                </Grid>
                <Grid item xs={12}>
                    <Stack direction="row" spacing={2}>
                        <Button variant="contained" disableElevation>
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

    const renderUserInfo = () => {
        return (
            <>
                <Grid item xs={2}>
                    <Typography sx={{ fontWeight: 'bold' }}>login</Typography>
                </Grid>
                <Grid item xs={10}>
                    <Typography>{userData?.login}</Typography>
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
                    <Typography>{userData?.age}</Typography>
                </Grid>
                <Grid item xs={2}>
                    <Typography sx={{ fontWeight: 'bold' }}>gender</Typography>
                </Grid>
                <Grid item xs={10}>
                    <Typography>{userData?.gender}</Typography>
                </Grid>
                <Grid item xs={12}>
                    <Button
                        variant="contained"
                        disableElevation
                        onClick={() => setEditMode(true)}
                    >
                        edit
                    </Button>
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
                            editMode ? renderUserEditor() : renderUserInfo())()}
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

export default ProfilePage
