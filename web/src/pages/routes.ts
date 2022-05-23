import { createRouting, segment } from 'ts-routes'

export const routes = createRouting({
    home: segment`/`,
    login: segment`/login`,
    register: segment`/register`,
    profile: segment`/profile`,
    admin: segment`/admin`,
})
