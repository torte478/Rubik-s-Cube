using MagicCube;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class Solver_Should
	{
		private Solver solver;
		private RubikCube cube;

		[SetUp]
		public void SetUp()
		{
			solver = new Solver();
			cube = TestHelper.GetCompleteCube();
		}

		[Test]
		public void FindEmptySolution_WhenUpperMiddleOnRightPlace()
		{
			cube = cube.SetColor(SideIndex.Down, 1, 2, CellColor.White);

			var solution = solver.FindAndReplaceUpperMiddleToStartPonint(cube);

			Assert.That(solution.Count, Is.EqualTo(0));
		}

		[Test]
		public void FindSolutionFromOneElement_WhenUpperMiddleOnLowerRight()
		{
			cube = cube
				.SetColor(SideIndex.Right, 3, 2, CellColor.Green)
				.SetColor(SideIndex.Down, 2, 3, CellColor.White);

			var solution = solver.FindAndReplaceUpperMiddleToStartPonint(cube);

			Assert.That(solution.Count, Is.EqualTo(1));
		}

		[Test]
		public void FindSolutionFromOneElement_WhenUpperMiddleOnLowerRightAndRotated()
		{
			cube = cube
				.SetColor(SideIndex.Right, 3, 2, CellColor.White)
				.SetColor(SideIndex.Down, 2, 3, CellColor.Green);

			var solution = solver.FindAndReplaceUpperMiddleToStartPonint(cube);

			Assert.That(solution.Count, Is.EqualTo(1));
		}

		[Test]
		public void FindSolutionFromOneElement_WhenUpperMiddleOnLowerLeft()
		{
			cube = cube
				.SetColor(SideIndex.Left, 3, 2, CellColor.Green)
				.SetColor(SideIndex.Down, 2, 1, CellColor.White);

			var solution = solver.FindAndReplaceUpperMiddleToStartPonint(cube);

			Assert.That(solution.Count, Is.EqualTo(1));
		}

		[Test]
		public void FindSolutionFromOneElement_WhenUpperMiddleOnUpperMiddle()
		{
			cube = cube
				.SetColor(SideIndex.Top, 3, 2, CellColor.White)
				.SetColor(SideIndex.Front, 1, 2, CellColor.Green);

			var solution = solver.FindAndReplaceUpperMiddleToStartPonint(cube);

			Assert.That(solution.Count, Is.EqualTo(1));
		}
	}
}
