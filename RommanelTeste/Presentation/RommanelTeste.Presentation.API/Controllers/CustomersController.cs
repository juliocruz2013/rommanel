using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RommanelTeste.Application.Common.Models.Error;
using RommanelTeste.Application.Customer.Commands.CustomerCreate;
using RommanelTeste.Application.Customer.Commands.CustomerDelete;
using RommanelTeste.Application.Customer.Commands.CustomerUpdate;
using RommanelTeste.Application.Customer.Queries.GetCustomerById;
using RommanelTeste.Application.Customer.Queries.GetCustomers;

namespace RommanelTeste.Presentation.API.Controllers;

[Authorize(Roles = "Teste")]
[Route("api/v1/[controller]")]
[ApiController]
public class CustomersController(IMediator mediator) : BaseController
{
    private readonly IMediator _mediator = mediator;

    [HttpPost()]
    [ProducesResponseType(typeof(CustomerCreateCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CustomerCreateAsync([FromBody] CustomerCreateCommandRequest request)
    {
        request.UserId = JwtUserData().Id;
        var response = await _mediator.Send(request);
        return StatusCode(response.HttpStatusCode, response.GetResultData);
    }

    [HttpPut()]
    [ProducesResponseType(typeof(CustomerUpdateCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CustomerUpdateAsync([FromBody] CustomerUpdateCommandRequest request)
    {
        request.UserId = JwtUserData().Id;
        var response = await _mediator.Send(request);
        return StatusCode(response.HttpStatusCode, response.GetResultData);
    }

    [HttpGet("id/{id}")]
    [ProducesResponseType(typeof(GetCustomerByIdQueryResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCustomerByIdAsync([FromRoute] int id)
    {
        var request = new GetCustomerByIdQueryRequest
        {
            UserId = JwtUserData().Id,
            Id = id
        };

        var response = await _mediator.Send(request);
        return StatusCode(response.HttpStatusCode, response.GetResultData);
    }

    [HttpGet()]
    [ProducesResponseType(typeof(IEnumerable<GetCustomersQueryResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetCustomersAsync()
    {
        var response = await _mediator.Send(new GetCustomersQueryRequest { UserId = JwtUserData().Id });
        return StatusCode(response.HttpStatusCode, response.GetResultData);
    }

    [HttpDelete("id/{id}")]
    [ProducesResponseType(typeof(CustomerDeleteCommandResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ResponseError), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CustomerDeleteAsync([FromRoute] int id)
    {
        var response = await _mediator.Send(new CustomerDeleteCommandRequest { UserId = JwtUserData().Id, Id = id });
        return StatusCode(response.HttpStatusCode, response.GetResultData);
    }
}
