using CbtBackend.Entities;
using CbtBackend.Models;
using CbtBackend.Models.Requests;
using CbtBackend.Models.Responses;
using Microsoft.EntityFrameworkCore;

namespace CbtBackend.Services;

public class AuthenticationCredentialsException : Exception { }

public class RegistrationException : Exception {
    public new string Message { get; set; }
    public RegistrationException(string message) {
        Message = message;
    }
}

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

    // implementation of user registration
    public async Task<UserRegistrationResponse> RegisterUserAsync(UserRegistrationRequest userRequest) {
        // check if user already exists
        var existingUser = await GetUserByEmailAsync(userRequest.Email);
        if (existingUser != null) {
            throw new RegistrationException("User already exists");
        }

        var user = new User {
            Email = userRequest.Email,
            Password = userRequest.Password,
            Banned = userRequest.Banned,
            Age = userRequest.Age,          // could it be null in userRequest? if yes then assign it outside like UserStatus below.
            Gender = userRequest.Gender,    // could it be null in userRequest? if yes then assign it outside like UserStatus below.
            UserStatus = 0,
            Roles = new List<string> { UserRoles.UserWrite }
        };

        if (userRequest.UserStatus != null) {
            user.UserStatus = userRequest.UserStatus.Value;
        }

        // add user to db
        await dbContext.Users.AddAsync(user);
        var registered = await dbContext.SaveChangesAsync();
        if (registered <= 0) {
            throw new RegistrationException("Operation failed");
        }

        var response = new UserRegistrationResponse() { // won't matter much if RegisterUser() returns status code 200 on success
            User = user
        };

        return response;
    }

    // implementation of user authentication
    public async Task<UserAuthenticationResponse> AuthenticateUserAsync(UserAuthenticationRequest userRequest) {
        var user = await GetUserByEmailAsync(userRequest.Email);

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
