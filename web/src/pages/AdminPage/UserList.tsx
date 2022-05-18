import { Button, Stack, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material";
import { useSnackbar } from 'notistack';
import { useEffect, useState } from "react";
import { apiClient } from "../../api";
import { User, UserUpdateRequest } from "../../model/user";
import UserEditor from "./UserEditor";

interface UserListProps {

}

const UserList : React.FC<UserListProps> = (props : UserListProps) => {
    const [users, setUsers] = useState<User[] | null>(null)
    const [error, setError] = useState<string | null>(null)
    const [selectedUser, setSelectedUser] = useState<User | null>(null)

    const {enqueueSnackbar} = useSnackbar()

    const refreshUserList = () => {
        apiClient.getUsers().then(list => {
            if (list) {
                setUsers(list)
            } else {
                setUsers([])
            }
        }).catch(error => {
            console.error(`failed to fetch user list: ${error}`)
            setError('failed to fetch the list of users, check console for details')
        })
    }

    useEffect(() => {
        refreshUserList()
    }, [])

    const renderUserActions = (user : User) => {
        return (
            <Stack direction="row" spacing={2}>
                <Button variant="outlined" onClick={() => {
                    setSelectedUser(user)
                }}>Edit</Button>
                <Button variant="outlined" onClick={() => setUserBan(user.userId, !user.banned)}>
                    {user.banned ? 'Unban' : 'Ban'}
                </Button>
                <Button variant="outlined">Delete</Button>
            </Stack>
        )
    }

    const setUserBan = (userId : number, banned : boolean) => {
        const req = { banned } as UserUpdateRequest
        apiClient.updateUser(userId, req).then(() => {
            refreshUserList()
        }).catch(err => {
            enqueueSnackbar(
                `failed to ${banned?'ban':'unban'} user ${userId} (details in console)`,
                { variant: 'error' }
            )
            console.error(`failed to change user ban: ${err}`)
        })
    }

    const saveUser = async (userId : number, req : UserUpdateRequest) => {
        await apiClient.updateUser(userId, req)
        setSelectedUser(null)
        refreshUserList()
    }

    if (users) {
        return (
            <Table>
                <UserEditor user={selectedUser}
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
                        return <TableRow key={user.userId}>
                            <TableCell>{user.userId}</TableCell>
                            <TableCell>{user.login}</TableCell>
                            <TableCell>{user.banned ? 'yes' : 'no'}</TableCell>
                            <TableCell>
                                {renderUserActions(user)}
                            </TableCell>
                        </TableRow>
                    })}
                </TableBody>
            </Table>
        )
    } else {
        return <Typography>there are no users registered in the system</Typography>;
    }
}

export default UserList;