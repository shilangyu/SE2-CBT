import * as React from 'react'
import List from '@mui/material/List'
import ListItemText from '@mui/material/ListItemText'
import ListSubheader from '@mui/material/ListSubheader'
import { useMemo } from 'react'
import { MoodtestFullResponse } from '../../model/moodtest'
import { ListItemButton, ListItemIcon } from '@mui/material'
import { Done } from '@mui/icons-material'

export type Props = {
    responses: MoodtestFullResponse[]
    selectedResponse: MoodtestFullResponse | undefined
    onResponseSelected: (response: MoodtestFullResponse | undefined) => void
}

const MoodTestResultsList: React.FC<Props> = ({
    responses,
    onResponseSelected,
    selectedResponse,
}) => {
    const grouped = useMemo(() => {
        return responses.reduce<
            Record<number, { label: string; items: MoodtestFullResponse[] }>
        >(
            (acc, curr) => ({
                ...acc,
                [curr.evaluation.id]: {
                    label: curr.evaluation.name,
                    items: [
                        ...(acc[curr.evaluation.id]?.items ?? []),
                        curr,
                    ].sort((a, b) => +b.submitted - +a.submitted),
                },
            }),
            {}
        )
    }, [responses])

    return (
        <List
            sx={{
                overflow: 'auto',
                '& ul': { padding: 0 },
            }}
            subheader={<li />}
        >
            {Object.keys(grouped).map(group => (
                <li key={group}>
                    <ul>
                        <ListSubheader>{grouped[+group]!.label}</ListSubheader>
                        {grouped[+group]!.items.map(item => {
                            const isSelected = selectedResponse?.id === item.id

                            return (
                                <ListItemButton
                                    key={item.id}
                                    selected={isSelected}
                                    onClick={() =>
                                        isSelected
                                            ? onResponseSelected(undefined)
                                            : onResponseSelected(item)
                                    }
                                >
                                    <ListItemText
                                        primary={formatListItem(item)}
                                    />
                                    {isSelected && (
                                        <ListItemIcon>
                                            <Done />
                                        </ListItemIcon>
                                    )}
                                </ListItemButton>
                            )
                        })}
                    </ul>
                </li>
            ))}
        </List>
    )
}

function formatListItem(item: MoodtestFullResponse): string {
    return item.submitted.toDateString()
}

export default MoodTestResultsList
