using CbtBackend.Entities;
using CbtBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace CbtBackend.Services;

public interface IUserService {
    // basic operations
    Task<User?> GetUserById(int id);
    Task<User?> GetUserByEmail(string email);

    // authentication
    Task<UserAuthenticationResponse> Authenticate(UserAuthenticationRequest model);
}

public class AuthenticationCredentialsException : Exception { }

public class UserService : IUserService {
    private readonly IJwtTokenService tokenService;
    private readonly IConfiguration configuration;
    private readonly CbtDbContext dbContext;

    public UserService(IJwtTokenService tokenService, IConfiguration configuration, CbtDbContext dbContext) {
        this.tokenService = tokenService;
        this.configuration = configuration;
        this.dbContext = dbContext;
    }

    public Task<User?> GetUserById(int id) {
        return dbContext.Users.SingleOrDefaultAsync(e => e.Id == id);
    }

    public Task<User?> GetUserByEmail(string email) {
        return dbContext.Users.SingleOrDefaultAsync(e => e.Email == email);
    }

    // implementation of user authentication
    public async Task<UserAuthenticationResponse> Authenticate(UserAuthenticationRequest model) {
        var user = await GetUserByEmail(model.Email);

        if (user == null) {
            throw new AuthenticationCredentialsException();
        }

        var validPeriod = configuration.GetValue<int>("jwt:ValidSeconds");
        var expiration = DateTime.UtcNow.AddSeconds(validPeriod);

        var response = new UserAuthenticationResponse() {
            User = user,
            Token = tokenService.GenerateToken(user, expiration),
            TokenExpiration = expiration
        };

        return response;
    }
}
