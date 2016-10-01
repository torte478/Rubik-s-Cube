using System;
using MagicCube;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class SearchItem_Should
	{
		[Test]
		public void HaveRubikCubeState_AfterInitialization()
		{
			var item = new SearchItem(TestHelper.GetCompleteCube(), 4);

			Assert.That(item.State[SideIndex.Top].IsFill(CellColor.White));
		}

		[Test]
		public void HaveParentIndex_AfterInitialization()
		{
			var item = new SearchItem(TestHelper.GetCompleteCube(), 7);

			Assert.That(item.ParentIndex, Is.EqualTo(7));
		}

		[Test]
		public void ThrowArgumentOutOfRangeException_OnNegativeParentIndex()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var item = new SearchItem(TestHelper.GetCompleteCube(), -6);
			});
		}
	}
}
