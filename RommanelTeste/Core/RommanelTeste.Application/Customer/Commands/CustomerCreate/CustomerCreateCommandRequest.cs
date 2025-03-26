using MediatR;
using RommanelTeste.Application.Common.Models.Response;
using RommanelTeste.Application.Customer.Dtos;
using System.Text.Json.Serialization;

namespace RommanelTeste.Application.Customer.Commands.CustomerCreate;

public class CustomerCreateCommandRequest : IRequest<ResponseApi<CustomerCreateCommandResponse>>
{
    [JsonIgnore]
    public string? UserId { get; set; }
    public string Name { get; set; }
    public string DocumentNumber { get; set; }
    public string Email { get; set; }
    public DateTime BirthDate { get; set; }
    public string Phone { get; set; }
    public bool IsCompany { get; set; }
    public string? StateRegistration { get; set; }
    public bool? IsExempt { get; set; }
    public AddressDto Address { get; set; } 
}