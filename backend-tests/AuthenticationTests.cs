namespace CbtBackend.Tests;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Xunit;

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

        Assert.Equal(HttpStatusCode.Forbidden, res.StatusCode);
    }
}