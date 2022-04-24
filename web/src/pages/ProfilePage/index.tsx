import {
    AppBar,
    Button,
    Grid,
    IconButton,
    Stack,
    TextField,
    Toolbar,
    Typography,
} from '@mui/material'
import { useSnackbar } from 'notistack'
import * as React from 'react'
import { useLoginStore } from '../../stores/loginStore'
import LogoutIcon from '@mui/icons-material/Logout'
import AccountCircleIcon from '@mui/icons-material/AccountCircle'

function ProfilePage() {
    return (
        <>
            <Typography>User profile</Typography>
        </>
    )
}

export default ProfilePage
