using RommanelTeste.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace RommanelTeste.Application.Common.Interfaces;

public interface IRommanelTesteContext
{
	DbSet<ApplicationRole> ApplicationRole { get; set; }
	DbSet<ApplicationUser> ApplicationUser { get; set; }
    DbSet<Domain.Entities.Customer> Customer { get; set; }
    DbSet<Domain.Entities.Address> Address { get; set; }

    IExecutionStrategy CreateExecutionStrategy();
	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    void SetModifiedState<T>(T entity);
	void AttachModelToContext<T>(T entity);
	DatabaseFacade DataBaseOrigim { get; }
}
