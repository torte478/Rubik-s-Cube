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
	}
}
