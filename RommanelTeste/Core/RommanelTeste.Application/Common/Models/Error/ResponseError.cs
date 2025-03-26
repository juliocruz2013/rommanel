using RommanelTeste.Common;
using RommanelTeste.Domain.Enumerators;

namespace RommanelTeste.Application.Common.Models.Error;

public class ResponseError
{
	public int Code { get; set; }
	public string Message { get; set; }
    public IEnumerable<ResponseErrorItem> Errors { get; set; }
	
	public ResponseError() { }

	public ResponseError(TypeError code, string? message)
	{
		Code = code.GetHashCode();
		Message = message;
	}
}
