import { Grid, TextField } from '@mui/material'
import * as React from 'react'

function App() {
    return (
        <Grid container direction="column" rowSpacing={4}>
            <Grid item>
                <TextField label="Username" />
            </Grid>
            <Grid item>
                <TextField label="Password" />
            </Grid>
        </Grid>
    )
}

export default App
