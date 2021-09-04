using System.Linq;


namespace Ralfred.Common.DataAccess.Entities
{
	public record Secret : Entity
	{
		public int GroupId { get; set; }

		public string Name { get; set; }

		public string Value { get; set; }

		public bool IsFile { get; set; }
	}
}