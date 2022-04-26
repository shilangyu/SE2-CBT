using System.ComponentModel.DataAnnotations;

namespace CbtBackend.Models.Requests;

public class UserUpdateRequest {
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
    public bool? Banned { get; set; }
    public int? UserStatus { get; set; }
}
