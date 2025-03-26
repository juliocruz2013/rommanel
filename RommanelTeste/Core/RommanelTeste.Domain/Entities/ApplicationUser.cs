using Microsoft.AspNetCore.Identity;

namespace RommanelTeste.Domain.Entities;

public class ApplicationUser : IdentityUser
{
    public ICollection<Customer> Customers { get; set; } = [];
}
