using Application.Models;
using MediatR;

namespace Application.UseCases.Login.Models;

public class LoginRequest : IRequest<Notifable<LoginResponse>>
{
    public LoginRequest()
    {
        Password = new List<Guid>();
    }

    public Guid IntentionId { get; set; }

    public IEnumerable<Guid> Password { get; set; }

}