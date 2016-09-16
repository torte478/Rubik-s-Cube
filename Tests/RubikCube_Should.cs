using System;
using System.Linq;
using MagicCube;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class RubikCube_Should
	{
		private readonly CubeSide frontSide = new CubeSide(CellColor.Green);
		private readonly CubeSide topSide = new CubeSide(CellColor.White);
		private readonly CubeSide rightSide = new CubeSide(CellColor.Orange);
		private readonly CubeSide leftSide = new CubeSide(CellColor.Red);
		private readonly CubeSide backSide = new CubeSide(CellColor.Yellow);
		private readonly CubeSide downSide = new CubeSide(CellColor.Blue);

		private RubikCube cube;

		[SetUp]
		public void SetUp()
		{
			cube = new RubikCube(frontSide, topSide, rightSide, backSide, downSide, leftSide);
		}

		[Test]
		public void GetColor_ByTwoIndices()
		{
			Assert.That(cube.GetColor(Side.Top, 2, 2), Is.EqualTo(CellColor.White));
		}

		#region RollTests

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

		private static bool HasFilledSide(RubikCube rubikCube, Side side, CellColor color)
		{
			return rubikCube[side].Colors.All(currentColor => currentColor == color);
		}

		[Test]
		public void MakeNotClockwiseTurnOfTop_AfterRollToRight()
		{
			var testCube = CreateCubeWithConcreteCell(Side.Top, 1, 1, CellColor.Red);

			var nextCube = testCube.MakeRollTurn(TurnTo.Right);

			Assert.That(nextCube[Side.Top].GetColor(3, 1), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void MakeClockwiseTurnOfDown_AfterRollToRight()
		{
			var testCube = CreateCubeWithConcreteCell(Side.Down, 1, 1, CellColor.Green);

			var nextCube = testCube.MakeRollTurn(TurnTo.Right);

			Assert.That(nextCube[Side.Down].GetColor(1, 3), Is.EqualTo(CellColor.Green));
		}

		[Test]
		public void MakeClockWiseTurnOfTop_AfterRollToLeft()
		{
			var testCube = CreateCubeWithConcreteCell(Side.Top, 1, 1, CellColor.Yellow);

			var nextCube = testCube.MakeRollTurn(TurnTo.Left);

			Assert.That(nextCube[Side.Top].GetColor(1, 3), Is.EqualTo(CellColor.Yellow));
		}

		[Test]
		public void CorrectChangeTopSide_AfterRollToDown()
		{
			var testCube = CreateCubeWithConcreteCell(Side.Back, 1, 1, CellColor.Red);

			var nextCube = testCube.MakeRollTurn(TurnTo.Down);

			Assert.That(nextCube[Side.Top].GetColor(3, 3), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void CorrectChangeBackSide_AfterRollToUp()
		{
			var testCube = CreateCubeWithConcreteCell(Side.Top, 1, 2, CellColor.Red);

			var nextCube = testCube.MakeRollTurn(TurnTo.Up);

			Assert.That(nextCube[Side.Back].GetColor(3, 2), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void CorrectChangeDownSide_AfterRollToUp()
		{
			var testCube = CreateCubeWithConcreteCell(Side.Back, 2, 3, CellColor.Red);

			var nextCube = testCube.MakeRollTurn(TurnTo.Up);

			Assert.That(nextCube[Side.Down].GetColor(2, 1), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void NotTurnTopSide_AfterRollToUp()
		{
			var testCube = CreateCubeWithConcreteCell(Side.Front, 1, 3, CellColor.Red);

			var nextCube = testCube.MakeRollTurn(TurnTo.Up);

			Assert.That(nextCube[Side.Top].GetColor(1, 3), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void NotTurnDownSide_AfterRollToDown()
		{
			var testCube = CreateCubeWithConcreteCell(Side.Front, 1, 1, CellColor.Red);

			var nextCube = testCube.MakeRollTurn(TurnTo.Down);

			Assert.That(nextCube[Side.Down].GetColor(1, 1), Is.EqualTo(CellColor.Red));
		}

		private RubikCube CreateCubeWithConcreteCell(Side side, int row, int column, CellColor color)
		{
			var newSides = Enum.GetValues(typeof(Side)).Cast<Side>()
				.Select(curentSide => cube.CloneSide(curentSide))
				.ToArray();
			
			newSides[(int)side].SetColor(color, row, column);

			return new RubikCube(newSides[0], newSides[1], newSides[2], newSides[3], newSides[4], newSides[5]);
		}

		#endregion RollTests

		#region TurnTests

		[Test]
		public void ChangeSides_AfterTurnToLeftFirstLayer()
		{
			var nextCube = cube.MakeTurn(TurnTo.Left, Layer.First);

			var expectedColors = new[]
			{
				CellColor.Orange, CellColor.Orange, CellColor.Orange,
				CellColor.Green, CellColor.Green, CellColor.Green,
				CellColor.Green, CellColor.Green, CellColor.Green
			};
			Assert.That(nextCube[Side.Front].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void ChangeSides_AfterTurnToLeftThirdLayer()
		{
			var nextCube = cube.MakeTurn(TurnTo.Left, Layer.Third);

			var expectedColors = new[]
			{
				CellColor.Yellow, CellColor.Yellow, CellColor.Yellow,
				CellColor.Yellow, CellColor.Yellow, CellColor.Yellow,
				CellColor.Red, CellColor.Red, CellColor.Red
			};
			Assert.That(nextCube[Side.Back].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void ChangeSides_AfterTurnToRightSecondLayer()
		{
			var nextCube = cube.MakeTurn(TurnTo.Right, Layer.Second);

			var expectedColors = new[]
			{
				CellColor.Orange, CellColor.Orange, CellColor.Orange,
				CellColor.Green, CellColor.Green, CellColor.Green,
				CellColor.Orange, CellColor.Orange, CellColor.Orange,
			};

			Assert.That(nextCube[Side.Right].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void MakeClockwiseTurnOfFront_ForTurnToCornerRight()
		{
			var testCube = CreateCubeWithConcreteCell(Side.Front, 1, 1, CellColor.White);

			var nextCube = testCube.MakeTurnToCorner(TurnTo.Right);

			Assert.That(nextCube[Side.Front].GetColor(1, 3), Is.EqualTo(CellColor.White));
		}

		[Test]
		public void MakeNotClockwiseTurnOfFront_ForTurnToCornerLeft()
		{
			var testCube = CreateCubeWithConcreteCell(Side.Front, 1, 1, CellColor.White);

			var nextCube = testCube.MakeTurnToCorner(TurnTo.Left);

			Assert.That(nextCube[Side.Front].GetColor(3, 1), Is.EqualTo(CellColor.White));
		}

		[Test]
		public void ThrowArgumentOutOfRangeException_ForTurnToCornerUp()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				cube.MakeTurnToCorner(TurnTo.Up);
			});
		}

		[Test]
		public void ThrowArgumentOutOfRangeException_ForTurnToCornerDown()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				cube.MakeTurnToCorner(TurnTo.Down);
			});
		}

		[Test]
		public void ChangeSides_AfterTurnToUpFirstLayer()
		{
			var nextCube = cube.MakeTurn(TurnTo.Up, Layer.First);

			var expectedColors = new[]
			{
				CellColor.Yellow, CellColor.Yellow, CellColor.White,
				CellColor.Yellow, CellColor.Yellow, CellColor.White,
				CellColor.Yellow, CellColor.Yellow, CellColor.White
			};
			Assert.That(nextCube[Side.Back].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void ChangeSides_AfterTurnToDownThirdLayer()
		{
			var nextCube = cube.MakeTurn(TurnTo.Down, Layer.Third);

			var expecredColors = new[]
			{
				CellColor.Blue, CellColor.Blue, CellColor.Green, 
				CellColor.Blue, CellColor.Blue, CellColor.Green, 
				CellColor.Blue, CellColor.Blue, CellColor.Green
			};
			Assert.That(nextCube[Side.Down].Colors, Is.EqualTo(expecredColors));
		}
		
		#endregion TurnTests
	}
}
