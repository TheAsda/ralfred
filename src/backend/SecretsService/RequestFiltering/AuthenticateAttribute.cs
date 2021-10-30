using System;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

using Ralfred.Common.Exceptions;
using Ralfred.Common.Helpers;
using Ralfred.Common.Types;


namespace Ralfred.SecretsService.RequestFiltering
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class AuthenticateAttribute : Attribute, IActionFilter
	{
		public void OnActionExecuting(ActionExecutingContext context)
		{
			var serviceProvider = context.HttpContext.RequestServices;

			var tokenService = serviceProvider.GetService<ITokenValidator>()!;
			var configuration = serviceProvider.GetService<Configuration>()!;

			var authenticationType = ResolveAuthenticationType(context.HttpContext);

			switch (authenticationType)
			{
				case AuthenticationType.Token:
				{
					var token = context.HttpContext.Request.Headers[TokenHeader];

					if (configuration.RootToken!.Equals(token, StringComparison.OrdinalIgnoreCase))
						return;

					if (context.HttpContext.Request.Path.ToString().StartsWith("/account"))
						throw new RestrictedAccessException("Account management only for admins");

					if (!tokenService.Validate(token))
						throw new UnauthorizedException("Token is not valid.");

					break;
				}

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public void OnActionExecuted(ActionExecutedContext context) { }

		private static AuthenticationType ResolveAuthenticationType(HttpContext context)
		{
			if (context.Connection.ClientCertificate != null)
				return AuthenticationType.Certificate;

			if (context.Request.Headers.ContainsKey(TokenHeader))
				return AuthenticationType.Token;

			return AuthenticationType.None;
		}

		private const string TokenHeader = "X-Token";
	}
}