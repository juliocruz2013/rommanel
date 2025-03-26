namespace RommanelTeste.Application.Customer.Dtos;

public class AddressDto
{
    public string ZipCode { get; set; }
    public string Street { get; set; }
    public string Number { get; set; }
    public string Neighborhood { get; set; }
    public string City { get; set; }
    public string State { get; set; }

    public AddressDto() { }

    public AddressDto(Domain.Entities.Address address)
    {
        ZipCode = address.ZipCode;
        Street = address.Street;
        Number = address.Number;
        Neighborhood = address.Neighborhood;
        City = address.City;
        State = address.State;
    }
}