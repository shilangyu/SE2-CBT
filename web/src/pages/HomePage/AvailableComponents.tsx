import {
    Button,
    Card,
    CardActions,
    CardContent,
    CardMedia,
    Stack,
    Typography,
} from '@mui/material'
import * as React from 'react'
import { Link } from 'react-router-dom'
import { useLoginStore } from '../../stores/loginStore'
import { dataTestAttr } from '../../utils/testing'
import { routes } from '../routes'

function AvailableComponents() {
    const isAdmin = useLoginStore(e => e.userData?.isAdmin ?? false)

    return (
        <Stack direction="row" spacing={4} alignSelf="center" padding={8}>
            <Card sx={{ maxWidth: 345 }}>
                <CardMedia
                    component="img"
                    height="300"
                    image="https://www.wasatch.org/blog/wp-content/uploads/2015/10/Mood.png"
                    alt="moodmeter"
                />
                <CardContent>
                    <Typography gutterBottom variant="h5" component="div">
                        Moodtests
                    </Typography>
                    <Typography variant="body2" color="text.secondary">
                        Take a quick moodtest for an instant mental health.
                        checkup
                    </Typography>
                </CardContent>
                <CardActions>
                    <Button
                        component={Link}
                        to={routes.moodtests()}
                        size="small"
                        {...dataTestAttr('dashboard-moodtests')}
                    >
                        See all moodtests
                    </Button>
                </CardActions>
            </Card>

            {isAdmin && (
                <Card sx={{ maxWidth: 345 }}>
                    <CardMedia
                        component="img"
                        height="300"
                        image="https://www.iconpacks.net/icons/1/free-user-group-icon-296-thumb.png"
                        alt="users"
                    />
                    <CardContent>
                        <Typography gutterBottom variant="h5" component="div">
                            User management
                        </Typography>
                        <Typography variant="body2" color="text.secondary">
                            Manage users in the system: edit, ban, or delete
                            users.
                        </Typography>
                    </CardContent>
                    <CardActions>
                        <Button
                            component={Link}
                            to={routes.admin()}
                            size="small"
                            {...dataTestAttr('dashboard-user-management')}
                        >
                            Manage existing users
                        </Button>
                    </CardActions>
                </Card>
            )}
        </Stack>
    )
}

export default AvailableComponents
