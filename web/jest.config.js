module.exports = {
    moduleFileExtensions: ['js', 'ts', 'tsx', 'json'],
    transform: {
        '^.+\\.ts$': 'ts-jest',
    },
    setupFilesAfterEnv: ['./src/tests/setup.ts'],
}
