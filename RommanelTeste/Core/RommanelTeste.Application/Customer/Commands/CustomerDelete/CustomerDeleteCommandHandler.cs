using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RommanelTeste.Application.Common.Interfaces;
using RommanelTeste.Application.Common.Models.Error;
using RommanelTeste.Application.Common.Models.Response;
using RommanelTeste.Common;
using RommanelTeste.Domain.Enumerators;
using System.Net;

namespace RommanelTeste.Application.Customer.Commands.CustomerDelete;

public class CustomerDeleteCommandHandler(ILogger<CustomerDeleteCommandHandler> logger, IRommanelTesteContext context) : IRequestHandler<CustomerDeleteCommandRequest, ResponseApi<CustomerDeleteCommandResponse>>
{
    private readonly ILogger<CustomerDeleteCommandHandler> _logger = logger;
    private readonly IRommanelTesteContext _context = context;

    public async Task<ResponseApi<CustomerDeleteCommandResponse>> Handle(CustomerDeleteCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new ResponseApi<CustomerDeleteCommandResponse>();

        try
        {
            if (!request.UserId.HasValue())
            {
                response.SetError(new ResponseError(TypeError.InvalidId, TypeError.InvalidId.GetDescription()), HttpStatusCode.BadRequest.GetHashCode());
                return response;
            }
            
            var customer = await _context.Customer.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (customer == null)
            {
                response.SetError(new ResponseError(TypeError.CustomerNotFound, TypeError.CustomerNotFound.GetDescription()), HttpStatusCode.BadRequest.GetHashCode());
                return response;
            }

            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync(cancellationToken);

            response.SetSuccess(new CustomerDeleteCommandResponse("Cliente excluído com sucesso!"), HttpStatusCode.OK.GetHashCode());
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", $"Erro in {nameof(CustomerDeleteCommandHandler)}. Request: {request.ToJson()} - Exception: {ex}");
            response.SetError(new ResponseError(TypeError.DefaultError, TypeError.DefaultError.GetDescription()), HttpStatusCode.InternalServerError.GetHashCode());
            return response;
        }
    }
}
