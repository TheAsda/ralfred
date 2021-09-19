using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.AspNetCore.Mvc;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.Managers;
using Ralfred.Common.Types;
using Ralfred.SecretsProvider.Models;
using Ralfred.SecretsProvider.Services;


namespace Ralfred.SecretsProvider.Controllers
{
	[ApiController]
	[Route("{*route}")]
	public class SecretsController : ControllerBase
	{
		public SecretsController(
			ISecretsManager    secretsManager,
			IFileConverter     fileConverter,
			IFormatterResolver formatterResolver,
			Configuration      configuration)
		{
			_secretsManager = secretsManager;
			_fileConverter = fileConverter;
			_formatterResolver = formatterResolver;
			_configuration = configuration;
		}

		[HttpPut]
		public void AddSecrets([FromRoute] RequestPayload payload)
		{
			var secretNames = payload.Secrets?.Split(',') ?? Array.Empty<string>();

			if (payload.Body is null && payload.FormData is null)
			{
				throw new Exception("Secrets are not provided");
			}

			if (payload.Body is not null)
			{
				_secretsManager.AddSecrets(payload.Route ?? string.Empty, payload.Body, new Dictionary<string, string>(), secretNames);

				return;
			}

			var files = _fileConverter.Convert(payload.FormData);

			_secretsManager.AddSecrets(payload.Route ?? string.Empty,
				payload.FormData.ToDictionary(x => x.Key, x => x.Value.ToString()),
				files,
				secretNames);
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
		public void RemoveSecrets([FromRoute] RequestPayload payload)
		{
			var secretNames = payload.Secrets?.Split(',') ?? Array.Empty<string>();

			throw new NotImplementedException();
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

		private readonly ISecretsManager _secretsManager;
		private readonly IFileConverter _fileConverter;
		private readonly IFormatterResolver _formatterResolver;
	}
}