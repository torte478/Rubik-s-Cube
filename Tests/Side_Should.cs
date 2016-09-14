using MagicCube;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class Side_Should
	{
		[Test]
		[TestCase(Side.Front, 0)]
		[TestCase(Side.Top, 1)]
		[TestCase(Side.Right, 2)]
		[TestCase(Side.Back, 3)]
		[TestCase(Side.Down, 4)]
		[TestCase(Side.Left, 5)]
		public void BeEqualTo_IntIndex(Side side, int expectedIndex)
		{
			var actualIndex = (int) side;

			Assert.That(actualIndex, Is.EqualTo(expectedIndex));
		}
	}
}
