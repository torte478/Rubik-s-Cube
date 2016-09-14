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
		public void ChangeSides_AfterTurnToLeftFirstLayer()
		{
			var expectedColors = new[]
			{
				CellColor.Orange, CellColor.Orange, CellColor.Orange,
				CellColor.Green, CellColor.Green, CellColor.Green,
				CellColor.Green, CellColor.Green, CellColor.Green
			};

			var nextState = cube.MakeTurn(TurnTo.Left, Layer.First);

			Assert.That(nextState[Side.Front].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void ChangeSides_AfterTurnToLeftThirdLayer()
		{
			var expectedColors = new[]
			{
				CellColor.Green, CellColor.Green, CellColor.Green,
				CellColor.Green, CellColor.Green, CellColor.Green,
				CellColor.Orange, CellColor.Orange, CellColor.Orange,
			};

			var nextState = cube.MakeTurn(TurnTo.Left, Layer.Third);

			Assert.That(nextState[Side.Front].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void ChangeSides_AfterTurnToRightSecondLayer()
		{
			var expectedColors = new[]
			{
				CellColor.Green, CellColor.Green, CellColor.Green,
				CellColor.Red, CellColor.Red, CellColor.Red,
				CellColor.Green, CellColor.Green, CellColor.Green
			};

			var nextState = cube.MakeTurn(TurnTo.Right, Layer.Second);

			Assert.That(nextState[Side.Front].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void ChangeSides_AfterTurnToUpFirstLayer()
		{
			var expectedColors = new[]
			{
				CellColor.Green, CellColor.White, CellColor.White,
				CellColor.Green, CellColor.White, CellColor.White,
				CellColor.Green, CellColor.White, CellColor.White,
			};

			var nextState = cube.MakeTurn(TurnTo.Up, Layer.First);

			Assert.That(nextState[Side.Top].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void ChangeSides_AfterTurnToDownSecondLayer()
		{
			var expectedColors = new[]
			{
				CellColor.Blue, CellColor.Green, CellColor.Blue,
				CellColor.Blue, CellColor.Green, CellColor.Blue,
				CellColor.Blue, CellColor.Green, CellColor.Blue,
			};

			var nextState = cube.MakeTurn(TurnTo.Down, Layer.Second);

			Assert.That(nextState[Side.Down].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void MoveToStartState_AfterFourEqualTurns()
		{
			var startState = cube
				.MakeTurn(TurnTo.Left, Layer.First)
				.MakeTurn(TurnTo.Right, Layer.Third);

			var finishState = startState
				.MakeTurn(TurnTo.Up, Layer.First)
				.MakeTurn(TurnTo.Up, Layer.First)
				.MakeTurn(TurnTo.Up, Layer.First)
				.MakeTurn(TurnTo.Up, Layer.First);

			Assert.That(finishState[Side.Front].Colors, Is.EqualTo(finishState[Side.Front].Colors));
		}
	}
}
