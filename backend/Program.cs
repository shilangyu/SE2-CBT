using System.Text;
using System.Runtime.InteropServices;

using Microsoft.AspNetCore.DataProtection;
using Microsoft.IdentityModel.Tokens;

using CbtBackend.Services;

var builder = WebApplication.CreateBuilder(args);

if (builder.Environment.IsDevelopment()) {
    builder.Services.AddCors(options => {
        options.AddDefaultPolicy(builder => {
            builder.SetIsOriginAllowed(origin => new Uri(origin).IsLoopback);
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

var app = builder.Build(); {
    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment()) {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseCors();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
