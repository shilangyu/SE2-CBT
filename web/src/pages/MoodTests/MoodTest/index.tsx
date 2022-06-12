import LoadingButton from '@mui/lab/LoadingButton'
import {
    Dialog,
    DialogTitle as MiuDialogTitle,
    IconButton,
    Stack,
} from '@mui/material'
import { useSnackbar } from 'notistack'
import * as React from 'react'
import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { apiClient } from '../../../api'
import { useAsync } from '../../../hooks/useAsync'
import { Moodtest, MoodtestResponse } from '../../../model/moodtest'
import { routes } from '../../routes'

import CloseIcon from '@mui/icons-material/Close'
import DialogActions from '@mui/material/DialogActions'
import DialogContent from '@mui/material/DialogContent'
import { dataTestAttr } from '../../../utils/testing'
import Question from './Question'

export interface DialogTitleProps {
    children?: React.ReactNode
    onClose: () => void
}

const DialogTitle = (props: DialogTitleProps) => {
    const { children, onClose, ...other } = props

    return (
        <MiuDialogTitle sx={{ m: 0, p: 2 }} variant="h4" {...other}>
            <Stack direction="row" justifyContent="space-between">
                {children}

                <IconButton
                    aria-label="close"
                    onClick={onClose}
                    sx={{
                        marginLeft: 8,
                        color: theme => theme.palette.grey[500],
                    }}
                >
                    <CloseIcon />
                </IconButton>
            </Stack>
        </MiuDialogTitle>
    )
}

export type Props = {
    moodtest: Moodtest
}

const MoodTest: React.FC<Props> = ({ moodtest }) => {
    const [response, setResponse] = useState<MoodtestResponse>({
        testId: moodtest.id,
        response1: 1,
        response2: 1,
        response3: 1,
        response4: 1,
        response5: 1,
    })
    const navigate = useNavigate()

    const {
        result,
        call: saveResponse,
        loading,
        error,
    } = useAsync(apiClient.saveMoodtestResponse)
    const { enqueueSnackbar } = useSnackbar()

    function handleClose() {
        if (!loading) {
            navigate(routes.moodtests())
        }
    }

    function handleSaveResponse() {
        saveResponse(response)
    }

    useEffect(() => {
        if (error) {
            enqueueSnackbar(`Failed to submit response: ${error}`, {
                variant: 'error',
            })
        }
    }, [error])

    useEffect(() => {
        if (result) {
            enqueueSnackbar(`Response submitted`, {
                variant: 'success',
            })
            navigate(routes.moodtests())
        }
    }, [result])

    return (
        <>
            <Dialog onClose={handleClose} open={true}>
                <DialogTitle onClose={handleClose}>
                    Moodtest: {moodtest.name}
                </DialogTitle>
                <DialogContent dividers>
                    <Stack
                        display="flex"
                        justifyContent="center"
                        alignItems="center"
                        minHeight="100%"
                    >
                        <Question
                            question={moodtest.question1}
                            response={response.response1}
                            onChange={val =>
                                setResponse(s => ({ ...s, response1: val }))
                            }
                        />
                        <Question
                            question={moodtest.question2}
                            response={response.response2}
                            onChange={val =>
                                setResponse(s => ({ ...s, response2: val }))
                            }
                        />
                        <Question
                            question={moodtest.question3}
                            response={response.response3}
                            onChange={val =>
                                setResponse(s => ({ ...s, response3: val }))
                            }
                        />
                        <Question
                            question={moodtest.question4}
                            response={response.response4}
                            onChange={val =>
                                setResponse(s => ({ ...s, response4: val }))
                            }
                        />
                        <Question
                            question={moodtest.question5}
                            response={response.response5}
                            onChange={val =>
                                setResponse(s => ({ ...s, response5: val }))
                            }
                        />
                    </Stack>
                </DialogContent>
                <DialogActions>
                    <LoadingButton
                        loading={loading}
                        variant="outlined"
                        onClick={handleSaveResponse}
                        {...dataTestAttr('moodtests-test-save-button')}
                    >
                        Save response
                    </LoadingButton>
                </DialogActions>
            </Dialog>
        </>
    )
}

export default MoodTest
