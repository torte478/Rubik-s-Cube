using MagicCube;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class CubeGenerator_Should
	{
		[Test]
		public void ReturnNotNull_FromGetRandomCube()
		{
			var generator = new CubeGenerator();

			var cube = generator.GetRandomCube();

			Assert.That(cube, Is.Not.Null);
		}

		[Test]
		public void ReturnNotNull_FromGetCompleteCube()
		{
			var generator = new CubeGenerator();

			var cube = generator.GetSolvedCube();

			Assert.That(cube, Is.Not.Null);
		}
	}
}
