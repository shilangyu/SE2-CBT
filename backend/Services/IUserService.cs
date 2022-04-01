using CbtBackend.Entities;
using CbtBackend.Models;
using CbtBackend.Models.Requests;
using CbtBackend.Models.Responses;

namespace CbtBackend.Services;

public interface IUserService {
    // basic operations
    Task<User?> GetUserByEmailAsync(string email);
    Task<List<User>> GetAllUsersAsync();
    Task<bool> UpdateUserAsync(User userToUpdate);
    Task<bool> DeleteUserAsync(string email);

    // registration
    Task<UserRegistrationResponse> RegisterUserAsync(UserRegistrationRequest userRequest);
    // authentication
    Task<UserAuthenticationResponse> AuthenticateUserAsync(UserAuthenticationRequest userRequest);
}

