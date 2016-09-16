using System;
using System.Linq;

namespace MagicCube
{
	public class RubikCube
	{
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

		public RubikCube MakeTurn(TurnTo turn, Layer layer)
		{
			if (turn == TurnTo.Left || turn == TurnTo.Right)
				return MakeHorizontalTurn(turn, layer);
			else
				return MakeVerticalTurn(turn, layer);
		}

		public CubeSide CloneSide(Side side)
		{
			return new CubeSide(this[side]);
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

		public RubikCube MakeRollTurn(TurnTo right)
		{
			var newTopSide = CloneSide(Side.Top);

			for (var i = 0; i < 3; ++i)
				for (var j = 0; j < 3; ++j)
					newTopSide[i*3 + j] = this[Side.Top][(2 - j % 3) * 3 + i];

			return new RubikCube(
				this[Side.Left],
				newTopSide,
				this[Side.Front],
				this[Side.Right],
				this[Side.Down],
				this[Side.Back]);
		}

		public CellColor GetColor(Side side, int row, int column)
		{
			return this[side].GetColor(row, column);
		}
	}
}