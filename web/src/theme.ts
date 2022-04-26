import { createTheme } from '@mui/material'
import { orange, grey } from '@mui/material/colors'

export const theme = createTheme({
    palette: {
        primary: { main: orange[500] },
        text: { primary: grey[900], secondary: grey[500] },
    },

    components: {
        MuiTextField: {
            defaultProps: {
                variant: 'outlined',
            },
        },
    },
})
