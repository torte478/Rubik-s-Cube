using System;
using System.Linq;

namespace MagicCube
{
	public class CubeSide
	{
		public CellColor[] Colors { get; private set; }

		public CubeSide(CellColor[] colors)
		{
			if (colors.Length != 9)
				throw new ArgumentOutOfRangeException(nameof(colors));

			Colors = colors;
		}

		public CubeSide(CellColor sideColor)
		{
			Colors = Enumerable
				.Range(1, 9)
				.Select(i => sideColor)
				.ToArray();
		}

		public CubeSide(CubeSide sideColor)
		{
			Colors = (CellColor[])sideColor.Colors.Clone();
		}

		public CellColor this[int i]
		{
			get { return Colors[i]; }
			set { Colors[i] = value; }
		}

		public CellColor GetColor(int row, int column)
		{
			return Colors[ConvertToIndex(row, column)];
		}

		public void SetColor(CellColor color, int row, int column)
		{
			Colors[ConvertToIndex(row, column)] = color;
		}

		private static int ConvertToIndex(int row, int column)
		{
			if (row < 1 || row > 3)
				throw new ArgumentOutOfRangeException(nameof(row));
			if (column < 1 || column > 3)
				throw new ArgumentOutOfRangeException(nameof(column));

			return (row - 1) * 3 + (column - 1);
		}

		public void MakeClockwiseTurn(bool isForwardTurn)
		{
			var newColors = Colors.Clone() as CellColor[];
			if (newColors == null)
				throw new NullReferenceException();

			for (var i = 1; i <= 3; ++i)
				for (var j = 1; j <= 3; ++j)
				{
					var color = Colors[ConvertToIndex(i, j)];

					var newRow = isForwardTurn ? j : 4 - j;
					var newColumn = isForwardTurn ? 4 - i : i;
					newColors[ConvertToIndex(newRow, newColumn)] = color;
				}

			Colors = newColors;
		}
	}
}