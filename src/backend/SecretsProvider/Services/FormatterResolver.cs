using System;

using Ralfred.Common.DependencyInjection;
using Ralfred.Common.Types;
using Ralfred.SecretsProvider.Services.Formatters;


namespace Ralfred.SecretsProvider.Services
{
	public class FormatterResolver : IFormatterResolver
	{
		public FormatterResolver(StorageResolvingExtensions.SerializerResolver serializerResolver)
		{
			_serializerResolver = serializerResolver;
		}

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

		private readonly StorageResolvingExtensions.SerializerResolver _serializerResolver;
	}
}