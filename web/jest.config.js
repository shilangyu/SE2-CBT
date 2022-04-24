module.exports = {
    moduleFileExtensions: ['js', 'ts', 'tsx', 'json'],
    transform: {
        '^.+\\.tsx?$': 'ts-jest',
    },
    setupFilesAfterEnv: ['./src/tests/setup.ts'],
    testEnvironment: 'jsdom',
}
