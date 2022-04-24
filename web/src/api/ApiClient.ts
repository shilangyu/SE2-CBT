import { EmailUsedError, UnauthorizedError } from '.'
import { useLoginStore } from '../stores/loginStore'

export class ApiClient {
    constructor(private baseUrl: string) {
        console.log(`Initialized ApiClient for ${baseUrl}`)
    }

    private async baseRequest<T>(path: string, options: RequestInit = {}) {
        const token = useLoginStore.getState().token
        const { headers, ...otherOptions } = options

        const res = await fetch(`${this.baseUrl}/${path}`, {
            headers: {
                ...(token && { Authorization: `Bearer ${token}` }),
                'Content-Type': 'application/json',
                ...headers,
            },
            ...otherOptions,
        })

        if (res.status === 401) {
            if (token !== undefined) {
                console.log('Unauthorized, logging out')
                useLoginStore.getState().logOut()
            }
            throw new UnauthorizedError()
        } else if (!res.ok) {
            throw new FailedRequestError(res)
        } else {
            const json = await res.json()
            return json as T
        }
    }

    async logIn(email: string, password: string): Promise<string> {
        const res = await this.baseRequest<{ accessToken: string }>(
            `user/login`,
            {
                method: 'POST',
                body: JSON.stringify({ login: email, password }),
            }
        )

        return res.accessToken
    }

    async register(
        email: string,
        password: string,
        age: number,
        gender: string
    ): Promise<void> {
        try {
            return await this.baseRequest<void>(`user`, {
                method: 'POST',
                body: JSON.stringify({
                    login: email,
                    password,
                    age,
                    gender,
                    banned: false,
                }),
            })
        } catch (err) {
            if (
                err instanceof FailedRequestError &&
                err.response.status === 400
            ) {
                throw new EmailUsedError()
            }
            throw err
        }
    }
}

class FailedRequestError extends Error {
    constructor(public response: Response) {
        super()
    }
}
