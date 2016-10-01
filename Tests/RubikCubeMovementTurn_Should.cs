using System;
using MagicCube;
using MagicCube.Movement;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class RubikCubeMovementTurn_Should
	{
		private RubikCube cube;

		[SetUp]
		public void SetUp()
		{
			cube = TestHelper.GetCompleteCube();
		}

		[Test]
		public void CycleShiftSides_AfterTurnToRight()
		{
			var nextCube = cube.MakeTurn(TurnTo.Right);

			Assert.That(nextCube[SideIndex.Front].IsFill(CellColor.Red), Is.True);
		}

		[Test]
		public void CycleShiftSides_AfterTurnToLeft()
		{
			var nextCube = cube.MakeTurn(TurnTo.Left);

			Assert.That(nextCube[SideIndex.Front].IsFill(CellColor.Orange), Is.True);
		}

		[Test]
		public void CycleShiftSides_AfterTurnToUp()
		{
			var nextCube = cube.MakeTurn(TurnTo.Up);

			Assert.That(nextCube[SideIndex.Front].IsFill(CellColor.Blue), Is.True);
		}

		[Test]
		public void MakeNotClockwiseTurnOfTop_AfterTurnToRight()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Top, 1, 1, CellColor.Red);

			var nextCube = testCube.MakeTurn(TurnTo.Right);

			Assert.That(nextCube[SideIndex.Top].GetColor(3, 1), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void MakeClockwiseTurnOfDown_AfterTurnToRight()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Down, 1, 1, CellColor.Green);

			var nextCube = testCube.MakeTurn(TurnTo.Right);

			Assert.That(nextCube[SideIndex.Down].GetColor(1, 3), Is.EqualTo(CellColor.Green));
		}

		[Test]
		public void MakeClockWiseTurnOfTop_AfterTurnToLeft()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Top, 1, 1, CellColor.Yellow);

			var nextCube = testCube.MakeTurn(TurnTo.Left);

			Assert.That(nextCube[SideIndex.Top].GetColor(1, 3), Is.EqualTo(CellColor.Yellow));
		}

		[Test]
		public void CorrectChangeTopSide_AfterTurnToDown()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Back, 1, 1, CellColor.Red);

			var nextCube = testCube.MakeTurn(TurnTo.Down);

			Assert.That(nextCube[SideIndex.Top].GetColor(3, 3), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void CorrectChangeBackSide_AfterTurnToUp()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Top, 1, 2, CellColor.Red);

			var nextCube = testCube.MakeTurn(TurnTo.Up);

			Assert.That(nextCube[SideIndex.Back].GetColor(3, 2), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void CorrectChangeDownSide_AfterTurnToUp()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Back, 2, 3, CellColor.Red);

			var nextCube = testCube.MakeTurn(TurnTo.Up);

			Assert.That(nextCube[SideIndex.Down].GetColor(2, 1), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void NotTurnTopSide_AfterTurnToUp()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Front, 1, 3, CellColor.Red);

			var nextCube = testCube.MakeTurn(TurnTo.Up);

			Assert.That(nextCube[SideIndex.Top].GetColor(1, 3), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void NotTurnDownSide_AfterTurnToDown()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Front, 1, 1, CellColor.Red);

			var nextCube = testCube.MakeTurn(TurnTo.Down);

			Assert.That(nextCube[SideIndex.Down].GetColor(1, 1), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void MakeClockwiseTurnOfFront_ForTurnToCornerRight()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Front, 1, 1, CellColor.White);

			var nextCube = testCube.MakeTurnToCorner(TurnTo.Right);

			Assert.That(nextCube[SideIndex.Front].GetColor(1, 3), Is.EqualTo(CellColor.White));
		}

		[Test]
		public void MakeNotClockwiseTurnOfFront_ForTurnToCornerLeft()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Front, 1, 1, CellColor.White);

			var nextCube = testCube.MakeTurnToCorner(TurnTo.Left);

			Assert.That(nextCube[SideIndex.Front].GetColor(3, 1), Is.EqualTo(CellColor.White));
		}

		[Test]
		[TestCase(TurnTo.Up, SideIndex.Top)]
		[TestCase(TurnTo.Down, SideIndex.Down)]
		public void MakeTurn_ForVerticalTurnToCorner(TurnTo turnTo, SideIndex sideIndex)
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Front, 2, 1, CellColor.Red);

			var nextCube = testCube.MakeTurnToCorner(turnTo);

			Assert.That(nextCube[sideIndex].GetColor(2, 1), Is.EqualTo(CellColor.Red));
		}
	}
}
