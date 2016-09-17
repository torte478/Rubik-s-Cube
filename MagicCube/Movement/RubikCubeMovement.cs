using System;
using System.Collections.Generic;

namespace MagicCube.Movement
{
	public static class RubikCubeMovement
	{
		private static readonly Side[] changedOnHorizontalTurnSides = { Side.Front, Side.Left, Side.Back, Side.Right };
		private static readonly Side[] changedOnVerticalTurnSides = { Side.Front, Side.Top, Side.Back, Side.Down };

		public static RubikCube MakeRollTurn(this RubikCube rubikCube, TurnTo turnTo)
		{
			return turnTo == TurnTo.Right || turnTo == TurnTo.Left
				? rubikCube.MakeHorizontalTurn(turnTo)
				: rubikCube.MakeVerticalTurn(turnTo);
		}

		private static RubikCube MakeHorizontalTurn(this RubikCube rubikCube, TurnTo turnTo)
		{
			return new RubikCube(
				rubikCube.GetShiftedSide(Side.Front, turnTo),
				rubikCube.GetClockwiseTurnedSide(Side.Top, turnTo == TurnTo.Left ? TurnTo.Right : TurnTo.Left),
				rubikCube.GetShiftedSide(Side.Right, turnTo),
				rubikCube.GetShiftedSide(Side.Back, turnTo),
				rubikCube.GetClockwiseTurnedSide(Side.Down, turnTo != TurnTo.Left ? TurnTo.Right : TurnTo.Left),
				rubikCube.GetShiftedSide(Side.Left, turnTo));
		}

		private static RubikCube MakeVerticalTurn(this RubikCube rubikCube, TurnTo turnTo)
		{
			var newBackSide = rubikCube.GetShiftedSide(Side.Back, turnTo);
			var newTopSide = rubikCube.GetShiftedSide(Side.Top, turnTo);
			var newDownSide = rubikCube.GetShiftedSide(Side.Down, turnTo);

			newBackSide.MakeTwiceClockwiseTurn();
			if (turnTo == TurnTo.Down)
				newTopSide.MakeTwiceClockwiseTurn();
			else
				newDownSide.MakeTwiceClockwiseTurn();

			return new RubikCube(
				rubikCube.GetShiftedSide(Side.Front, turnTo),
				newTopSide,
				rubikCube.GetClockwiseTurnedSide(Side.Right, turnTo == TurnTo.Up ? TurnTo.Right : TurnTo.Left),
				newBackSide,
				newDownSide,
				rubikCube.GetClockwiseTurnedSide(Side.Left, turnTo != TurnTo.Up ? TurnTo.Right : TurnTo.Left));
		}

		private static CubeSide GetShiftedSide(this RubikCube rubikCube, Side side, TurnTo turnTo)
		{
			var changedSides = turnTo == TurnTo.Left || turnTo == TurnTo.Right
				? changedOnHorizontalTurnSides
				: changedOnVerticalTurnSides;

			var shiftFactor = turnTo == TurnTo.Right || turnTo == TurnTo.Down
				? 1
				: -1;

			var newSideIndex = GetNextSideForTurn(side, changedSides, shiftFactor);

			return rubikCube.CloneSide(newSideIndex);
		}

		private static CubeSide GetClockwiseTurnedSide(this RubikCube rubikCube, Side side, TurnTo turnTo)
		{
			var newSide = rubikCube.CloneSide(side);
			newSide.MakeClockwiseTurn(turnTo);
			return newSide;
		}

		private static Side GetNextSideForTurn(Side side, IReadOnlyList<Side> changedSides, int shiftFactor)
		{
			var currentSideIndex = 0;
			while (changedSides[currentSideIndex] != side)
				++currentSideIndex;

			var nextSideIndex = (shiftFactor + currentSideIndex + 4) % 4;
			return changedSides[nextSideIndex];
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
				rubikCube.GetHorizontalTurnedSide(Side.Front, turn, layer),
				rubikCube[Side.Top],
				rubikCube.GetHorizontalTurnedSide(Side.Right, turn, layer),
				rubikCube.GetHorizontalTurnedSide(Side.Back, turn, layer),
				rubikCube[Side.Down],
				rubikCube.GetHorizontalTurnedSide(Side.Left, turn, layer));
		}

		private static CubeSide GetHorizontalTurnedSide(this RubikCube rubikCube, Side side, TurnTo turnTo, Layer layer)
		{
			var rowNumber = (int)layer + 1;
			var newSide = rubikCube.CloneSide(side);
			var fromSide = GetNextSideForTurn(
				side,
				changedOnHorizontalTurnSides,
				turnTo == TurnTo.Right ? 1 : -1);

			for (var column = 1; column <= 3; ++column)
				newSide.SetColor(
					rubikCube[fromSide].GetColor(rowNumber, column),
					rowNumber,
					column);

			return newSide;
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
