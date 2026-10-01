using AgentAssignment.Server.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace AgentAssignment.Server.Controllers;

[ApiController]
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
            return BadRequest(new ApiErrorResponse
            (
                new ApiErrorBody
                (
                    "Messages is null or empty",
                    "400"
                )
            ));

        else if (!_chatService.IsReady)
            return StatusCode(
                StatusCodes.Status503ServiceUnavailable,
                new ApiErrorResponse
                (
                    new ApiErrorBody
                    (
                        "Messages is null or empty",
                        "503"
                    )
                )
            );

        try
        {
            var response = await _chatService.CompleteAsync(request, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new ApiErrorResponse
                (
                    new ApiErrorBody
                    (
                        "Error occured",
                        "500"
                    )
                )
            );
        }
    }

    [HttpGet("/health")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<ActionResult<ChatCompletionResponse>> GetHealthStatus()
    {
        if (!_chatService.IsReady)
        {
            return StatusCode(StatusCodes.Status503ServiceUnavailable,
                new ApiErrorResponse
                (
                    new ApiErrorBody
                    (
                        "Starting",
                        "503"
                    )
                )
            );
        }

        return Ok(new { status = "ready" });
    }
}
