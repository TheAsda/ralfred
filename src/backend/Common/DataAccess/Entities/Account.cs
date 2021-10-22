using System;


namespace Ralfred.Common.DataAccess.Entities
{
	public record Account : Entity
	{
		public string? Name { get; init; }

		public string? TokenHash { get; init; }

		public string? CertificateThumbprint { get; init; }

		public Guid[]? RoleIds { get; init; }
	}
}