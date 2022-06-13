// kept as an enum to prevent repetitions
export enum DataTestId {
    'register-email-input',
    'register-password-input',
    'register-age-input',
    'register-gender-input',
    'register-gender-input-item',
    'register-login-button',
    'register-create-account-button',
    'login-email-input',
    'login-password-input',
    'login-create-account-button',
    'login-login-button',
    'navbar-profile-button',
    'navbar-home-button',
    'navbar-logout-button',
    'navbar-admin-button',
    'dashboard-moodtests',
    'dashboard-user-management',
    'admin-user-list-id',
    'admin-user-list-login',
    'admin-user-list-banned',
    'admin-user-list-edit-button',
    'admin-user-list-ban-button',
    'admin-user-list-delete-button',
    'admin-user-edit-login-input',
    'admin-user-edit-cancel',
    'admin-user-edit-save',
    'moodtests-test-start-button',
    'moodtests-test-rating',
    'moodtests-test-save-button',
    'moodtests-results-button',
    'moodtests-results-details-button',
    'moodtests-results-details-score',
    'moodtests-results-back-button',
}

export const dataTestAttr = (key: keyof typeof DataTestId) => ({
    'data-testid': key,
})

export const dataTestInputProp = (key: keyof typeof DataTestId) => ({
    inputProps: dataTestAttr(key),
})
