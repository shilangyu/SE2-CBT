using CbtBackend.Entities;
using CbtBackend.Models;
using CbtBackend.Models.Requests;
using CbtBackend.Models.Responses;

namespace CbtBackend.Services; 

public interface IUserService {
    // basic operations
    Task<User?> GetUserByIdAsync(int userId);
    Task<User?> GetUserByEmailAsync(string email);
    Task<List<User>> GetAllUsersAsync();
    Task<bool> UpdateUserAsync(User userToUpdate);
    Task<bool> DeleteUserAsync(string email);
    Task<bool> RegisterUserAsync(User user);



    // authentication
    Task<UserAuthenticationResponse> AuthenticateUserAsync(UserAuthenticationRequest model);
}

