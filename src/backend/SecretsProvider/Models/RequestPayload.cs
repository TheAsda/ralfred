using System;
using System.Collections.Generic;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Ralfred.SecretsProvider.Models
{
	[Serializable]
	public record RequestPayload(string Route)
	{
		[FromBody] public Dictionary<string, string>? Body { get; init; }

		[FromForm] public IFormCollection? FormData { get; init; }

		[FromQuery] public string? Secrets { get; init; }
	}
}