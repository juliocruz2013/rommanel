using RommanelTeste.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RommanelTeste.Persistence.Configurations;

public class AddressEntityTypeConfiguration : BaseEntityTypeConfiguration<Address>
{
	public override void Configure(EntityTypeBuilder<Address> builder)
	{
        base.Configure(builder);
        
        builder.ToTable(nameof(Address));
        
        builder.HasKey(b => b.Id)
            .IsClustered();

        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd()
            .UsePropertyAccessMode(PropertyAccessMode.FieldDuringConstruction)
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property(x => x.CustomerId)
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property(x => x.Street)
            .HasMaxLength(150)
            .HasColumnOrder(3)
            .IsRequired();

        builder.Property(x => x.Number)
            .HasMaxLength(10)
            .HasColumnOrder(4)
            .IsRequired();

		builder.Property(x => x.Neighborhood)
            .HasMaxLength(100)
            .HasColumnOrder(5)
            .IsRequired();

		builder.Property(x => x.City)
            .HasMaxLength(100)
            .HasColumnOrder(6)
            .IsRequired();

		builder.Property(x => x.State)
            .HasMaxLength(2)
            .HasColumnOrder(7)
            .IsRequired();
        
        builder.HasIndex(x=>x.CustomerId).IsUnique();
	}
}
