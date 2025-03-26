using MediatR;
using RommanelTeste.Application.Common.Models.Response;

namespace RommanelTeste.Application.Authentication.Commands.AuthenticationCreate;

public record AuthenticationCreateCommandRequest(string? Email, string? Password) : IRequest<ResponseApi<AuthenticationCreateCommandResponse>>;
