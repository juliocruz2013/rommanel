using RommanelTeste.Application;
using RommanelTeste.Common;
using RommanelTeste.Persistence;
using RommanelTeste.Presentation.API.Extensions;
using RommanelTeste.Presentation.API.Middlewares;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.MSSqlServer;

Configuration.Build(Directory.GetCurrentDirectory());

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

configuration.SetBasePath(Directory.GetCurrentDirectory())
			 .AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true)
			 .AddEnvironmentVariables();

Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.Filter.ByExcluding(x => x.Level == LogEventLevel.Warning)
				 .WriteTo.MSSqlServer(Configuration.ConnectionString,
				 	sinkOptions: new MSSqlServerSinkOptions
				 	{
				 		SchemaName = "dbo",
				 		AutoCreateSqlTable = true,
				 		TableName = "Logs"
				 	})
				.CreateLogger();

builder.Services.AddCustomFramework();
builder.Services.AddCustomOpenAPI();
builder.Services.AddCustomAuthenticationAPI();
builder.Services.AddApplication();
builder.Services.AddAuthorization();
builder.Services.AddPersistence();
builder.Host.UseSerilog();

var app = builder.Build();

app.UseCors(option =>
{
    option.AllowAnyOrigin();
    option.AllowAnyHeader();
    option.AllowAnyMethod();
    option.WithExposedHeaders("Content-Disposition");
});

if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI(options =>
	{
		options.SwaggerEndpoint("../swagger/v1/swagger.json", "RommanelTeste v1");
		options.RoutePrefix = "docs";
		options.ConfigObject.AdditionalItems["syntaxHighlight"] = new Dictionary<string, object>
		{
			["activated"] = true
		};
	});
}

app.UseSerilogRequestLogging("HTTP {RequestMethod} {RequestPath} STATUS {StatusCode} IN {Elapsed:0.0000} ms");
app.UseCustomExceptionHandler();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();