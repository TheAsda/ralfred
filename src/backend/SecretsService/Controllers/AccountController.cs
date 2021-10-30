using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.Helpers;
using Ralfred.Common.Managers;
using Ralfred.Common.Types;
using Ralfred.SecretsService.Models;
using Ralfred.SecretsService.RequestFiltering;


namespace Ralfred.SecretsService.Controllers
{
	[ApiController]
	[Authenticate]
	[Route("account")]
	public class AccountController : ControllerBase
	{
		public AccountController(IAccountManager accountManager, ITokenService tokenService)
		{
			_accountManager = accountManager;
			_tokenService = tokenService;
		}

		[HttpPost]
		public string CreateAccount([FromRoute] RequestPayload payload)
		{
			var accountType = GetAccountType(payload.Data);

			switch (accountType)
			{
				case AccountType.Token:
					string token;

					if (payload.Data!.ContainsKey("generate") && payload.Data!["generate"] == "true")
					{
						token = _tokenService.GenerateToken();
					}
					else
					{
						if (!payload.Data.ContainsKey("token"))
						{
							throw new ArgumentException("Token is not provided");
						}

						token = payload.Data["token"];
					}

					_accountManager.CreateTokenAccount(token);

					return token;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		[HttpDelete("{accountId}")]
		public void DeleteAccount([FromRoute] Guid accountId)
		{
			_accountManager.DeleteAccount(accountId);
		}

		[HttpGet]
		public IEnumerable<Account> ListAccounts()
		{
			return _accountManager.GetAccounts();
		}

		private static AccountType GetAccountType(IDictionary<string, string>? body)
		{
			if (body is null || !body.ContainsKey("type"))
			{
				throw new ArgumentException("Type is not provided");
			}

			return body["type"].ToLower() switch
			{
				"token" => AccountType.Token,
				_       => throw new ArgumentOutOfRangeException()
			};
		}

		private readonly IAccountManager _accountManager;
		private readonly ITokenService _tokenService;
	}
}