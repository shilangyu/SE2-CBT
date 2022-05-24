import {
    Button,
    Card,
    CardActions,
    CardContent,
    CircularProgress,
    Grid,
    Stack,
    Typography,
} from '@mui/material'

import { useSnackbar } from 'notistack'
import * as React from 'react'
import { useEffect } from 'react'
import { Link, Route, Routes, useParams } from 'react-router-dom'
import { apiClient } from '../../api'
import { useAsync } from '../../hooks/useAsync'

import { routes } from '../routes'
import MoodTest from './MoodTest'

function MoodTests() {
    const {
        result: moodtests,
        call: fetchMoodtests,
        loading,
        error,
    } = useAsync(apiClient.getAllMoodtests)
    const { enqueueSnackbar } = useSnackbar()
    const params = useParams<'*'>()

    useEffect(() => {
        fetchMoodtests()
    }, [])

    useEffect(() => {
        if (error) {
            enqueueSnackbar(`Failed to fetch mood tests: ${error}`, {
                variant: 'error',
            })
        }
    }, [error])

    return (
        <Grid
            container
            justifyContent="center"
            alignItems="center"
            minHeight="100%"
            spacing={5}
        >
            {loading && (
                <Stack alignItems="center">
                    <CircularProgress />
                    <Typography>Fetching moodtests</Typography>
                </Stack>
            )}
            {error && (
                <Button variant="outlined" onClick={fetchMoodtests}>
                    Try again
                </Button>
            )}
            {moodtests && (
                <>
                    <Routes>
                        {params['*'] && (
                            <Route
                                path="*"
                                element={
                                    <MoodTest
                                        moodtest={
                                            moodtests.find(
                                                e => e.id === +params['*']!
                                            )!
                                        }
                                    />
                                }
                            />
                        )}
                    </Routes>
                    {moodtests.map(test => (
                        <Grid item key={test.id}>
                            <Card sx={{ minWidth: 275 }}>
                                <CardContent>
                                    <Typography variant="h5" component="div">
                                        {test.name}
                                    </Typography>
                                    <Typography variant="body2">
                                        {test.description}
                                    </Typography>
                                </CardContent>
                                <CardActions>
                                    <Button
                                        component={Link}
                                        size="small"
                                        to={routes.moodtest({
                                            testId: test.id.toString(),
                                        })}
                                    >
                                        Take test
                                    </Button>
                                </CardActions>
                            </Card>
                        </Grid>
                    ))}
                </>
            )}
        </Grid>
    )
}

export default MoodTests
