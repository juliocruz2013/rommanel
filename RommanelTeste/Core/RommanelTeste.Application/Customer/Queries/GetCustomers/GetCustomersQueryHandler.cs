using RommanelTeste.Application.Common.Interfaces;
using RommanelTeste.Application.Common.Models.Error;
using RommanelTeste.Application.Common.Models.Response;
using RommanelTeste.Common;
using RommanelTeste.Domain.Enumerators;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Net;

namespace RommanelTeste.Application.Customer.Queries.GetCustomers;

public class GetCustomersQueryHandler(ILogger<GetCustomersQueryHandler> _logger, IRommanelTesteContext _context) : IRequestHandler<GetCustomersQueryRequest, ResponseApi<IEnumerable<GetCustomersQueryResponse>>>
{
    public async Task<ResponseApi<IEnumerable<GetCustomersQueryResponse>>> Handle(GetCustomersQueryRequest request, CancellationToken cancellationToken)
    {
        var response = new ResponseApi<IEnumerable<GetCustomersQueryResponse>>();

        try
        {
            if (!request.UserId.HasValue())
            {
                response.SetError(new ResponseError(TypeError.InvalidId, TypeError.InvalidId.GetDescription()), HttpStatusCode.BadRequest.GetHashCode());
                return response;
            }

            var customers = await _context.Customer.Include(x => x.Address).ToListAsync(cancellationToken);
            response.SetSuccess(customers.Select(x => new GetCustomersQueryResponse(x)), HttpStatusCode.OK.GetHashCode());
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", $"Erro in {nameof(GetCustomersQueryHandler)}. Request: {request.ToJson()} - Exception: {ex}");
            response.SetError(new ResponseError(TypeError.DefaultError, TypeError.DefaultError.GetDescription()), HttpStatusCode.InternalServerError.GetHashCode());
        }
        return response;
    }
}