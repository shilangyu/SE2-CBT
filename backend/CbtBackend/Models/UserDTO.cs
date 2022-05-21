using System.Text.Json;
using CbtBackend.Entities;

namespace CbtBackend.Models;

public class UserDTO {
    public int UserId { get; set; }

    public string Login { get; set; } = default!;

    public int? Age { get; set; }

    public string? Gender { get; set; }

    public int UserStatus { get; set; }

    public bool Banned { get; set; }

    public UserDTO() { }

    public UserDTO(User user) {
        UserId = user.Id;
        Login = user.Email;
        Age = user.Age;
        Gender = user.Gender;
        UserStatus = user.UserStatus;
        Banned = user.Banned;
    }
}
