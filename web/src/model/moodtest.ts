export type Moodtest = {
    id: number
    name: string
    description: string
    question1: string
    question2: string
    question3: string
    question4: string
    question5: string
}

export type MoodtestResponse = {
    testId: number
    response1: number
    response2: number
    response3: number
    response4: number
    response5: number
}

export type MoodtestFullResponse = Omit<MoodtestResponse, 'testId'> & {
    id: number
    userId: number
    submitted: string
    evaluation: Moodtest
}
