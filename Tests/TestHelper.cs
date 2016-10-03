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

		public static RubikCube GetNotSolvedCube()
		{
			return new RubikCube(
				new CubeSide(new[]
				{
					CellColor.White, CellColor.Blue, CellColor.Yellow,
					CellColor.Yellow, CellColor.Green, CellColor.White,
					CellColor.Blue, CellColor.Green, CellColor.Green
				}),
				new CubeSide(new[]
				{
					CellColor.Yellow, CellColor.Yellow, CellColor.Orange,
					CellColor.Red, CellColor.White, CellColor.Green,
					CellColor.Green, CellColor.Green, CellColor.White
				}),
				new CubeSide(new[]
				{
					CellColor.Red, CellColor.Orange, CellColor.Green,
					CellColor.Yellow, CellColor.Orange, CellColor.Orange,
					CellColor.Red, CellColor.Red, CellColor.Yellow
				}),
				new CubeSide(new[]
				{
					CellColor.Blue, CellColor.Blue, CellColor.White,
					CellColor.White, CellColor.Yellow, CellColor.Yellow,
					CellColor.Blue, CellColor.Green, CellColor.Blue
				}),
				new CubeSide(new[]
				{
					CellColor.Red, CellColor.Red, CellColor.White,
					CellColor.Blue, CellColor.Blue, CellColor.White,
					CellColor.Red, CellColor.White, CellColor.Orange
				}),
				new CubeSide(new[]
				{
					CellColor.Orange, CellColor.Blue, CellColor.Orange,
					CellColor.Orange, CellColor.Red, CellColor.Red,
					CellColor.Yellow, CellColor.Orange, CellColor.Green
				}));
		}
	}
}
