using System.Collections.Generic;
using System.Linq;
using MagicCube.Movement;
using MagicCube.PathSearch;

using CubeAction = System.Func<MagicCube.RubikCube, MagicCube.RubikCube>;

namespace MagicCube
{
	public class Solver
	{
		public List<CubeCommand> FindAndReplaceUpperMiddleToStartPonint(RubikCube startCube)
		{
			var searcher = new PathSearhcer(
				startCube,
				new[]
				{
					CommandFactory.GetRotation(TurnTo.Left, Layer.Third),
					CommandFactory.GetRotation(TurnTo.Right, Layer.Third),
					MoveUpperMiddleToLowerMiddle
				},
				IsUpperMiddleOnStartPoint);

			return searcher.Path;
		}

		private static readonly CubeCommand MoveUpperMiddleToLowerMiddle = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
		});

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