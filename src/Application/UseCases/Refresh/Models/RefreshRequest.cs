using Application.Models;
using MediatR;

namespace Application.UseCases.Refresh.Models;

public class RefreshRequest : IRequest<Notifable<RefreshResponse>>
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}