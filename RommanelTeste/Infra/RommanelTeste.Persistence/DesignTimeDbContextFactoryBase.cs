using RommanelTeste.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace RommanelTeste.Persistence;

public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : RommanelTesteContext
{
	public TContext CreateDbContext(string[] args)
	{
		var basePath = Directory.GetCurrentDirectory() + string.Format(".{0}..{0}..{0}Presentation{0}RommanelTeste.Presentation.API", Path.DirectorySeparatorChar);
		Console.WriteLine(basePath);
		Configuration.Build(basePath);
		return Create(connectionString: Configuration.ConnectionString);
	}

	protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

	private TContext Create(string connectionString)
	{
		if (string.IsNullOrEmpty(connectionString))
		{
			throw new ArgumentException($"Connection string is null or empty.", nameof(connectionString));
		}

		Console.WriteLine($"DesignTimeDbContextFactoryBase.Create(string): Connection string: '{connectionString}'.");

		var optionsBuilder = new DbContextOptionsBuilder<TContext>();

		optionsBuilder.UseSqlServer(connectionString);

		return CreateNewInstance(optionsBuilder.Options);
	}
}
