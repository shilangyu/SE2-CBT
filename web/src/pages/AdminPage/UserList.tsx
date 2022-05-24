import {
    Button,
    Stack,
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableRow,
    Typography,
} from '@mui/material'
import { useSnackbar } from 'notistack'
import * as React from 'react'
import { useEffect, useState } from 'react'
import { apiClient } from '../../api'
import { User, UserUpdateRequest } from '../../model/user'
import { dataTestAttr } from '../../utils/testing'
import UserEditor from './UserEditor'

const UserList: React.FC = () => {
    const [users, setUsers] = useState<User[] | null>(null)
    const [selectedUser, setSelectedUser] = useState<User | null>(null)

    const { enqueueSnackbar } = useSnackbar()

    const refreshUserList = () => {
        apiClient
            .getUsers()
            .then(list => {
                setUsers(list)
            })
            .catch(error => {
                console.error(`failed to fetch user list: ${error}`)
                enqueueSnackbar(
                    `failed to refresh user list (check console for details)`,
                    {
                        variant: 'error',
                    }
                )
            })
    }

    useEffect(() => {
        refreshUserList()
    }, [])

    const renderUserActions = (user: User) => {
        return (
            <Stack direction="row" spacing={2}>
                <Button
                    variant="outlined"
                    onClick={() => setSelectedUser(user)}
                    {...dataTestAttr('admin-user-list-edit-button')}
                >
                    Edit
                </Button>
                <Button
                    variant="outlined"
                    onClick={() => setUserBan(user.userId, !user.banned)}
                    {...dataTestAttr('admin-user-list-ban-button')}
                >
                    {user.banned ? 'Unban' : 'Ban'}
                </Button>
                <Button
                    variant="outlined"
                    onClick={() => deleteUser(user.userId)}
                    {...dataTestAttr('admin-user-list-delete-button')}
                >
                    Delete
                </Button>
            </Stack>
        )
    }

    const setUserBan = (userId: number, banned: boolean) => {
        const req: UserUpdateRequest = { banned }
        apiClient
            .updateUser(userId, req)
            .then(() => {
                refreshUserList()
            })
            .catch(err => {
                enqueueSnackbar(
                    `failed to ${
                        banned ? 'ban' : 'unban'
                    } user ${userId} (details in console)`,
                    { variant: 'error' }
                )
                console.error(`failed to change user ban: ${err}`)
            })
    }

    const deleteUser = (userId: number) => {
        apiClient
            .deleteUser(userId)
            .then(() => {
                refreshUserList()
            })
            .catch(err => {
                enqueueSnackbar(`failed to delete user ${userId}`, {
                    variant: 'error',
                })
                console.error(`failed to delete user: ${err}`)
            })
    }

    const saveUser = async (userId: number, req: UserUpdateRequest) => {
        await apiClient.updateUser(userId, req)
        setSelectedUser(null)
        refreshUserList()
    }

    if (users) {
        return (
            <Table>
                <UserEditor
                    user={selectedUser}
                    onCancel={() => {
                        setSelectedUser(null)
                    }}
                    onSave={saveUser}
                />
                <TableHead>
                    <TableCell>User ID</TableCell>
                    <TableCell>User Login</TableCell>
                    <TableCell>Banned</TableCell>
                    <TableCell>User Actions</TableCell>
                </TableHead>
                <TableBody>
                    {users.map((user, index) => {
                        return (
                            <TableRow key={user.userId}>
                                <TableCell
                                    {...dataTestAttr('admin-user-list-id')}
                                >
                                    {user.userId}
                                </TableCell>
                                <TableCell
                                    {...dataTestAttr('admin-user-list-login')}
                                >
                                    {user.login}
                                </TableCell>
                                <TableCell
                                    {...dataTestAttr('admin-user-list-banned')}
                                >
                                    {user.banned ? 'yes' : 'no'}
                                </TableCell>
                                <TableCell>{renderUserActions(user)}</TableCell>
                            </TableRow>
                        )
                    })}
                </TableBody>
            </Table>
        )
    } else {
        return (
            <Typography>there are no users registered in the system</Typography>
        )
    }
}

export default UserList
