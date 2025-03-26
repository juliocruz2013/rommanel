using RommanelTeste.Application.Common.Models.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace RommanelTeste.Application.Customer.Queries.GetCustomerById;

public class GetCustomerByIdQueryRequest : IRequest<ResponseApi<GetCustomerByIdQueryResponse>>
{
    [JsonIgnore]
    public string UserId { get; set; }
    public int Id { get; set; }
}
