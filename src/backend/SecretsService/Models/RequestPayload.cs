using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Ralfred.Common.Types;


namespace Ralfred.SecretsService.Models
{
	[Serializable]
	public record RequestPayload(string? Route)
	{
		[FromBody] public Dictionary<string, string>? Body { get; init; }

		[FromForm] public IFormCollection? FormData { get; init; }

		[FromQuery] public string? Secrets { get; init; }

		[FromQuery] public FormatType? Format { get; set; }

		[FromQuery] public bool IncludeFiles { get; set; }
	}
}