using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using NUnit.Framework;

using Ralfred.SecretsProvider.Services;


namespace SecretsProvider.UnitTests.Services
{
	public class FileConverterTest
	{
		[SetUp]
		public void Setup()
		{
			_target = new FileConverter();
		}

		[Test]
		public void ConvertForm()
		{
			// arrange 
			using var mockStream = new MemoryStream();
			var file = Encoding.UTF8.GetBytes("file");
			mockStream.Write(file);

			var form = new FormCollection(new Dictionary<string, StringValues>
			{
				{ "test", "test" }
			}, new FormFileCollection
			{
				new FormFile(mockStream, 0, mockStream.Length, "file", "file")
			});

			// act
			var dictionary = _target.Convert(form);

			// assert
			Assert.AreEqual(1, dictionary.Keys.Count);
			Assert.AreEqual(true, dictionary.ContainsKey("file"));
			Assert.AreEqual("file", Encoding.UTF8.GetString(Convert.FromBase64String(dictionary["file"])));
		}

		private IFileConverter _target;
	}
}