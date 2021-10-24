using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

		public Dictionary<string, string>? Data => Body ?? FormData?.ToDictionary(x => x.Key, x => x.Value.ToString());

		public Dictionary<string, IFormFile>? Files => FormData?.Files.ToDictionary(x => x.Name, x => x);

		[FromQuery] public string? Secrets { get; init; }

		[FromQuery] public FormatType? Format { get; set; }

		[FromQuery] public bool IncludeFiles { get; set; }
	}
}