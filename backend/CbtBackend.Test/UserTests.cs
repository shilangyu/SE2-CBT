using System.Net;
using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace CbtBackend.Test;

public class UserTests : IClassFixture<CustomWebApplicationFactory<Startup>> {
    private readonly CustomWebApplicationFactory<Startup> factory;

    public UserTests(CustomWebApplicationFactory<Startup> factory) {
        this.factory = factory;
    }

    [Theory]
    [InlineData(ApiRoutes.User.GetAll)]
    public async Task EndpointIsProtectedFromAnonymous(string endpoint) {
        using var client = factory.GetClient();
        var res = await client.GetAsync(endpoint);

        Assert.Equal(HttpStatusCode.Unauthorized, res.StatusCode);
    }

    [Theory]
    [InlineData(ApiRoutes.User.GetAll)]
    public async Task EndpointProtectedFromMissingPermissions(string endpoint) {
        using var client = factory.GetClient();

        var expirationDate = DateTime.Now.AddDays(5);

        using var scope = factory.Services.CreateScope();
        var jwtTokenService = scope.ServiceProvider.GetRequiredService<IJwtTokenService>();
        var user = new User {
            Email = "xd@xd.pl",
            PasswordHash = "9494949494d949d4w9d9w4d9w",
        };

        var token = jwtTokenService.GenerateToken(user, new string[] { }, expirationDate);


        using var reqMessage = new HttpRequestMessage(HttpMethod.Get, endpoint);
        reqMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var res = await client.SendAsync(reqMessage);


        Assert.Equal(HttpStatusCode.Forbidden, res.StatusCode);
    }

    [Fact]
    public async Task CanCreateUser() {
        var client = factory.GetClient();

        var email = $"user_{Guid.NewGuid()}@email.com";
        var password = "Qweqweqwe$3";

        var res = await client.PostAsync(ApiRoutes.User.Register, JsonBody(new UserRegistrationRequest {
            Login = email,
            Password = password,
            Age = 21,
            Gender = "other",
        }));

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task CanLogin() {
        var client = factory.GetClient();

        var email = $"user_{Guid.NewGuid()}@email.com";
        var password = "Qweqweqwe$3";

        var res = await client.PostAsync(ApiRoutes.User.Register, JsonBody(new UserRegistrationRequest {
            Login = email,
            Password = password,
            Age = 21,
            Gender = "other",
        }));
        res.EnsureSuccessStatusCode();

        res = await client.PostAsync(ApiRoutes.User.Login, JsonBody(new UserAuthenticationRequest {
            Login = email,
            Password = password,
        }));

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task CanDeserializeLoginResponse() {
        var client = factory.GetClient();

        var email = $"user_{Guid.NewGuid()}@email.com";
        var password = "Qweqweqwe$3";

        var res = await client.PostAsync(ApiRoutes.User.Register, JsonBody(new UserRegistrationRequest {
            Login = email,
            Password = password,
            Age = 21,
            Gender = "other",
        }));
        res.EnsureSuccessStatusCode();

        res = await client.PostAsync(ApiRoutes.User.Login, JsonBody(new UserAuthenticationRequest {
            Login = email,
            Password = password,
        }));
        res.EnsureSuccessStatusCode();

        var loginResponse = await res.ReadAsJson<LoginResponseDTO>();

        Assert.Equal(0, loginResponse.UserStatus);
    }

    [Fact]
    public async Task CanLogout() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.PostAsync(ApiRoutes.User.Logout, JsonBody(new { }));

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task CanGetAllUsers() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.GetAsync(ApiRoutes.User.GetAll);

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task CanDeserializeGetAllUsers() {
        var (client, _) = await factory.GetAuthenticatedClient();

        var res = await client.GetAsync(ApiRoutes.User.GetAll);
        res.EnsureSuccessStatusCode();

        await res.ReadAsJson<List<UserDTO>>();
    }

    [Fact]
    public async Task CanGetUser() {
        var (client, user) = await factory.GetAuthenticatedClient();

        var res = await client.GetAsync(ApiRoutes.User.GetByUserId.ReplaceParam("userId", user.UserId));

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task CanDeserializeGetUser() {
        var (client, user) = await factory.GetAuthenticatedClient();

        var res = await client.GetAsync(ApiRoutes.User.GetByUserId.ReplaceParam("userId", user.UserId));
        res.EnsureSuccessStatusCode();

        await res.ReadAsJson<UserDTO>();
    }

    [Fact]
    public async Task CanUpdateUser() {
        var (client, user) = await factory.GetAuthenticatedClient();
        var newEmail = TestEmail();

        var res = await client.PutAsync(ApiRoutes.User.UpdateByUserId.ReplaceParam("userId", user.UserId), JsonBody(new UserUpdateRequest {
            Login = newEmail,
            Password = "Qweqweqwe$4",
            Age = 24,
            Gender = "male",
        }));

        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }

    [Fact]
    public async Task UpdatesUser() {
        var (client, user) = await factory.GetAuthenticatedClient();
        var newEmail = TestEmail();

        var res = await client.PutAsync(ApiRoutes.User.UpdateByUserId.ReplaceParam("userId", user.UserId), JsonBody(new UserUpdateRequest {
            Login = newEmail,
            Password = "Qweqweqwe$4",
            Age = 24,
            Gender = "male",
        }));
        var updated = await res.ReadAsJson<UpdateUserResponseDTO>();


        Assert.Equal(newEmail, updated.Login);
    }

    [Fact]
    public async Task CannotUpdateDifferentUser() {
        var (client1, _) = await factory.GetAuthenticatedClient();
        var (_, user2) = await factory.GetAuthenticatedClient();
        var newEmail = TestEmail();

        var res = await client1.PutAsync(ApiRoutes.User.UpdateByUserId.ReplaceParam("userId", user2.UserId), JsonBody(new UserUpdateRequest {
            Login = newEmail,
            Password = "Qweqweqwe$4",
            Age = 24,
            Gender = "male",
        }));

        Assert.Equal(HttpStatusCode.Forbidden, res.StatusCode);
    }

    [Fact]
    public async Task CanDeleteUser() {
        var (client, user) = await factory.GetAuthenticatedClient();

        var res = await client.DeleteAsync(ApiRoutes.User.DeleteByUserId.ReplaceParam("userId", user.UserId));

        Assert.Equal(HttpStatusCode.NoContent, res.StatusCode);
    }

    [Fact]
    public async Task DeletesUser() {
        var (client, user) = await factory.GetAuthenticatedClient();

        var res = await client.DeleteAsync(ApiRoutes.User.DeleteByUserId.ReplaceParam("userId", user.UserId));
        res.EnsureSuccessStatusCode();

        res = await client.GetAsync(ApiRoutes.User.GetByUserId.ReplaceParam("userId", user.UserId));


        Assert.Equal(HttpStatusCode.NotFound, res.StatusCode);
    }

    [Fact]
    public async Task CannotDeleteDifferentUser() {
        var (client1, _) = await factory.GetAuthenticatedClient();
        var (_, user2) = await factory.GetAuthenticatedClient();

        var res = await client1.DeleteAsync(ApiRoutes.User.DeleteByUserId.ReplaceParam("userId", user2.UserId));

        Assert.Equal(HttpStatusCode.Forbidden, res.StatusCode);
    }
}
