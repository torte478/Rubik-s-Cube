using System;
using System.Collections.Generic;

namespace MagicCube.Movement
{
	public static class RubikCubeMovement
	{
		private const int shiftedSideNumber = 4;
		private static readonly SideIndex[] changedOnHorizontalRotateSides = { SideIndex.Front, SideIndex.Left, SideIndex.Back, SideIndex.Right };
		private static readonly SideIndex[] changedOnVerticalRotateSides   = { SideIndex.Front, SideIndex.Top,  SideIndex.Back, SideIndex.Down  };

		public static RubikCube MakeTurn(this RubikCube rubikCube, TurnTo turnTo)
		{
			return turnTo == TurnTo.Right || turnTo == TurnTo.Left
				? rubikCube.MakeHorizontalTurn(turnTo)
				: rubikCube.MakeVerticalTurn(turnTo);
		}

		private static RubikCube MakeVerticalTurn(this RubikCube rubikCube, TurnTo turnTo)
		{
			var rightSideClockwiseDirection = turnTo == TurnTo.Up   ? TurnTo.Right : TurnTo.Left;
			var leftSideClockwiseDirection  = turnTo == TurnTo.Down ? TurnTo.Right : TurnTo.Left;

			return new RubikCube(
				rightSide: rubikCube.GetClockwiseRotateSide(SideIndex.Right, rightSideClockwiseDirection),
				leftSide:  rubikCube.GetClockwiseRotateSide(SideIndex.Left,  leftSideClockwiseDirection),
				frontSide: rubikCube.GetClockwiseTransformedSide(SideIndex.Front, turnTo, false),
				backSide:  rubikCube.GetClockwiseTransformedSide(SideIndex.Back,  turnTo, true),
				topSide:   rubikCube.GetClockwiseTransformedSide(SideIndex.Top,   turnTo, turnTo == TurnTo.Down),
				downSide:  rubikCube.GetClockwiseTransformedSide(SideIndex.Down,  turnTo, turnTo == TurnTo.Up));
		}

		private static CubeSide GetClockwiseTransformedSide(this RubikCube rubikCube,
			SideIndex sideIndex,
			TurnTo turnTo,
			bool isNeedTransform)
		{
			var transformedSide = rubikCube.GetShiftedSide(sideIndex, turnTo);

			if (isNeedTransform)
				transformedSide.MakeTwiceClockwiseRotate();

			return transformedSide;
		}

		private static CubeSide GetShiftedSide(this RubikCube rubikCube, SideIndex sideIndex, TurnTo turnTo)
		{
			var changedSides = turnTo == TurnTo.Left || turnTo == TurnTo.Right
				? changedOnHorizontalRotateSides
				: changedOnVerticalRotateSides;

			var shiftFactor = turnTo == TurnTo.Right || turnTo == TurnTo.Down
				? 1
				: -1;

			var newSideIndex = GetNextSideForRotate(sideIndex, changedSides, shiftFactor);

			return rubikCube.CloneSide(newSideIndex);
		}

		private static SideIndex GetNextSideForRotate(SideIndex sideIndex, IReadOnlyList<SideIndex> changedSides, int shiftFactor)
		{
			var currentSideIndex = 0;
			while (changedSides[currentSideIndex] != sideIndex)
				++currentSideIndex;

			var nextSideIndex = (shiftFactor + currentSideIndex + shiftedSideNumber) % shiftedSideNumber;

			return changedSides[nextSideIndex];
		}

		private static RubikCube MakeHorizontalTurn(this RubikCube rubikCube, TurnTo turnTo)
		{
			var topSideClockwiseDirection  = turnTo == TurnTo.Left  ? TurnTo.Right : TurnTo.Left;
			var downSideClockwiseDirection = turnTo == TurnTo.Right ? TurnTo.Right : TurnTo.Left;

			return new RubikCube(
				frontSide: rubikCube.GetShiftedSide(SideIndex.Front, turnTo),
				backSide:  rubikCube.GetShiftedSide(SideIndex.Back,  turnTo),
				rightSide: rubikCube.GetShiftedSide(SideIndex.Right, turnTo),
				leftSide:  rubikCube.GetShiftedSide(SideIndex.Left,  turnTo),
				topSide:   rubikCube.GetClockwiseRotateSide(SideIndex.Top,  topSideClockwiseDirection),
				downSide:  rubikCube.GetClockwiseRotateSide(SideIndex.Down, downSideClockwiseDirection));
		}

