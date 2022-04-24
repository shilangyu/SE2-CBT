using CbtBackend.Entities;

namespace CbtBackend.Models.Responses;

public class UserAuthenticationResponse {
    public User User { get; set; } = default!;

    public string Token { get; set; } = default!;

    public DateTime TokenExpiration { get; set; }
}
