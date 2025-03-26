using RommanelTeste.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace RommanelTeste.Persistence.Configurations;

public class CustomerEntityTypeConfiguration : BaseEntityTypeConfiguration<Customer>
{
	public override void Configure(EntityTypeBuilder<Customer> builder)
	{
        base.Configure(builder);

        builder.ToTable(nameof(Customer));

        builder.HasKey(b => b.Id)
            .IsClustered();

        builder.Property(b => b.Id)
            .ValueGeneratedOnAdd()
            .HasColumnOrder(1)
            .IsRequired();

        builder.Property(x => x.UserId)
            .HasColumnOrder(2)
            .IsRequired();

        builder.Property(x => x.Name)
            .HasMaxLength(150)
            .HasColumnOrder(3)
            .IsRequired();

		builder.Property(x => x.DocumentNumber)
            .HasMaxLength(20)
            .HasColumnOrder(4)
            .IsRequired();

		builder.Property(x => x.BirthDate)
            .HasColumnOrder(5)
            .IsRequired();

		builder.Property(x => x.Phone)
            .HasMaxLength(11)
            .HasColumnOrder(6)
            .IsRequired();

		builder.Property(x => x.Email)
            .HasMaxLength(255)
            .HasColumnOrder(7)
            .IsRequired();
        
		builder.Property(x => x.IsCompany)
            .HasColumnOrder(8)
            .IsRequired();
        
		builder.Property(x => x.StateRegistration)
            .HasMaxLength(30)
            .HasColumnOrder(9)
            .IsRequired(false);
        
		builder.Property(x => x.IsExempt)
            .HasColumnOrder(10)
            .IsRequired(false);

        builder.HasIndex(x => new { x.DocumentNumber, x.Email })
            .IsUnique();

        builder.HasOne(c => c.ApplicationUser)
            .WithMany(u => u.Customers)
            .HasForeignKey(c => c.UserId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
