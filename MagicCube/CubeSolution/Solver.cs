﻿using System;
using System.Collections.Generic;
using System.Linq;
using MagicCube.Movement;
using MagicCube.PathSearch;

namespace MagicCube.CubeSolution
{
	public class Solver
	{
		private static SolutionItem SolveFourSides(RubikCube cube, Func<RubikCube, SolutionItem> solveFunc)
		{
			var solveFrontItem = solveFunc(cube);
			var solveRightItem = solveFunc(solveFrontItem.GoalState.MakeTurn(TurnTo.Left));
			var solveBackItem = solveFunc(solveRightItem.GoalState.MakeTurn(TurnTo.Left));
			var solveLeftItem = solveFunc(solveBackItem.GoalState.MakeTurn(TurnTo.Left));

            var items = new[] { solveFrontItem, solveLeftItem, solveBackItem, solveRightItem };

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
				GoalState = solveLeftItem.GoalState,
                MaxRecursionDeep = items.Select(item => item.MaxRecursionDeep).Max(),
                MaxHandledElementsCount = items.Select(item => item.MaxHandledElementsCount).Max()
            };
		}

		private static SolutionItem FindAndMoveToPointIfNeed(
			RubikCube cube, 
			bool isCellOnPoint, 
			Func<RubikCube, SolutionItem> moveToPoint)
		{
			return isCellOnPoint 
				? GetEmptySolution(cube) 
				: moveToPoint(cube);
		}

