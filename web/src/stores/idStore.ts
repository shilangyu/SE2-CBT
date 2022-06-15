import create from 'zustand'

export const useIdStore = create((set, get) => ({
    userId: -1,
}))
