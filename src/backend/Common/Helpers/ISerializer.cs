namespace Ralfred.Common.Helpers
{
	public interface ISerializer
	{
		public string? Serialiaze(object? @object);

		public T Deserialize<T>(string? serializedObject) where T : class;
	}
}