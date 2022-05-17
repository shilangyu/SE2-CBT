import { Table, TableBody, TableCell, TableRow, Typography } from "@mui/material";
import { useEffect, useState } from "react";
import { apiClient } from "../../api";
import { User } from "../../model/user";

interface UserListProps {

}

const UserList : React.FC<UserListProps> = (props : UserListProps) => {
    const [users, setUsers] = useState<User[] | null>(null);
    const [error, setError] = useState<string | null>(null);

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

    if (users) {
        return (
            <Table>
                <TableBody>
                    {users.map((user, index) => {
                        return <TableRow key={user.userId}>
                            <TableCell>{user.userId}</TableCell>
                            <TableCell>{user.login}</TableCell>
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