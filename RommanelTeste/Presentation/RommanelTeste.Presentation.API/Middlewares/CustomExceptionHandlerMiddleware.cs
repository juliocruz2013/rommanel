using RommanelTeste.Application.Common.Exceptions;
using RommanelTeste.Application.Common.Models.Error;
using RommanelTeste.Common;
using System.Net;

namespace RommanelTeste.Presentation.API.Middlewares;

public class CustomExceptionHandlerMiddleware(RequestDelegate next, ILogger<CustomExceptionHandlerMiddleware> log)
{
	private readonly ILogger<CustomExceptionHandlerMiddleware> _log = log;
	private readonly RequestDelegate _next = next;

	public async Task Invoke(HttpContext context)
	{
		try
		{
			await _next(context);
		}
		catch (Exception ex)
		{
			await HandleExceptionAsync(context, ex);
		}
	}

	private Task HandleExceptionAsync(HttpContext context, Exception exception)
	{
		_log.LogError(exception, exception.Message);

		var code = HttpStatusCode.InternalServerError;

		ResponseError result = null;

		switch (exception)
		{
			case ValidationException validationException:
				code = HttpStatusCode.BadRequest;

				result = new ResponseError
				{
					Message = "Validation error",
					Errors = validationException.Failures?.Select(x => new ResponseErrorItem
					{
						Key = x.Key,
						Value = x.Value
					})
				};
				break;
			case BadRequestException badRequestException:
				code = HttpStatusCode.BadRequest;
				result = new ResponseError { Message = badRequestException.Message };
				break;
			case NotFoundException _:
				code = HttpStatusCode.NotFound;
				break;
		}

		context.Response.ContentType = "application/json";
		context.Response.StatusCode = (int)code;

		if (result == null)
			result = new ResponseError { Message = exception.Message };

		return context.Response.WriteAsync(result.ToJson());
	}
}
