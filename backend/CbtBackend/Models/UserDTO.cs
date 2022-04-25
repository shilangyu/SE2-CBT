using CbtBackend.Entities;

namespace CbtBackend.Models;

public class UserDTO {
    public int UserID { get; set; }

    public string Login { get; set; }

    public string Password { get; set; }

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public int UserStatus { get; set; }

    public bool Banned { get; set; }

    public UserDTO(User user) {
        UserID = user.Id.GetHashCode();
        Login = user.Email;
        Password = user.PasswordHash;
        Age = user.Age;
        Gender = user.Gender;
        UserStatus = user.UserStatus;
        Banned = user.Banned;
    }
}
