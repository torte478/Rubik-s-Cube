using System;
using MagicCube;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class PathSearcher_Should
	{
		[Test]
		public void ThrowArgumentOutOfRangeException_WhenStartCubeIsNull()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var searcher = new PathSearhcer(null, new CubeCommand[0], x => true);
			});
		}

		[Test]
		public void ReturnEmptyPath_WhenStartPositionRespondsToFinishCondition()
		{
			var searcher = new PathSearhcer(TestHelper.GetCompleteCube(), new CubeCommand[0], x => true);

			Assert.That(searcher.GetPath().Count, Is.EqualTo(0));
		}

//		[Test]
//		public void ReturnPathFromOneCommand_ForSimpleCase()
//		{
//			var startCube = TestHelper.GetCompleteCube().MakeRotation(TurnTo.Left, Layer.First);
//			var searcher = new PathSearhcer(startCube, new CubeCommand[] {new CommandFactory.GetRotation()}, x => true); //TODO
//
//			Assert.That(searcher.GetPath().Count, Is.EqualTo(1));
//		}
	}
}
