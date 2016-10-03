using System.Linq;
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
			cube = new RubikCube(
				new CubeSide(CellColor.Yellow),
				new CubeSide(CellColor.Yellow),
				new CubeSide(CellColor.Yellow),
				new CubeSide(CellColor.Yellow),
				new CubeSide(CellColor.Yellow),
				new CubeSide(CellColor.Yellow)
				);

			cube[SideIndex.Front].SetColor(CellColor.Green, 2, 2);
			cube[SideIndex.Top].SetColor(CellColor.White, 2, 2);
			cube[SideIndex.Right].SetColor(CellColor.Orange, 2, 2);
			cube[SideIndex.Back].SetColor(CellColor.Yellow, 2, 2);
			cube[SideIndex.Down].SetColor(CellColor.Blue, 2, 2);
			cube[SideIndex.Left].SetColor(CellColor.Red, 2, 2);
		}

		[Test]
		public void HaveStartState_WhenUpperMiddleOnRightPlace()
		{
			cube = cube
				.SetColor(SideIndex.Down, 1, 2, CellColor.White)
				.SetColor(SideIndex.Front, 3, 2, CellColor.Green);

			var solution = solver.MoveUpperMiddleToStart(cube);

			Assert.That(solution.GoalState[SideIndex.Down].GetColor(1, 2), Is.EqualTo(CellColor.White));
		}

		[Test]
		[TestCase(SideIndex.Down,  1, 2, SideIndex.Front, 3, 2, 0)]
		[TestCase(SideIndex.Down,  2, 3, SideIndex.Right, 3, 2, 1)]
		[TestCase(SideIndex.Right, 3, 2, SideIndex.Down,  2, 3, 1)]
		[TestCase(SideIndex.Down,  2, 1, SideIndex.Left,  3, 2, 1)]
		[TestCase(SideIndex.Top,   3, 2, SideIndex.Front, 1, 2, 1)]
		[TestCase(SideIndex.Right, 1, 2, SideIndex.Top,   2, 3, 1)]
		[TestCase(SideIndex.Left,  1, 2, SideIndex.Top,   2, 1, 1)]
		[TestCase(SideIndex.Back,  1, 2, SideIndex.Top,   1, 2, 3)]
		[TestCase(SideIndex.Front, 2, 3, SideIndex.Right, 2, 1, 1)]
		[TestCase(SideIndex.Front, 2, 1, SideIndex.Left,  2, 3, 1)]
		[TestCase(SideIndex.Back,  2, 1, SideIndex.Right, 2, 3, 2)]
		[TestCase(SideIndex.Back,  2, 3, SideIndex.Left,  2, 1, 2)]
		public void MoveUpperMiddle_ToStart(
			SideIndex firstSideIndex, int firstRow, int firstColumn, 
			SideIndex secondSideIndex, int secondRow, int secondColumn, 
			int expectedActionCount)
		{
			cube = cube
				.SetColor(firstSideIndex, firstRow, firstColumn, CellColor.White)
				.SetColor(secondSideIndex, secondRow, secondColumn, CellColor.Green);

			var solution = solver.MoveUpperMiddleToStart(cube);

			Assert.That(solution.Actions.Count, Is.EqualTo(expectedActionCount));
		}

		[Test]
		public void MoveUpperMiddleToPoint_WhenCorrectOriented()
		{
			cube = cube
				.SetColor(SideIndex.Front, 3, 2, CellColor.Green)
				.SetColor(SideIndex.Down, 1, 2, CellColor.White);

			var solution = solver.MoveUpperMiddleToPoint(cube);
			
			Assert.That(solution.Actions.Count, Is.EqualTo(2));
		}

		[Test]
		public void MoveUpperMiddleToPoint_WhenIncorrectOriented()
		{
			cube = cube
				.SetColor(SideIndex.Front, 3, 2, CellColor.White)
				.SetColor(SideIndex.Down, 1, 2, CellColor.Green);

			var solution = solver.MoveUpperMiddleToPoint(cube);

			Assert.That(solution.Actions.Count, Is.EqualTo(1));
		}

		[Test]
		public void DoNothing_WhenUpperMiddleOnPlace()
		{
			cube = cube
				.SetColor(SideIndex.Front, 1, 2, CellColor.Green)
				.SetColor(SideIndex.Top, 3, 2, CellColor.White);

			var solution = solver.SolveUpperMiddle(cube);

			Assert.That(solution.Actions.Count, Is.EqualTo(0));
		}

		[Test]
		public void SolveUpperMiddle_WhenUpperMiddleOnWrongPlace()
		{
			cube = cube
				.SetColor(SideIndex.Front, 2, 3, CellColor.White)
				.SetColor(SideIndex.Right, 2, 1, CellColor.Green);

			var solution = solver.SolveUpperMiddle(cube);

			Assert.That(solution.GoalState[SideIndex.Front].GetColor(1, 2), Is.EqualTo(CellColor.Green));
			Assert.That(solution.GoalState[SideIndex.Top].GetColor(3, 2), Is.EqualTo(CellColor.White));
		}

		[Test]
		public void SolveUpperCross()
		{
			var solution = solver.SolveUpperCross(TestHelper.GetNotSolvedCube());

			Assert.That(AlgorithmBase.IsSolvedUpperCross(solution.GoalState), Is.True);
		}

		[Test]
		public void ReturnCorrectActions_ForUpperCrossSolution()
		{
			var testCube = TestHelper.GetNotSolvedCube();

			var solution = solver.SolveUpperCross(testCube);

			testCube = solution.Actions.Aggregate(testCube, (current, solutionAction) => solutionAction.Execute(current));
			Assert.That(AlgorithmBase.IsSolvedUpperCross(testCube), Is.True);
		}
	}
}
