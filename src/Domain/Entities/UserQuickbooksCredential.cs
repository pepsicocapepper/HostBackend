namespace Domain.Entities;

public class UserQuickbooksCredential
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    public required string AccessToken { get; set; }
    public required string RefreshToken { get; set; }
}