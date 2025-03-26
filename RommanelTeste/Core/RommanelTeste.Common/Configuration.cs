using Microsoft.Extensions.Configuration;

namespace RommanelTeste.Common;

public static class Configuration
{
	public static IConfigurationRoot _configuration;

	public static void Build(string pathJsonFile)
	{
		var environment1 = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT");
		var environment2 = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
		var builder = new ConfigurationBuilder()
			.SetBasePath(pathJsonFile)
			.AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
			.AddJsonFile($"appsettings.{environment1}.json", optional: true, reloadOnChange: false)
			.AddJsonFile($"appsettings.{environment2}.json", optional: true, reloadOnChange: false)
			.AddJsonFile("secrets/appsettings.secrets.json", optional: true, reloadOnChange: false)
			.AddEnvironmentVariables();

		_configuration = builder.Build();
	}

	public static IConfiguration GetConfiguration()
	{
		return _configuration;
	}

	public static string ConnectionString => _configuration.GetConnectionString("DefaultConnection");
	public static int Expiration => _configuration["JwtExpiration"].HasValue() ? int.Parse(_configuration["JwtExpiration"]) : 7;
	public static string JWTSecret => "d5=(o3c}@FR8gc&u]RXaq3jYxI}pkvZJTj0,J)";
}
