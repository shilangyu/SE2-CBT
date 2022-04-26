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
} from '@mui/material'
import { useSnackbar } from 'notistack'
import * as React from 'react'
import { useLoginStore } from '../../stores/loginStore'
import LogoutIcon from '@mui/icons-material/Logout'
import AccountCircleIcon from '@mui/icons-material/AccountCircle'
import { parseJwt } from '../../utils/jwt'
import { apiClient } from '../../api'
import { User } from '../../model/user'

function ProfilePage() {
    const theme = useTheme()
    const loginStore = useLoginStore()

    const [userLogin, setUserLogin] = React.useState<string>('...')
    const [userData, setUserData] = React.useState<User | undefined>(undefined)

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

    const renderUserProfile = () => {
        if (userData) {
            return <Typography>{userData.gender}</Typography>
        } else {
            return <Typography>loading...</Typography>
        }
    }

    return (
        <>
            <Grid container>
                <Grid item>
                    <Typography>{userLogin}</Typography>
                    {renderUserProfile()}
                </Grid>
            </Grid>
        </>
    )
}

export default ProfilePage
