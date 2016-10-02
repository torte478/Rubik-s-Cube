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

		public static RubikCube GetCubeWithConcreteCell(SideIndex sideIndex, int rowIndex, int columnIndex, CellColor color)
		{
			return GetCompleteCube()
				.SetColor(sideIndex, rowIndex, columnIndex, color);
		}
	}
}
