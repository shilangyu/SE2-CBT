namespace CbtBackend.Tests;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

using Xunit;

public class GeneralTests : IClassFixture<CustomWebApplicationFactory<Startup>> {
    private readonly CustomWebApplicationFactory<Startup> factory;

    public GeneralTests(CustomWebApplicationFactory<Startup> factory) {
        this.factory = factory;
    }

    [Fact]
    public async Task InvalidEndpointReturnsNotFoundError() {
        var client = factory.CreateClient();
        var res = await client.GetAsync("/this/isnotavalidendpoint");

        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }
}