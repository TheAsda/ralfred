using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Ralfred.Common.Managers;
using Ralfred.Common.Types;
using Ralfred.SecretsService.Models;
using Ralfred.SecretsService.Services;


namespace Ralfred.SecretsService.Controllers
{
	[ApiController]
	[Route("{*route}")]
	public class SecretsController : ControllerBase
	{
		public SecretsController(
			ISecretsManager            secretsManager,
			IFileConverter             fileConverter,
			IFormatterResolver         formatterResolver,
			Configuration              configuration,
			ILogger<SecretsController> logger)
		{
			_secretsManager = secretsManager;
			_fileConverter = fileConverter;
			_formatterResolver = formatterResolver;
			_configuration = configuration;
			_logger = logger;
		}

		[HttpPut]
		public void AddSecrets([FromRoute] RequestPayload payload)
		{
			var secretNames = payload.Secrets?.Split(',') ?? Array.Empty<string>();

			if (payload.Data is null || !payload.Data.Keys.Any())
			{
				throw new Exception("Secrets are not provided");
			}

			_secretsManager.AddSecrets(payload.Route ?? string.Empty, payload.Data, _fileConverter.Convert(payload.Files), secretNames);
		}

		[HttpGet]
		public string? GetSecrets([FromRoute] RequestPayload payload)
		{
			var secretNames = payload.Secrets?.Split(',') ?? Array.Empty<string>();

			var secrets = _secretsManager.GetSecrets(payload.Route ?? string.Empty, secretNames);

			if (!payload.IncludeFiles)
				secrets = secrets.Where(x => !x.IsFile);

			var format = payload.Format ?? _configuration.DefaultFormat;

			var formatter = _formatterResolver.Resolve(format);
			HttpContext.Response.ContentType = ResolveContentType(format);

			return formatter.Format(secrets);
		}

		[HttpDelete]
		public void DeleteSecrets([FromRoute] RequestPayload payload)
		{
			var secretNames = payload.Secrets?.Split(',') ?? Array.Empty<string>();

			_secretsManager.DeleteSecrets(payload.Route ?? string.Empty, secretNames);
		}

		private static string ResolveContentType(FormatType? format)
		{
			return format switch
			{
				FormatType.Env  => "text/plain",
				FormatType.Json => "application/json",
				FormatType.Xml  => "text/plain",
				_               => "text/plain"
			};
		}

		private readonly Configuration _configuration;
		private readonly ILogger<SecretsController> _logger;

		private readonly ISecretsManager _secretsManager;
		private readonly IFileConverter _fileConverter;
		private readonly IFormatterResolver _formatterResolver;
	}
}