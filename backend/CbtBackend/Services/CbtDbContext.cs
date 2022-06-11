using Microsoft.EntityFrameworkCore;
using CbtBackend.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CbtBackend.Services;

public sealed class CbtDbContext : IdentityDbContext<User, Role, int> {
    public DbSet<MudTest> Evaluations => Set<MudTest>();
    public DbSet<MudTestResponse> EvaluationResponses => Set<MudTestResponse>();

    public CbtDbContext(DbContextOptions<CbtDbContext> options) : base(options) {
    }
}
