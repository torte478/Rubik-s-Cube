using MagicCube;
using MagicCube.Movement;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class RubikCubeMovementRotation_Should
	{
		private RubikCube cube;

		[SetUp]
		public void SetUp()
		{
			cube = TestHelper.GetCompleteCube();
		}

		[Test]
		public void ChangeSides_AfterRotationToLeftFirstLayer()
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
		public void ChangeSides_AfterRotationToLeftThirdLayer()
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
		public void ChangeSides_AfterRotationToRightSecondLayer()
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
		public void ChangeSides_AfterRotationToUpFirstLayer()
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
		public void ChangeSides_AfterRotationToDownThirdLayer()
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
		public void MakeClockwiseRotationOfTop_AfterRotationToLeftFirstLayer()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Top, 1, 1, CellColor.Red);

			var nextState = testCube.MakeRotation(TurnTo.Left, Layer.First);

			Assert.That(nextState[SideIndex.Top].GetColor(1, 3), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void MakeNotClockwiseRotationOfDown_AfterRotationToLeftThirdLayer()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Down, 1, 1, CellColor.White);

			var nextState = testCube.MakeRotation(TurnTo.Left, Layer.Third);

			Assert.That(nextState[SideIndex.Down].GetColor(3, 1), Is.EqualTo(CellColor.White));
		}

		[Test]
		public void NotMakeRotationOfUp_AfterRotationOfSecondLayer()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Top, 1, 1, CellColor.Red);

			var nextState = testCube.MakeRotation(TurnTo.Right, Layer.Second);

			Assert.That(nextState[SideIndex.Top].GetColor(1, 1), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void NotMakeRotationOfDown_AfterRotationOfFirstLayer()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Down, 1, 1, CellColor.Red);

			var nextState = testCube.MakeRotation(TurnTo.Left, Layer.First);

			Assert.That(nextState[SideIndex.Down].GetColor(1, 1), Is.EqualTo(CellColor.Red));
		}

		[Test]
		public void NotMakeRotationOfRight_AfterRotationOFFirstLayer()
		{
			var testCube = TestHelper.GetCubeWithConcreteCell(SideIndex.Right, 1, 1, CellColor.White);

			var nextState = testCube.MakeRotation(TurnTo.Up, Layer.First);

			Assert.That(nextState[SideIndex.Right].GetColor(1, 1), Is.EqualTo(CellColor.White));
		}
	}
}
