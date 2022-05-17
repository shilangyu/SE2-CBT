using CbtBackend.Entities;
using CbtBackend.Models.Requests;
using CbtBackend.Models.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

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
    private readonly UserManager<User> userManager;

    public UserService(IJwtTokenService tokenService, IConfiguration configuration, CbtDbContext dbContext, UserManager<User> userManager) {
        this.tokenService = tokenService;
        this.configuration = configuration;
        this.dbContext = dbContext;
        this.userManager = userManager;
    }

    public async Task<List<User>> GetAllUsersAsync() {
        return await dbContext.Users.ToListAsync();
    }

    public async Task<User> UpdateUserAsync(int userId, UserUpdateRequest userRequest) {
        // check that user exists
        var existingUser = await userManager.FindByIdAsync(userId);
        if (existingUser == null) {
            throw new RegistrationException("User does not exist");
        }

        if (userRequest.Email != null) {
            // check if user's email is being updated it's not already in the db
            if (userRequest.Email != existingUser.Email) {
                var emailOwner = await userManager.FindByEmailAsync(userRequest.Email);
                if (emailOwner != null) {
                    throw new UpdateException("Email already belongs to another user");
                }
            }

            existingUser.Email = userRequest.Email;
        }

        if (userRequest.Banned is bool banned) {
            existingUser.Banned = banned;
        }

        if (userRequest.Age is int age) {
            existingUser.Age = age;
        }

        if (userRequest.Gender != null) {
            existingUser.Gender = userRequest.Gender;
        }

        if (userRequest.UserStatus is int userStatus) {
            existingUser.UserStatus = userStatus;
        }

        // update user in db
        var identityResult = await userManager.UpdateAsync(existingUser);
        if (!identityResult.Succeeded) {
            throw new RegistrationException("Operation failed");
        }

        return existingUser;
    }

    public async Task<bool> DeleteUserAsync(int userId) {
        // check that user exists
        var user = await userManager.FindByIdAsync(userId);
        if (user == null) {
            throw new DeleteException("User does not exist");
        }

        var identityResult = await userManager.DeleteAsync(user);
        if (!identityResult.Succeeded) {
            throw new RegistrationException("Operation failed");
        }

        return true;
    }

    // implementation of user registration
    public async Task<User?> RegisterUserAsync(UserRegistrationRequest userRequest) {
        // check if user already exists
        var existingUser = await userManager.FindByEmailAsync(userRequest.Login);
        if (existingUser != null) {
            throw new RegistrationException("User already exists");
        }

        var user = new User {
            UserName = userRequest.Login,
            Email = userRequest.Login,
            Banned = false,
            Age = userRequest.Age,          // could it be null in userRequest? if yes then assign it outside like UserStatus below.
            Gender = userRequest.Gender,    // could it be null in userRequest? if yes then assign it outside like UserStatus below.
            UserStatus = 0,
        };

        // add user to db
        var identityResult = await userManager.CreateAsync(user, userRequest.Password);
        if (!identityResult.Succeeded) {
            throw new RegistrationException("Operation failed");
        }

        // grant default role
        identityResult = await userManager.AddToRolesAsync(user, new[] {
            UserRoles.UserRead,
            UserRoles.UserWrite,
            UserRoles.EvaluationRead,
            UserRoles.EvaluationWrite
        });

        if (!identityResult.Succeeded) {
            throw new RegistrationException("Operation failed");
        }

        return user;
    }

    // implementation of user authentication
    public async Task<UserAuthenticationResponse> AuthenticateUserAsync(UserAuthenticationRequest userRequest) {
        var user = await userManager.FindByEmailAsync(userRequest.Login);

        if (user == null) {
            throw new AuthenticationCredentialsException();
        }

        var validPassword = await userManager.CheckPasswordAsync(user, userRequest.Password);
        if (!validPassword) {
            throw new AuthenticationCredentialsException();
        }

        var validPeriod = configuration.GetValue<int>("jwt:ValidSeconds");
        var expiration = DateTime.UtcNow.AddSeconds(validPeriod);
        var roles = await userManager.GetRolesAsync(user);


        var response = new UserAuthenticationResponse() {
            User = user,
            Token = tokenService.GenerateToken(user, roles, expiration),
            TokenExpiration = expiration
        };

        return response;
    }
}


public static class UserManagerExtension {
    public static Task<User?> FindByIdAsync(this UserManager<User> self, int userId) {
        return self.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
    }
}
