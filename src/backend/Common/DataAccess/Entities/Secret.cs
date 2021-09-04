using System.Linq;


namespace Ralfred.Common.DataAccess.Entities
{
	public record Secret : Entity
	{
		public string Name { get; set; }

		public string Value { get; set; }
	}
}