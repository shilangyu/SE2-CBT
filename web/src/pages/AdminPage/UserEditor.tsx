import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    TextField,
} from '@mui/material'
import * as React from 'react'
import { useEffect, useState } from 'react'
import { User, UserUpdateRequest } from '../../model/user'
import { dataTestAttr, dataTestInputProp } from '../../utils/testing'

interface UserEditorProps {
    user: User | null
    onSave: (userId: number, newUser: UserUpdateRequest) => Promise<void>
    onCancel: () => void
}

const UserEditor: React.FC<UserEditorProps> = (props: UserEditorProps) => {
    const [user, setUser] = useState<User | null>(null)
    const [error, setError] = useState<string | null>(null)
    const [login, setLogin] = useState('')

    useEffect(() => {
        setUser(props.user)

        if (props.user) {
            setLogin(props.user.login)
        }
    }, [props.user])

    const handleSubmit = () => {
        if (user) {
            const req: UserUpdateRequest = { login }
            props.onSave(user.userId, req).catch(err => {
                setError(err)
            })
        }
    }

    if (props.user) {
        return (
            <Dialog open={!!user} onClose={props.onCancel}>
                <DialogTitle>Editing {props.user.login}</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Editing user {props.user.login} with id{' '}
                        {props.user.userId}
                    </DialogContentText>
                    <TextField
                        margin="dense"
                        id="login"
                        label="User Login"
                        type="email"
                        defaultValue={login}
                        fullWidth
                        variant="standard"
                        onChange={ev => setLogin(ev.target.value)}
                        {...dataTestInputProp('admin-user-edit-login-input')}
                    />
                </DialogContent>
                <DialogActions>
                    <Button
                        onClick={props.onCancel}
                        variant="outlined"
                        {...dataTestAttr('admin-user-edit-cancel')}
                    >
                        Cancel
                    </Button>
                    <Button
                        onClick={handleSubmit}
                        variant="contained"
                        {...dataTestAttr('admin-user-edit-save')}
                    >
                        Save
                    </Button>
                </DialogActions>
            </Dialog>
        )
    } else {
        return <></>
    }
}

export default UserEditor
