namespace Application.Models;

public class Login
{
    private readonly RefreshToken _refreshToken;

    public Login(Guid userId, int timeToExpireInMinutes, string accesstoken)
    {
        UserId = userId;
        LoggedAt = DateTimeOffset.Now;
        ExpireIn = DateTimeOffset.Now.AddMinutes(timeToExpireInMinutes);
        AccessToken = accesstoken;

        _refreshToken = new RefreshToken(userId, timeToExpireInMinutes * 2);
        RefreshToken = _refreshToken.Token;
    }

    public Guid UserId { get; set; }
    public DateTimeOffset LoggedAt { get; set; }
    public DateTimeOffset ExpireIn { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public RefreshToken GetRefreshToken()
    {
        return _refreshToken;
    }
}