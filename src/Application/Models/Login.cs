namespace Application.Models;

public class Login
{
    public Guid UserId { get; set; }
    public DateTimeOffset LoggedAt { get; set; }
    public DateTimeOffset ExpireIn { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}