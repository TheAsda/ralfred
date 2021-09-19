using System.Collections.Generic;
using System.Linq;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.Helpers.Serialization;


namespace Ralfred.SecretsProvider.Services.Formatters
{
	public class JsonContentFormatter : IContentFormatter
	{
		public JsonContentFormatter(ISerializer serializer)
		{
			_serializer = serializer;
		}

		public string? Format(IEnumerable<Secret> data)
		{
			var dictionary = data.ToDictionary(x => x.Name, x => x.Value);

			return _serializer.Serialize(dictionary);
		}

		private readonly ISerializer _serializer;
	}
}