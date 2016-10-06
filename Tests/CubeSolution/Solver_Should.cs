using System.Linq;
using MagicCube;
using MagicCube.CubeSolution;
using MagicCube.Movement;
using NUnit.Framework;

namespace Tests.CubeSolution
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
			FillCubeByOneColor();
			SetCenterColors();
		}

		private void FillCubeByOneColor()
		{
			cube = new RubikCube(
				new CubeSide(CellColor.Yellow),
				new CubeSide(CellColor.Yellow),
				new CubeSide(CellColor.Yellow),
				new CubeSide(CellColor.Yellow),
				new CubeSide(CellColor.Yellow),
				new CubeSide(CellColor.Yellow)
				);
		}

		private void SetCenterColors()
		{
			cube[SideIndex.Front].SetColor(CellColor.Green, 2, 2);
			cube[SideIndex.Top].SetColor(CellColor.White, 2, 2);
			cube[SideIndex.Right].SetColor(CellColor.Orange, 2, 2);
			cube[SideIndex.Back].SetColor(CellColor.Yellow, 2, 2);
			cube[SideIndex.Down].SetColor(CellColor.Blue, 2, 2);
			cube[SideIndex.Left].SetColor(CellColor.Red, 2, 2);
		}

		#region UpperLayerSolutionTests

		#region UpperCrossSolutionTests

		[Test]
		public void ReturnStartState_WhenUpperMiddleOnStart()
		{
			cube = cube
				.SetColor(SideIndex.Down, 1, 2, CellColor.White)
				.SetColor(SideIndex.Front, 3, 2, CellColor.Green);

			var solution = solver.MoveUpperMiddleToStart(cube);

			Assert.That(solution.GoalState[SideIndex.Down].GetColor(1, 2), Is.EqualTo(CellColor.White));
		}

		[Test]
		[TestCase(SideIndex.Down,  1, 2, SideIndex.Front, 3, 2)]
		[TestCase(SideIndex.Down,  2, 3, SideIndex.Right, 3, 2)]
		[TestCase(SideIndex.Right, 3, 2, SideIndex.Down,  2, 3)]
		[TestCase(SideIndex.Down,  2, 1, SideIndex.Left,  3, 2)]
		[TestCase(SideIndex.Top,   3, 2, SideIndex.Front, 1, 2)]
		[TestCase(SideIndex.Right, 1, 2, SideIndex.Top,   2, 3)]
		[TestCase(SideIndex.Left,  1, 2, SideIndex.Top,   2, 1)]
		[TestCase(SideIndex.Back,  1, 2, SideIndex.Top,   1, 2)]
		[TestCase(SideIndex.Front, 2, 3, SideIndex.Right, 2, 1)]
		[TestCase(SideIndex.Front, 2, 1, SideIndex.Left,  2, 3)]
		[TestCase(SideIndex.Back,  2, 1, SideIndex.Right, 2, 3)]
		[TestCase(SideIndex.Back,  2, 3, SideIndex.Left,  2, 1)]
		public void MoveUpperMiddle_ToStart(
			SideIndex firstSideIndex, int firstRow, int firstColumn, 
			SideIndex secondSideIndex, int secondRow, int secondColumn)
		{
			cube = cube
				.SetColor(firstSideIndex, firstRow, firstColumn, CellColor.White)
				.SetColor(secondSideIndex, secondRow, secondColumn, CellColor.Green);

			var solution = solver.MoveUpperMiddleToStart(cube);

            cube = solution.Actions.Aggregate(cube, (current, solutionAction) => solutionAction.Execute(current));
            Assert.That(AlgorithmBase.IsUpperMiddleOnStart(cube), Is.True);
		}

		[Test]
		public void MoveUpperMiddleToPoint_WhenCorrectOriented()
		{
			cube = cube
				.SetColor(SideIndex.Front, 3, 2, CellColor.Green)
				.SetColor(SideIndex.Down, 1, 2, CellColor.White);

			var solution = solver.MoveUpperMiddleFromStartToPoint(cube);
			
			Assert.That(solution.Actions.Count, Is.EqualTo(2));
		}

		[Test]
		public void MoveUpperMiddleToPoint_WhenIncorrectOriented()
		{
			cube = cube
				.SetColor(SideIndex.Front, 3, 2, CellColor.White)
				.SetColor(SideIndex.Down, 1, 2, CellColor.Green);

			var solution = solver.MoveUpperMiddleFromStartToPoint(cube);

			Assert.That(solution.Actions.Count, Is.EqualTo(1));
		}

		[Test]
		public void DoNothing_WhenUpperMiddleSolved()
		{
			cube = cube
				.SetColor(SideIndex.Front, 1, 2, CellColor.Green)
				.SetColor(SideIndex.Top, 3, 2, CellColor.White);

			var solution = solver.SolveUpperMiddle(cube);

			Assert.That(solution.Actions.Count, Is.EqualTo(0));
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

		#endregion

		#region UpperCornersSolutionTests

		[Test]
		public void ReturnStartState_WhenUpperCornerOnStart()
		{
			cube = cube
				.SetColor(SideIndex.Front, 3, 3, CellColor.Green)
				.SetColor(SideIndex.Down, 1, 3, CellColor.Orange)
				.SetColor(SideIndex.Right, 3, 1, CellColor.White);

			var solution = solver.MoveUpperCornerToStart(cube);

			Assert.That(solution.Actions.Count, Is.EqualTo(0));
		}

		[Test]
		[TestCase(SideIndex.Down, 3, 3, SideIndex.Right, 3, 3, SideIndex.Back, 3, 1)]
		[TestCase(SideIndex.Down, 1, 1, SideIndex.Left, 3, 3, SideIndex.Front, 3, 1)]
		[TestCase(SideIndex.Right, 1, 1, SideIndex.Front, 1, 3, SideIndex.Top, 3, 3)]
		[TestCase(SideIndex.Top, 1, 3, SideIndex.Back, 1, 1, SideIndex.Right, 1, 3)]
		[TestCase(SideIndex.Top, 1, 1, SideIndex.Left, 1, 1, SideIndex.Back, 1, 3)]
		[TestCase(SideIndex.Front, 1, 1, SideIndex.Left, 1, 3, SideIndex.Top, 3, 1)]
		public void MoveUpperCorner_ToStart(
			SideIndex firstSideIndex, int firstRow, int firstColumn,
			SideIndex secondSideIndex, int secondRow, int secondColumn,
			SideIndex thirdSideIndex, int thirdRow, int thirdColumn)
		{
			cube = cube
				.SetColor(firstSideIndex, firstRow, firstColumn, CellColor.Green)
				.SetColor(secondSideIndex, secondRow, secondColumn, CellColor.White)
				.SetColor(thirdSideIndex, thirdRow, thirdColumn, CellColor.Orange);

			var solution = solver.MoveUpperCornerToStart(cube);

            cube = solution.Actions.Aggregate(cube, (current, solutionAction) => solutionAction.Execute(current));
            Assert.That(AlgorithmBase.IsUpperCornerOnStart(cube), Is.True);
		}

		[Test]
		[TestCase(SideIndex.Down, 1, 3, SideIndex.Front, 3, 3, SideIndex.Right, 3, 1, 1)]
		[TestCase(SideIndex.Front, 3, 3, SideIndex.Right, 3, 1, SideIndex.Down, 1, 3, 1)]
		[TestCase(SideIndex.Right, 3, 1, SideIndex.Down, 1, 3, SideIndex.Front, 3, 3, 2)]
		public void MoveUpperCornerToPoint_FromStart(
			SideIndex firstSideIndex, int firstRow, int firstColumn,
			SideIndex secondSideIndex, int secondRow, int secondColumn,
			SideIndex thirdSideIndex, int thirdRow, int thirdColumn,
			int expectedActionCount)
		{
			cube = cube
				.SetColor(firstSideIndex, firstRow, firstColumn, CellColor.Green)
				.SetColor(secondSideIndex, secondRow, secondColumn, CellColor.White)
				.SetColor(thirdSideIndex, thirdRow, thirdColumn, CellColor.Orange);

			var solution = solver.MoveUpperCornerFromStartToPoint(cube);

			Assert.That(solution.Actions.Count, Is.EqualTo(expectedActionCount));
		}

		[Test]
		public void DoNothing_WhenUpperCornerSolved()
		{
			cube = cube
				.SetColor(SideIndex.Front, 1, 3, CellColor.Green)
				.SetColor(SideIndex.Top, 3, 3, CellColor.White)
				.SetColor(SideIndex.Right, 1, 1, CellColor.Orange);

			var solution = solver.SolveUpperCorner(cube);

			Assert.That(solution.Actions.Count, Is.EqualTo(0));
		}

		[Test]
		public void SolveUpperCorners()
		{
			var solution = solver.SolveUpperCorners(TestHelper.GetNotSolvedCube());

			Assert.That(AlgorithmBase.IsSolvedUpperCorners(solution.GoalState), Is.True);
		}

		[Test]
		public void ReturnCorrectActions_ForUpperCornersSolution()
		{
			var testCube = TestHelper.GetNotSolvedCube();

			var solution = solver.SolveUpperCorners(testCube);

			testCube = solution.Actions.Aggregate(testCube, (current, solutionAction) => solutionAction.Execute(current));
			Assert.That(AlgorithmBase.IsSolvedUpperCorners(testCube), Is.True);
		}

		#endregion

		[Test]
		public void SolveUpperLayer()
		{
			var solution = solver.SolveUpperLayer(TestHelper.GetNotSolvedCube());

			Assert.That(AlgorithmBase.IsSolvedUpperLayer(solution.GoalState), Is.True);
		}

		[Test]
		public void ReturnCorrectActions_ForUpperLayerSolution()
		{
			var testCube = TestHelper.GetNotSolvedCube();

			var solution = solver.SolveUpperLayer(testCube);

			testCube = solution.Actions.Aggregate(testCube, (current, solutionAction) => solutionAction.Execute(current));
			Assert.That(AlgorithmBase.IsSolvedUpperLayer(testCube), Is.True);
		}

		#endregion

		#region MiddleLayerSolutionTests

		[Test]
        public void ReturnStartState_WhenMiddleMiddleOnStart()
        {
            cube = cube
                .SetColor(SideIndex.Front, 3, 2, CellColor.Green)
                .SetColor(SideIndex.Down, 1, 2, CellColor.Orange);

            var solution = solver.MoveMiddleMiddleToStart(cube);

            Assert.That(solution.Actions.Count, Is.EqualTo(0));
        }

        [Test]
        [TestCase(SideIndex.Right, 3, 2, SideIndex.Down, 2, 3)]
        [TestCase(SideIndex.Right, 2, 1, SideIndex.Front, 2, 3)]
        [TestCase(SideIndex.Right, 2, 3, SideIndex.Back, 2, 1)]
        public void MoveMiddleMiddle_ToStart(
            SideIndex firstSideIndex, int firstRow, int firstColumn,
            SideIndex secondSideIndex, int secondRow, int secondColumn)
        {
            cube = cube
                .SetColor(firstSideIndex, firstRow, firstColumn, CellColor.Green)
                .SetColor(secondSideIndex, secondRow, secondColumn, CellColor.Orange);

            var solution = solver.MoveMiddleMiddleToStart(cube);

            cube = solution.Actions.Aggregate(cube, (current, solutionAction) => solutionAction.Execute(current));
            Assert.That(AlgorithmBase.IsMiddleMiddleOnStart(cube), Is.True);
        }

        [Test]
        public void MoveMiddleMiddleToPoint_WhenCorrectOriented()
        {
            cube = cube
                .SetColor(SideIndex.Front, 3, 2, CellColor.Green)
                .SetColor(SideIndex.Down, 1, 2, CellColor.Orange);

            var solution = solver.MoveMiddleMiddleFromStartToPoint(cube);

            Assert.That(solution.Actions.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void MoveMiddleMiddleToPoint_WhenIncorrectOriented()
        {
            cube = cube
                .SetColor(SideIndex.Front, 3, 2, CellColor.Orange)
                .SetColor(SideIndex.Down, 1, 2, CellColor.Green);

            var solution = solver.MoveMiddleMiddleFromStartToPoint(cube);

            Assert.That(solution.Actions.Count, Is.EqualTo(1));
        }

		[Test]
		public void SolveMiddleLayer()
		{
			var cubeWithSolvedUpperLayer = solver.SolveUpperLayer(TestHelper.GetNotSolvedCube()).GoalState;

			var solution = solver.SolveMiddleLayer(cubeWithSolvedUpperLayer);

			Assert.That(AlgorithmBase.IsSolvedMiddleMiddle(solution.GoalState), Is.True);
		}

		[Test]
		public void ReturnCorrectActions_ForMiddleLayerSolution()
		{
			var startCube = solver.SolveUpperLayer(TestHelper.GetNotSolvedCube()).GoalState;

			var solution = solver.SolveMiddleLayer(startCube);

			var testCube = solution.Actions.Aggregate(startCube, (current, solutionAction) => solutionAction.Execute(current));
			Assert.That(AlgorithmBase.IsSolvedMiddleMiddle(testCube), Is.True);
		}

		#endregion

		#region LowerCrossSolutionTests

		[Test]
		public void ReturnStartState_WhenLowerCrossOnStart()
		{
			cube = cube.MakeTurn(TurnTo.Up).MakeTurn(TurnTo.Up)
				.SetColor(SideIndex.Front, 1, 2, CellColor.Yellow).SetColor(SideIndex.Top, 3, 2, CellColor.Blue)
				.SetColor(SideIndex.Right, 1, 2, CellColor.Orange).SetColor(SideIndex.Top, 2, 3, CellColor.Blue)
				.SetColor(SideIndex.Back, 1, 2, CellColor.Green).SetColor(SideIndex.Top, 1, 2, CellColor.Blue)
				.SetColor(SideIndex.Left, 1, 2, CellColor.Red).SetColor(SideIndex.Top, 2, 1, CellColor.Blue);

			var solution = solver.MoveLowerCrossToStart(cube);

			Assert.That(solution.Actions.Count, Is.EqualTo(0));
		}

		[Test]
		public void MoveLowerCross_ToStart()
		{
			var solution = solver.MoveLowerCrossToStart(GetCubeWithTwoSolvedLayers());

			Assert.That(AlgorithmBase.IsLowerCrossOnStart(solution.GoalState), Is.True);
		}

		private RubikCube GetCubeWithTwoSolvedLayers()
		{
			var cubeWithSolvedUpperLayer = solver.SolveUpperLayer(TestHelper.GetNotSolvedCube()).GoalState;
			var cubeWithSolvedMiddleLayer = solver.SolveMiddleLayer(cubeWithSolvedUpperLayer).GoalState;

			return cubeWithSolvedMiddleLayer.MakeTurn(TurnTo.Up).MakeTurn(TurnTo.Up);
		}

		[Test]
		public void SolveLowerCross()
		{
			var solution = solver.SolveLowerCross(GetCubeWithTwoSolvedLayers());

			Assert.That(AlgorithmBase.IsSolvedLowerCross(solution.GoalState), Is.True);
		}

		[Test]
		public void ReturnCorrectActions_ForLowerCrossSolution()
		{
			var testCube = GetCubeWithTwoSolvedLayers();

			var solution = solver.SolveLowerCross(testCube);

			testCube = solution.Actions.Aggregate(testCube, (current, solutionAction) => solutionAction.Execute(current));
			Assert.That(AlgorithmBase.IsSolvedLowerCross(testCube), Is.True);
		}

		#endregion

		#region LowerCornersSolutionTests

		[Test]
		public void ReturnStartState_WhenLowerCornersOnStart()
		{
			cube = cube.MakeTurn(TurnTo.Up).MakeTurn(TurnTo.Up)
				.SetColor(SideIndex.Back, 1, 2, CellColor.Red)
				.SetColor(SideIndex.Left, 1, 2, CellColor.Yellow)
				.SetColor(SideIndex.Top, 2, 2, CellColor.Blue)
				.SetColor(SideIndex.Top, 1, 1, CellColor.Red)
				.SetColor(SideIndex.Back, 1, 3, CellColor.Blue)
				.SetColor(SideIndex.Left, 1, 1, CellColor.Yellow);

			var solution = solver.MoveLowerCornersToStart(cube);

			Assert.That(solution.Actions.Count, Is.EqualTo(0));
		}

		[Test]
		public void MoveLowerCorners_ToStart()
		{
			var solution = solver.MoveLowerCornersToStart(
				solver.SolveLowerCross(GetCubeWithTwoSolvedLayers()).GoalState);

			Assert.That(AlgorithmBase.IsAllLowerCornersOnStart(solution.GoalState), Is.True);
		}

		[Test]
		public void MoveLowerCorners_ToPoint()
		{
			var solution = solver.MoveLowerCornersToPoint(
				solver.MoveLowerCornersToStart(
				   solver.SolveLowerCross(GetCubeWithTwoSolvedLayers())
				   .GoalState)
				.GoalState);

			Assert.That(AlgorithmBase.IsAllLowerCornersOnPoint(solution.GoalState), Is.True);
		}

		#endregion
	}
}
