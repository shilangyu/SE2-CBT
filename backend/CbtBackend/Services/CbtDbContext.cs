using Microsoft.EntityFrameworkCore;
using CbtBackend.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace CbtBackend.Services;

public sealed class CbtDbContext : IdentityDbContext<User, IdentityRole, string> {
    public DbSet<MudTest> Evaluations => Set<MudTest>();
    public DbSet<MudTestResponse> EvaluationResponses => Set<MudTestResponse>();

    public CbtDbContext(DbContextOptions<CbtDbContext> options) : base(options) {
    }
}
