export interface User {
    userId: number
    login: string
    age: number
    gender: string
    userStatus: number
    banned: boolean
}

export interface UserUpdateRequest {
    login?: string
    password?: string
    age?: number
    gender?: string
    banned?: boolean
}

export interface UserTokenClaims {
    id: string
    unique_name: string
    role: string[]

    // expiration, issuer and audience
    nbf: number
    exp: number
    iat: number
    iss: string
    aud: string
}
