using System.Net;

namespace CbtBackend.Test;

[Collection("Sequential")]
public class MoodtestTests : IClassFixture<CustomWebApplicationFactory<Startup>> {
    private readonly CustomWebApplicationFactory<Startup> factory;

    public MoodtestTests(CustomWebApplicationFactory<Startup> factory) {
        this.factory = factory;
    }

    [Fact]
    public async Task CanGetEvaluations() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.GetAsync(ApiRoutes.Evaluation.GetEvaluations);

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task CanParseGetEvaluations() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.GetAsync(ApiRoutes.Evaluation.GetEvaluations);
        res.EnsureSuccessStatusCode();

        await res.ReadAsJson<List<MudTest>>();
    }

    [Fact]
    public async Task CanGetEvaluation() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.GetAsync(ApiRoutes.Evaluation.GetEvaluation.ReplaceParam("id", 1));

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task EvaluationReturns404() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.GetAsync(ApiRoutes.Evaluation.GetEvaluation.ReplaceParam("id", 100));

        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task CanParseGetEvaluation() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.GetAsync(ApiRoutes.Evaluation.GetEvaluation.ReplaceParam("id", 1));
        res.EnsureSuccessStatusCode();

        await res.ReadAsJson<MudTest>();
    }

    [Fact]
    public async Task CanPostResponse() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.PostAsync(ApiRoutes.Evaluation.PostEvaluationResponse, JsonBody(new {
            TestId = 1,
            Response1 = 1,
            Response2 = 1,
            Response3 = 1,
            Response4 = 1,
            Response5 = 1,
        }));

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }
}
