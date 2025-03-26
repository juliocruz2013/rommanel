using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RommanelTeste.Application.Common.Interfaces;
using RommanelTeste.Application.Common.Models.Error;
using RommanelTeste.Application.Common.Models.Response;
using RommanelTeste.Common;
using RommanelTeste.Domain.Enumerators;
using System.Net;

namespace RommanelTeste.Application.Customer.Commands.CustomerUpdate;

public class CustomerUpdateCommandHandler(ILogger<CustomerUpdateCommandHandler> logger, IRommanelTesteContext context) : IRequestHandler<CustomerUpdateCommandRequest, ResponseApi<CustomerUpdateCommandResponse>>
{
    private readonly ILogger<CustomerUpdateCommandHandler> _logger = logger;
    private readonly IRommanelTesteContext _context = context;

    public async Task<ResponseApi<CustomerUpdateCommandResponse>> Handle(CustomerUpdateCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new ResponseApi<CustomerUpdateCommandResponse>();

        try
        {
            if (!request.UserId.HasValue())
            {
                response.SetError(new ResponseError(TypeError.InvalidId, TypeError.InvalidId.GetDescription()), HttpStatusCode.BadRequest.GetHashCode());
                return response;
            }

            var customer = await _context.Customer.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
            if (customer == null)
            {
                response.SetError(new ResponseError(TypeError.CustomerNotFound, TypeError.CustomerNotFound.GetDescription()), HttpStatusCode.BadRequest.GetHashCode());
                return response;
            }

            var invalidData = await _context.Customer.AnyAsync(x => x.Id != customer.Id && x.Email == request.Email 
                                                                 || x.Id != customer.Id && x.DocumentNumber == request.DocumentNumber, 
                                                                 cancellationToken);
            if (invalidData)
            {
                response.SetError(new ResponseError(TypeError.CustomerRegisteredWithTheSameData, TypeError.CustomerRegisteredWithTheSameData.GetDescription()), HttpStatusCode.BadRequest.GetHashCode());
                return response;
            }

            customer.SetCustomer(
                request.UserId,
                request.Name,
                request.DocumentNumber.UnMask(),
                request.Email,
                request.BirthDate,
                request.Phone.UnMask(),
                request.IsCompany,
                request.StateRegistration,
                request.IsExempt,
                Domain.Entities.Address.SetAddress(
                    customer.Id,
                    request.Address.ZipCode.UnMask(),
                    request.Address.Street,
                    request.Address.Number,
                    request.Address.Neighborhood,
                    request.Address.City,
                    request.Address.State));

            _context.Customer.Update(customer);
            await _context.SaveChangesAsync(cancellationToken);

            response.SetSuccess(new CustomerUpdateCommandResponse(customer), HttpStatusCode.OK.GetHashCode());
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", $"Erro in {nameof(CustomerUpdateCommandHandler)}. Request: {request.ToJson()} - Exception: {ex}");
            response.SetError(new ResponseError(TypeError.DefaultError, TypeError.DefaultError.GetDescription()), HttpStatusCode.InternalServerError.GetHashCode());
            return response;
        }
    }
}
