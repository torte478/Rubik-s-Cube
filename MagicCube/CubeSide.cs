using System;
using System.Linq;

namespace MagicCube
{
	public class CubeSide
	{
		private const int cellCount = 9;

		public CellColor[] Colors { get; }

		public CubeSide(CellColor[] colors)
		{
			if (colors.Length != cellCount)
				throw new ArgumentOutOfRangeException(nameof(colors));

			Colors = colors;
		}

		public CubeSide(CellColor sideColor)
		{
			Colors = Enumerable
				.Range(1, cellCount)
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

		/// <param name="row">from 1 to 3</param>
		/// <param name="column">from 1 to 3</param>
		private static int ConvertToIndex(int row, int column)
		{
			if (row < 1 || row > 3)
				throw new ArgumentOutOfRangeException(nameof(row));
			if (column < 1 || column > 3)
				throw new ArgumentOutOfRangeException(nameof(column));

			return (row - 1) * 3 + (column - 1);
		}

		public bool IsFill(CellColor color)
		{
			return Colors.All(curColor => curColor == color);
		}
	}
}