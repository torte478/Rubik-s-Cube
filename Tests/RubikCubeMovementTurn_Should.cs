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
		public void ChangeSides_AfterTurnToLeftFirstLayer()
		{
			var nextCube = cube.MakeTurn(TurnTo.Left, Layer.First);

			var expectedColors = new[]
			{
				CellColor.Orange, CellColor.Orange, CellColor.Orange,
				CellColor.Green, CellColor.Green, CellColor.Green,
				CellColor.Green, CellColor.Green, CellColor.Green
			};
			Assert.That(nextCube[SideIndex.Front].Colors, Is.EqualTo(expectedColors));
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
			Assert.That(nextCube[SideIndex.Back].Colors, Is.EqualTo(expectedColors));
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

			Assert.That(nextCube[SideIndex.Right].Colors, Is.EqualTo(expectedColors));
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
			Assert.That(nextCube[SideIndex.Back].Colors, Is.EqualTo(expectedColors));
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
			Assert.That(nextCube[SideIndex.Down].Colors, Is.EqualTo(expecredColors));
		}

		[Test]
		public void MakeClockwiseTurnOfTop_AfterTurnToLeftFirstLayer()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Top, 1, 1, CellColor.Red);

			var nextState = testCube.MakeTurn(TurnTo.Left, Layer.First);

			Assert.That(nextState[SideIndex.Top].GetColor(1, 3), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void MakeNotClockwiseTurnOfDown_AfterTurnToLeftThirdLayer()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Down, 1, 1, CellColor.White);

			var nextState = testCube.MakeTurn(TurnTo.Left, Layer.Third);

			Assert.That(nextState[SideIndex.Down].GetColor(3, 1), Is.EqualTo(CellColor.White));
		}
	}
}
