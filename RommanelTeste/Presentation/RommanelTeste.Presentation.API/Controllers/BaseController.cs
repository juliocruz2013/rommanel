using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Mvc;
using RommanelTeste.Application.Common.Models;

namespace RommanelTeste.Presentation.API.Controllers;

public class BaseController : ControllerBase
{
	protected JwtDataUser JwtUserData()
	{
		try
		{
			string authorizationHeader = Request.Headers.Authorization;
			string? token = authorizationHeader?["Bearer ".Length..].Trim();

			var tokenHandler = new JwtSecurityTokenHandler();
			var jwtToken = tokenHandler.ReadJwtToken(token);

			return new JwtDataUser(jwtToken.Claims.FirstOrDefault(x => x.Type == "id")?.Value?.ToString());
		}
		catch (Exception ex)
		{
			throw new Exception(ex.Message, ex);
		}
	}
}
