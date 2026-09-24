using AgentAssignment.Server.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AgentAssignment.Server.Controllers;

[ApiController]
[Route("/v1/chat/completions")]
[Produces("application/json")]
public class ChatController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType<ChatCompletionResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<ChatCompletionResponse>> CompleteAsync([FromBody] ChatCompletionRequest request, CancellationToken cancellationToken = default)
    {
        return Ok();
    }
}
