using System;
using System.Collections.Generic;
using System.Linq;

using Moq;

using NUnit.Framework;

using Ralfred.Common.DataAccess.Entities;
using Ralfred.Common.DataAccess.Repositories;
using Ralfred.Common.Helpers;
using Ralfred.Common.Managers;
using Ralfred.Common.Types;


namespace SecretsProvider.UnitTests.Managers
{
	[TestFixture]
	public class SecretsManagerTest
	{
		[SetUp]
		public void Setup()
		{
			_pathResolver = new Mock<IPathResolver>();
			_secretsRepository = new Mock<ISecretsRepository>();
			_groupRepository = new Mock<IGroupRepository>();
			_target = new SecretsManager(_pathResolver.Object, _secretsRepository.Object, _groupRepository.Object);
		}

		[Test]
		public void GetSecretsNoneTest()
		{
			// arrange
			_pathResolver.Setup(x => x.Resolve(It.IsAny<string>())).Returns(PathType.None);

			// act

			// assert
			Assert.Throws<Exception>(() => _target.GetSecrets("test", Array.Empty<string>()));
		}

		[Test]
		public void GetSecretsGroupTest()
		{
			// arrange
			var mockSecret = new Secret
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Value = "test",
				GroupId = Guid.Empty,
				IsFile = false
			};

			_pathResolver.Setup(x => x.Resolve(It.IsAny<string>())).Returns(PathType.Group);
			_pathResolver.Setup(x => x.DeconstructPath(It.IsAny<string>())).Returns(("test", "test"));

			_secretsRepository.Setup(x => x.GetGroupSecrets(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Secret>
			{
				mockSecret
			});

			// act
			var result = _target.GetSecrets("test", Array.Empty<string>()).ToList();

			// assert
			Assert.AreEqual(result.Count, 1);
			Assert.AreEqual(result[0], mockSecret);
		}

		[Test]
		public void GetSecretsSecretTest()
		{
			// arrange
			var mockSecret = new Secret
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Value = "test",
				GroupId = Guid.Empty,
				IsFile = false
			};

			_pathResolver.Setup(x => x.Resolve(It.IsAny<string>())).Returns(PathType.Secret);
			_pathResolver.Setup(x => x.DeconstructPath(It.IsAny<string>())).Returns(("test", "test"));

			_secretsRepository.Setup(x => x.GetGroupSecrets(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Secret>
			{
				mockSecret
			});

			// act
			var result = _target.GetSecrets("test/test", Array.Empty<string>()).ToList();

			// assert
			Assert.AreEqual(result.Count, 1);
			Assert.AreEqual(result[0], mockSecret);
		}

		[Test]
		public void GetSecretsSecretNotFoundTest()
		{
			// arrange
			var mockSecret = new Secret
			{
				Id = Guid.NewGuid(),
				Name = "random",
				Value = "random",
				GroupId = Guid.Empty,
				IsFile = false
			};

			_pathResolver.Setup(x => x.Resolve(It.IsAny<string>())).Returns(PathType.Secret);
			_pathResolver.Setup(x => x.DeconstructPath(It.IsAny<string>())).Returns(("test", "test"));

			_secretsRepository.Setup(x => x.GetGroupSecrets(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Secret>
			{
				mockSecret
			});

			// assert
			Assert.Throws<Exception>(() => _target.GetSecrets("random", Array.Empty<string>()));
		}

		[Test]
		public void GetSecretsGroupWithNamesTest()
		{
			// arrange
			var mockSecret = new Secret
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Value = "test",
				GroupId = Guid.Empty,
				IsFile = false
			};

			var mockSecret2 = new Secret
			{
				Id = Guid.NewGuid(),
				Name = "test2",
				Value = "test2",
				GroupId = Guid.Empty,
				IsFile = false
			};

			_pathResolver.Setup(x => x.Resolve(It.IsAny<string>())).Returns(PathType.Group);
			_pathResolver.Setup(x => x.DeconstructPath(It.IsAny<string>())).Returns(("test", "test"));

			_secretsRepository.Setup(x => x.GetGroupSecrets(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Secret>
			{
				mockSecret,
				mockSecret2
			});

			// act
			var result = _target.GetSecrets("test", new[] { "test2" }).ToList();

			// assert
			Assert.AreEqual(result.Count, 1);
			Assert.AreEqual(result[0], mockSecret2);
		}

		[Test]
		public void GetSecretsGroupWithRandomNamesTest()
		{
			// arrange
			var mockSecret = new Secret
			{
				Id = Guid.NewGuid(),
				Name = "test",
				Value = "test",
				GroupId = Guid.Empty,
				IsFile = false
			};

			var mockSecret2 = new Secret
			{
				Id = Guid.NewGuid(),
				Name = "test2",
				Value = "test2",
				GroupId = Guid.Empty,
				IsFile = false
			};

			_pathResolver.Setup(x => x.Resolve(It.IsAny<string>())).Returns(PathType.Group);
			_pathResolver.Setup(x => x.DeconstructPath(It.IsAny<string>())).Returns(("test", "test"));

			_secretsRepository.Setup(x => x.GetGroupSecrets(It.IsAny<string>(), It.IsAny<string>())).Returns(new List<Secret>
			{
				mockSecret,
				mockSecret2
			});

			// act
			var result = _target.GetSecrets("test", new[] { "random" }).ToList();

			// assert
			Assert.AreEqual(result.Count, 0);
		}

		[Test]
		public void AddSecretsTest()
		{
			// arrange 
			const string fullPath = "path/to/group";
			const string name = "group";
			const string path = "path/to";

			var secrets = new Dictionary<string, string> { { "test", "test" } };
			var files = new Dictionary<string, string> { { "file", "file" } };

			_pathResolver.Setup(x => x.Resolve(It.IsAny<string>())).Returns(PathType.None);
			_pathResolver.Setup(x => x.DeconstructPath(It.IsAny<string>())).Returns((name, path));
			_groupRepository.Setup(x => x.CreateGroup(name, path, secrets, files)).Verifiable();

			// act
			_target.AddSecrets(fullPath, secrets, files, new string[] { });

			// assert
			_groupRepository.Verify(x => x.CreateGroup(name, path, secrets, files), Times.Once);
		}

		private ISecretsManager _target;
		private Mock<IPathResolver> _pathResolver;
		private Mock<ISecretsRepository> _secretsRepository;
		private Mock<IGroupRepository> _groupRepository;
	}
}