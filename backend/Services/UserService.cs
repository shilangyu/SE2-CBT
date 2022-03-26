using CbtBackend.Entities;
using CbtBackend.Models;

namespace CbtBackend.Services;

public interface IUserService {
    // basic operations
    IEnumerable<User> GetAllUsers();
    User GetUserById(int id);
    User GetUserByEmail(string email);

    // authentication
    UserAuthenticationResponse Authenticate(UserAuthenticationRequest model);
}

public class AuthenticationCredentialsException : Exception { }

public class UserService : IUserService {
    private readonly IJwtTokenService tokenService;
    private readonly IConfiguration configuration;

    public IEnumerable<User> GetAllUsers() {
        throw new NotImplementedException();
    }

    public User GetUserById(int id) {
        return GetExampleUser();
    }

    public User GetUserByEmail(string email) {
        return GetExampleUser();
    }

    public UserService(IJwtTokenService tokenService, IConfiguration configuration) {
        this.tokenService = tokenService;
        this.configuration = configuration;
    }

    // implementation of user authentication
    public UserAuthenticationResponse Authenticate(UserAuthenticationRequest model) {
        var user = GetUserByEmail(model.Email);

        if (user == null) {
            throw new AuthenticationCredentialsException();
        }

        var validPeriod = configuration.GetValue<int>("jwt:ValidSeconds");
        var expiration = DateTime.UtcNow.AddSeconds(validPeriod);

        var response = new UserAuthenticationResponse() {
            User = user,
            Token = tokenService.GenerateToken(user, expiration, model.Scopes),
            TokenExpiration = expiration
        };

        return response;
    }

    private static User GetExampleUser() {
        return new User() {
            Email = "mail@mail.com",
            Password = "password",
            Gender = "male"
        };
    }
}