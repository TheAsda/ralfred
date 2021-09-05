using System;


namespace Ralfred.Common.DataAccess.Entities
{
	public record Group : Entity
	{
		public string Name { get; init; }

		public string Path { get; init; }

		public Secret[] Secrets { get; init; }
	}
}