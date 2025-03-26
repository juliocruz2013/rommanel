using RommanelTeste.Application.Common.Interfaces;
using RommanelTeste.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RommanelTeste.Persistence;

public static class DependencyInjection
{
	public static IServiceCollection AddPersistence(this IServiceCollection services)
	{
		services.AddHttpContextAccessor();
		services.AddDbContext<RommanelTesteContext>(options =>
		{
			options.UseSqlServer(Configuration.ConnectionString,
				sqlServerOptionsAction: sqlOptions =>
				{
					sqlOptions.EnableRetryOnFailure(
						maxRetryCount: 5,
						maxRetryDelay: TimeSpan.FromSeconds(10),
						errorNumbersToAdd: null);
				});
		},
			ServiceLifetime.Scoped
		);

		services.AddScoped<IRommanelTesteContext, RommanelTesteContext>();

		return services;
	}
}
