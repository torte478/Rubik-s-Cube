using MagicCube;
using MagicCube.Movement;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class RubikCubeMovementRotate_Should
	{
		private RubikCube cube;

		[SetUp]
		public void SetUp()
		{
			cube = TestHelper.GetCompleteCube();
		}

		[Test]
		public void ChangeSides_AfterRotateToLeftFirstLayer()
		{
			var nextCube = cube.MakeRotation(TurnTo.Left, Layer.First);

			var expectedColors = new[]
			{
				CellColor.Orange, CellColor.Orange, CellColor.Orange,
				CellColor.Green, CellColor.Green, CellColor.Green,
				CellColor.Green, CellColor.Green, CellColor.Green
			};
			Assert.That(nextCube[SideIndex.Front].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void ChangeSides_AfterRotateToLeftThirdLayer()
		{
			var nextCube = cube.MakeRotation(TurnTo.Left, Layer.Third);

			var expectedColors = new[]
			{
				CellColor.Yellow, CellColor.Yellow, CellColor.Yellow,
				CellColor.Yellow, CellColor.Yellow, CellColor.Yellow,
				CellColor.Red, CellColor.Red, CellColor.Red
			};
			Assert.That(nextCube[SideIndex.Back].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void ChangeSides_AfterRotateToRightSecondLayer()
		{
			var nextCube = cube.MakeRotation(TurnTo.Right, Layer.Second);

			var expectedColors = new[]
			{
				CellColor.Orange, CellColor.Orange, CellColor.Orange,
				CellColor.Green, CellColor.Green, CellColor.Green,
				CellColor.Orange, CellColor.Orange, CellColor.Orange,
			};

			Assert.That(nextCube[SideIndex.Right].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void ChangeSides_AfterRotateToUpFirstLayer()
		{
			var nextCube = cube.MakeRotation(TurnTo.Up, Layer.First);

			var expectedColors = new[]
			{
				CellColor.Yellow, CellColor.Yellow, CellColor.White,
				CellColor.Yellow, CellColor.Yellow, CellColor.White,
				CellColor.Yellow, CellColor.Yellow, CellColor.White
			};
			Assert.That(nextCube[SideIndex.Back].Colors, Is.EqualTo(expectedColors));
		}

		[Test]
		public void ChangeSides_AfterRotateToDownThirdLayer()
		{
			var nextCube = cube.MakeRotation(TurnTo.Down, Layer.Third);

			var expecredColors = new[]
			{
				CellColor.Blue, CellColor.Blue, CellColor.Green,
				CellColor.Blue, CellColor.Blue, CellColor.Green,
				CellColor.Blue, CellColor.Blue, CellColor.Green
			};
			Assert.That(nextCube[SideIndex.Down].Colors, Is.EqualTo(expecredColors));
		}

		[Test]
		public void MakeClockwiseRotateOfTop_AfterRotateToLeftFirstLayer()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Top, 1, 1, CellColor.Red);

			var nextState = testCube.MakeRotation(TurnTo.Left, Layer.First);

			Assert.That(nextState[SideIndex.Top].GetColor(1, 3), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void MakeNotClockwiseRotateOfDown_AfterRotateToLeftThirdLayer()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Down, 1, 1, CellColor.White);

			var nextState = testCube.MakeRotation(TurnTo.Left, Layer.Third);

			Assert.That(nextState[SideIndex.Down].GetColor(3, 1), Is.EqualTo(CellColor.White));
		}
	}
}
