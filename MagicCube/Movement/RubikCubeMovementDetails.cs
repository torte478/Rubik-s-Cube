using System.Collections.Generic;

namespace MagicCube.Movement
{
	public static partial class RubikCubeMovement
	{
		private const int shiftedSideNumber = 4;
		private static readonly SideIndex[] changedOnHorizontalRotationSides = { SideIndex.Front, SideIndex.Left, SideIndex.Back, SideIndex.Right };
		private static readonly SideIndex[] changedOnVerticalRotationSides = { SideIndex.Front, SideIndex.Top, SideIndex.Back, SideIndex.Down };

		private static RubikCube MakeVerticalTurn(this RubikCube rubikCube, TurnTo turnTo)
		{
			var rightSideClockwiseDirection = turnTo == TurnTo.Up ? TurnTo.Right : TurnTo.Left;
			var leftSideClockwiseDirection = turnTo == TurnTo.Down ? TurnTo.Right : TurnTo.Left;

			return new RubikCube(
				rightSide: rubikCube.GetClockwiseRotationSide(SideIndex.Right, rightSideClockwiseDirection),
				leftSide:  rubikCube.GetClockwiseRotationSide(SideIndex.Left,  leftSideClockwiseDirection),
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
				transformedSide.MakeTwiceClockwiseRotation();

			return transformedSide;
		}

		private static CubeSide GetShiftedSide(this RubikCube rubikCube, SideIndex sideIndex, TurnTo turnTo)
		{
			var changedSides = turnTo == TurnTo.Left || turnTo == TurnTo.Right
				? changedOnHorizontalRotationSides
				: changedOnVerticalRotationSides;

			var shiftFactor = turnTo == TurnTo.Right || turnTo == TurnTo.Down
				? 1
				: -1;

			var newSideIndex = GetNextSideForRotation(sideIndex, changedSides, shiftFactor);

			return rubikCube.CloneSide(newSideIndex);
		}

		private static SideIndex GetNextSideForRotation(
			SideIndex sideIndex, 
			IReadOnlyList<SideIndex> changedSides, 
			int shiftFactor)
		{
			var currentSideIndex = 0;
			while (changedSides[currentSideIndex] != sideIndex)
				++currentSideIndex;

			var nextSideIndex = (shiftFactor + currentSideIndex + shiftedSideNumber) % shiftedSideNumber;

			return changedSides[nextSideIndex];
		}

		private static RubikCube MakeHorizontalTurn(this RubikCube rubikCube, TurnTo turnTo)
		{
			var topSideClockwiseDirection = turnTo == TurnTo.Left ? TurnTo.Right : TurnTo.Left;
			var downSideClockwiseDirection = turnTo == TurnTo.Right ? TurnTo.Right : TurnTo.Left;

			return new RubikCube(
				frontSide: rubikCube.GetShiftedSide(SideIndex.Front, turnTo),
				backSide:  rubikCube.GetShiftedSide(SideIndex.Back, turnTo),
				rightSide: rubikCube.GetShiftedSide(SideIndex.Right, turnTo),
				leftSide:  rubikCube.GetShiftedSide(SideIndex.Left, turnTo),
				topSide:   rubikCube.GetClockwiseRotationSide(SideIndex.Top, topSideClockwiseDirection),
				downSide:  rubikCube.GetClockwiseRotationSide(SideIndex.Down, downSideClockwiseDirection));
		}

		private static CubeSide GetClockwiseRotationSide(this RubikCube rubikCube, SideIndex side, TurnTo turnTo)
		{
			return rubikCube
				.CloneSide(side)
				.MakeClockwiseRotation(turnTo);
		}


		private static RubikCube MakeHorizontalRotation(this RubikCube rubikCube, TurnTo turnTo, Layer layer)
		{
			var newTopSide = layer != Layer.First
				? rubikCube.CloneSide(SideIndex.Top)
				: rubikCube.GetClockwiseRotationSide(SideIndex.Top, turnTo == TurnTo.Left
																		? TurnTo.Right
																		: TurnTo.Left);
			var newDownSide = layer != Layer.Third
				? rubikCube.CloneSide(SideIndex.Down)
				: rubikCube.GetClockwiseRotationSide(SideIndex.Down, turnTo == TurnTo.Right 
																		? TurnTo.Right 
																		: TurnTo.Left);
			return new RubikCube(
				topSide:   newTopSide,
				downSide:  newDownSide,
				frontSide: rubikCube.GetHorizontalRotationSide(SideIndex.Front, turnTo, layer),
				rightSide: rubikCube.GetHorizontalRotationSide(SideIndex.Right, turnTo, layer),
				backSide:  rubikCube.GetHorizontalRotationSide(SideIndex.Back, turnTo, layer),
				leftSide:  rubikCube.GetHorizontalRotationSide(SideIndex.Left, turnTo, layer));
		}

		private static CubeSide GetHorizontalRotationSide(this RubikCube rubikCube, SideIndex sideIndex, TurnTo turnTo, Layer layer)
		{
			var fromSideIndex = GetNextSideForRotation(
				sideIndex: sideIndex,
				changedSides: changedOnHorizontalRotationSides,
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
  
	}
}
