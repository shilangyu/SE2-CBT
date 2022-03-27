using System.Diagnostics.CodeAnalysis;

namespace CbtBackend.Entities;

public class User {
    public int Id { get; set; }

    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public int Age { get; set; }

    public string? Gender { get; set; }

    public int UserStatus { get; set; }

    public bool Banned { get; set; }

    public List<string> Roles { get; set; } = default!;
}
