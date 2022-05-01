// kept as an enum to prevent repetitions
export enum DataTestId {
    'register-email-input',
    'register-password-input',
    'register-age-input',
    'register-gender-input',
    'register-login-button',
    'register-create-account-button',
    'login-email-input',
    'login-password-input',
    'login-create-account-button',
    'login-login-button',
}

export const dataTestAttr = (key: keyof typeof DataTestId) => ({
    'data-testid': key,
})

export const dataTestInputProp = (key: keyof typeof DataTestId) => ({
    inputProps: dataTestAttr(key),
})
