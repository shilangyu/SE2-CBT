const localStorageMock = new (class implements Storage {
    [name: string]: any

    length = 0

    clear(): void {
        this.length = 0
    }

    getItem(key: string): string | null {
        return this[key]
    }

    key(index: number): string | null {
        throw new Error('Method not implemented.')
    }

    removeItem(key: string): void {
        delete this[key]
    }

    setItem(key: string, value: string): void {
        this[key] = value
    }
})()

global.localStorage = localStorageMock

jest.mock('../utils/config.ts', () => ({
    apiUrl: 'something',
}))

jest.mock('../api/ApiClient.ts')

export {}
