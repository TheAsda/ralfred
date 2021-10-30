using System;

using Ralfred.Common.DependencyInjection;
using Ralfred.Common.Types;
using Ralfred.SecretsService.Services.Formatters;


namespace Ralfred.SecretsService.Services
{
	public class FormatterResolver : IFormatterResolver
	{
		private readonly StorageResolvingExtensions.SerializerResolver _serializerResolver;

		public FormatterResolver(StorageResolvingExtensions.SerializerResolver serializerResolver) =>
			_serializerResolver = serializerResolver;

		public ISecretFormatter Resolve(FormatType? type)
		{
			var serializer = _serializerResolver(type);

			return type switch
			{
				FormatType.Env  => new KeyValueSecretFormatter(),
				FormatType.Json => new JsonSecretFormatter(serializer),
				FormatType.Xml  => new XmlSecretFormatter(serializer),

				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
		}
	}
}