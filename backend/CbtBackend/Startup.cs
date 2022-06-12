using CbtBackend.Attributes;

namespace CbtBackend;

using System.Text;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using CbtBackend.Services;
using CbtBackend.Entities;
using Microsoft.AspNetCore.HttpOverrides;

public class Startup {
    public Startup(IConfiguration configuration, IWebHostEnvironment environment) {
        Configuration = configuration;
        Environment = environment;
    }

    public IConfiguration Configuration { get; }
    public IWebHostEnvironment Environment { get; }

    public void ConfigureServices(IServiceCollection services) {
        services.AddCors(options => {
            options.AddDefaultPolicy(builder => {
                if (Environment.IsDevelopment()) {
                    builder.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback)
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                } else {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }
            });
        });

        services.AddControllers();

        // Explicitly set data protection keys directory to make it Docker-volume compatible
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
            services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("keys"));
        }

        // Forwarded Headers Middleware
        // https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-6.0
        services.Configure<ForwardedHeadersOptions>(options => {
            options.ForwardedHeaders = ForwardedHeaders.All;
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        // register services for DI
        {
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IEvaluationService, EvaluationService>();

            services.AddDbContext<CbtDbContext>(options => {
                options.UseNpgsql(Configuration.GetValue<string>("db:ConnectionString"));
            });
        }

        // authentication
        services.AddAuthentication("jwtauth").AddJwtBearer("jwtauth", options => {
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"])),

                ValidateIssuer = true,
                ValidIssuer = Configuration["Jwt:Issuer"],

                ValidateAudience = true,
                ValidAudience = Configuration["Jwt:Audience"],
            };
        });

        services.AddIdentityCore<User>(options => {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Lockout.AllowedForNewUsers = false;
        })
            .AddRoles<Role>()
            .AddEntityFrameworkStores<CbtDbContext>()
            .AddDefaultTokenProviders();
    }

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        ILogger<Startup> logger,
        CbtDbContext dbContext,
        RoleManager<Role> roleManager,
        UserManager<User> userManager) {
        // Throttle configuration
        if (Configuration.GetValue("Throttle:Bypass", false)) {
            logger.LogWarning("Throttle is set to BYPASS mode");
            Throttle.Bypass = true;
        }

        if (!dbContext.Database.IsInMemory()) {
            dbContext.Database.Migrate();
        }

        if (env.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Current deploy uses an HTTPS proxy with redirection enabled
        // It also passes X-Forwarded-* headers with true client IPs
        if (Utilities.IsDocker()) {
            app.UseForwardedHeaders();
        }

        SeedRoles(roleManager).Wait();
        SeedAdmin(userManager, dbContext).Wait();
        SeedMoodtests(dbContext).Wait();

        app.UseCors();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }

    private static async Task SeedRoles(RoleManager<Role> roleManager) {
        foreach (var roleName in UserRoles.All) {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist) {
                await roleManager.CreateAsync(new Role(roleName));
            }
        }
    }

    private static async Task SeedAdmin(UserManager<User> userManager, CbtDbContext dbContext) {
        var adminEmail = "admin@admin.com";
        var adminPassword = "adminadmin";
        var exists = dbContext.Users.Any(u => u.Email == adminEmail);

        if (!exists) {
            var user = new User {
                UserName = adminEmail,
                Email = adminEmail,
                Banned = false,
                Age = 21,
                Gender = "other",
                UserStatus = 1,
            };

            await userManager.CreateAsync(user, adminPassword);
            await userManager.AddToRolesAsync(user, UserRoles.All);
        }
    }

    private static async Task SeedMoodtests(CbtDbContext dbContext) {
        var dummyResultsTable = new MudTestResultsTable {
            Id = 1,
            EntryCategory = "Severity",
            Entries = new() {
                new() {
                    Id = 1,
                    ScoreFrom = 0,
                    ScoreTo = 0,
                    EntryName = "No symptoms",
                    Description = "That's terrific! You don't seem to have any symptoms at all."
                },
                new() {
                    Id = 2,
                    ScoreFrom = 1,
                    ScoreTo = 2,
                    EntryName = "Borderline",
                    Description = "These scores are normal, but you could use a little tune-up."
                },
                new() {
                    Id = 3,
                    ScoreFrom = 3,
                    ScoreTo = 5,
                    EntryName = "Mild",
                    Description = "Although your scores are not greatly elevated, this is enough depression or anxiety to take the joy out of life. If we work together, we can probably get your scores down to 0, which would be terrific!"
                },
                new() {
                    Id = 4,
                    ScoreFrom = 6,
                    ScoreTo = 10,
                    EntryName = "Moderate",
                    Description = "You're feeling quite a bit of depression or anxiety. Although you're not in the severe range, these scores reflect consider- able unhappiness."
                },
                new() {
                    Id = 5,
                    ScoreFrom = 11,
                    ScoreTo = 15,
                    EntryName = "Severe",
                    Description = "You have fairly strong feelings of depression or anxiety. That makes me sad, but there's some really good news. The tools in this book can help you transform your negative feelings into joy."
                },
                new() {
                    Id = 6,
                    ScoreFrom = 16,
                    ScoreTo = 20,
                    EntryName = "Extreme",
                    Description = "Scores in this range indicate that your suffering is intense. Friends or family may have trouble grasping how much pain you're in. The good news is that the prognosis for improvement is very positive. In fact, recovery is one of the greatest feelings a human being can have."
                },
            }
        };

        var moodtests = new MudTest[] {
            new() {
                Id = 1,
                Name = "Depression",
                Description = "A test to check your current depression levels",
                Question1 = "Sad or down in the dumps",
                Question2 = "Discouraged or hopeless",
                Question3 = "Low in self-esteem, inferior, or worthless",
                Question4 = "Unmotivated to do things",
                Question5 = "Decreased pleasure or satisfaction in life",
                ResultsTable = dummyResultsTable
            },
            new() {
                Id = 2,
                Name = "Anxiety",
                Description = "A test to check your current anxiety levels",
                Question1 = "Anxious",
                Question2 = "Frightened",
                Question3 = "Worrying about things",
                Question4 = "Tense or on edge",
                Question5 = "Nervous",
                ResultsTable = dummyResultsTable
            },
            new() {
                Id = 3,
                Name = "Addictions",
                Description = "A test to check your current addictions levels",
                Question1 = "Sometimes I crave drugs or alcohol",
                Question2 = "Sometimes I have the urge to use drugs or alcohol",
                Question3 = "Sometimes it's hard to resist the urge to use drugs or alcohol",
                Question4 = "Sometimes I struggle with the temptaion to use drugs or alcohol",
                Question5 = "Nervous",
                ResultsTable = dummyResultsTable
            },
            new() {
                Id = 4,
                Name = "Anger",
                Description = "A test to check your current anger levels",
                Question1 = "Frustrated",
                Question2 = "Annoyed",
                Question3 = "Resentful",
                Question4 = "Angry",
                Question5 = "Irritated",
                ResultsTable = dummyResultsTable
            },
            new() {
                Id = 5,
                Name = "Relationships",
                Description = "A test to check your current relationship levels",
                Question1 = "Communication and openness",
                Question2 = "Resolving conflicts",
                Question3 = "Degree of affection and caring",
                Question4 = "Intimacy and closeness",
                Question5 = "Overall satisfaction",
                ResultsTable = dummyResultsTable
            },
            new() {
                Id = 6,
                Name = "Happiness",
                Description = "A test to check your current happiness levels",
                Question1 = "Happy and joyful",
                Question2 = "Hopeful and optimistic",
                Question3 = "Worthwhile, high self-esteem",
                Question4 = "Motivated, productive",
                Question5 = "Pleased and satisfied with life",
                ResultsTable = dummyResultsTable
             }
        };

        foreach (var t in moodtests) {
            if (!await dbContext.Evaluations.AnyAsync(e => e.Id == t.Id)) {
                await dbContext.Evaluations.AddAsync(t);
            }
        }

        await dbContext.SaveChangesAsync();
    }
}
