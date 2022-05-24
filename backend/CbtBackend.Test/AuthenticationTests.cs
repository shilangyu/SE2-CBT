using System.Net;

namespace CbtBackend.Test;

[Collection("Sequential")]
public class AuthenticationTests : IClassFixture<CustomWebApplicationFactory<Startup>> {
    private readonly CustomWebApplicationFactory<Startup> factory;

    public AuthenticationTests(CustomWebApplicationFactory<Startup> factory) {
        this.factory = factory;
    }

    [Fact]
    public async Task LoginEndpointEmptyLoginFails() {
        using var client = factory.GetClient();
        var res = await client.PostAsync(ApiRoutes.User.Login, JsonBody(new UserAuthenticationRequest { Login = "asd", Password = "asd" }));

        Assert.Equal(HttpStatusCode.Unauthorized, res.StatusCode);
    }
}
