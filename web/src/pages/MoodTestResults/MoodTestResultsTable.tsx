import * as React from 'react'
import Table from '@mui/material/Table'
import TableBody from '@mui/material/TableBody'
import TableCell from '@mui/material/TableCell'
import TableContainer from '@mui/material/TableContainer'
import TableHead from '@mui/material/TableHead'
import TableRow from '@mui/material/TableRow'
import Paper from '@mui/material/Paper'
import { Moodtest } from '../../model/moodtest'
import { Typography } from '@mui/material'

export type Props = {
    moodtest: Moodtest
    totalScore: number
}

const MoodTestResultsTable: React.FC<Props> = ({ moodtest, totalScore }) => {
    return (
        <>
            <Typography variant="h4" sx={{ textAlign: 'center' }}>
                Results table for {moodtest.name}
            </Typography>
            <Typography
                variant="subtitle1"
                sx={{ textAlign: 'center', marginBottom: 4 }}
            >
                Your score: {totalScore}
            </Typography>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell align="center" width="20%">
                                Score
                            </TableCell>
                            <TableCell align="center" width="20%">
                                {moodtest.resultsTable.entryCategory}
                            </TableCell>
                            <TableCell align="center" width="20%">
                                Meaning
                            </TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {moodtest.resultsTable.entries
                            .sort((a, b) => a.scoreFrom - b.scoreFrom)
                            .map(entry => (
                                <TableRow
                                    key={entry.id}
                                    hover
                                    selected={
                                        totalScore <= entry.scoreTo &&
                                        totalScore >= entry.scoreFrom
                                    }
                                >
                                    <TableCell align="center">
                                        {entry.scoreFrom !== entry.scoreTo
                                            ? `${entry.scoreFrom}-${entry.scoreTo}`
                                            : entry.scoreFrom.toString()}
                                    </TableCell>
                                    <TableCell align="center">
                                        {entry.entryName}
                                    </TableCell>
                                    <TableCell align="center">
                                        {entry.description}
                                    </TableCell>
                                </TableRow>
                            ))}
                    </TableBody>
                </Table>
            </TableContainer>
        </>
    )
}

export default MoodTestResultsTable
