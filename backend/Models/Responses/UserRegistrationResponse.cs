using CbtBackend.Entities;

namespace CbtBackend.Models.Responses;

public class UserRegistrationResponse {
    public User User { get; set; } = default!;
}
