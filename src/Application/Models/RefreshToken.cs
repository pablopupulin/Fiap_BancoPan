namespace Application.Models;

public class RefreshToken
{
    public RefreshToken(Guid userId, int timeToExpireInMinutes) : this()
    {
        UserId = userId;
        Token = Guid.NewGuid().ToString();
        ExpireIn = DateTimeOffset.Now.AddMinutes(timeToExpireInMinutes);
    }

    public RefreshToken()
    {
    }

    public string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset ExpireIn { get; set; }
}