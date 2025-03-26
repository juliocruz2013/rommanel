using RommanelTeste.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RommanelTeste.Persistence.Configurations;

public abstract class BaseEntityTypeConfiguration<TBase> : IEntityTypeConfiguration<TBase> where TBase : BaseEntity
{
	public virtual void Configure(EntityTypeBuilder<TBase> builder)
	{
        builder.Property(b => b.CreatedAt)
		   .UsePropertyAccessMode(PropertyAccessMode.Field)
           .HasColumnType("datetime2")
           .HasDefaultValueSql("GETDATE()")
           .IsRequired(true);

		builder.Property(b => b.ModifiedAt)
		   .UsePropertyAccessMode(PropertyAccessMode.Field)
           .HasColumnType("datetime2")
           .HasDefaultValueSql("GETDATE()")
           .IsRequired(false);
	}
}
