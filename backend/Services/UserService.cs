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
    public IEnumerable<User> GetAllUsers() {
        throw new NotImplementedException();
    }

    public User GetUserById(int id) {
        throw new NotImplementedException();
    }

    public User GetUserByEmail(string email) {
        throw new NotImplementedException();
    }

    // implementation of user authentication
    public UserAuthenticationResponse Authenticate(UserAuthenticationRequest model) {
        var user = GetUserByEmail(model.Email);

        if (user == null) {
            throw new AuthenticationCredentialsException();
        }

        throw new NotImplementedException ();
    }

    // TODO: move this to different class?
    private string GenerateJwtToken (User user, DateTime expiration) {
        throw new NotImplementedException();
    }
}