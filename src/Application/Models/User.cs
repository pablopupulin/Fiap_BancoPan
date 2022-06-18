using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Application.Models;

public class User
{
    private string _password;

    public Guid UserId { get; set; }

    public string Name { get; set; }
    public string Email { get; set; }
    public string Document { get; set; }

    public string Password { get; set; }

    public bool ValidPasswords(IEnumerable<string> possiblePasswords)
    {
        return possiblePasswords.Any(ValidPassword);
    }

    public bool ValidPassword(string password)
    {
        var passwordHasher = new PasswordHasher<User>();

        var result = passwordHasher.VerifyHashedPassword(this, Password, password);

        return result > 0;
    }

    public string GetAccessToken(DateTimeOffset expireIn)
    {
        return GenerateAccessToken(GetClaims(), expireIn);
    }

    private string HashPassword(string password)
    {
        var passwordHasher = new PasswordHasher<User>();

        return passwordHasher.HashPassword(this, password);
    }

    public User ChangePassword(string password)
    {
        Password = HashPassword(password);
        return this;
    }

    private IEnumerable<Claim> GetClaims()
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, Name),
            new(ClaimTypes.Email, Email),
            new(ClaimTypes.Sid, UserId.ToString())
        };

        return claims;
    }

    private static string GenerateAccessToken(IEnumerable<Claim> claims, DateTimeOffset expiresIn)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Convert.FromBase64String("aQgzcRBPfKzDOyFE05ac3KJZl892wUup5dGnTVd+tRE=");

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Audience = "BancoPan",
            Expires = expiresIn.UtcDateTime,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public Login Login()
    {
        var expireIn = DateTimeOffset.Now.AddMinutes(1);

        return new Login
        {
            UserId = UserId,
            AccessToken = GetAccessToken(expireIn),
            RefreshToken = Guid.NewGuid().ToString(),
            ExpireIn = expireIn,
            LoggedAt = DateTimeOffset.Now
        };
    }
}