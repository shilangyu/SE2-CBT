using System.Diagnostics.CodeAnalysis;
using CbtBackend.Entities;

namespace CbtBackend.Models;

public class UserAuthenticationResponse {
    public User? User { get; set; }

    [NotNull]
    public string? Token { get; set; }
    public DateTime TokenExpiration { get; set; }
}
