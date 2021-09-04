using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.Formatters;


namespace Ralfred.SecretsProvider
{
	public class BypassFormDataInputFormatter : IInputFormatter
	{
		public bool CanRead(InputFormatterContext context)
		{
			return true;

			// return context.HttpContext.Request.HasFormContentType || context.HttpContext.Request.Method.ToLower() == "get";
		}

		public Task<InputFormatterResult> ReadAsync(InputFormatterContext context)
		{
			return InputFormatterResult.SuccessAsync(null);
		}
	}
}