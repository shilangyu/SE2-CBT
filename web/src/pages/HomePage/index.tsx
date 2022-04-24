import {
    AppBar,
    Button,
    Grid,
    IconButton,
    Stack,
    TextField,
    Toolbar,
    Typography,
    styled,
} from '@mui/material'
import { useSnackbar } from 'notistack'
import * as React from 'react'
import { useLoginStore } from '../../stores/loginStore'
import LogoutIcon from '@mui/icons-material/Logout'
import AccountCircleIcon from '@mui/icons-material/AccountCircle'
import { HashRouter, Link, Navigate, Route, Routes } from 'react-router-dom'
import ProfilePage from '../ProfilePage'
import HomeIcon from '@mui/icons-material/Home'
import { routes } from '../routes'

const ToolbarOffset = styled('div')(({ theme }) => theme.mixins.toolbar)

function HomePage() {
    const logOut = useLoginStore(e => e.logOut)
    const { enqueueSnackbar } = useSnackbar()

    function onLogout() {
        logOut()
        enqueueSnackbar('Logout successful', { variant: 'success' })
    }

    return (
        <>
            <AppBar position="fixed">
                <Toolbar>
                    <Typography
                        variant="h6"
                        component="div"
                        sx={{ flexGrow: 1 }}
                    >
                        Cognitive behavioral therapy
                    </Typography>
                    <Stack direction="row" alignItems="center" spacing={1}>
                        <Routes>
                            <Route
                                index
                                element={
                                    <IconButton
                                        size="large"
                                        component={Link}
                                        to={routes.profile()}
                                    >
                                        <AccountCircleIcon fontSize="inherit" />
                                    </IconButton>
                                }
                            />
                            <Route
                                path={routes.profile.pattern}
                                element={
                                    <IconButton
                                        size="large"
                                        component={Link}
                                        to={routes.home()}
                                    >
                                        <HomeIcon fontSize="inherit" />
                                    </IconButton>
                                }
                            />
                        </Routes>

                        <Button
                            color="inherit"
                            variant="text"
                            onClick={onLogout}
                            startIcon={<LogoutIcon />}
                        >
                            Log out
                        </Button>
                    </Stack>
                </Toolbar>
            </AppBar>
            <ToolbarOffset />
            <Routes>
                <Route index element={<div>here</div>} />
                <Route
                    path={routes.profile.pattern}
                    element={<ProfilePage />}
                />
                <Route
                    path="*"
                    element={<Navigate to={routes.home()} replace />}
                />
            </Routes>
        </>
    )
}

export default HomePage
