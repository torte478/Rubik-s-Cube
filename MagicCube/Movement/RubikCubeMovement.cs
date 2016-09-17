using System;
using System.Collections.Generic;

namespace MagicCube.Movement
{
	public static class RubikCubeMovement
	{
		private const int shiftedSideNumber = 4;
		private static readonly SideIndex[] changedOnHorizontalTurnSides = { SideIndex.Front, SideIndex.Left, SideIndex.Back, SideIndex.Right };
		private static readonly SideIndex[] changedOnVerticalTurnSides   = { SideIndex.Front, SideIndex.Top,  SideIndex.Back, SideIndex.Down  };

		public static RubikCube MakeRollTurn(this RubikCube rubikCube, TurnTo turnTo)
		{
			return turnTo == TurnTo.Right || turnTo == TurnTo.Left
				? rubikCube.MakeHorizontalRollTurn(turnTo)
				: rubikCube.MakeVerticalRollTurn(turnTo);
		}

		private static RubikCube MakeVerticalRollTurn(this RubikCube rubikCube, TurnTo turnTo)
		{
			var rightSideClockwiseDirection = turnTo == TurnTo.Up   ? TurnTo.Right : TurnTo.Left;
			var leftSideClockwiseDirection  = turnTo == TurnTo.Down ? TurnTo.Right : TurnTo.Left;

			return new RubikCube(
				rightSide: rubikCube.GetClockwiseTurnedSide(SideIndex.Right, rightSideClockwiseDirection),
				leftSide:  rubikCube.GetClockwiseTurnedSide(SideIndex.Left,  leftSideClockwiseDirection),
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
				transformedSide.MakeTwiceClockwiseTurn();

			return transformedSide;
		}

		private static CubeSide GetShiftedSide(this RubikCube rubikCube, SideIndex sideIndex, TurnTo turnTo)
		{
			var changedSides = turnTo == TurnTo.Left || turnTo == TurnTo.Right
				? changedOnHorizontalTurnSides
				: changedOnVerticalTurnSides;

			var shiftFactor = turnTo == TurnTo.Right || turnTo == TurnTo.Down
				? 1
				: -1;

			var newSideIndex = GetNextSideForTurn(sideIndex, changedSides, shiftFactor);

			return rubikCube.CloneSide(newSideIndex);
		}

		private static SideIndex GetNextSideForTurn(SideIndex sideIndex, IReadOnlyList<SideIndex> changedSides, int shiftFactor)
		{
			var currentSideIndex = 0;
			while (changedSides[currentSideIndex] != sideIndex)
				++currentSideIndex;

			var nextSideIndex = (shiftFactor + currentSideIndex + shiftedSideNumber) % shiftedSideNumber;

			return changedSides[nextSideIndex];
		}

		private static RubikCube MakeHorizontalRollTurn(this RubikCube rubikCube, TurnTo turnTo)
		{
			var topSideClockwiseDirection  = turnTo == TurnTo.Left  ? TurnTo.Right : TurnTo.Left;
			var downSideClockwiseDirection = turnTo == TurnTo.Right ? TurnTo.Right : TurnTo.Left;

			return new RubikCube(
				frontSide: rubikCube.GetShiftedSide(SideIndex.Front, turnTo),
				backSide:  rubikCube.GetShiftedSide(SideIndex.Back,  turnTo),
				rightSide: rubikCube.GetShiftedSide(SideIndex.Right, turnTo),
				leftSide:  rubikCube.GetShiftedSide(SideIndex.Left,  turnTo),
				topSide:   rubikCube.GetClockwiseTurnedSide(SideIndex.Top,  topSideClockwiseDirection),
				downSide:  rubikCube.GetClockwiseTurnedSide(SideIndex.Down, downSideClockwiseDirection));
		}

		private static CubeSide GetClockwiseTurnedSide(this RubikCube rubikCube, SideIndex side, TurnTo turnTo)
		{
			return rubikCube
				.CloneSide(side)
				.MakeClockwiseTurn(turnTo);
		}

		public static RubikCube MakeTurn(this RubikCube rubikCube, TurnTo turn, Layer layer)
		{
			if (turn == TurnTo.Left || turn == TurnTo.Right)
				return rubikCube.MakeHorizontalTurn(turn, layer);
			else
				return rubikCube.MakeVerticalTurn(turn, layer);
		}

		private static RubikCube MakeHorizontalTurn(this RubikCube rubikCube, TurnTo turn, Layer layer)
		{
			return new RubikCube(
				topSide:   rubikCube[SideIndex.Top],
				downSide:  rubikCube[SideIndex.Down],
				frontSide: rubikCube.GetHorizontalTurnedSide(SideIndex.Front, turn, layer),
				rightSide: rubikCube.GetHorizontalTurnedSide(SideIndex.Right, turn, layer),
				backSide:  rubikCube.GetHorizontalTurnedSide(SideIndex.Back,  turn, layer),
				leftSide:  rubikCube.GetHorizontalTurnedSide(SideIndex.Left,  turn, layer));
		}

		private static CubeSide GetHorizontalTurnedSide(this RubikCube rubikCube, SideIndex sideIndex, TurnTo turnTo, Layer layer)
		{
			var fromSideIndex = GetNextSideForTurn(
				sideIndex: sideIndex,
				changedSides: changedOnHorizontalTurnSides,
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

		private static RubikCube MakeVerticalTurn(this RubikCube rubikCube, TurnTo turn, Layer layer)
		{
			return rubikCube
				.MakeRollToCorner(TurnTo.Right)
				.MakeHorizontalTurn(
					turn == TurnTo.Up ? TurnTo.Right : TurnTo.Left,
					layer)
				.MakeRollToCorner(TurnTo.Left);
		}

		public static RubikCube MakeRollToCorner(this RubikCube rubikCube, TurnTo turnTo)
		{
			if (turnTo == TurnTo.Up || turnTo == TurnTo.Down)
				throw new ArgumentOutOfRangeException(nameof(turnTo));

			return rubikCube
				.MakeRollTurn(TurnTo.Down)
				.MakeRollTurn(turnTo)
				.MakeRollTurn(TurnTo.Up);
		}
	}
}
