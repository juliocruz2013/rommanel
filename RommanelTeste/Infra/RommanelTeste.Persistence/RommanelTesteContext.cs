using RommanelTeste.Application.Common.Interfaces;
using RommanelTeste.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace RommanelTeste.Persistence;

public class RommanelTesteContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>, IRommanelTesteContext
{
    public RommanelTesteContext(DbContextOptions<RommanelTesteContext> options) : base(options) { }

    public RommanelTesteContext() { }

    public DatabaseFacade DataBaseOrigim => throw new NotImplementedException();

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        foreach (EntityEntry<BaseEntity> entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added && entry.Entity.CreatedAt == DateTime.MinValue)
                entry.Entity.SetCreatedAt();

            if (entry.State == EntityState.Modified)
                entry.Entity.SetModifiedAt();
        }
        return base.SaveChangesAsync(cancellationToken);
    }
    public IExecutionStrategy CreateExecutionStrategy() => Database.CreateExecutionStrategy();

    public void SetModifiedState<T>(T entity) => base.Entry(entity).State = EntityState.Modified;

    public void AttachModelToContext<T>(T entity) => base.Attach(entity);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(RommanelTesteContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    public DbSet<ApplicationRole> ApplicationRole { get; set; }
    public DbSet<ApplicationUser> ApplicationUser { get; set; }
    public DbSet<Customer> Customer { get; set; }
    public DbSet<Address> Address { get; set; }
}
