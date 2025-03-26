using MediatR;
using RommanelTeste.Application.Common.Models.Response;

namespace RommanelTeste.Application.Users.Commands.UsersCreate;

public class UsersCreateCommandRequest : IRequest<ResponseApi<UsersCreateCommandResponse>>
{
	public string Email { get; set; }
	public string Password { get; set; }
	public string PasswordConfirmation { get; set; }
}