using System;
using System.Linq;

namespace MagicCube
{
	public class CubeSide
	{
		public CellColor[] Colors { get; }

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
	}
}