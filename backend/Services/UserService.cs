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
public class UpdateException : Exception {
    public new string Message { get; set; }
    public UpdateException(string message) {
        Message = message;
    }
}

public class DeleteException : Exception {
    public new string Message { get; set; }
    public DeleteException(string message) {
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

    public async Task<User?> GetUserByEmailAsync(string email) {
        return await dbContext.Users.SingleOrDefaultAsync(e => e.Email == email);
    }

    public async Task<List<User>> GetAllUsersAsync() {
        return await dbContext.Users.ToListAsync();
    }

    public async Task<bool> UpdateUserAsync(string email, UserUpdateRequest userRequest) {
        // check that user exists
        var existingUser = await GetUserByEmailAsync(email);
        if (existingUser == null) {
            throw new RegistrationException("User does not exist");
        }

        var user = new User {
            Id = existingUser.Id,
            Email = userRequest.Email,
            Password = userRequest.Password,
            Banned = userRequest.Banned,
            Age = userRequest.Age,
            Gender = userRequest.Gender,
            UserStatus = userRequest.UserStatus,
            Roles = existingUser.Roles
        };

        // update user in db
        dbContext.ChangeTracker.Clear();
        dbContext.Users.Update(user);
        var updated = await dbContext.SaveChangesAsync();
        if (updated <= 0) {
            throw new UpdateException("Operation failed");
        }

        return true;
    }

    public async Task<bool> DeleteUserAsync(string email) {
        // check that user exists
        var user = await GetUserByEmailAsync(email);
        if (user == null) {
            throw new DeleteException("User does not exist");
        }

        dbContext.Users.Remove(user);
        var deleted = await dbContext.SaveChangesAsync();
        if (deleted <= 0) {
            throw new DeleteException("Operation failed");
        }

        return true;
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
