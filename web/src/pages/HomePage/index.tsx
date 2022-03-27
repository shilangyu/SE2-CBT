import { Button, Grid, TextField } from '@mui/material'
import { useSnackbar } from 'notistack'
import * as React from 'react'
import { useLoginStore } from '../../stores/loginStore'

function HomePage() {
    const logOut = useLoginStore(e => e.logOut)
    const { enqueueSnackbar } = useSnackbar()

    function onLogout() {
        logOut()
        enqueueSnackbar('Logout successful', { variant: 'success' })
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
            <Grid item>Logged in!</Grid>
            <Button variant="outlined" onClick={onLogout}>
                Log out
            </Button>
        </Grid>
    )
}

export default HomePage
