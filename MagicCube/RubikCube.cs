using System.Linq;

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
			var changedSidesIndices = new[] { Side.Front, Side.Left, Side.Back, Side.Right };
			var changedSides = changedSidesIndices.Select(CloneSide).ToArray();
			var startCell = (int)layer * 3;
			var turnDirection = turn == TurnTo.Left ? -1 : 1;

			for (var sideIndex = 0; sideIndex < changedSidesIndices.Length; ++sideIndex)
			{
				var oldSideIndex = (sideIndex + turnDirection + 4) % changedSidesIndices.Length;
				var oldSide = this[changedSidesIndices[oldSideIndex]];

				for (var i = 0; i < 3; ++i)
					changedSides[sideIndex][startCell + i] = oldSide[startCell + i];
			}

			return new RubikCube(
				changedSides[0],
				this[Side.Top],
				changedSides[3],
				changedSides[2],
				this[Side.Down],
				changedSides[1]);
		}

		private RubikCube MakeVerticalTurn(TurnTo turn, Layer layer)
		{
			var changedSidesIndices = new[] { Side.Front, Side.Top, Side.Back, Side.Down };
			var changedSides = changedSidesIndices.Select(side => this[side]).ToArray();
			var startCell = (int)layer;
			var turnDirection = turn == TurnTo.Up ? -1 : 1;

			for (var sideIndex = 0; sideIndex < changedSidesIndices.Length; ++sideIndex)
			{
				var oldSideIndex = (sideIndex + turnDirection + 4) % changedSidesIndices.Length;
				var oldSide = this[changedSidesIndices[oldSideIndex]];

				for (var i = 0; i < 3; ++i)
					changedSides[sideIndex][startCell + i * 3] = oldSide[startCell + i * 3];
			}

			return new RubikCube(
				changedSides[0],
				changedSides[1],
				this[Side.Right],
				changedSides[2],
				changedSides[3],
				this[Side.Left]);
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

			return new RubikCube(
				GetShiftedSide(Side.Front, turnTo),
				GetShiftedSide(Side.Top, turnTo),
				GetClockwiseTurnedSide(Side.Right, isClockWiseForRight),
				GetShiftedSide(Side.Back, turnTo),
				GetShiftedSide(Side.Down, turnTo),
				GetClockwiseTurnedSide(Side.Left, !isClockWiseForRight));
		}

		private CubeSide GetShiftedSide(Side side, TurnTo turnTo)
		{
			var changedSides =
				turnTo == TurnTo.Left || turnTo == TurnTo.Right
					? changedOnHorizontalTurnSides
					: changedOnVerticalTurnSides;

			var shiftFactor =
				turnTo == TurnTo.Right || turnTo == TurnTo.Down
					? 1
					: -1;

			var currentSideIndex = 0;
			while (changedSides[currentSideIndex] != side)
				++currentSideIndex;

			var newSideIndex = (shiftFactor + currentSideIndex + 4) % 4;

			return CloneSide(changedSides[newSideIndex]);
		}

		private CubeSide GetClockwiseTurnedSide(Side side, bool isClockwise)
		{
			var newSide = CloneSide(side);
			newSide.MakeClockwiseTurn(isClockwise);
			return newSide;
		}
	}
}