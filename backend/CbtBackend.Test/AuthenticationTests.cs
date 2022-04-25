using System.Net;
using Xunit;

namespace CbtBackend.Test;

public class AuthenticationTests : IClassFixture<CustomWebApplicationFactory<Startup>> {
    private readonly CustomWebApplicationFactory<Startup> factory;

    public AuthenticationTests(CustomWebApplicationFactory<Startup> factory) {
        this.factory = factory;
    }

    [Fact]
    public async Task LoginEndpointEmptyLoginFails() {
        // arrange
        var client = factory.CreateClient();
        var res = await client.GetAsync("/" + Contracts.ApiRoutes.User.Login);

        Assert.Equal(HttpStatusCode.Unauthorized, res.StatusCode);
    }
}
