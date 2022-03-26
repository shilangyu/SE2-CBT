namespace CbtBackend.Entities;

public class User {
    public int Id { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public int Age { get; set; }
    public string? Gender { get; set; }
    public int UserStatus { get; set; }
    public bool Banned { get; set; }
}