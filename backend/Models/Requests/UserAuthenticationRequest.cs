namespace CbtBackend.Models.Requests;

public class UserAuthenticationRequest {
    public string Email { get; set; } = default!;

    public string Password { get; set; } = default!;
}
