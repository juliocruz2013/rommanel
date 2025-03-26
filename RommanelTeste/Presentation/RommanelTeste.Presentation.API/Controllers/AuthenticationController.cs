using RommanelTeste.Application.Authentication.Commands.AuthenticationCreate;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RommanelTeste.Application.Common.Models.Error;

namespace RommanelTeste.Presentation.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthenticationController(IMediator mediator) : BaseController
{
	private readonly IMediator _mediator = mediator;

	[HttpPost()]
	[ProducesResponseType(typeof(AuthenticationCreateCommandResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> AuthenticationUserAsync([FromBody] AuthenticationCreateCommandRequest request)
	{
		var response = await _mediator.Send(request);
		return StatusCode(response.HttpStatusCode, response.GetResultData);
	}
}
