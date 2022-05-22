import * as React from 'react'
import Rating, { IconContainerProps } from '@mui/material/Rating'
import SentimentVeryDissatisfiedIcon from '@mui/icons-material/SentimentVeryDissatisfied'
import SentimentDissatisfiedIcon from '@mui/icons-material/SentimentDissatisfied'
import SentimentSatisfiedIcon from '@mui/icons-material/SentimentSatisfied'
import SentimentSatisfiedAltIcon from '@mui/icons-material/SentimentSatisfiedAltOutlined'
import SentimentVerySatisfiedIcon from '@mui/icons-material/SentimentVerySatisfied'

export const ratingPickerOptions: {
    [index: string]: {
        icon: React.ReactElement
        label: string
    }
} = {
    1: {
        icon: <SentimentVeryDissatisfiedIcon />,
        label: 'Not at all',
    },
    2: {
        icon: <SentimentDissatisfiedIcon />,
        label: 'Somewhat',
    },
    3: {
        icon: <SentimentSatisfiedIcon />,
        label: 'Moderately',
    },
    4: {
        icon: <SentimentSatisfiedAltIcon />,
        label: 'A lot',
    },
    5: {
        icon: <SentimentVerySatisfiedIcon />,
        label: 'Extremely',
    },
}

function IconContainer(props: IconContainerProps) {
    const { value, ...other } = props

    return <span {...other}>{ratingPickerOptions[value].icon}</span>
}

export type Props = {
    value: number
    onChange: (rating: number) => void
}

const RatingPicker: React.FC<Props> = ({ value, onChange }) => {
    return (
        <Rating
            name="highlight-selected-only"
            value={value}
            size="large"
            onChange={(_, val) => val && onChange(val)}
            IconContainerComponent={IconContainer}
            highlightSelectedOnly
        />
    )
}

export default RatingPicker
