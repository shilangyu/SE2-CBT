import { ArrowBack } from '@mui/icons-material'
import {
    Button,
    CircularProgress,
    Divider,
    IconButton,
    Stack,
    Typography,
} from '@mui/material'

import { useSnackbar } from 'notistack'
import * as React from 'react'
import { useEffect } from 'react'
import { useState } from 'react'
import { Link } from 'react-router-dom'
import { apiClient } from '../../api'
import { useAsync } from '../../hooks/useAsync'
import { MoodtestFullResponse } from '../../model/moodtest'
import { useLoginStore } from '../../stores/loginStore'

import { routes } from '../routes'
import MoodTestResultsList from './MoodTestResultsList'
import MoodTestResultsTable from './MoodTestResultsTable'

function MoodTestResults() {
    const loginStore = useLoginStore()
    const {
        result: moodtests,
        call: fetchMoodtests,
        loading,
        error,
    } = useAsync(() =>
        // assuming to be logged in in this route
        apiClient.getAllMoodtestResponses(loginStore.userData!.userId)
    )
    const { enqueueSnackbar } = useSnackbar()

    const [selectedResponse, setSelectedResponse] = useState<
        MoodtestFullResponse | undefined
    >(undefined)

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
        <>
            <IconButton size="large" component={Link} to={routes.moodtests()}>
                <ArrowBack fontSize="inherit" />
            </IconButton>

            <Stack
                justifyContent="center"
                alignItems="center"
                direction="row"
                minHeight="100%"
                divider={<Divider orientation="vertical" flexItem />}
                spacing={5}
                sx={{
                    '&': {
                        padding: 5,
                    },
                    '& > ul': {
                        width: '30%',
                        height: 500,
                    },
                    '& > div': {
                        width: '70%',
                    },
                }}
            >
                {loading && (
                    <Stack alignItems="center">
                        <CircularProgress />
                        <Typography>Fetching moodtest responses</Typography>
                    </Stack>
                )}
                {error && (
                    <Button variant="outlined" onClick={fetchMoodtests}>
                        Try again
                    </Button>
                )}
                {moodtests?.length === 0 && (
                    <Typography variant="h5">
                        You did not complete any moodtests, create one to see
                        your results here!
                    </Typography>
                )}
                {moodtests && moodtests.length !== 0 && (
                    <MoodTestResultsList
                        responses={moodtests}
                        onResponseSelected={setSelectedResponse}
                        selectedResponse={selectedResponse}
                    />
                )}
                {moodtests && moodtests.length !== 0 && (
                    <div>
                        {selectedResponse ? (
                            <MoodTestResultsTable
                                totalScore={calculateTotalScore(
                                    selectedResponse
                                )}
                                moodtest={selectedResponse.evaluation}
                            />
                        ) : (
                            <Typography
                                variant="h6"
                                sx={{ textAlign: 'center' }}
                            >
                                Select a moodtest response to see the results.
                            </Typography>
                        )}
                    </div>
                )}
            </Stack>
        </>
    )
}

function calculateTotalScore(response: MoodtestFullResponse): number {
    return (
        response.response1 +
        response.response2 +
        response.response3 +
        response.response4 +
        response.response5
    )
}

export default MoodTestResults
