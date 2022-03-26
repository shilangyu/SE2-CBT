using System.Diagnostics.CodeAnalysis;

namespace CbtBackend.Entities;

public class User {
    public int Id { get; set; }

    [NotNull]
    public string? Email { get; set; }

    [NotNull]
    public string? Password { get; set; }

    public int Age { get; set; }

    [NotNull]
    public string? Gender { get; set; }

    public int UserStatus { get; set; }
    
    public bool Banned { get; set; }
}