using FluentAssertions;

using NUnit.Framework;

using Ralfred.Common.DataAccess.Helpers;


namespace SecretsProvider.UnitTests.DataAccess.Helpers
{
	[TestFixture]
	public class TransactionFactoryTests
	{
		[SetUp]
		public void Setup()
		{
			_target = new TransactionFactory();
		}

		[Test]
		public void BeginTransactionTest()
		{
			// act
			var result = _target.BeginTransaction();

			// assert
			result.Should().NotBeNull();
		}

		private TransactionFactory _target;
	}
}