using RommanelTeste.Application.Customer.Dtos;

namespace RommanelTeste.Application.Customer.Queries.GetCustomerById;

public class GetCustomerByIdQueryResponse
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string DocumentNumber { get; set; }
    public string Email { get; set; }
    public string BirthDate { get; set; }
    public string Phone { get; set; }
    public bool IsCompany { get; set; }
    public string? StateRegistration { get; set; }
    public bool? IsExempt { get; set; }

    public AddressDto Address { get; set; }

    public GetCustomerByIdQueryResponse() { }

    public GetCustomerByIdQueryResponse(Domain.Entities.Customer customer)
    {
        Id = customer.Id;
        Name = customer.Name;
        DocumentNumber = customer.DocumentNumber;
        Email = customer.Email;
        BirthDate = customer.BirthDate.ToShortDateString();
        Phone = customer.Phone;
        IsCompany = customer.IsCompany;
        StateRegistration = customer.StateRegistration;
        IsExempt = customer.IsExempt;
        Address = new AddressDto(customer.Address);
    }
}