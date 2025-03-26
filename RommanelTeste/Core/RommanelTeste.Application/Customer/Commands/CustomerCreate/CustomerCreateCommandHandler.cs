using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RommanelTeste.Application.Common.Interfaces;
using RommanelTeste.Application.Common.Models.Error;
using RommanelTeste.Application.Common.Models.Response;
using RommanelTeste.Common;
using RommanelTeste.Domain.Enumerators;
using System.Net;

namespace RommanelTeste.Application.Customer.Commands.CustomerCreate;

public class CustomerCreateCommandHandler(ILogger<CustomerCreateCommandHandler> logger, IRommanelTesteContext context) : IRequestHandler<CustomerCreateCommandRequest, ResponseApi<CustomerCreateCommandResponse>>
{
    private readonly ILogger<CustomerCreateCommandHandler> _logger = logger;
    private readonly IRommanelTesteContext _context = context;

    public async Task<ResponseApi<CustomerCreateCommandResponse>> Handle(CustomerCreateCommandRequest request, CancellationToken cancellationToken)
    {
        var response = new ResponseApi<CustomerCreateCommandResponse>();

        try
        {
            if (!request.UserId.HasValue())
            {
                response.SetError(new ResponseError(TypeError.InvalidId, TypeError.InvalidId.GetDescription()), HttpStatusCode.BadRequest.GetHashCode());
                return response;
            }

            var dataCustomerExists = await _context.Customer.AnyAsync(x => x.Email == request.Email || x.DocumentNumber == request.DocumentNumber, cancellationToken);
            if (dataCustomerExists)
            {
                response.SetError(new ResponseError(TypeError.CustomerRegisteredWithTheSameData, TypeError.CustomerRegisteredWithTheSameData.GetDescription()), HttpStatusCode.BadRequest.GetHashCode());
                return response;
            }
            
            var customer = new Domain.Entities.Customer();

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

            await _context.Customer.AddAsync(customer, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            response.SetSuccess(new CustomerCreateCommandResponse("Cliente cadastrado com sucesso!"), HttpStatusCode.Created.GetHashCode());
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError("{Message}", $"Erro in {nameof(CustomerCreateCommandHandler)}. Request: {request.ToJson()} - Exception: {ex}");
            response.SetError(new ResponseError(TypeError.DefaultError, TypeError.DefaultError.GetDescription()), HttpStatusCode.InternalServerError.GetHashCode());
            return response;
        }
    }
}