		private static CubeSide GetClockwiseRotateSide(this RubikCube rubikCube, SideIndex side, TurnTo turnTo)
		{
			return rubikCube
				.CloneSide(side)
				.MakeClockwiseRotate(turnTo);
		}

		public static RubikCube MakeRotation(this RubikCube rubikCube, TurnTo turn, Layer layer)
		{
			if (turn == TurnTo.Left || turn == TurnTo.Right)
				return rubikCube.MakeHorizontalRotation(turn, layer);
			else
				return rubikCube.MakeVerticalRotation(turn, layer);
		}

		private static RubikCube MakeHorizontalRotation(this RubikCube rubikCube, TurnTo turnTo, Layer layer)
		{
			var topSideClockwiseDirection = turnTo == TurnTo.Left ? TurnTo.Right : TurnTo.Left;
			var downSideClockwiseDirection = turnTo == TurnTo.Right ? TurnTo.Right : TurnTo.Left;

			return new RubikCube(
				topSide:   rubikCube.GetClockwiseRotateSide(SideIndex.Top,  topSideClockwiseDirection),
				downSide:  rubikCube.GetClockwiseRotateSide(SideIndex.Down, downSideClockwiseDirection),
				frontSide: rubikCube.GetHorizontalRotateSide(SideIndex.Front, turnTo, layer),
				rightSide: rubikCube.GetHorizontalRotateSide(SideIndex.Right, turnTo, layer),
				backSide:  rubikCube.GetHorizontalRotateSide(SideIndex.Back,  turnTo, layer),
				leftSide:  rubikCube.GetHorizontalRotateSide(SideIndex.Left,  turnTo, layer));
		}

		private static CubeSide GetHorizontalRotateSide(this RubikCube rubikCube, SideIndex sideIndex, TurnTo turnTo, Layer layer)
		{
			var fromSideIndex = GetNextSideForRotate(
				sideIndex: sideIndex,
				changedSides: changedOnHorizontalRotateSides,
				shiftFactor: turnTo == TurnTo.Right ? 1 : -1);

			var rowIndex = (int)layer + 1;
			return rubikCube.GetSideWithChangedColors(fromSideIndex, sideIndex, rowIndex);
		}

		private static CubeSide GetSideWithChangedColors(this RubikCube rubikCube, 
			SideIndex fromSideIndex, 
			SideIndex toSideIndex, 
			int rowIndex)
		{
			var turnedSide = rubikCube.CloneSide(toSideIndex);

			for (var columnIndex = 1; columnIndex <= 3; ++columnIndex)
			{
				var color = rubikCube[fromSideIndex].GetColor(rowIndex, columnIndex);
				turnedSide.SetColor(color, rowIndex, columnIndex);
			}

			return turnedSide;
		}

		private static RubikCube MakeVerticalRotation(this RubikCube rubikCube, TurnTo turn, Layer layer)
		{
			return rubikCube
				.MakeTurnToCorner(TurnTo.Right)
				.MakeHorizontalRotation(
					turn == TurnTo.Up ? TurnTo.Right : TurnTo.Left,
					layer)
				.MakeTurnToCorner(TurnTo.Left);
		}

		/// <param name="rubikCube"></param>
		/// <param name="turnTo">Left or Right</param>
		public static RubikCube MakeTurnToCorner(this RubikCube rubikCube, TurnTo turnTo)
		{
			if (turnTo == TurnTo.Up || turnTo == TurnTo.Down)
				throw new ArgumentOutOfRangeException(nameof(turnTo));

			return rubikCube
				.MakeTurn(TurnTo.Down)
				.MakeTurn(turnTo)
				.MakeTurn(TurnTo.Up);
		}
	}
}
