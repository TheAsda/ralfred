using System.Net;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using Ralfred.Common.Exceptions;
using Ralfred.SecretsService.Models;


namespace Ralfred.SecretsService
{
	public class ExceptionsFilter : IExceptionFilter
	{
		public ExceptionsFilter(ILogger<ExceptionsFilter> logger)
		{
			_logger = logger;
		}

		public void OnException(ExceptionContext context)
		{
			var status = HttpStatusCode.InternalServerError;
			var message = "Internal sever error.";
			
			switch (context.Exception)
			{
				case NotFoundException:
					status = HttpStatusCode.NotFound;
					message = context.Exception.Message;

					break;
				default:
					_logger.LogError(context.Exception, message);

					break;
			}

			context.ExceptionHandled = true;
			context.HttpContext.Response.StatusCode = (int)status;
			context.HttpContext.Response.ContentType = "application/json";

			context.Result = new ObjectResult(new Error
			{
				StatusCode = (int)status,
				Message = message
			});
		}

		private readonly ILogger<ExceptionsFilter> _logger;
	}
}