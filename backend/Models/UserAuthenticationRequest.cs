using System.Diagnostics.CodeAnalysis;

namespace CbtBackend.Models;

public class UserAuthenticationRequest {
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
}
