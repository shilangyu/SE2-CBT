global using CbtBackend.Services;
global using CbtBackend.Models;
global using CbtBackend.Contracts;
global using CbtBackend.Models.Requests;
global using CbtBackend.Entities;
global using Xunit;
global using static CbtBackend.Test.Helpers;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace CbtBackend.Test;

public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class {
    protected override void ConfigureWebHost(IWebHostBuilder builder) {
        builder.ConfigureServices(services => {
            // remove the db context that is used for production
            var descriptor = services.SingleOrDefault(d => {
                return d.ServiceType == typeof(DbContextOptions<CbtDbContext>);
            });

            if (descriptor != null) {
                services.Remove(descriptor);
            }

            // add memory db context instead
            services.AddDbContext<CbtDbContext>(options => {
                options.UseInMemoryDatabase("InMemoryTestDatabase");
            });
        });
    }

    public HttpClient GetClient() {
        var customTestUrl = Environment.GetEnvironmentVariable("TEST_URL");

        var client = customTestUrl switch {
            not null => new() { BaseAddress = new(customTestUrl) },
            _ => CreateClient(),
        };

        client.DefaultRequestHeaders
            .Accept
            .Add(new MediaTypeWithQualityHeaderValue("application/json"));

        return client;
    }

    public async Task<(HttpClient, UserDTO)> GetAuthenticatedClient() {
        var client = GetClient();

        var email = $"user_{Guid.NewGuid()}@email.com";
        var password = "Qweqweqwe$3";

        var res = await client.PostAsync($"/{ApiRoutes.User.Register}", JsonBody(new UserRegistrationRequest {
            Login = email,
            Password = password,
            Age = 21,
            Gender = "other",
        }));
        res.EnsureSuccessStatusCode();

        res = await client.PostAsync($"/{ApiRoutes.User.Login}", JsonBody(new UserAuthenticationRequest {
            Login = email,
            Password = password,
        }));
        res.EnsureSuccessStatusCode();

        var loginResponse = await res.ReadAsJson<LoginResponseDTO>();
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResponse.AccessToken);

        res = await client.GetAsync($"/{ApiRoutes.User.GetByUserId.ReplaceParam("userId", loginResponse.UserId)}");
        res.EnsureSuccessStatusCode();

        var userData = await res.ReadAsJson<UserDTO>();

        return (client, userData);
    }
}
