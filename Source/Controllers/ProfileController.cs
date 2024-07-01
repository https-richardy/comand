#pragma warning disable CS8601

namespace Comanda.WebApi.Handlers;

[ApiController]
[Route("api/profiles")]
public sealed class ProfileController(IMediator mediator) : ControllerBase
{
    [HttpPost("register-address")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> RegisterCustomerAddress(RegisterAddressRequest request)
    {
        request.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var response = await mediator.Send(request);

        return StatusCode(response.StatusCode, response);
    }
}