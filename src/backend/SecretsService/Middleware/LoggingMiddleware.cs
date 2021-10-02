using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;


namespace Ralfred.SecretsService.Middleware
{
	public class LoggingMiddleware
	{
		public LoggingMiddleware(ILogger<LoggingMiddleware> logger, RequestDelegate next)
		{
			_next = next;
			_logger = logger;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			_logger.LogInformation($"Processing {context.Request.Method} request on {context.Request.Path}");

			await _next(context);
		}

		private readonly RequestDelegate _next;

		private readonly ILogger<LoggingMiddleware> _logger;
	}
}