using RommanelTeste.Application.Common.Models.Error;
using RommanelTeste.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace RommanelTeste.Presentation.API.Filters;

public class ValidateModelStateAttribute : ActionFilterAttribute
{
	public override void OnActionExecuting(ActionExecutingContext context)
	{
		if (!context.ModelState.IsValid)
		{
			try
			{
				var values = context.ModelState.Values.Where(v => v.Errors.Count > 0).ToList();

				var errors = new List<ResponseErrorItem>();

				foreach (var val in values)
				{
					var key = val.ToJson().FromJson<ResponseErrorItem>().Key;

					foreach (var error in val.Errors)
					{
						errors.Add(new ResponseErrorItem
						{
							Key = key,
							Value = error.ErrorMessage
						});

					}
				}

				var result = new ResponseError
				{
					Message = "Validation error",
					Errors = errors
				};

				context.Result = new JsonResult(result)
				{
					StatusCode = 400
				};
			}
			catch
			{
				var result = new ResponseError
				{
					Message = "An error occur."
				};

				context.Result = new JsonResult(result)
				{
					StatusCode = 400
				};
			}
		}
	}
}
