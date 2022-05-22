import AccountCircleIcon from '@mui/icons-material/AccountCircle'
import HomeIcon from '@mui/icons-material/Home'
import LogoutIcon from '@mui/icons-material/Logout'
import {
    AppBar,
    Button,
    IconButton,
    Stack,
    styled,
    Toolbar,
    Typography,
} from '@mui/material'
import { useSnackbar } from 'notistack'
import * as React from 'react'
import { Link, Navigate, Route, Routes } from 'react-router-dom'
import { useLoginStore } from '../../stores/loginStore'
import { dataTestAttr } from '../../utils/testing'
import AdminPage from '../AdminPage'
import ProfilePage from '../ProfilePage'
import { routes } from '../routes'

const ToolbarOffset = styled('div')(({ theme }) => theme.mixins.toolbar)

function HomePage() {
    const isAdmin = useLoginStore(e => e.userData?.isAdmin ?? false)
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
                                        {...dataTestAttr(
                                            'navbar-profile-button'
                                        )}
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
                                        {...dataTestAttr('navbar-home-button')}
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
                            {...dataTestAttr('navbar-logout-button')}
                        >
                            Log out
                        </Button>
                    </Stack>
                </Toolbar>
            </AppBar>
            <ToolbarOffset />
            <Routes>
                <Route
                    index
                    element={
                        isAdmin ? (
                            <AdminPage />
                        ) : (
                            <div>Welcome normal user!</div>
                        )
                    }
                />
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
