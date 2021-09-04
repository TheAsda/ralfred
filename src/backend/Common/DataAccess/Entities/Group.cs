namespace Ralfred.Common.DataAccess.Entities
{
	public record Group : Entity
	{
		public string Path { get; set; }

		public Secret[] Secrets { get; set; }
	}
}