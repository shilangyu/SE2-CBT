import { UserTokenClaims } from '../model/user'
import { Buffer } from 'buffer'

export class JwtParseError extends Error {}

export const parseJwt = (token: string): UserTokenClaims => {
    const tokenParts = token.split('.')

    if (tokenParts.length !== 3) {
        throw new JwtParseError('invalid jwt format')
    }

    const claimsBuffer = Buffer.from(tokenParts[1], 'base64')
    const claimsString = claimsBuffer.toString('utf8')

    return JSON.parse(claimsString) as UserTokenClaims
}
