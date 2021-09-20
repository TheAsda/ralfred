using System;
using System.Collections.Generic;
using System.Linq;

using Ralfred.Common.DataAccess.Entities;


namespace Ralfred.SecretsProvider.Services.Formatters
{
	public class KeyValueSecretFormatter : ISecretFormatter
	{
		public string Format(IEnumerable<Secret> data)
		{
			return string.Join(Environment.NewLine, data.Select(x => $"{x.Name}={x.Value}"));
		}
	}
}