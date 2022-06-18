namespace Application.UseCases.Login.Models;

public class LoginResponse
{
    public LoginResponse(Application.Models.Login login)
    {
        AccessToken = login.AccessToken;
        ExpireIn = login.ExpireIn;
        RefreshToken = login.RefreshToken;
    }

    public DateTimeOffset ExpireIn { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}