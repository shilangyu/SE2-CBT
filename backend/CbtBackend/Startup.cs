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
                    builder.WithOrigins("https://shilangyu.github.io")
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
            services.AddDbContext<CbtDbContext>(options => {
                options.UseNpgsql(Configuration.GetValue<string>("db:ConnectionString"));
            });
        }

        // authentication
        {
            services.AddAuthentication("jwtauth").AddJwtBearer("jwtauth", options => {
                options.TokenValidationParameters = new TokenValidationParameters() {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            services.AddIdentity<User, IdentityRole>(options => {
                options.User.RequireUniqueEmail = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Lockout.AllowedForNewUsers = false;
            })
            .AddEntityFrameworkStores<CbtDbContext>()
            .AddDefaultTokenProviders();
        }

    }

    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        CbtDbContext dbContext,
        RoleManager<IdentityRole> roleManager) {
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
        } else {
            app.UseHttpsRedirection();
        }

        SeedRoles(roleManager).Wait();

        app.UseCors();
        app.UseAuthentication();
        app.UseRouting();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => {
            endpoints.MapControllers();
        });
    }

    private async Task SeedRoles(RoleManager<IdentityRole> roleManager) {
        foreach (var roleName in UserRoles.All) {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist) {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }
    }
}
