using System.Runtime.InteropServices;
using Microsoft.AspNetCore.DataProtection;

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

// Explicitly set data protection keys directory to make it Docker-volume compatible
if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {
    builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo("keys"));
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
