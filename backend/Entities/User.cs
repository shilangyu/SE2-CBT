using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using CbtBackend.Models;
using CbtBackend.Models.Requests;

namespace CbtBackend.Entities;

public class User {
    [Key]
    public int Id { get; set; }

    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;

    public int Age { get; set; }

    public string Gender { get; set; } = default!;

    public int UserStatus { get; set; }

    public bool Banned { get; set; }

    public List<string> Roles { get; set; } = default!;
}
