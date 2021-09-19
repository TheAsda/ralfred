using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;


namespace Ralfred.Common.Helpers
{
	public class YamlSerializer : ISerializer
	{
		public string? Serialiaze(object? @object)
		{
			if (@object is null)
				return null;

			var serializer = new SerializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();

			return serializer.Serialize(@object);
		}

		public T Deserialize<T>(string? serializedObject) where T : class
		{
			if (serializedObject is null)
				return default;

			var deserializer = new DeserializerBuilder()
				.WithNamingConvention(CamelCaseNamingConvention.Instance)
				.Build();

			return deserializer.Deserialize<T>(serializedObject);
		}
	}
}