		private static SolutionItem GetEmptySolution(RubikCube cube)
		{
			return new SolutionItem
			{
				Actions = new List<CubeCommand>(),
				GoalState = cube
			};
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
				GoalState = moveToPointItem.GoalState,
                MaxRecursionDeep = Math.Max(
                    moveToStartItem.MaxRecursionDeep, 
                    moveToPointItem.MaxRecursionDeep),
                MaxHandledElementsCount = Math.Max(
                    moveToStartItem.MaxHandledElementsCount, 
                    moveToPointItem.MaxHandledElementsCount)
            };
		}

		private static SolutionItem FindSolution(RubikCube cube, CubeCommand[] commands, Func<RubikCube, bool> condition)
		{
			var searcher = new PathSearhcer(cube, commands, condition);

			return new SolutionItem
			{
				Actions = searcher.Path,
				GoalState = searcher.GoalState,
                MaxRecursionDeep = searcher.Path.Count,
                MaxHandledElementsCount = searcher.HandledStates.Count
			};
		}

		#region UpperLayerSolution

		public SolutionItem SolveUpperLayer(RubikCube cube)
		{
			var solveCross = SolveUpperCross(cube);
			var solveCorners = SolveUpperCorners(solveCross.GoalState);

			return new SolutionItem
			{
				Actions = solveCross.Actions.Concat(solveCorners.Actions).ToList(),
				GoalState = solveCorners.GoalState,
                MaxRecursionDeep = Math.Max(solveCross.MaxRecursionDeep, solveCorners.MaxRecursionDeep),
                MaxHandledElementsCount = Math.Max(solveCross.MaxHandledElementsCount, solveCorners.MaxHandledElementsCount),
            };
		}

		#region UpperCrossSolution

		public SolutionItem SolveUpperCross(RubikCube cube)
		{
			return SolveFourSides(cube, SolveUpperMiddle);
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
				c => c[SideIndex.Front].GetCenterColor() == frontColor
				     && AlgorithmBase.IsUpperMiddleOnStart(c));
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

		#endregion

		#region UpperCornersSolution

		public SolutionItem SolveUpperCorners(RubikCube cube)
		{
			return SolveFourSides(cube, SolveUpperCorner);
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
				c => c[SideIndex.Front].GetCenterColor() == frontColor
				     && AlgorithmBase.IsUpperCornerOnStart(c));
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

		#endregion

		#endregion

		#region MiddleLayerSolution

		public SolutionItem SolveMiddleLayer(RubikCube cube)
		{
			return SolveFourSides(cube, SolveMiddleMiddle);
		}

		private SolutionItem SolveMiddleMiddle(RubikCube cube)
		{
			return FindAndMoveToPointIfNeed(
				cube, 
				AlgorithmBase.IsMiddleMiddleOnPoint(cube), 
				FindAndMoveMiddleMiddleToPoint);
		}

		private SolutionItem FindAndMoveMiddleMiddleToPoint(RubikCube cube)
		{
			return FindAndMove(cube, MoveMiddleMiddleToStart, MoveMiddleMiddleFromStartToPoint);
		}

		public SolutionItem MoveMiddleMiddleToStart(RubikCube cube)
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
					AlgorithmBase.MoveCorrectOrientedMiddleMiddleToPoint
				},
				c => c[SideIndex.Front].GetCenterColor() == frontColor
					 && AlgorithmBase.IsMiddleMiddleOnStart(c));
		}

		public SolutionItem MoveMiddleMiddleFromStartToPoint(RubikCube cube)
		{
			return FindSolution(
				cube,
				new[]
				{
					AlgorithmBase.MoveCorrectOrientedMiddleMiddleToPoint,
					AlgorithmBase.MoveIncorrectOrientedMiddleMiddleToPoint,
				},
				AlgorithmBase.IsMiddleMiddleOnPoint);
		}

		#endregion

		#region LowerLayerSolution

		public SolutionItem SolveLowerLayer(RubikCube cube)
		{
			var solveCrossItem = SolveLowerCross(cube);
			var moveToStartItem = MoveLowerCornersToStart(solveCrossItem.GoalState);
			var moveToPointItem = MoveLowerCornersToPoint(moveToStartItem.GoalState);

		    var items = new[] {solveCrossItem, moveToStartItem, moveToPointItem};

			return new SolutionItem
			{
				Actions = solveCrossItem.Actions
					.Concat(moveToStartItem.Actions)
					.Concat(moveToPointItem.Actions)
					.ToList(),
				GoalState = moveToPointItem.GoalState,
                MaxRecursionDeep = items.Select(item => item.MaxRecursionDeep).Max(),
                MaxHandledElementsCount = items.Select(item => item.MaxHandledElementsCount).Max()
			};
		}

		#region LowerCrossSolution

		public SolutionItem SolveLowerCross(RubikCube cube)
		{
			var moveToStartItem = MoveLowerCrossToStart(cube);
			var moveToPointItem = MoveLowerCrossToPoint(moveToStartItem.GoalState);

			return new SolutionItem
			{
				Actions = moveToStartItem.Actions.Concat(moveToPointItem.Actions).ToList(),
				GoalState = moveToPointItem.GoalState,
                MaxRecursionDeep = Math.Max(moveToStartItem.MaxRecursionDeep, moveToPointItem.MaxRecursionDeep),
                MaxHandledElementsCount = Math.Max(moveToStartItem.MaxHandledElementsCount, moveToPointItem.MaxHandledElementsCount),
			};
		}

		public SolutionItem MoveLowerCrossToStart(RubikCube cube)
		{
			return FindSolution(
				cube,
				new[]
				{
					CommandFactory.GetRotation(TurnTo.Left, Layer.First),
					CommandFactory.GetRotation(TurnTo.Right, Layer.First),
					CommandFactory.GetTurn(TurnTo.Left),
					CommandFactory.GetTurn(TurnTo.Right),
					AlgorithmBase.ReplaceLowerLayerMiddles
				},
				AlgorithmBase.IsLowerCrossOnStart);
		}

		public SolutionItem MoveLowerCrossToPoint(RubikCube cube)
		{
			return FindSolution(
				cube,
				new[]
				{
					CommandFactory.GetRotation(TurnTo.Left, Layer.First),
					CommandFactory.GetRotation(TurnTo.Right, Layer.First),
					AlgorithmBase.ReorientateLowerMiddle
				},
				AlgorithmBase.IsSolvedLowerCross);
		}

		#endregion

		#region LowerCornersSolution

		public SolutionItem MoveLowerCornersToStart(RubikCube cube)
		{
			return FindSolution(
				cube,
				new[]
				{
					CommandFactory.GetRotation(TurnTo.Left, Layer.First),
					CommandFactory.GetRotation(TurnTo.Right, Layer.First),
					AlgorithmBase.ReorientateLowerCornersByLeft,
					AlgorithmBase.ReorientateLowerCornersByRight,
				},
				AlgorithmBase.IsAllLowerCornersOnStart);
		}

		public SolutionItem MoveLowerCornersToPoint(RubikCube cube)
		{
			return FindSolution(
				cube,
				new[]
				{
					CommandFactory.GetRotation(TurnTo.Left, Layer.First),
					CommandFactory.GetRotation(TurnTo.Right, Layer.First),
					AlgorithmBase.RotateLowerCornerByLeft,
					AlgorithmBase.RotateLowerCornerByRight,
				},
				AlgorithmBase.IsAllLowerCornersOnPoint);
		}

		#endregion

		#endregion

		public SolutionItem SolveCube(RubikCube cube)
		{
			var solveUpperLayer = SolveUpperLayer(cube);
			var solveMiddleLayer = SolveMiddleLayer(solveUpperLayer.GoalState);
			var solveLowerLayer = SolveLowerLayer(solveMiddleLayer.GoalState.MakeTurn(TurnTo.Up).MakeTurn(TurnTo.Up));

		    var items = new[] {solveUpperLayer, solveMiddleLayer, solveLowerLayer};

			return new SolutionItem
			{
				Actions = solveUpperLayer.Actions
					.Concat(solveMiddleLayer.Actions)
					.Concat(new []
					{
						CommandFactory.GetTurn(TurnTo.Up),
						CommandFactory.GetTurn(TurnTo.Up)
					})
					.Concat(solveLowerLayer.Actions)
					.ToList(),
				GoalState = solveLowerLayer.GoalState,
                MaxRecursionDeep = items.Select(item => item.MaxRecursionDeep).Max(),
                MaxHandledElementsCount = items.Select(item => item.MaxHandledElementsCount).Max(),
			};
		}
	}
}