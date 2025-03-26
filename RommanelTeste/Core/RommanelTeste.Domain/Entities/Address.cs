namespace RommanelTeste.Domain.Entities;

public class Address : BaseEntity
{
    public int Id { get; private set; }
    public int CustomerId { get; private set; }
    public string ZipCode { get; private set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string Neighborhood { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }

    public static Address SetAddress(int customerId, string zipCode, string street, string number, string neighborhood, string city, string state)
        => new Address
        {
            CustomerId = customerId,
            ZipCode = zipCode,
            Street = street,
            Number = number,
            Neighborhood = neighborhood,
            City = city,
            State = state
        };

}