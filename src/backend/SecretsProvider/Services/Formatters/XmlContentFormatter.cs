using System.Collections.Generic;
using System.Linq;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.Helpers.Serialization;


namespace Ralfred.SecretsProvider.Services.Formatters
{
	public class XmlContentFormatter : IContentFormatter
	{
		public XmlContentFormatter(ISerializer? serializer)
		{
			_serializer = serializer;
		}

		public string? Format(IEnumerable<Secret> data)
		{
			var outputData = data.Select(x => new Models.Secret
			{
				Name = x.Name,
				Value = x.Value
			});

			return _serializer?.Serialize(outputData.ToList());
		}

		private readonly ISerializer? _serializer;
	}
}