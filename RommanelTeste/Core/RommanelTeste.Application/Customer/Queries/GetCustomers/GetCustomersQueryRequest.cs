using RommanelTeste.Application.Common.Models.Response;
using MediatR;
using System.Text.Json.Serialization;

namespace RommanelTeste.Application.Customer.Queries.GetCustomers;

public class GetCustomersQueryRequest : IRequest<ResponseApi<IEnumerable<GetCustomersQueryResponse>>>
{
    [JsonIgnore]
    public string UserId { get; set; }
}
