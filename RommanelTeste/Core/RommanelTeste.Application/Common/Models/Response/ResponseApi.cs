using RommanelTeste.Application.Common.Models.Error;
using Newtonsoft.Json;

namespace RommanelTeste.Application.Common.Models.Response
{
    public class ResponseApi<T>
	{
		public bool Success { get; private set; }

		[JsonIgnore]
		public int HttpStatusCode { get; private set; }
		public T Data { get; set; }
		public ResponseError Error { get; private set; }

		public ResponseApi() { }

		public void SetSuccess(T data, int httpStatusCode)
		{
			Data = data;
			HttpStatusCode = httpStatusCode;
			Success = true;
		}

		public void SetSuccess(int httpStatusCode)
		{
			HttpStatusCode = httpStatusCode;
			Success = true;

		}

		public void SetError(ResponseError error, int httpStatusCode)
		{
			Error = error;
			HttpStatusCode = httpStatusCode;
		}

		[JsonIgnore]
		public object GetResultData => Success ? Data : Error;
	}
}
