using System;
using System.Collections.Generic;

namespace MagicCube
{
	public class RubikCube
	{
		private static readonly Side[] changedOnHorizontalTurnSides = { Side.Front, Side.Left, Side.Back, Side.Right };
		private static readonly Side[] changedOnVerticalTurnSides = {Side.Front, Side.Top, Side.Back, Side.Down};

		private readonly CubeSide[] sides;

		public RubikCube(CubeSide frontSide, CubeSide topSide, CubeSide rightSide, CubeSide backSide, CubeSide downSide, CubeSide leftSide)
		{
			sides = new[]
			{
				new CubeSide(frontSide),
				new CubeSide(topSide),
				new CubeSide(rightSide),
				new CubeSide(backSide),
				new CubeSide(downSide),
				new CubeSide(leftSide)
			};
		}

		public CubeSide this[Side side] => sides[(int)side];

		public CubeSide CloneSide(Side side)
		{
			return new CubeSide(this[side]);
		}

		public CellColor GetColor(Side side, int row, int column)
		{
			return this[side].GetColor(row, column);
		}

		public RubikCube MakeTurn(TurnTo turn, Layer layer)
		{
			if (turn == TurnTo.Left || turn == TurnTo.Right)
				return MakeHorizontalTurn(turn, layer);
			else
				return MakeVerticalTurn(turn, layer);
		}

		private RubikCube MakeHorizontalTurn(TurnTo turn, Layer layer)
		{
			return new RubikCube(
				GetHorizontalTurnedSide(Side.Front, turn, layer),
				this[Side.Top],
				GetHorizontalTurnedSide(Side.Right, turn, layer),
				GetHorizontalTurnedSide(Side.Back, turn, layer),
				this[Side.Down],
				GetHorizontalTurnedSide(Side.Left, turn, layer));
		}

		private CubeSide GetHorizontalTurnedSide(Side side, TurnTo turnTo, Layer layer)
		{
			var rowNumber = (int) layer + 1;
			var newSide = CloneSide(side);
			var fromSide = GetNextSideForTurn(
				side,  
				changedOnHorizontalTurnSides,
				turnTo == TurnTo.Right ? 1 : -1);

			for (var column = 1; column <= 3; ++column)
				newSide.SetColor(
					this[fromSide].GetColor(rowNumber, column), 
					rowNumber, 
					column);

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

		private RubikCube MakeVerticalTurn(TurnTo turn, Layer layer)
		{
			return MakeTurnToCorner(TurnTo.Right)
				.MakeHorizontalTurn(
					turn == TurnTo.Up ? TurnTo.Right : TurnTo.Left, 
					layer)
				.MakeTurnToCorner(TurnTo.Left);
		}

		public RubikCube MakeRollTurn(TurnTo turnTo)
		{
			return turnTo == TurnTo.Right || turnTo == TurnTo.Left 
				? MakeHorizontalTurn(turnTo) 
				: MakeVerticalTurn(turnTo);
		}

		private RubikCube MakeHorizontalTurn(TurnTo turnTo)
		{
			var isClockwiseForTop = turnTo == TurnTo.Left;
			
			return new RubikCube(
				GetShiftedSide(Side.Front, turnTo),
				GetClockwiseTurnedSide(Side.Top, isClockwiseForTop),
				GetShiftedSide(Side.Right, turnTo),
				GetShiftedSide(Side.Back, turnTo),
				GetClockwiseTurnedSide(Side.Down, !isClockwiseForTop),
				GetShiftedSide(Side.Left, turnTo));
		}

		private RubikCube MakeVerticalTurn(TurnTo turnTo)
		{
			var isClockWiseForRight = turnTo == TurnTo.Up;

			var newBackSide = GetShiftedSide(Side.Back, turnTo);
			var newTopSide = GetShiftedSide(Side.Top, turnTo);
			var newDownSide = GetShiftedSide(Side.Down, turnTo);

			newBackSide.MakeTwiceTurn();
			if (turnTo == TurnTo.Down)
				newTopSide.MakeTwiceTurn();
			else
				newDownSide.MakeTwiceTurn();

			return new RubikCube(
				GetShiftedSide(Side.Front, turnTo),
				newTopSide,
				GetClockwiseTurnedSide(Side.Right, isClockWiseForRight),
				newBackSide,
				newDownSide,
				GetClockwiseTurnedSide(Side.Left, !isClockWiseForRight));
		}

		private CubeSide GetShiftedSide(Side side, TurnTo turnTo)
		{
			var changedSides = turnTo == TurnTo.Left || turnTo == TurnTo.Right
				? changedOnHorizontalTurnSides
				: changedOnVerticalTurnSides;

			var shiftFactor = turnTo == TurnTo.Right || turnTo == TurnTo.Down
				? 1
				: -1;

			var newSideIndex = GetNextSideForTurn(side, changedSides, shiftFactor);

			return CloneSide(newSideIndex);
		}

		private CubeSide GetClockwiseTurnedSide(Side side, bool isClockwise)
		{
			var newSide = CloneSide(side);
			newSide.MakeClockwiseTurn(isClockwise);
			return newSide;
		}

		public RubikCube MakeTurnToCorner(TurnTo turnTo)
		{
			if (turnTo == TurnTo.Up || turnTo == TurnTo.Down)
				throw new ArgumentOutOfRangeException(nameof(turnTo));

			return MakeRollTurn(TurnTo.Down)
				.MakeRollTurn(turnTo)
				.MakeRollTurn(TurnTo.Up);
		}
	}
}