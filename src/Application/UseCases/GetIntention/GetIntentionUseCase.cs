using Application.Models;
using Application.Repository;
using Application.UseCases.GetIntention.Models;
using MediatR;

namespace Application.UseCases.GetIntention;

public class GetIntentionUseCase : IRequestHandler<GetIntentionRequest, GetIntentionResponse>
{
    private readonly IIntentionRepository _intentionRepository;

    public GetIntentionUseCase(IIntentionRepository intentionRepository)
    {
        _intentionRepository = intentionRepository;
    }

    public async Task<GetIntentionResponse> Handle(GetIntentionRequest request, CancellationToken cancellationToken)
    {
        var intention = new Intention(request.User);

        await _intentionRepository.SaveIntentionAsync(intention, intention.ExpireIn, cancellationToken);

        return new GetIntentionResponse(intention);
    }
}