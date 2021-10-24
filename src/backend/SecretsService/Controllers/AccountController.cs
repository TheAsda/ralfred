using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories;
using Ralfred.Common.DataAccess.Repositories.Abstractions;
using Ralfred.Common.Managers;
using Ralfred.Common.Types;
using Ralfred.SecretsService.Models;
using Ralfred.SecretsService.Services;


namespace Ralfred.SecretsService.Controllers
{
	[ApiController]
	[Route("account")]
	public class AccountController
	{
		public AccountController(IAccountManager accountManager, ITokenService tokenService)
		{
			_accountManager = accountManager;
			_tokenService = tokenService;
		}

		[HttpPost]
		public Account CreateAccount([FromRoute] RequestPayload payload)
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

					return _accountManager.CreateTokenAccount(token);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		[HttpDelete("{accountId}")]
		public void DeleteAccount([FromQuery] Guid accountId)
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