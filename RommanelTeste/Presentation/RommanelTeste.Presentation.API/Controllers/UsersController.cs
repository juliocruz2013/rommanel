using MediatR;
using Microsoft.AspNetCore.Mvc;
using RommanelTeste.Application.Common.Models.Error;
using RommanelTeste.Application.Users.Commands.UsersCreate;

namespace RommanelTeste.Presentation.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class UsersController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost()]
    [ProducesResponseType(typeof(UsersCreateCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateUsersAsync([FromBody] UsersCreateCommandRequest request)
    {
        var response = await _mediator.Send(request);
        return StatusCode(response.HttpStatusCode, response.GetResultData);
    }
}
