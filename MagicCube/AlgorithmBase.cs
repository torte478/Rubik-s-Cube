using System.Linq;
using MagicCube.Movement;

using CubeAction = System.Func<MagicCube.RubikCube, MagicCube.RubikCube>;

namespace MagicCube
{
	public static class AlgorithmBase
	{
		#region UpperCrossSolution

		#region MoveUpperMiddleToStart

		public static readonly CubeCommand MoveMiddleUpperFrontToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
			cube => cube.MakeClockwiseRotation(TurnTo.Right)
		});

		public static readonly CubeCommand MoveMiddleUpperRightToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Right)
		});

		public static readonly CubeCommand MoveMiddleUpperLeftToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Down, Layer.First),
			cube => cube.MakeClockwiseRotation(TurnTo.Left)
		});

		public static readonly CubeCommand MoveMiddleUpperBackToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeTurn(TurnTo.Left),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeTurn(TurnTo.Right)
		});

		public static readonly CubeCommand MoveMiddleSecondRightToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeTurn(TurnTo.Left),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Left, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeTurn(TurnTo.Right)
		});

		public static readonly CubeCommand MoveMiddleSecondLeftToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeTurn(TurnTo.Right),
			cube => cube.MakeRotation(TurnTo.Down, Layer.First),
			cube => cube.MakeRotation(TurnTo.Right, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Up, Layer.First),
			cube => cube.MakeTurn(TurnTo.Left)
		});

		public static bool IsUpperMiddleOnStart(RubikCube cube)
		{
			var availableColors = new[]
			{
				cube[SideIndex.Front].GetCenterColor(),
				cube[SideIndex.Top].GetCenterColor()
			};
			return availableColors.Contains(cube[SideIndex.Front].GetColor(3, 2))
				   && availableColors.Contains(cube[SideIndex.Down].GetColor(1, 2));
		}

		#endregion

		#region MoveUpperMiddleFromStartToPoint

		public static bool IsUpperMiddleOnStartCorrectOriented(RubikCube cube)
		{
			return cube[SideIndex.Front].GetColor(3, 2) == cube[SideIndex.Front].GetCenterColor();
		}

		public static bool IsUpperMiddleOnPoint(RubikCube cube)
		{
			return cube[SideIndex.Front].GetColor(1, 2) == cube[SideIndex.Front].GetCenterColor()
			       && cube[SideIndex.Top].GetColor(3, 2) == cube[SideIndex.Top].GetCenterColor();
		}

		public static readonly CubeCommand MoveIncorrectOrientedUpperMiddleToPoint = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Right, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Left)
		});

		#endregion

		public static bool IsSolvedUpperCross(RubikCube rubikCube)
		{
			var topSideColor = rubikCube[SideIndex.Top].GetCenterColor();
			var isSolvedTop = rubikCube[SideIndex.Top].GetColor(1, 2) == topSideColor
							  && rubikCube[SideIndex.Top].GetColor(2, 1) == topSideColor
							  && rubikCube[SideIndex.Top].GetColor(2, 3) == topSideColor
							  && rubikCube[SideIndex.Top].GetColor(3, 2) == topSideColor;

			var isSolvedWalls = rubikCube[SideIndex.Front].GetColor(1, 2) == rubikCube[SideIndex.Front].GetCenterColor()
			                    && rubikCube[SideIndex.Right].GetColor(1, 2) == rubikCube[SideIndex.Right].GetCenterColor()
			                    && rubikCube[SideIndex.Back].GetColor(1, 2) == rubikCube[SideIndex.Back].GetCenterColor()
			                    && rubikCube[SideIndex.Left].GetColor(1, 2) == rubikCube[SideIndex.Left].GetCenterColor();

			return isSolvedTop && isSolvedWalls;
		}

		#endregion

		#region UpperCornersSolution

		#region MoveUpperCornerToStart

		public static readonly CubeCommand MoveCornerUpperFrontToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
			cube => cube.MakeRotation(TurnTo.Left, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Left)
		});

		public static readonly CubeCommand MoveCornerUpperRightToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeTurn(TurnTo.Left),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Left, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeTurn(TurnTo.Right)
		});

		public static readonly CubeCommand MoveCornerUpperBackToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeTurn(TurnTo.Right),
			cube => cube.MakeRotation(TurnTo.Down, Layer.First),
			cube => cube.MakeRotation(TurnTo.Right, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Up, Layer.First),
			cube => cube.MakeTurn(TurnTo.Left)
		});

		public static readonly CubeCommand MoveCornerUpperLeftToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Down, Layer.First),
			cube => cube.MakeRotation(TurnTo.Right, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Up, Layer.First),
		});

		public static bool IsUpperCornerOnStart(RubikCube cube)
		{
			var availableColors = new[]
			{
				cube[SideIndex.Front].GetCenterColor(),
				cube[SideIndex.Top].GetCenterColor(),
				cube[SideIndex.Right].GetCenterColor()
			};

			return availableColors.Contains(cube[SideIndex.Front].GetColor(3, 3))
				   && availableColors.Contains(cube[SideIndex.Right].GetColor(3, 1))
				   && availableColors.Contains(cube[SideIndex.Down].GetColor(1, 3));
		}

		#endregion

		#region MoveUpperCornerToPoint

		public static readonly CubeCommand MoveUpperCornerFrontOrientedToStart = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Left, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Right, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third)
		});

		public static readonly CubeCommand MoveUpperCornerRightOrientedToStart = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Right, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
			cube => cube.MakeRotation(TurnTo.Left, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Left)
		});

		public static readonly CubeCommand ReorientateDownOrientedUpperCorner = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Left, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Left, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Left, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third)
		});

		public static bool IsUpperCornerOnPoint(RubikCube cube)
		{
			return cube[SideIndex.Front].GetColor(1, 3) == cube[SideIndex.Front].GetCenterColor()
			       && cube[SideIndex.Top].GetColor(3, 3) == cube[SideIndex.Top].GetCenterColor()
				   && cube[SideIndex.Right].GetColor(1, 1) == cube[SideIndex.Right].GetCenterColor();
		}

		#endregion

		public static bool IsSolvedUpperCorners(RubikCube cube)
		{
			var topSideColor = cube[SideIndex.Top].GetCenterColor();

			var isTopSolved = cube[SideIndex.Top].GetColor(1, 1) == topSideColor
			                  && cube[SideIndex.Top].GetColor(1, 3) == topSideColor
			                  && cube[SideIndex.Top].GetColor(3, 1) == topSideColor
			                  && cube[SideIndex.Top].GetColor(3, 3) == topSideColor;

			var isLeftCornersSolved = cube[SideIndex.Front].GetColor(1, 1) == cube[SideIndex.Front].GetCenterColor()
			                          && cube[SideIndex.Right].GetColor(1, 1) == cube[SideIndex.Right].GetCenterColor()
			                          && cube[SideIndex.Back].GetColor(1, 1) == cube[SideIndex.Back].GetCenterColor()
			                          && cube[SideIndex.Left].GetColor(1, 1) == cube[SideIndex.Left].GetCenterColor();

			var isRightCornersSolved = cube[SideIndex.Front].GetColor(1, 3) == cube[SideIndex.Front].GetCenterColor()
			                           && cube[SideIndex.Right].GetColor(1, 3) == cube[SideIndex.Right].GetCenterColor()
			                           && cube[SideIndex.Back].GetColor(1, 3) == cube[SideIndex.Back].GetCenterColor()
			                           && cube[SideIndex.Left].GetColor(1, 3) == cube[SideIndex.Left].GetCenterColor();

			return isTopSolved && isLeftCornersSolved && isRightCornersSolved;
		}

		#endregion
	}
}
