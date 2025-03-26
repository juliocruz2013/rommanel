namespace RommanelTeste.Domain.Entities;

public class Customer : BaseEntity
{
    public int Id { get; private set; }
    public string UserId { get; private set; }
    public string Name { get; private set; }
    public string DocumentNumber { get; private set; }
    public string Email { get; private set; }
    public DateTime BirthDate { get; private set; }
    public string Phone { get; private set; }
    public bool IsCompany { get; private set; }
    public string? StateRegistration { get; private set; }
    public bool? IsExempt { get; private set; }

    public virtual ApplicationUser ApplicationUser { get; private set; }
    
    public virtual Address Address { get; private set; }

    public void SetCustomer(string userId, string name, string documentNumber, string email, DateTime birthDate, string phone, bool isCompany, string? stateRegistration, bool? isExempt, Address address)
    {
        UserId = userId;
        Name = name;
        DocumentNumber = documentNumber;
        Email = email;
        BirthDate = birthDate;
        Phone = phone;
        IsCompany = isCompany;
        StateRegistration = stateRegistration;
        IsExempt = isExempt;
        Address = address;
    }
}