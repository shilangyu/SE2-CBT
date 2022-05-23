import SettingsIcon from '@mui/icons-material/Settings'
import { Card, Container, Grid, Typography, useTheme } from '@mui/material'
import * as React from 'react'
import { dataTestAttr } from '../../utils/testing'
import UserList from './UserList'

function AdminPage() {
    const theme = useTheme()

    return (
        <Container maxWidth="xl" {...dataTestAttr('admin-page')}>
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
                                <SettingsIcon
                                    style={{
                                        width: '100%',
                                        height: '100%',
                                    }}
                                />
                            </Grid>
                            <Grid item sm={8} sx={{ padding: 3 }}>
                                <Typography variant="h3">
                                    admin panel
                                </Typography>
                                <Typography
                                    variant="h5"
                                    color={theme.palette.text.secondary}
                                >
                                    application management
                                </Typography>
                            </Grid>
                        </Grid>
                    </Card>
                </Grid>
                <Grid item xs={12}>
                    <Card variant="outlined" elevation={0} sx={{ padding: 3 }}>
                        <Typography variant="h5">User management</Typography>
                        <UserList />
                    </Card>
                </Grid>
            </Grid>
        </Container>
    )
}

export default AdminPage
