using CbtBackend.Entities;
using CbtBackend.Models;
using CbtBackend.Models.Requests;
using CbtBackend.Models.Responses;

namespace CbtBackend.Services;

public interface IUserService {
    // basic operations
    Task<List<User>> GetAllUsersAsync();
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> UpdateUserAsync(string email, UserUpdateRequest userRequest);
    Task<bool> DeleteUserAsync(string email);

    // registration
    Task<UserRegistrationResponse> RegisterUserAsync(UserRegistrationRequest userRequest);
    // authentication
    Task<UserAuthenticationResponse> AuthenticateUserAsync(UserAuthenticationRequest userRequest);
}

