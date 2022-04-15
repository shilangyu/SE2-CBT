using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using CbtBackend.Models;
using CbtBackend.Models.Requests;
using Microsoft.AspNetCore.Identity;

namespace CbtBackend.Entities;

public class User : IdentityUser {
    public int Age { get; set; }

    public string Gender { get; set; } = default!;

    public int UserStatus { get; set; }

    public bool Banned { get; set; }
}
