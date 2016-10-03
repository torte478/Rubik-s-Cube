using System;
using System.Collections.Generic;
using System.Linq;
using MagicCube.Movement;
using MagicCube.PathSearch;

using CubeAction = System.Func<MagicCube.RubikCube, MagicCube.RubikCube>;

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

		#region UpperCrossSolution

		public SolutionItem MoveUpperMiddleToStart(RubikCube cube)
		{
			return FindSolution(
				cube,
				new[]
				{
					CommandFactory.GetRotation(TurnTo.Left, Layer.Third),
					CommandFactory.GetRotation(TurnTo.Right, Layer.Third),
					CommandFactory.GetClockwiseRotation(TurnTo.Right),
					CommandFactory.GetClockwiseRotation(TurnTo.Left),
					AlgorithmBase.MoveMiddleUpperFrontToLower,
					AlgorithmBase.MoveMiddleUpperRightToLower,
					AlgorithmBase.MoveMiddleUpperLeftToLower,
					AlgorithmBase.MoveMiddleUpperBackToLower,
					AlgorithmBase.MoveMiddleSecondRightToLower,
					AlgorithmBase.MoveMiddleSecondLeftToLower
				},
				AlgorithmBase.IsUpperMiddleOnStart);
		}

		public SolutionItem MoveUpperMiddleFromStartToPoint(RubikCube cube)
		{
			return FindSolution(
				cube,
				new[]
				{
					CommandFactory.GetClockwiseRotation(TurnTo.Right),
					AlgorithmBase.MoveIncorrectOrientedUpperMiddleToPoint
				},
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
			var moveToPointItem = MoveUpperMiddleFromStartToPoint(moveToStartItem.GoalState);

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

		#endregion

		#region UpperCornersSolution

		public SolutionItem MoveUpperCornerToStart(RubikCube cube)
		{
			return FindSolution(
				cube,
				new[]
				{
					CommandFactory.GetRotation(TurnTo.Left, Layer.Third),
					CommandFactory.GetRotation(TurnTo.Right, Layer.Third),
					AlgorithmBase.MoveCornerUpperFrontToLower,
					AlgorithmBase.MoveCornerUpperRightToLower,
					AlgorithmBase.MoveCornerUpperBackToLower,
					AlgorithmBase.MoveCornerUpperLeftToLower
				},
				AlgorithmBase.IsUpperCornerOnStart);
		}

		public SolutionItem MoveUpperCornerFromStartToPoint(RubikCube cube)
		{
			return FindSolution(
				cube,
				new[]
				{
					AlgorithmBase.MoveUpperCornerFrontOrientedToStart,
					AlgorithmBase.MoveUpperCornerRightOrientedToStart,
					AlgorithmBase.ReorientateDownOrientedUpperCorner
				},
				AlgorithmBase.IsUpperCornerOnPoint);
		}

		public SolutionItem SolveUpperCorner(RubikCube cube)
		{
			if (AlgorithmBase.IsUpperCornerOnPoint(cube))
			{
				return new SolutionItem
				{
					Actions = new List<CubeCommand>(),
					GoalState = cube
				};
			}
			else
			{
				return FindAndMoveUpperCornerToPoint(cube);
			}
		}

		private SolutionItem FindAndMoveUpperCornerToPoint(RubikCube cube)
		{
			var moveToStartItem = MoveUpperCornerToStart(cube);
			var moveToPointItem = MoveUpperCornerFromStartToPoint(moveToStartItem.GoalState);

			return new SolutionItem
			{
				Actions = moveToStartItem.Actions
					.Concat(moveToPointItem.Actions)
					.ToList(),
				GoalState = moveToPointItem.GoalState
			};
		}

		public SolutionItem SolveUpperCorners(RubikCube cube)
		{
			var solveFrontItem = SolveUpperCorner(cube);
			var solveRightItem = SolveUpperCorner(solveFrontItem.GoalState.MakeTurn(TurnTo.Left));
			var solveBackItem = SolveUpperCorner(solveRightItem.GoalState.MakeTurn(TurnTo.Left));
			var solveLeftItem = SolveUpperCorner(solveBackItem.GoalState.MakeTurn(TurnTo.Left));

			return new SolutionItem
			{
				Actions = solveFrontItem.Actions
					.Concat(new[] {CommandFactory.GetTurn(TurnTo.Left)})
					.Concat(solveRightItem.Actions)
					.Concat(new[] {CommandFactory.GetTurn(TurnTo.Left)})
					.Concat(solveBackItem.Actions)
					.Concat(new[] {CommandFactory.GetTurn(TurnTo.Left)})
					.Concat(solveLeftItem.Actions)
					.ToList(),
				GoalState = solveLeftItem.GoalState
			};
		}

		#endregion
	}
}