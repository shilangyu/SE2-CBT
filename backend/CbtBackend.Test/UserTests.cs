using System.Net;
using System.Net.Http.Headers;
using CbtBackend.Entities;
using CbtBackend.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace CbtBackend.Test;

public class UserTests : IClassFixture<CustomWebApplicationFactory<Startup>> {
    private readonly CustomWebApplicationFactory<Startup> factory;

    public UserTests(CustomWebApplicationFactory<Startup> factory) {
        this.factory = factory;
    }

    [Theory]
    [InlineData("/" + Contracts.ApiRoutes.User.GetAll)]
    [InlineData("/" + Contracts.ApiRoutes.User.GetByUserId)]
    public async Task EndpointIsProtectedFromAnonymous(string endpoint) {
        using var client = factory.GetClient();
        var res = await client.GetAsync(endpoint);

        Assert.Equal(HttpStatusCode.Unauthorized, res.StatusCode);
    }

    [Theory]
    [InlineData("/" + Contracts.ApiRoutes.User.GetAll)]
    [InlineData("/" + Contracts.ApiRoutes.User.GetByUserId)]
    public async Task EndpointProtectedFromMissingPermissions(string endpoint) {
        using var client = factory.GetClient();

        var expirationDate = DateTime.Now.AddDays(5);

        using var scope = factory.Services.CreateScope();
        var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
        var user = new User {
            Email = "xd@xd.pl",
            PasswordHash = "9494949494d949d4w9d9w4d9w",
        };

        var token = jwtTokenService.GenerateToken(user, new string[] { }, expirationDate);


        using var reqMessage = new HttpRequestMessage(HttpMethod.Get, endpoint);
        reqMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res = await client.SendAsync(reqMessage);


        Assert.Equal(HttpStatusCode.Forbidden, res.StatusCode);
    }
}
