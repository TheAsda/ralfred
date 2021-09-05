using System;


namespace Ralfred.Common.DataAccess.Entities
{
	public record Secret : Entity
	{
		public Guid GroupId { get; init; }

		public string Name { get; set; }

		public string Value { get; set; }

		public bool IsFile { get; set; }
	}
}