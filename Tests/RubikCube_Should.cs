using MagicCube;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class RubikCube_Should
	{
		private RubikCube cube;

		[SetUp]
		public void SetUp()
		{
			cube = TestHelper.GetCompleteCube();
		}

		[Test]
		public void GetColor_ByTwoIndices()
		{
			Assert.That(cube.GetColor(SideIndex.Top, 2, 2), Is.EqualTo(CellColor.White));
		}
	}
}
