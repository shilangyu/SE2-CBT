using Microsoft.EntityFrameworkCore;
using CbtBackend.Entities;

namespace CbtBackend.Services;

public sealed class CbtDbContext : DbContext {
    private readonly IConfiguration configuration;
    public DbSet<User> Users { get; set; }

    public CbtDbContext(IConfiguration configuration) {
        this.configuration = configuration;
        Users = Set<User>();
    }
}
