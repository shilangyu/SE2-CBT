using System.Net;

namespace CbtBackend.Test;

public class AuthenticationTests : IClassFixture<CustomWebApplicationFactory<Startup>> {
    private readonly CustomWebApplicationFactory<Startup> factory;

    public AuthenticationTests(CustomWebApplicationFactory<Startup> factory) {
        this.factory = factory;
    }

    [Fact]
    public async Task LoginEndpointEmptyLoginFails() {
        using var client = factory.GetClient();
        var res = await client.GetAsync("/" + Contracts.ApiRoutes.User.Login);

        Assert.Equal(HttpStatusCode.Unauthorized, res.StatusCode);
    }
}
