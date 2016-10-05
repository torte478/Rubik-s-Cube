using System;
using System.Collections.Generic;
using System.Linq;
using MagicCube.Movement;
using MagicCube.PathSearch;

using CubeAction = System.Func<MagicCube.RubikCube, MagicCube.RubikCube>;

namespace MagicCube.CubeSolution
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

		private static SolutionItem FindAndMoveToPointIfNeed(
			RubikCube cube, 
			bool isNotNeedMovement, 
			Func<RubikCube, SolutionItem> moveToPoint)
		{
			if (isNotNeedMovement)
			{
				return new SolutionItem
				{
					Actions = new List<CubeCommand>(),
					GoalState = cube
				};
			}
			else
			{
				return moveToPoint(cube);
			}
		}

		private static SolutionItem FindAndMove(
			RubikCube cube,
			Func<RubikCube, SolutionItem> moveToStart,
			Func<RubikCube, SolutionItem> moveToPoint)
		{
			var moveToStartItem = moveToStart(cube);
			var moveToPointItem = moveToPoint(moveToStartItem.GoalState);

			return new SolutionItem
			{
				Actions = moveToStartItem.Actions.Concat(moveToPointItem.Actions).ToList(),
				GoalState = moveToPointItem.GoalState
			};
		}

        private SolutionItem SolveFourSides(RubikCube cube, Func<RubikCube, SolutionItem> solveFunc)
        {
            var solveFrontItem = solveFunc(cube);
            var solveRightItem = solveFunc(solveFrontItem.GoalState.MakeTurn(TurnTo.Left));
            var solveBackItem = solveFunc(solveRightItem.GoalState.MakeTurn(TurnTo.Left));
            var solveLeftItem = solveFunc(solveBackItem.GoalState.MakeTurn(TurnTo.Left));

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

        #region UpperLayerSolution

        #region UpperCrossSolution

        public SolutionItem MoveUpperMiddleToStart(RubikCube cube)
		{
            var frontColor = cube[SideIndex.Front].GetCenterColor();

            return FindSolution(
                cube,
                new[]
                {
                    CommandFactory.GetRotation(TurnTo.Left, Layer.Third),
                    CommandFactory.GetRotation(TurnTo.Right, Layer.Third),
                    CommandFactory.GetTurn(TurnTo.Right),
                    CommandFactory.GetTurn(TurnTo.Left),
                    AlgorithmBase.MoveMiddleUpperTopToLower,
                    AlgorithmBase.MoveMiddleUpperRightToLower,
                },
                c => 
                {
                    return c[SideIndex.Front].GetCenterColor() == frontColor
                    && AlgorithmBase.IsUpperMiddleOnStart(c);
                });
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
			return FindAndMoveToPointIfNeed(
				cube, 
				AlgorithmBase.IsUpperMiddleOnPoint(cube), 
				FindAndMoveUpperMiddleToPoint);
		}

		private SolutionItem FindAndMoveUpperMiddleToPoint(RubikCube cube)
		{
			return FindAndMove(cube, MoveUpperMiddleToStart, MoveUpperMiddleFromStartToPoint);
		}

		public SolutionItem SolveUpperCross(RubikCube cube)
		{
            return SolveFourSides(cube, SolveUpperMiddle);
		}

		#endregion

		#region UpperCornersSolution

		public SolutionItem MoveUpperCornerToStart(RubikCube cube)
		{
            var frontColor = cube[SideIndex.Front].GetCenterColor();

			return FindSolution(
				cube,
				new[]
				{
					CommandFactory.GetRotation(TurnTo.Left, Layer.Third),
					CommandFactory.GetRotation(TurnTo.Right, Layer.Third),
                    CommandFactory.GetTurn(TurnTo.Left),
                    CommandFactory.GetTurn(TurnTo.Right),
                    AlgorithmBase.MoveUpperCornerFrontToLower
                },
				c =>
                {
                    return c[SideIndex.Front].GetCenterColor() == frontColor
                    && AlgorithmBase.IsUpperCornerOnStart(c);
                });
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
			return FindAndMoveToPointIfNeed(
				cube,
				AlgorithmBase.IsUpperCornerOnPoint(cube),
				FindAndMoveUpperCornerToPoint);
		}

		private SolutionItem FindAndMoveUpperCornerToPoint(RubikCube cube)
		{
			return FindAndMove(cube, MoveUpperCornerToStart, MoveUpperCornerFromStartToPoint);
		}

		public SolutionItem SolveUpperCorners(RubikCube cube)
		{
            return SolveFourSides(cube, SolveUpperCorner);
		}

		#endregion

		public SolutionItem SolveUpperLayer(RubikCube cube)
		{
			var solveCross = SolveUpperCross(cube);
			var solveCorners = SolveUpperCorners(solveCross.GoalState);

			return new SolutionItem
			{
				Actions = solveCross.Actions.Concat(solveCorners.Actions).ToList(),
				GoalState = solveCorners.GoalState
			};
		}

		#endregion
	}
}