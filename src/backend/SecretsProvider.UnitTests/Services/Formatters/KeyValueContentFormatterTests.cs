using System;
using System.Linq;

using AutoFixture;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.Helpers.Serialization;
using Ralfred.SecretsProvider.Services.Formatters;


namespace SecretsProvider.UnitTests.Services.Formatters
{
	[TestFixture]
	public class KeyValueContentFormatterTests
	{
		[SetUp]
		public void Setup()
		{
			_fixture = new Fixture();

			_target = new KeyValueContentFormatter();
		}

		[Test]
		public void FormatTest()
		{
			// arrange
			var data = _fixture.CreateMany<Secret>().ToList();
			var expected = string.Join(Environment.NewLine, data.Select(x => $"{x.Name}={x.Value}"));

			// act
			var result = _target.Format(data);

			// assert
			result.Should().Be(expected);
		}

		private IFixture _fixture;

		private KeyValueContentFormatter _target;
	}
}