import { Grid, Typography } from '@mui/material'

import * as React from 'react'

import RatingPicker from './RatingPicker'

export type Props = {
    question: string
    response: number
    onChange: (value: number) => void
}

const Question: React.FC<Props> = ({ question, response, onChange }) => {
    return (
        <Grid container alignItems="center">
            <Grid item xs={7}>
                <Typography variant="body1" fontSize={24}>
                    {question}
                </Typography>
            </Grid>

            <Grid item xs={5}>
                <RatingPicker value={response} onChange={onChange} />
            </Grid>
        </Grid>
    )
}

export default Question
