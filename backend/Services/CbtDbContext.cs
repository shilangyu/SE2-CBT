using Microsoft.EntityFrameworkCore;
using CbtBackend.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace CbtBackend.Services;

public sealed class CbtDbContext : IdentityDbContext<User, IdentityRole, string> {
    public CbtDbContext(DbContextOptions<CbtDbContext> options) : base(options) {
    }
}
