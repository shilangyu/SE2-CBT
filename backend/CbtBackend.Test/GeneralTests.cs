using System.Net;
using Xunit;

namespace CbtBackend.Test;

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
