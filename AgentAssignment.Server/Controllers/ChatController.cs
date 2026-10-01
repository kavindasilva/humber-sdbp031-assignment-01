using AgentAssignment.Server.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AgentAssignment.Server.Controllers;

[ApiController]
//[Route("/")]
//[Route("/v1/chat/completions")]
[Produces("application/json")]
public class ChatController : ControllerBase
{
    private IChatCompletionService _chatService;

    public ChatController(IChatCompletionService chatService)
    {
        _chatService = chatService;
    }

    [HttpPost("/v1/chat/completions")]
    [ProducesResponseType<ChatCompletionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<ChatCompletionResponse>> CompleteAsync([FromBody] ChatCompletionRequest request, CancellationToken cancellationToken = default)
    {
        if (request.Messages is null || request.Messages.Count == 0)
            return BadRequest();

        else if (!_chatService.IsReady)
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { status = "starting" });

        return Ok();
    }

    [HttpGet("/health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<ChatCompletionResponse>> GetHealthStatus()
    {
        if (!_chatService.IsReady)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable, new { status = "starting" });
        }

        return Ok(new { status = "ready" });
    }
}
