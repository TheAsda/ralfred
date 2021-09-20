using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AutoFixture;

using FluentAssertions;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using NUnit.Framework;

using Ralfred.SecretsService.Services;


namespace SecretsService.UnitTests.Services
{
	public class FileConverterTests
	{
		[SetUp]
		public void Setup()
		{
			_fixture = new Fixture();

			_target = new FileConverter();
		}

		[Test]
		public void ConvertForm()
		{
			// arrange
			const string fileKey = "file";

			using var mockStream = new MemoryStream();

			var content = _fixture.Create<string>();
			var file = Encoding.UTF8.GetBytes(content);
			mockStream.Write(file);

			var form = new FormCollection(new Dictionary<string, StringValues>
			{
				{ _fixture.Create<string>(), _fixture.Create<string>() }
			}, new FormFileCollection
			{
				new FormFile(mockStream, 0, mockStream.Length, fileKey, fileKey)
			});

			// act
			var dictionary = _target.Convert(form);

			// assert
			dictionary.Keys.Count.Should().Be(1);
			dictionary.ContainsKey(fileKey).Should().BeTrue();

			Encoding.UTF8.GetString(Convert.FromBase64String(dictionary[fileKey]))
				.Should().Be(content);
		}

		private IFixture _fixture;

		private IFileConverter _target;
	}
}