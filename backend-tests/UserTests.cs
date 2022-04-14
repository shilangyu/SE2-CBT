namespace CbtBackend.Tests;

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using CbtBackend.Entities;
using CbtBackend.Services;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

public class UserTests : IClassFixture<CustomWebApplicationFactory<Startup>> {
    private readonly CustomWebApplicationFactory<Startup> factory;

    public UserTests(CustomWebApplicationFactory<Startup> factory) {
        this.factory = factory;
    }

    [Theory]
    [InlineData("/" + Contracts.ApiRoutes.User.GetAll)]
    [InlineData("/" + Contracts.ApiRoutes.User.GetByEmail)]
    public async Task EndpointIsProtectedFromAnonymous(string endpoint) {
        var client = factory.CreateClient();
        var res = await client.GetAsync(endpoint);

        Assert.Equal(HttpStatusCode.Unauthorized, res.StatusCode);
    }

    [Theory]
    [InlineData("/" + Contracts.ApiRoutes.User.GetAll)]
    [InlineData("/" + Contracts.ApiRoutes.User.GetByEmail)]
    public async Task EndpointProtectedFromMissingPermissions(string endpoint) {
        var client = factory.CreateClient();

        HttpResponseMessage res;
        string token;

        var expirationDate = System.DateTime.Now;
        expirationDate = expirationDate.AddDays(5);

        using (var scope = factory.Services.CreateScope()) {
            var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
            var user = new User {
                Email = "xd@xd.pl",
                PasswordHash = "9494949494d949d4w9d9w4d9w",
            };

            token = jwtTokenService.GenerateToken(user, new string[] { }, expirationDate);
        }

        using (var reqMessage = new HttpRequestMessage(HttpMethod.Get, endpoint)) {
            reqMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            res = await client.SendAsync(reqMessage);
        }

        Assert.Equal(HttpStatusCode.Forbidden, res.StatusCode);
    }
}
