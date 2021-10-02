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
	public class TokenValidatorTests
	{
		[SetUp]
		public void Setup()
		{
			_fixture = new Fixture();

			_guidProvider = new Mock<IGuidProvider>();
			_dateTimeProvider = new Mock<IDateTimeProvider>();

			_target = new TokenValidator(_dateTimeProvider.Object);
			_tokenProvider = new TokenProvider(_guidProvider.Object, _dateTimeProvider.Object);
		}

		[Test]
		public void Validate_SuccessTest()
		{
			// arrange
			var date = DateTime.UtcNow;

			var days = _fixture.Create<int>();
			var guid = _fixture.Create<Guid>();

			_guidProvider.Setup(x => x.Generate()).Returns(guid);
			_dateTimeProvider.Setup(x => x.GetUtc()).Returns(date);

			var token = _tokenProvider.Generate(days);

			// act
			var result = _target.Validate(token);

			// assert
			result.Should().BeTrue();

			_guidProvider.Verify(x => x.Generate(), Times.Once);
			_dateTimeProvider.Verify(x => x.GetUtc(), Times.Exactly(2));
		}

		[Test]
		public void Validate_FailedTest()
		{
			// arrange
			var date = DateTime.UtcNow;

			var days = _fixture.Create<int>();
			var guid = _fixture.Create<Guid>();

			_dateTimeProvider.Setup(x => x.GetUtc()).Returns(date);
			_guidProvider.Setup(x => x.Generate()).Returns(guid);

			var token = _tokenProvider.Generate(days);

			_dateTimeProvider.Setup(x => x.GetUtc()).Returns(date.AddDays(days + 1));

			// act
			var result = _target.Validate(token);

			// assert
			result.Should().BeFalse();

			_guidProvider.Verify(x => x.Generate(), Times.Once);
			_dateTimeProvider.Verify(x => x.GetUtc(), Times.Exactly(2));
		}

		private IFixture _fixture;

		private Mock<IGuidProvider> _guidProvider;
		private Mock<IDateTimeProvider> _dateTimeProvider;

		private TokenValidator _target;
		private TokenProvider _tokenProvider;
	}
}