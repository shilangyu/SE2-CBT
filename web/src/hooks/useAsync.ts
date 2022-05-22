import { useCallback, useEffect, useRef, useState } from 'react'

export function useAsync<T extends any[], U>(
    promise: (...args: T) => Promise<U>
) {
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState<Error | undefined>(undefined)
    const [result, setResult] = useState<U | undefined>(undefined)
    const cancelled = useRef(false)

    useEffect(
        () => () => {
            cancelled.current = true
        },
        []
    )

    const call = useCallback(
        async (...args: T) => {
            setError(undefined)
            setResult(undefined)
            setLoading(true)

            try {
                const res = await promise(...args)
                if (!cancelled.current) {
                    setResult(res)
                    setLoading(false)
                }
                return res
            } catch (err) {
                if (!cancelled.current) {
                    setError(err as Error)
                    setLoading(false)
                }
            }
        },
        [promise, cancelled]
    )

    return { call, result, loading, error }
}
