import { EmailUsedError, UnauthorizedError } from '.'
import { LoginResponse } from '../model/responses'
import { User, UserUpdateRequest } from '../model/user'
import { useLoginStore } from '../stores/loginStore'
import {
    Moodtest,
    MoodtestFullResponse,
    MoodtestFullResponseDto,
    MoodtestResponse,
} from '../model/moodtest'

export class ApiClient {
    constructor(private baseUrl: string) {
        console.log(`Initialized ApiClient for ${baseUrl}`)
    }

    private async baseRequest<T>(path: string, options: RequestInit = {}) {
        const token = useLoginStore.getState().userData?.token
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
            try {
                const json = await res.json()
                return json as T
            } catch {
                // we assume `T` would be void here
                return null as unknown as T
            }
        }
    }

    async logIn(email: string, password: string): Promise<LoginResponse> {
        const res = await this.baseRequest<LoginResponse>(`user/login`, {
            method: 'POST',
            body: JSON.stringify({ login: email, password }),
        })

        return res
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
                }),
            })
        } catch (err) {
            if (
                err instanceof FailedRequestError &&
                err.response.status === 409
            ) {
                throw new EmailUsedError()
            }
            throw err
        }
    }

    async getUsers(): Promise<User[]> {
        const users = await this.baseRequest<User[]>(`user`, {
            method: 'GET',
        })

        return users
    }

    async getUser(userId: number): Promise<User> {
        return await this.baseRequest<User>(`user/${userId}`, {
            method: 'GET',
        })
    }

    async updateUser(userId: number, body: UserUpdateRequest): Promise<void> {
        return await this.baseRequest<void>(`user/${userId}`, {
            method: 'PUT',
            body: JSON.stringify(body),
        })
    }

    async deleteUser(userId: number): Promise<void> {
        return await this.baseRequest<void>(`user/${userId}`, {
            method: 'DELETE',
        })
    }

    getAllMoodtests = async (): Promise<Moodtest[]> => {
        return await this.baseRequest<Moodtest[]>('moodtest', {
            method: 'GET',
        })
    }

    saveMoodtestResponse = async (
        response: MoodtestResponse
    ): Promise<MoodtestFullResponse> => {
        response = {
            ...response,
            response1: response.response1 - 1,
            response2: response.response2 - 1,
            response3: response.response3 - 1,
            response4: response.response4 - 1,
            response5: response.response5 - 1,
        }

        const res = await this.baseRequest<MoodtestFullResponseDto>(
            'evaluation',
            {
                method: 'POST',
                body: JSON.stringify(response),
            }
        )

        return mapMoodtestResponseDto(res)
    }

    getAllMoodtestResponses = async (
        userId: number
    ): Promise<MoodtestFullResponse[]> => {
        const res = await this.baseRequest<MoodtestFullResponseDto[]>(
            `evaluation/findByUserId?userId=${userId}`
        )

        return res.map(mapMoodtestResponseDto)
    }
}

export class FailedRequestError extends Error {
    constructor(public response: Response) {
        super()
    }
}

function mapMoodtestResponseDto(
    dto: MoodtestFullResponseDto
): MoodtestFullResponse {
    return { ...dto, submitted: new Date(dto.submitted) }
}
