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

		public IContentFormatter Resolve(FormatType? type)
		{
			var serializer = _serializerResolver(type);

			return type switch
			{
				FormatType.Env  => new KeyValueContentFormatter(),
				FormatType.Json => new JsonContentFormatter(serializer),
				FormatType.Xml  => new XmlContentFormatter(serializer),

				_ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
			};
		}

		private readonly StorageResolvingExtensions.SerializerResolver _serializerResolver;
	}
}