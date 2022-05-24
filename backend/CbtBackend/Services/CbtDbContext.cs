using Microsoft.EntityFrameworkCore;
using CbtBackend.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace CbtBackend.Services;

public sealed class CbtDbContext : IdentityDbContext<User, Role, int> {
    public DbSet<MudTest> Evaluations => Set<MudTest>();
    public DbSet<MudTestResponse> EvaluationResponses => Set<MudTestResponse>();

    public CbtDbContext(DbContextOptions<CbtDbContext> options) : base(options) {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MudTest>().HasData(
            new MudTest() {
                Id = 1,
                Name = "Depression",
                Description = "A test to check your current depression levels",
                Question1 = "Sad or down in the dumps",
                Question2 = "Discouraged or hopeless",
                Question3 = "Low in self-esteem, inferior, or worthless",
                Question4 = "Unmotivated to do things",
                Question5 = "Decreased pleasure or satisfaction in life"
            },
            new MudTest() {
                Id = 2,
                Name = "Anxiety",
                Description = "A test to check your current anxiety levels",
                Question1 = "Anxious",
                Question2 = "Frightened",
                Question3 = "Worrying about things",
                Question4 = "Tense or on edge",
                Question5 = "Nervous"
            },
            new MudTest() {
                Id = 3,
                Name = "Addictions",
                Description = "A test to check your current addictions levels",
                Question1 = "Sometimes I crave drugs or alcohol",
                Question2 = "Sometimes I have the urge to use drugs or alcohol",
                Question3 = "Sometimes it's hard to resist the urge to use drugs or alcohol",
                Question4 = "Sometimes I struggle with the temptaion to use drugs or alcohol",
                Question5 = "Nervous"
            },
            new MudTest() {
                Id = 4,
                Name = "Anger",
                Description = "A test to check your current anger levels",
                Question1 = "Frustrated",
                Question2 = "Annoyed",
                Question3 = "Resentful",
                Question4 = "Angry",
                Question5 = "Irritated"
            },
            new MudTest() {
                Id = 5,
                Name = "Relationships",
                Description = "A test to check your current relationship levels",
                Question1 = "Communication and openness",
                Question2 = "Resolving conflicts",
                Question3 = "Degree of affection and caring",
                Question4 = "Intimacy and closeness",
                Question5 = "Overall satisfaction"
            },
            new MudTest() {
                Id = 6,
                Name = "Happiness",
                Description = "A test to check your current happiness levels",
                Question1 = "Happy and joyful",
                Question2 = "Hopeful and optimistic",
                Question3 = "Worthwhile, high self-esteem",
                Question4 = "Motivated, productive",
                Question5 = "Pleased and satisfied with life"
            }
        );
    }
}
