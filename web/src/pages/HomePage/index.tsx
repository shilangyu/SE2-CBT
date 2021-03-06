import AccountCircleIcon from '@mui/icons-material/AccountCircle'
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
import { Link, Navigate, Route, Routes } from 'react-router-dom'
import { useLoginStore } from '../../stores/loginStore'
import { dataTestAttr } from '../../utils/testing'
import AdminPage from '../AdminPage'
import MoodTestResults from '../MoodTestResults'
import MoodTests from '../MoodTests'
import ProfilePage from '../ProfilePage'
import { routes } from '../routes'
import AvailableComponents from './AvailableComponents'

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
                        component={Link}
                        to={routes.home()}
                        {...dataTestAttr('navbar-home-button')}
                        sx={{ flexGrow: 1 }}
                    >
                        Cognitive behavioral therapy
                    </Typography>
                    <Stack direction="row" alignItems="center" spacing={1}>
                        <IconButton
                            size="large"
                            component={Link}
                            to={routes.profile()}
                            {...dataTestAttr('navbar-profile-button')}
                        >
                            <AccountCircleIcon fontSize="inherit" />
                        </IconButton>

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
                <Route index element={<AvailableComponents />} />
                <Route
                    path={routes.profile.pattern}
                    element={<ProfilePage />}
                />
                <Route
                    path={routes.moodtestResults.pattern}
                    element={<MoodTestResults />}
                />
                <Route
                    path={routes.moodtests.pattern + '/*'}
                    element={<MoodTests />}
                />
                <Route path={routes.admin.pattern} element={<AdminPage />} />
                <Route
                    path="*"
                    element={<Navigate to={routes.home()} replace />}
                />
            </Routes>
        </>
    )
}

export default HomePage
