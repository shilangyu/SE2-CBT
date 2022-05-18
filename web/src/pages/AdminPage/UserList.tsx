import { Button, Stack, Table, TableBody, TableCell, TableHead, TableRow, Typography } from "@mui/material";
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
                {user.banned 
                    ? <Button variant="outlined">Unban</Button>
                    : <Button variant="outlined">Ban</Button>}
                <Button variant="outlined">Delete</Button>
            </Stack>
        )
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