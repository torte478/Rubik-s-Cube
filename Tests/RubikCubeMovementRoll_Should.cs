using System;
using System.Linq;
using MagicCube;
using MagicCube.Movement;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class RubikCubeMovementRoll_Should
	{
		private RubikCube cube;

		[SetUp]
		public void SetUp()
		{
			cube = TestHelper.GetCompleteCube();
		}

		private static bool HasFilledSide(RubikCube rubikCube, Side side, CellColor color)
		{
			return rubikCube[side].Colors.All(currentColor => currentColor == color);
		}

		[Test]
		public void CycleShiftSides_AfterRollToRight()
		{
			var nextCube = cube.MakeRollTurn(TurnTo.Right);

			Assert.That(HasFilledSide(nextCube, Side.Front, CellColor.Red), Is.True);
		}

		[Test]
		public void CycleShiftSides_AfterRollToLeft()
		{
			var nextCube = cube.MakeRollTurn(TurnTo.Left);

			Assert.That(HasFilledSide(nextCube, Side.Front, CellColor.Orange), Is.True);
		}

		[Test]
		public void CycleShiftSides_AfterRollToUp()
		{
			var nextCube = cube.MakeRollTurn(TurnTo.Up);

			Assert.That(HasFilledSide(nextCube, Side.Front, CellColor.Blue), Is.True);
		}

		[Test]
		public void MakeNotClockwiseTurnOfTop_AfterRollToRight()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(Side.Top, 1, 1, CellColor.Red);

			var nextCube = testCube.MakeRollTurn(TurnTo.Right);

			Assert.That(nextCube[Side.Top].GetColor(3, 1), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void MakeClockwiseTurnOfDown_AfterRollToRight()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(Side.Down, 1, 1, CellColor.Green);

			var nextCube = testCube.MakeRollTurn(TurnTo.Right);

			Assert.That(nextCube[Side.Down].GetColor(1, 3), Is.EqualTo(CellColor.Green));
		}

		[Test]
		public void MakeClockWiseTurnOfTop_AfterRollToLeft()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(Side.Top, 1, 1, CellColor.Yellow);

			var nextCube = testCube.MakeRollTurn(TurnTo.Left);

			Assert.That(nextCube[Side.Top].GetColor(1, 3), Is.EqualTo(CellColor.Yellow));
		}

		[Test]
		public void CorrectChangeTopSide_AfterRollToDown()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(Side.Back, 1, 1, CellColor.Red);

			var nextCube = testCube.MakeRollTurn(TurnTo.Down);

			Assert.That(nextCube[Side.Top].GetColor(3, 3), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void CorrectChangeBackSide_AfterRollToUp()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(Side.Top, 1, 2, CellColor.Red);

			var nextCube = testCube.MakeRollTurn(TurnTo.Up);

			Assert.That(nextCube[Side.Back].GetColor(3, 2), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void CorrectChangeDownSide_AfterRollToUp()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(Side.Back, 2, 3, CellColor.Red);

			var nextCube = testCube.MakeRollTurn(TurnTo.Up);

			Assert.That(nextCube[Side.Down].GetColor(2, 1), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void NotTurnTopSide_AfterRollToUp()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(Side.Front, 1, 3, CellColor.Red);

			var nextCube = testCube.MakeRollTurn(TurnTo.Up);

			Assert.That(nextCube[Side.Top].GetColor(1, 3), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void NotTurnDownSide_AfterRollToDown()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(Side.Front, 1, 1, CellColor.Red);

			var nextCube = testCube.MakeRollTurn(TurnTo.Down);

			Assert.That(nextCube[Side.Down].GetColor(1, 1), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void MakeClockwiseTurnOfFront_ForRollToCornerRight()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(Side.Front, 1, 1, CellColor.White);

			var nextCube = testCube.MakeRollToCorner(TurnTo.Right);

			Assert.That(nextCube[Side.Front].GetColor(1, 3), Is.EqualTo(CellColor.White));
		}

		[Test]
		public void MakeNotClockwiseTurnOfFront_ForRollToCornerLeft()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(Side.Front, 1, 1, CellColor.White);

			var nextCube = testCube.MakeRollToCorner(TurnTo.Left);

			Assert.That(nextCube[Side.Front].GetColor(3, 1), Is.EqualTo(CellColor.White));
		}

		[Test]
		public void ThrowArgumentOutOfRangeException_ForRollToCornerUp()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				cube.MakeRollToCorner(TurnTo.Up);
			});
		}

		[Test]
		public void ThrowArgumentOutOfRangeException_ForRollToCornerDown()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				cube.MakeRollToCorner(TurnTo.Down);
			});
		}
	}
}
