import create from 'zustand'

type LoginStore = {
    token: string
}

const useLoginStore = create<LoginStore>(set => ({
    token: '',
    setToken: (token: string) => set(state => ({ token })),
}))
