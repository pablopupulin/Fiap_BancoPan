using MediatR;

namespace Application.UseCases.GetIntention.Models;

public class GetIntentionRequest : IRequest<GetIntentionResponse>
{
    public string User { get; set; }
}