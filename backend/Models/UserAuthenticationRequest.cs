using System.Diagnostics.CodeAnalysis;

namespace CbtBackend.Models;

public class UserAuthenticationRequest {
    [NotNull]
    public string? Email { get; set; }

    [NotNull]
    public string? Password { get; set; }

    [NotNull]
    public string[]? Scopes { get; set; }
}
