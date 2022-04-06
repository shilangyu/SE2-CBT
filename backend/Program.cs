using System.Text;
using System.Runtime.InteropServices;
using CbtBackend;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;

using CbtBackend.Services;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment()) {
    builder.Services.AddCors(options => {
        options.AddDefaultPolicy(builder => {
            builder.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback)
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
    });
}

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// register services for DI
{
    builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddDbContext<CbtDbContext>();
}

// authentication
{
    builder.Services.AddAuthentication("jwtauth").AddJwtBearer("jwtauth", options => {
        options.TokenValidationParameters = new TokenValidationParameters() {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
}

// Explicitly set data protection keys directory to make it Docker-volume compatible
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
    builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("keys"));
}

// Forwarded Headers Middleware
// https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-6.0
builder.Services.Configure<ForwardedHeadersOptions>(options => {
    options.ForwardedHeaders = ForwardedHeaders.All;
});

var app = builder.Build(); {
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment()) {
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

    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
