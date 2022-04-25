namespace CbtBackend.Models.Requests;

public class UserAuthenticationRequest {
    public string Login { get; set; } = default!;

    public string Password { get; set; } = default!;
}
