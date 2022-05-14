using Microsoft.AspNetCore.Identity;

namespace CbtBackend.Entities;

public class User : IdentityUser<int> {
    public int? Age { get; set; }

    public string? Gender { get; set; }

    public int UserStatus { get; set; }

    public bool Banned { get; set; }
}


public class Role : IdentityRole<int> {
    public Role() { }
    public Role(string roleName) : base(roleName) { }
}
