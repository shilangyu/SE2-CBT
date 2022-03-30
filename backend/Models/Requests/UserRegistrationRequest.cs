using System.ComponentModel.DataAnnotations;

namespace CbtBackend.Models.Requests;

public class UserRegistrationRequest {
    [Required]
    public string Email { get; set; } = default!;
    [Required]
    public string Password { get; set; } = default!;
    [Required]
    public int Age { get; set; }
    [Required]
    public string Gender { get; set; } = default!;
    [Required]
    public bool Banned { get; set; }
}

