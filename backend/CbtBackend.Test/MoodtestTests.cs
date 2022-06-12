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
    public async Task EvaluationsHaveResultsTable() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.GetAsync(ApiRoutes.Evaluation.GetEvaluations);
        res.EnsureSuccessStatusCode();

        var mudTests = await res.ReadAsJson<List<MudTest>>();

        foreach (var t in mudTests) {
            Assert.NotNull(t.ResultsTable);
        }
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

    [Fact]
    public async Task CanParsePostResponse() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.PostAsync(ApiRoutes.Evaluation.PostEvaluationResponse, JsonBody(new {
            TestId = 1,
            Response1 = 1,
            Response2 = 1,
            Response3 = 1,
            Response4 = 1,
            Response5 = 1,
        }));
        res.EnsureSuccessStatusCode();

        await res.ReadAsJson<MudTestResponse>();
    }

    [Fact]
    public async Task PostResponseHasResultsTable() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.PostAsync(ApiRoutes.Evaluation.PostEvaluationResponse, JsonBody(new {
            TestId = 1,
            Response1 = 1,
            Response2 = 1,
            Response3 = 1,
            Response4 = 1,
            Response5 = 1,
        }));
        res.EnsureSuccessStatusCode();

        var response = await res.ReadAsJson<MudTestResponse>();

        Assert.NotNull(response.Evaluation.ResultsTable);
    }

    [Fact]
    public async Task CanGetPostedResponse() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.PostAsync(ApiRoutes.Evaluation.PostEvaluationResponse, JsonBody(new {
            TestId = 1,
            Response1 = 1,
            Response2 = 1,
            Response3 = 1,
            Response4 = 1,
            Response5 = 1,
        }));
        res.EnsureSuccessStatusCode();

        var response = await res.ReadAsJson<MudTestResponse>();

        res = await client.GetAsync(ApiRoutes.Evaluation.GetEvaluationResponse.ReplaceParam("id", response.Id));

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task CanDeletePostedResponse() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.PostAsync(ApiRoutes.Evaluation.PostEvaluationResponse, JsonBody(new {
            TestId = 1,
            Response1 = 1,
            Response2 = 1,
            Response3 = 1,
            Response4 = 1,
            Response5 = 1,
        }));
        res.EnsureSuccessStatusCode();

        var response = await res.ReadAsJson<MudTestResponse>();

        res = await client.DeleteAsync(ApiRoutes.Evaluation.DeleteEvaluationResponse.ReplaceParam("id", response.Id));

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task DeletedPostResponseIsNoLongerThere() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.PostAsync(ApiRoutes.Evaluation.PostEvaluationResponse, JsonBody(new {
            TestId = 1,
            Response1 = 1,
            Response2 = 1,
            Response3 = 1,
            Response4 = 1,
            Response5 = 1,
        }));
        res.EnsureSuccessStatusCode();

        var response = await res.ReadAsJson<MudTestResponse>();

        res = await client.DeleteAsync(ApiRoutes.Evaluation.DeleteEvaluationResponse.ReplaceParam("id", response.Id));
        res.EnsureSuccessStatusCode();

        res = await client.GetAsync(ApiRoutes.Evaluation.GetEvaluationResponse.ReplaceParam("id", response.Id));

        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task CanGetResponsesByUserId() {
        var (client, user) = await factory.GetAuthenticatedClient();

        var res = await client.PostAsync(ApiRoutes.Evaluation.PostEvaluationResponse, JsonBody(new {
            TestId = 1,
            Response1 = 1,
            Response2 = 1,
            Response3 = 1,
            Response4 = 1,
            Response5 = 1,
        }));
        res.EnsureSuccessStatusCode();

        res = await client.GetAsync(ApiRoutes.Evaluation.GetEvaluationResponseById + $"?userId={user.UserId}");

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task CanParseGetResponsesByUserId() {
        var (client, user) = await factory.GetAuthenticatedClient();

        var res = await client.PostAsync(ApiRoutes.Evaluation.PostEvaluationResponse, JsonBody(new {
            TestId = 1,
            Response1 = 1,
            Response2 = 1,
            Response3 = 1,
            Response4 = 1,
            Response5 = 1,
        }));
        res.EnsureSuccessStatusCode();

        res = await client.GetAsync(ApiRoutes.Evaluation.GetEvaluationResponseById + $"?userId={user.UserId}");

        var responses = await res.ReadAsJson<List<MudTestResponse>>();

        Assert.Single(responses);
        Assert.Equal(1, responses.Single().Response1);
    }

    [Fact]
    public async Task GetResponsesByUserIdReturnsOnlyUsersResponses() {
        var (client1, _) = await factory.GetAuthenticatedClient();

        var res = await client1.PostAsync(ApiRoutes.Evaluation.PostEvaluationResponse, JsonBody(new {
            TestId = 1,
            Response1 = 1,
            Response2 = 1,
            Response3 = 1,
            Response4 = 1,
            Response5 = 1,
        }));
        res.EnsureSuccessStatusCode();

        var (client2, user2) = await factory.GetAuthenticatedClient();

        res = await client2.GetAsync(ApiRoutes.Evaluation.GetEvaluationResponseById + $"?userId={user2.UserId}");

        var responses = await res.ReadAsJson<List<MudTestResponse>>();

        Assert.Empty(responses);
    }

    [Fact]
    public async Task CannotGetOtherUsersResponses() {
        var (client1, _) = await factory.GetAuthenticatedClient();
        var (_, user2) = await factory.GetAuthenticatedClient();

        var res = await client1.GetAsync(ApiRoutes.Evaluation.GetEvaluationResponseById + $"?userId={user2.UserId}");

        Assert.Equal(HttpStatusCode.Forbidden, res.StatusCode);
    }
}
