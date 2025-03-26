using RommanelTeste.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace RommanelTeste.Persistence.Migration;

class Program
{
	public static async Task Main(string[] args)
	{
		Configuration.Build(Directory.GetCurrentDirectory());

		var loggerFactory = LoggerFactory.Create(builder =>
		{
			builder.AddConfiguration(Configuration.GetConfiguration().GetSection("Logging")).AddSimpleConsole();
		});
		
		var optionsBuilder = new DbContextOptionsBuilder<RommanelTesteContext>();
		optionsBuilder.UseLoggerFactory(loggerFactory)
					  .UseSqlServer(Configuration.ConnectionString);

		await new RommanelTesteContext(optionsBuilder.Options).Database.MigrateAsync();
	}
}
