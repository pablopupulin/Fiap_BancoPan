using Application.UseCases.GetIntention.Models;
using Application.UseCases.Login.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[Route("OAuth")]
public class OAuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public OAuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("intention")]
    [ProducesResponseType(typeof(GetIntentionResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetIntetionAsync([FromQuery] GetIntentionRequest request)
    {
        var result = await _mediator.Send(request);

        return Ok(result);
    }

    [HttpPost]
    [Route("login")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginAsync([FromBody] LoginRequest request)
    {
        var response = await _mediator.Send(request);

        if (response.Sucess)
        {
            return Ok(response.Data);
        }

        return Unauthorized(response.Error);
    }

    [HttpPost]
    [Route("refresh")]
    [ProducesResponseType(typeof(LoginResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RefreshAsync([FromBody] LoginRequest request)
    {
        var response = await _mediator.Send(request);

        if (response.Sucess)
        {
            return Ok(response.Data);
        }

        return Unauthorized(response.Error);
    }
}