using Moq;

using NUnit.Framework;

using Ralfred.Common.DataAccess.Repositories;
using Ralfred.Common.Extensions;
using Ralfred.Common.Types;
using Ralfred.SecretsProvider.Helpers;


namespace SecretsProvider.UnitTests.Helpers
{
	[TestFixture]
	public class PathResolverTests
	{
		[SetUp]
		public void Setup()
		{
			_groupRepository = new Mock<IGroupRepository>();

			_target = new PathResolver(_groupRepository.Object);
		}

		[Test]
		public void Resolve_NotExistingSingleFolderPathTest()
		{
			// arrange
			const string path = "path";

			_groupRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);

			// act
			var result = _target.Resolve(path);

			// assert
			Assert.AreEqual(PathType.None, result);

			_groupRepository.Verify(x => x.Exists(It.IsAny<string>()), Times.Once);
		}

		[Test]
		public void Resolve_RandomPathTest()
		{
			// arrange
			const string path = "not/existing/path";

			_groupRepository.Setup(x => x.Exists(It.IsAny<string>())).Returns(false);

			// act
			var result = _target.Resolve(path);

			// assert
			Assert.AreEqual(PathType.None, result);

			_groupRepository.Verify(x => x.Exists(It.IsAny<string>()), Times.Exactly(2));
		}

		[Test]
		public void Resolve_PathToGroupTest()
		{
			// arrange
			const string path = "path";

			_groupRepository.Setup(x => x.Exists(path)).Returns(true);

			// act
			var result = _target.Resolve(path);

			// assert
			Assert.AreEqual(PathType.Group, result);

			_groupRepository.Verify(x => x.Exists(path), Times.Once);
		}

		[Test]
		public void Resolve_PathToSecretTest()
		{
			// arrange
			const string path = "path/to/folder";

			_groupRepository.Setup(x => x.Exists(path)).Returns(false);

			var (_, abovePath) = path.DeconstructPath();

			_groupRepository.Setup(x => x.Exists(abovePath)).Returns(true);

			// act
			var result = _target.Resolve(path);

			// assert
			Assert.AreEqual(PathType.Secret, result);

			_groupRepository.Verify(x => x.Exists(path), Times.Once);
			_groupRepository.Verify(x => x.Exists(abovePath), Times.Once);
		}

		private Mock<IGroupRepository> _groupRepository;

		private IPathResolver _target;
	}
}