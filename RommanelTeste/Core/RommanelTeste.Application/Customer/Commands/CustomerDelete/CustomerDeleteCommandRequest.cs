using MediatR;
using RommanelTeste.Application.Common.Models.Response;
using System.Text.Json.Serialization;

namespace RommanelTeste.Application.Customer.Commands.CustomerDelete;

public class CustomerDeleteCommandRequest : IRequest<ResponseApi<CustomerDeleteCommandResponse>>
{
    [JsonIgnore]
    public string? UserId { get; set; }
    public int Id { get; set; }
}