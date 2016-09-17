using MagicCube;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class Side_Should
	{
		[Test]
		[TestCase(SideIndex.Front, 0)]
		[TestCase(SideIndex.Top, 1)]
		[TestCase(SideIndex.Right, 2)]
		[TestCase(SideIndex.Back, 3)]
		[TestCase(SideIndex.Down, 4)]
		[TestCase(SideIndex.Left, 5)]
		public void BeEqualTo_IntIndex(SideIndex side, int expectedIndex)
		{
			var actualIndex = (int) side;

			Assert.That(actualIndex, Is.EqualTo(expectedIndex));
		}
	}
}
