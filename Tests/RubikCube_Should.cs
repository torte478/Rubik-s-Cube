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
		[TestCase(Side.Front, CellColor.Green)]
		[TestCase(Side.Top, CellColor.White)]
		[TestCase(Side.Right, CellColor.Orange)]
		[TestCase(Side.Back, CellColor.Yellow)]
		[TestCase(Side.Down, CellColor.Blue)]
		[TestCase(Side.Left, CellColor.Red)]
		public void HaveCorrectSide_AfterInitialization(Side side, CellColor sideColor)
		{
			Assert.That(cube[side].Colors.All(c => c == sideColor), Is.True);
		}

		[Test]
		public void GetColor_ByTwoIndices()
		{
			var newTopSide = cube.CloneSide(Side.Top);
			newTopSide.Colors[4] = CellColor.Red;
			var newCube = new RubikCube(frontSide, newTopSide, rightSide, backSide, downSide, leftSide);

			Assert.That(newCube.GetColor(Side.Top, 2, 2), Is.EqualTo(CellColor.Red));
		}

		#region RollTests

		[Test]
		public void CycleShiftWallSides_AfterRollToRight()
		{
			var nextState = cube.MakeRollTurn(TurnTo.Right);

			Assert.That(nextState[Side.Front].Colors.All(color => color == CellColor.Red),
				Is.True);
		}

		[Test]
		public void CycleShiftWallSides_AfterRollToLeft()
		{
			var nextState = cube.MakeRollTurn(TurnTo.Left);

			Assert.That(nextState[Side.Front].Colors.All(color => color == CellColor.Orange),
				Is.True);
		}

		[Test]
		public void MakeNotClockwiseTurnOfTop_AfterRollToRight()
		{
			const CellColor color = CellColor.Red;
			const Side side = Side.Top;

			var newTopSide = cube.CloneSide(side);
			newTopSide.SetColor(color, 1, 1);
			var newCube = new RubikCube(frontSide, newTopSide, rightSide, backSide, downSide, leftSide);

			var nextState = newCube.MakeRollTurn(TurnTo.Right);

			Assert.That(nextState[side].GetColor(3, 1), Is.EqualTo(color));
		}

		[Test]
		public void MakeClockwiseTurnOfDown_AfterRollToRight()
		{
			const CellColor color = CellColor.Green;
			const Side side = Side.Down;
			
			var newDownSide = cube.CloneSide(side);
			newDownSide.SetColor(color, 1, 1);
			var newCube = new RubikCube(frontSide, topSide, rightSide, backSide, newDownSide, leftSide);

			var nextState = newCube.MakeRollTurn(TurnTo.Right);

			Assert.That(nextState[side].GetColor(1, 3), Is.EqualTo(color));
		}

		[Test]
		public void MakeClockWiseTurnOfTop_AfterRollToLeft()
		{
			const CellColor color = CellColor.Yellow;
			const Side side = Side.Top;

			var newTopSide = cube.CloneSide(side);
			newTopSide.SetColor(color, 1, 1);
			var newCube = new RubikCube(frontSide, newTopSide, rightSide, backSide, downSide, leftSide);

			var nextState = newCube.MakeRollTurn(TurnTo.Left);

			Assert.That(nextState[side].GetColor(1, 3), Is.EqualTo(color));
		}

		#endregion RollTests

		#region TurnTests

		[Test]
		public void ChangeSides_AfterTurnToLeftFirstLayer()
		{
			var nextState = cube.MakeTurn(TurnTo.Left, Layer.First);

			var expectedColors = new[]
			{
				CellColor.Orange, CellColor.Orange, CellColor.Orange,
				CellColor.Green, CellColor.Green, CellColor.Green,
				CellColor.Green, CellColor.Green, CellColor.Green
			};
			Assert.That(nextState[Side.Front].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void ChangeSides_AfterTurnToLeftThirdLayer()
		{
			var nextState = cube.MakeTurn(TurnTo.Left, Layer.Third);

			var expectedColors = new[]
			{
				CellColor.Yellow, CellColor.Yellow, CellColor.Yellow,
				CellColor.Yellow, CellColor.Yellow, CellColor.Yellow,
				CellColor.Red, CellColor.Red, CellColor.Red
			};
			Assert.That(nextState[Side.Back].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void ChangeSides_AfterTurnToRightSecondLayer()
		{
			var nextState = cube.MakeTurn(TurnTo.Right, Layer.Second);

			var expectedColors = new[]
			{
				CellColor.Orange, CellColor.Orange, CellColor.Orange,
				CellColor.Green, CellColor.Green, CellColor.Green,
				CellColor.Orange, CellColor.Orange, CellColor.Orange,
			};

			Assert.That(nextState[Side.Right].Colors, Is.EqualTo(expectedColors));
		}

		//[Test]
		//public void ChangeSides_AfterTurnToUpFirstLayer()
		//{
		//	var expectedColors = new CellColor[]
		//	{
		//		CellColor.Yellow, CellColor.Yellow, CellColor.White, 
		//		CellColor.Yellow, CellColor.Yellow, CellColor.White, 
		//		CellColor.Yellow, CellColor.Yellow, CellColor.White
		//	};

		//	var nextState = cube.MakeTurn(TurnTo.Up, Layer.First);

		//	Assert.That(nextState[Side.Back].Colors, Is.EqualTo(expectedColors));
		//}

		//		[Test]
		//		public void ChangeSides_AfterTurnToDownSecondLayer()
		//		{
		//			var expectedColors = new[]
		//			{
		//				CellColor.Blue, CellColor.Green, CellColor.Blue,
		//				CellColor.Blue, CellColor.Green, CellColor.Blue,
		//				CellColor.Blue, CellColor.Green, CellColor.Blue,
		//			};
		//
		//			var nextState = cube.MakeTurn(TurnTo.Down, Layer.Second);
		//
		//			Assert.That(nextState[Side.Down].Colors, Is.EqualTo(expectedColors));
		//		}

		#endregion TurnTests
	}
}
