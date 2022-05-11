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

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MudTest>().HasData(
            new MudTest() {
                Id = 1,
                Name = "example test",
                Description = "example test",
                Question1 = "example q1",
                Question2 = "example q2",
                Question3 = "example q3",
                Question4 = "example q4",
                Question5 = "example q5"
            }
        );
    }
}
