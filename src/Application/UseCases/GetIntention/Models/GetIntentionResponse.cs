using Application.Models;

namespace Application.UseCases.GetIntention.Models;

public class GetIntentionResponse
{
    public GetIntentionResponse(Intention intention)
    {
        IntentionId = intention.IntentionId;
        Keyboard = intention.Keyboard;
    }

    public Guid IntentionId { get; set; }
    public IEnumerable<Keyboard> Keyboard { get; set; }
}