import { createTheme } from '@mui/material'
import { orange } from '@mui/material/colors'

export const theme = createTheme({
    palette: {
        primary: { main: orange[500] },
    },
    components: {
        MuiTextField: {
            defaultProps: {
                variant: 'outlined',
            },
        },
    },
})
