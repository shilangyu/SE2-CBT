import { createRouting, number, segment } from 'ts-routes'

export const routes = createRouting({
    home: segment`/`,
    login: segment`/login`,
    register: segment`/register`,
    profile: segment`/profile`,
    admin: segment`/admin`,
    moodtests: segment`/moodtests`,
    moodtest: segment`/moodtests/${number('testId')}`,
})
