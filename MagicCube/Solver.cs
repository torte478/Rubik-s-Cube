using System.Linq;
using MagicCube.Movement;
using MagicCube.PathSearch;

namespace MagicCube
{
	public class Solver
	{
		public SolutionItem FindAndReplaceUpperMiddleToStartPonint(RubikCube startCube)
		{
			var searcher = new PathSearhcer(
				startCube,
				new[]
				{
					CommandFactory.GetRotation(TurnTo.Left, Layer.Third),
					CommandFactory.GetRotation(TurnTo.Right, Layer.Third),
					SolveAlgorithm.MoveMiddleUpperFrontToLower,
					SolveAlgorithm.MoveMiddleUpperRightToLower,
					SolveAlgorithm.MoveMiddleUpperLeftToLower,
					SolveAlgorithm.MoveMiddleUpperBackToLower,
					CommandFactory.GetClockwiseRotation(TurnTo.Right),
					CommandFactory.GetClockwiseRotation(TurnTo.Left),
					SolveAlgorithm.MoveMiddleSecondRightToLower,
					SolveAlgorithm.MoveMiddleSecondLeftToLower
				},
				IsUpperMiddleOnStartPoint);

			return new SolutionItem
			{
				Actions = searcher.Path,
				GoalState = searcher.GoalState
			};
		}

		private static bool IsUpperMiddleOnStartPoint(RubikCube cube)
		{
			var availableColors = new[]
			{
				cube[SideIndex.Front].GetCenterColor(),
				cube[SideIndex.Top].GetCenterColor()
			};
			return availableColors.Contains(cube[SideIndex.Front].GetColor(3, 2)) &&
			       availableColors.Contains(cube[SideIndex.Down].GetColor(1, 2));
		}
	}
}