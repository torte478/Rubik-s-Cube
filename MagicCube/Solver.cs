using System;
using System.Collections.Generic;
using System.Linq;
using MagicCube.Movement;
using MagicCube.PathSearch;

namespace MagicCube
{
	public class Solver
	{
		private static SolutionItem FindSolution(RubikCube cube, CubeCommand[] commands, Func<RubikCube, bool> condition)
		{
			var searcher = new PathSearhcer(cube, commands, condition);

			return new SolutionItem
			{
				Actions = searcher.Path,
				GoalState = searcher.GoalState
			};
		}

		public SolutionItem MoveUpperMiddleToStart(RubikCube cube)
		{
			return FindSolution(
				cube,
				new[]
				{
					CommandFactory.GetRotation(TurnTo.Left, Layer.Third),
					CommandFactory.GetRotation(TurnTo.Right, Layer.Third),
					AlgorithmBase.MoveMiddleUpperFrontToLower,
					AlgorithmBase.MoveMiddleUpperRightToLower,
					AlgorithmBase.MoveMiddleUpperLeftToLower,
					AlgorithmBase.MoveMiddleUpperBackToLower,
					CommandFactory.GetClockwiseRotation(TurnTo.Right),
					CommandFactory.GetClockwiseRotation(TurnTo.Left),
					AlgorithmBase.MoveMiddleSecondRightToLower,
					AlgorithmBase.MoveMiddleSecondLeftToLower
				},
				AlgorithmBase.IsUpperMiddleOnStart);
		}

		public SolutionItem MoveUpperMiddleToPoint(RubikCube cube)
		{
			var action = AlgorithmBase.IsUpperMiddleOnStartCorrectOriented(cube)
				? CommandFactory.GetClockwiseRotation(TurnTo.Right)
				: AlgorithmBase.MoveIncorrectOrientedUpperMiddleToPoint;

			return FindSolution(
				cube,
				new[] { action },
				AlgorithmBase.IsUpperMiddleOnPoint);
		}

		public SolutionItem SolveUpperMiddle(RubikCube cube)
		{
			if (AlgorithmBase.IsUpperMiddleOnPoint(cube))
			{
				return new SolutionItem
				{
					Actions = new List<CubeCommand>(),
					GoalState = cube
				};
			}
			else
			{
				return FindAndMoveUpperMiddleToStart(cube);
			}
		}

		private SolutionItem FindAndMoveUpperMiddleToStart(RubikCube cube)
		{
			var moveToStartItem = MoveUpperMiddleToStart(cube);
			var moveToPointItem = MoveUpperMiddleToPoint(moveToStartItem.GoalState);

			return new SolutionItem
			{
				Actions = moveToStartItem.Actions.Concat(moveToPointItem.Actions).ToList(),
				GoalState = moveToPointItem.GoalState
			};
		}

		public SolutionItem SolveUpperCross(RubikCube cube)
		{
			var solveFrontItem = SolveUpperMiddle(cube);
			var solveRightItem = SolveUpperMiddle(solveFrontItem.GoalState.MakeTurn(TurnTo.Left));
			var solveBackItem = SolveUpperMiddle(solveRightItem.GoalState.MakeTurn(TurnTo.Left));
			var solveLeftItem = SolveUpperMiddle(solveBackItem.GoalState.MakeTurn(TurnTo.Left));

			return new SolutionItem
			{
				Actions = solveFrontItem.Actions
					.Concat(new[] { CommandFactory.GetTurn(TurnTo.Left) })
					.Concat(solveRightItem.Actions)
					.Concat(new[] { CommandFactory.GetTurn(TurnTo.Left) })
					.Concat(solveBackItem.Actions)
					.Concat(new[] { CommandFactory.GetTurn(TurnTo.Left) })
					.Concat(solveLeftItem.Actions)
					.ToList(),
				GoalState = solveLeftItem.GoalState
			};
		}
	}
}