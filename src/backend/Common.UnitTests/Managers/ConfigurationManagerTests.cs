using AutoFixture;

using FluentAssertions;

using Moq;

using NUnit.Framework;

using Ralfred.Common.Helpers;
using Ralfred.Common.Helpers.Serialization;
using Ralfred.Common.Managers;
using Ralfred.Common.Types;


namespace SecretsProvider.UnitTests.Managers
{
	[TestFixture]
	public class ConfigurationManagerTests
	{
		[SetUp]
		public void Setup()
		{
			_serializer = new Mock<ISerializer>(MockBehavior.Strict);
			_contentProvider = new Mock<IContentManager>(MockBehavior.Strict);
			_tokenService = new Mock<ITokenService>();

			_target = new ConfigurationManager(_serializer.Object, _contentProvider.Object, _tokenService.Object);
		}

		private readonly IFixture _fixture = new Fixture();

		private Mock<ISerializer> _serializer;
		private Mock<IContentManager> _contentProvider;
		private Mock<ITokenService> _tokenService;

		private ConfigurationManager _target;

		[Test]
		public void GetTest()
		{
			// arrange
			var path = _fixture.Create<string>();
			var content = _fixture.Create<string>();

			var expected = new Configuration
			{
				EnableWebUi = false,
				Engine = StorageEngineType.Mongo,
				ConnectionString = _fixture.Create<string>()
			};

			_contentProvider.Setup(x => x.Get(path)).Returns(content);
			_serializer.Setup(x => x.Deserialize<Configuration>(content)).Returns(expected);

			// act
			var result = _target.Get(path);

			// assert
			result.Should().NotBeNull();
			result.Should().Be(expected);
		}

		[Test]
		public void MergeTest()
		{
			// arrange
			var firstConfiguration = _fixture.Create<Configuration>();
			var secondConfiguration = _fixture.Build<Configuration>().Without(x => x.ConnectionString).Without(x => x.Engine).Create();

			var expected = secondConfiguration with
			{
				RootToken = firstConfiguration.RootToken,
				ConnectionString = firstConfiguration.ConnectionString,
				Engine = firstConfiguration.Engine
			};

			// act
			var result = _target.Merge(firstConfiguration, secondConfiguration);

			// assert
			result.Should().NotBeNull();
			result.Should().Be(expected);
		}
	}
}