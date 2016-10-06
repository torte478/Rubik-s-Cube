using System;
using System.Linq;
using MagicCube.Movement;

using CubeAction = System.Func<MagicCube.RubikCube, MagicCube.RubikCube>;

namespace MagicCube.CubeSolution
{
	public static class AlgorithmBase
	{
		private static bool IsTrueOnFourSides(RubikCube cube, Func<RubikCube, bool> predFunc)
		{
			return predFunc(cube)
				   && predFunc(cube.MakeTurn(TurnTo.Left))
				   && predFunc(cube.MakeTurn(TurnTo.Left).MakeTurn(TurnTo.Left))
				   && predFunc(cube.MakeTurn(TurnTo.Right));
		}

		#region UpperLayerSolution

		public static bool IsSolvedUpperLayer(RubikCube cube)
		{
			return IsSolvedUpperCross(cube) && IsSolvedUpperCorners(cube);
		}

		#region UpperCrossSolution

		public static bool IsSolvedUpperCross(RubikCube rubikCube)
		{
			return IsTrueOnFourSides(rubikCube, IsSolvedUpperMiddleCell);
		}

		private static bool IsSolvedUpperMiddleCell(RubikCube cube)
		{
			return cube[SideIndex.Front].GetColor(1, 2) == cube[SideIndex.Front].GetCenterColor()
			       && cube[SideIndex.Top].GetColor(3, 2) == cube[SideIndex.Top].GetCenterColor();
		}

		#region MoveUpperMiddleToStart

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

