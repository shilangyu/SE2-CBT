using CbtBackend.Entities;
using CbtBackend.Models;
using CbtBackend.Models.Requests;
using CbtBackend.Models.Responses;

namespace CbtBackend.Services;

public interface IUserService {
    // basic operations
    Task<List<User>> GetAllUsersAsync();
    Task<bool> UpdateUserAsync(string email, UserUpdateRequest userRequest);

    // registration
    Task<User?> RegisterUserAsync(UserRegistrationRequest userRequest);
    // authentication
    Task<UserAuthenticationResponse> AuthenticateUserAsync(UserAuthenticationRequest userRequest);
}
