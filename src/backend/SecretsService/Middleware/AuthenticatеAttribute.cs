using System;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

using Ralfred.Common.Exceptions;
using Ralfred.Common.Security;
using Ralfred.Common.Types;


namespace Ralfred.SecretsService.Middleware
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class AuthenticatеAttribute : Attribute, IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext context)
		{
			var serviceProvider = context.HttpContext.RequestServices;
			var tokenValidator = serviceProvider.GetService<ITokenValidator>()!;

			var authenticationType = ResolveAuthenticationType(context.HttpContext);

			switch (authenticationType)
			{
				case AuthenticationType.Token:
				{
					var token = context.HttpContext.Request.Headers[TokenHeader];

					if (!tokenValidator.Validate(token))
						throw new UnauthorizedException("Token is not valid.");

					break;
				}

				case AuthenticationType.Certificate:
					break;

				case AuthenticationType.None:
					break;

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		private static AuthenticationType ResolveAuthenticationType(HttpContext context)
		{
			if (context.Connection.ClientCertificate != null)
				return AuthenticationType.Certificate;

			if (context.Request.Headers.ContainsKey(TokenHeader))
				return AuthenticationType.Token;

			return AuthenticationType.None;
		}

		public void OnActionExecuted(ActionExecutedContext context) { }

		private const string TokenHeader = "X-Token";
	}
}