		public static readonly CubeCommand MoveMiddleUpperTopToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
            cube => cube.MakeRotation(TurnTo.Left, Layer.Third),
            cube => cube.MakeClockwiseRotation(TurnTo.Right),
            cube => cube.MakeClockwiseRotation(TurnTo.Right),
            cube => cube.MakeRotation(TurnTo.Right, Layer.Third),
        },
            "Move top middle to lower layer");

		public static readonly CubeCommand MoveMiddleUpperRightToLower = new CubeCommand(new CubeAction[]
        {
            c => c.MakeRotation(TurnTo.Down, Layer.Third),
            c => c.MakeRotation(TurnTo.Left, Layer.Third),
            c => c.MakeRotation(TurnTo.Up, Layer.Third),
        },
            "Move middle middle to lower layer");

		#endregion

		#region MoveUpperMiddleFromStartToPoint

		public static readonly CubeCommand MoveIncorrectOrientedUpperMiddleToPoint = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Right, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Left)
		},
            "Move lower middle to upert layer");

		public static bool IsUpperMiddleOnStartCorrectOriented(RubikCube cube)
		{
			return cube[SideIndex.Front].GetColor(3, 2) == cube[SideIndex.Front].GetCenterColor();
		}

		public static bool IsUpperMiddleOnPoint(RubikCube cube)
		{
			return cube[SideIndex.Front].GetColor(1, 2) == cube[SideIndex.Front].GetCenterColor()
			       && cube[SideIndex.Top].GetColor(3, 2) == cube[SideIndex.Top].GetCenterColor();
		}

		#endregion

        #endregion

        #region UpperCornersSolution

        #region MoveUpperCornerToStart

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

		public static readonly CubeCommand MoveUpperCornerFrontToLower = new CubeCommand(new CubeAction[]
        {
            c => c.MakeRotation(TurnTo.Down, Layer.Third),
            c => c.MakeRotation(TurnTo.Left, Layer.Third),
            c => c.MakeRotation(TurnTo.Up, Layer.Third),
            c => c.MakeRotation(TurnTo.Right, Layer.Third)
        },
            "Move upper corner to lower");

		#endregion

		#region MoveUpperCornerToPoint

		public static bool IsUpperCornerOnPoint(RubikCube cube)
		{
			return cube[SideIndex.Front].GetColor(1, 3) == cube[SideIndex.Front].GetCenterColor()
			       && cube[SideIndex.Top].GetColor(3, 3) == cube[SideIndex.Top].GetCenterColor()
			       && cube[SideIndex.Right].GetColor(1, 1) == cube[SideIndex.Right].GetCenterColor();
		}

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

		#endregion

		public static bool IsSolvedUpperCorners(RubikCube cube)
		{
			return IsTrueOnFourSides(cube, IsUpperCornerOnPoint);
		}

		#endregion

		#endregion

		#region MiddleLayerSolution

		public static bool IsSolvedMiddleMiddle(RubikCube cube)
		{
			return IsTrueOnFourSides(cube, IsMiddleMiddleOnPoint);
		}

		public static bool IsMiddleMiddleOnPoint(RubikCube cube)
		{
			return cube[SideIndex.Front].GetColor(2, 3) == cube[SideIndex.Front].GetCenterColor()
				   && cube[SideIndex.Right].GetColor(2, 1) == cube[SideIndex.Right].GetCenterColor();
		}

		public static bool IsMiddleMiddleOnStart(RubikCube cube)
        {
            var availableColors = new[]
            {
                cube[SideIndex.Front].GetCenterColor(),
                cube[SideIndex.Right].GetCenterColor()
            };

            return availableColors.Contains(cube[SideIndex.Front].GetColor(3, 2))
                   && availableColors.Contains(cube[SideIndex.Down].GetColor(1, 2));
        }

        public static readonly CubeCommand MoveCorrectOrientedMiddleMiddleToPoint = new CubeCommand(new CubeAction[]
        {
            c => c.MakeRotation(TurnTo.Left, Layer.Third),
            c => c.MakeRotation(TurnTo.Down, Layer.Third),
            c => c.MakeRotation(TurnTo.Right, Layer.Third),
            c => c.MakeRotation(TurnTo.Up, Layer.Third),
            c => c.MakeRotation(TurnTo.Right, Layer.Third),
            c => c.MakeClockwiseRotation(TurnTo.Right),
            c => c.MakeRotation(TurnTo.Left, Layer.Third),
            c => c.MakeClockwiseRotation(TurnTo.Left)
        });

        public static readonly CubeCommand MoveIncorrectOrientedMiddleMiddleToPoint = new CubeCommand(new CubeAction[]
        {
            c => c.MakeRotation(TurnTo.Right, Layer.Third),
            c => c.MakeRotation(TurnTo.Right, Layer.Third),
            c => c.MakeClockwiseRotation(TurnTo.Right),
            c => c.MakeRotation(TurnTo.Left, Layer.Third),
            c => c.MakeClockwiseRotation(TurnTo.Left),
            c => c.MakeRotation(TurnTo.Left, Layer.Third),
            c => c.MakeRotation(TurnTo.Down, Layer.Third),
            c => c.MakeRotation(TurnTo.Right, Layer.Third),
            c => c.MakeRotation(TurnTo.Up, Layer.Third)
        });

		#endregion

		#region LowerLayerSolution

		public static bool IsSolvedLowerLayer(RubikCube cube)
		{
			return IsTrueOnFourSides(cube, IsSolvedLowerLayerOnFront);
		}

		private static bool IsSolvedLowerLayerOnFront(RubikCube cube)
		{
			var centerColor = cube[SideIndex.Front].GetCenterColor();

			return cube[SideIndex.Front].GetColor(1, 1) == centerColor
				   && cube[SideIndex.Front].GetColor(1, 2) == centerColor
				   && cube[SideIndex.Front].GetColor(1, 3) == centerColor;
		}

		#region LowerCrossSolution

		public static bool IsSolvedLowerCross(RubikCube cube)
		{
			var centerColor = cube[SideIndex.Top].GetCenterColor();
			return cube[SideIndex.Top].GetColor(1, 2) == centerColor
			       && cube[SideIndex.Top].GetColor(2, 1) == centerColor
			       && cube[SideIndex.Top].GetColor(2, 3) == centerColor
			       && cube[SideIndex.Top].GetColor(3, 2) == centerColor;
		}

		public static bool IsLowerCrossOnStart(RubikCube cube)
		{
			return IsTrueOnFourSides(cube, IsLowerMiddleOnStart);
		}

		private static bool IsLowerMiddleOnStart(RubikCube cube)
		{
			return cube[SideIndex.Front].GetColor(1, 2) == cube[SideIndex.Front].GetCenterColor()
			       || cube[SideIndex.Top].GetColor(3, 2) == cube[SideIndex.Front].GetCenterColor();
		}

		public static CubeCommand ReplaceLowerLayerMiddles = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Left, Layer.First),
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Left, Layer.First),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Right, Layer.First),
			cube => cube.MakeClockwiseRotation(TurnTo.Left)
		});

		public static CubeCommand ReorientateLowerMiddle = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Right, Layer.Second),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Right, Layer.Second),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Right, Layer.Second),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Right, Layer.Second)
		});

		#endregion

		#region LowerCornersSolution

		private static bool IsLowerCornerOnPoint(RubikCube cube)
		{
			return cube[SideIndex.Front].GetColor(1, 3) == cube[SideIndex.Front].GetCenterColor()
				   && cube[SideIndex.Top].GetColor(3, 3) == cube[SideIndex.Top].GetCenterColor()
				   && cube[SideIndex.Right].GetColor(1, 1) == cube[SideIndex.Right].GetCenterColor();
		}

		public static bool IsAllLowerCornersOnPoint(RubikCube cube)
		{
			return IsTrueOnFourSides(cube, IsLowerCornerOnPoint);
		}

		public static bool IsAllLowerCornersOnStart(RubikCube cube)
		{
			return IsTrueOnFourSides(cube, IsLowerCornerOnStart);
		}

		private static bool IsLowerCornerOnStart(RubikCube cube)
		{
			var availableColors = new[]
			{
				cube[SideIndex.Left].GetColor(1, 2),
				cube[SideIndex.Back].GetColor(1, 2),
				cube[SideIndex.Top].GetCenterColor()
			};

			return availableColors.Contains(cube[SideIndex.Top].GetColor(1, 1))
				&& availableColors.Contains(cube[SideIndex.Left].GetColor(1, 1))
				&& availableColors.Contains(cube[SideIndex.Back].GetColor(1, 3));
		}

		public static CubeCommand ReorientateLowerCornersByRight = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Left),
			cube => cube.MakeRotation(TurnTo.Up, Layer.First),
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Left),
			cube => cube.MakeRotation(TurnTo.Down, Layer.First),
			cube => cube.MakeClockwiseRotation(TurnTo.Right)
		});

		public static CubeCommand ReorientateLowerCornersByLeft = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeClockwiseRotation(TurnTo.Left),
			cube => cube.MakeRotation(TurnTo.Up, Layer.First),
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Left),
			cube => cube.MakeRotation(TurnTo.Down, Layer.First),
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
		});

		public static CubeCommand RotateLowerCornerByRight = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Left),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Left),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
		});

		public static CubeCommand RotateLowerCornerByLeft = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeClockwiseRotation(TurnTo.Left),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Left),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
		});

		#endregion

		#endregion

		public static bool IsSolvedCube(RubikCube cube)
		{
			return cube[SideIndex.Front].IsFill(cube[SideIndex.Front].GetCenterColor())
			       && cube[SideIndex.Top].IsFill(cube[SideIndex.Top].GetCenterColor())
			       && cube[SideIndex.Right].IsFill(cube[SideIndex.Right].GetCenterColor())
			       && cube[SideIndex.Back].IsFill(cube[SideIndex.Back].GetCenterColor())
			       && cube[SideIndex.Down].IsFill(cube[SideIndex.Down].GetCenterColor())
			       && cube[SideIndex.Left].IsFill(cube[SideIndex.Left].GetCenterColor());
		}
	}
}
