using System;

using AutoFixture;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Ralfred.Common.Helpers;
using Ralfred.Common.Security;


namespace SecretsProvider.UnitTests.Security
{
	[TestFixture]
	public class TokenProviderTests
	{
		[SetUp]
		public void Setup()
		{
			_fixture = new Fixture();

			_guidProvider = new Mock<IGuidProvider>();
			_dateTimeProvider = new Mock<IDateTimeProvider>();

			_target = new TokenProvider(_guidProvider.Object, _dateTimeProvider.Object);
		}

		[Test]
		public void GenerateTest()
		{
			// arrange
			var days = 2;

			var guid = _fixture.Create<Guid>();
			var dateTime = DateTime.UtcNow;

			_guidProvider.Setup(x => x.Generate()).Returns(guid);
			_dateTimeProvider.Setup(x => x.GetUtc()).Returns(dateTime);

			// act
			var result = _target.Generate(days);

			// assert
			result.Should().NotBeNullOrWhiteSpace();

			_guidProvider.Verify(x => x.Generate(), Times.Once);
			_dateTimeProvider.Verify(x => x.GetUtc(), Times.Once);
		}

		private IFixture _fixture;

		private Mock<IDateTimeProvider> _dateTimeProvider;
		private Mock<IGuidProvider> _guidProvider;

		private TokenProvider _target;
	}
}