namespace CbtBackend.Tests;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Xunit;

public class UserTests : IClassFixture<CustomWebApplicationFactory<Startup>> {
    private readonly CustomWebApplicationFactory<Startup> factory;

    public UserTests(CustomWebApplicationFactory<Startup> factory) {
        this.factory = factory;
    }

    [Theory]
    [InlineData("/" + Contracts.ApiRoutes.User.GetAll)]
    [InlineData("/" + Contracts.ApiRoutes.User.GetByEmail)]
    [InlineData("/" + Contracts.ApiRoutes.User.DeleteByEmail)]
    [InlineData("/" + Contracts.ApiRoutes.User.UpdateByEmail)]
    public async Task EndpointIsProtectedFromAnonymous(string endpoint) {
        var client = factory.CreateClient();
        var res = await client.GetAsync(endpoint);

        Assert.Equal(HttpStatusCode.Forbidden, res.StatusCode);
    }
}