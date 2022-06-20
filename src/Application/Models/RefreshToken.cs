namespace Application.Models;

public class RefreshToken
{
    public string Token { get; set; }
    public Guid UserId { get; set; }
    public DateTimeOffset ExpireIn { get; set; }

    public RefreshToken(Guid userId, int timeToExpireInMinutes)
    {
        UserId = userId;
        Token = Guid.NewGuid().ToString();
        ExpireIn = DateTimeOffset.Now.AddMinutes(timeToExpireInMinutes);
    }
}