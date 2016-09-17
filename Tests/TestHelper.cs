using System;
using System.Linq;
using MagicCube;

namespace Tests
{
	internal static class TestHelper
	{
		private static readonly CubeSide frontSide = new CubeSide(CellColor.Green);
		private static readonly CubeSide topSide = new CubeSide(CellColor.White);
		private static readonly CubeSide rightSide = new CubeSide(CellColor.Orange);
		private static readonly CubeSide leftSide = new CubeSide(CellColor.Red);
		private static readonly CubeSide backSide = new CubeSide(CellColor.Yellow);
		private static readonly CubeSide downSide = new CubeSide(CellColor.Blue);

		public static RubikCube GetCompleteCube()
		{
			return new RubikCube(frontSide, topSide, rightSide, backSide, downSide, leftSide);
		}

		public static RubikCube GetCubeWithConcreteCell(SideIndex side, int row, int column, CellColor color)
		{
			var cube = GetCompleteCube();

			var newSides = Enum.GetValues(typeof(SideIndex)).Cast<SideIndex>()
				.Select(curentSide => cube.CloneSide(curentSide))
				.ToArray();

			newSides[(int)side].SetColor(color, row, column);

			return new RubikCube(newSides[0], newSides[1], newSides[2], newSides[3], newSides[4], newSides[5]);
		}
	}
}
