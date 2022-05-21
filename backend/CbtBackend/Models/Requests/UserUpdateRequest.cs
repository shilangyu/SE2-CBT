using System.ComponentModel.DataAnnotations;

namespace CbtBackend.Models.Requests;

public class UserUpdateRequest {
    public string? Login { get; set; }
    public string? Password { get; set; }
    public int? Age { get; set; }
    public string? Gender { get; set; }
}
