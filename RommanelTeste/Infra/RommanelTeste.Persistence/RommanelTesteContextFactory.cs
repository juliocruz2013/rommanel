using Microsoft.EntityFrameworkCore;

namespace RommanelTeste.Persistence;

public class RommanelTesteContextFactory : DesignTimeDbContextFactoryBase<RommanelTesteContext>
{
	protected override RommanelTesteContext CreateNewInstance(DbContextOptions<RommanelTesteContext> options)
	{
		return new RommanelTesteContext(options);
	}
}
