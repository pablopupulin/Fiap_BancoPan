using Application.Models;
using MediatR;

namespace Application.UseCases.Refresh.Models;

public class RefreshRequest : IRequest<Notifable<RefreshResponse>>
{
    public string RefreshToken { get; set; }
}