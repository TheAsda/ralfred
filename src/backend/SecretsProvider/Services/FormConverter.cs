using System.Collections.Generic;
using System.IO;

using Microsoft.AspNetCore.Http;


namespace Ralfred.SecretsProvider.Services
{
	public class FormConverter : IFormConverter
	{
		public Dictionary<string, string> Convert(IFormCollection? form)
		{
			var dict = new Dictionary<string, string>();

			if (form is null)
			{
				return dict;
			}

			foreach (var key in form.Keys)
			{
				dict.Add(key, form[key]);
			}

			foreach (var file in form.Files)
			{
				var ms = new MemoryStream();
				file.OpenReadStream().CopyTo(ms);
				dict.Add(file.Name, System.Convert.ToBase64String(ms.ToArray()));
			}

			return dict;
		}
	}
}