namespace CbtBackend.Models;

public record LoginResponseDTO(string AccessToken, int UserStatus, int UserId);
