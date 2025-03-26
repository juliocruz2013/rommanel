using RommanelTeste.Application.Common.Interfaces;
using RommanelTeste.Application.Common.Models.Error;
using RommanelTeste.Application.Common.Models.Response;
using RommanelTeste.Common;
using RommanelTeste.Domain.Enumerators;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace RommanelTeste.Application.Customer.Queries.GetCustomerById;

public class GetCustomerByIdQueryHandler(ILogger<GetCustomerByIdQueryHandler> _logger, IRommanelTesteContext _context) : IRequestHandler<GetCustomerByIdQueryRequest, ResponseApi<GetCustomerByIdQueryResponse>>
{
    public async Task<ResponseApi<GetCustomerByIdQueryResponse>> Handle(GetCustomerByIdQueryRequest request, CancellationToken cancellationToken)
    {
        var response = new ResponseApi<GetCustomerByIdQueryResponse>();

        try
        {
            if (!request.UserId.HasValue())
            {
                response.SetError(new ResponseError(TypeError.InvalidId, TypeError.InvalidId.GetDescription()), HttpStatusCode.BadRequest.GetHashCode());
                return response;
            }

            var customer = await _context.Customer
                .Include(x=>x.Address)
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (customer == null)
            {
                response.SetError(new ResponseError(TypeError.CustomerNotFound, TypeError.CustomerNotFound.GetDescription()), HttpStatusCode.BadRequest.GetHashCode());
                return response;
            }

            response.SetSuccess(new GetCustomerByIdQueryResponse(customer), HttpStatusCode.OK.GetHashCode());
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", $"Erro in {nameof(GetCustomerByIdQueryHandler)}. Request: {request.ToJson()} - Exception: {ex}");
            response.SetError(new ResponseError(TypeError.DefaultError, TypeError.DefaultError.GetDescription()), HttpStatusCode.InternalServerError.GetHashCode());
        }
        return response;
    }
}