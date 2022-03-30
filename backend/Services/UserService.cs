using CbtBackend.Entities;
using CbtBackend.Models;
using CbtBackend.Models.Requests;
using CbtBackend.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace CbtBackend.Services;

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

    public async Task<User?> GetUserByIdAsync(int userId) {
        return await dbContext.Users.SingleOrDefaultAsync(e => e.Id == userId);
    }

    public async Task<User?> GetUserByEmailAsync(string email) {
        return await dbContext.Users.SingleOrDefaultAsync(e => e.Email == email);
    }

    public async Task<List<User>> GetAllUsersAsync() {
        return await dbContext.Users.ToListAsync();
    }

    public async Task<bool> UpdateUserAsync(User userToUpdate) {
        dbContext.Users.Update(userToUpdate);
        var updated = await dbContext.SaveChangesAsync();
        return updated > 0;
    }

    public async Task<bool> DeleteUserAsync(string email) {
        var user = await GetUserByEmailAsync(email);
        if (user == null) {
            return false;
        }

        dbContext.Users.Remove(user);
        var deleted = await dbContext.SaveChangesAsync();
        return deleted > 0;
    }

    public async Task<bool> RegisterUserAsync(User user) {
        await dbContext.Users.AddAsync(user);
        var registered = await dbContext.SaveChangesAsync();
        return registered > 0;
    }

    // implementation of user authentication
    public async Task<UserAuthenticationResponse> AuthenticateUserAsync(UserAuthenticationRequest model) {
        var user = await GetUserByEmailAsync(model.Email);

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
