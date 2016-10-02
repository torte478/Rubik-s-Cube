using System;
using System.Linq;
using MagicCube;

namespace Tests
{
	internal static class TestCubeConstructor
	{
		public static RubikCube SetColor(this RubikCube cube, SideIndex sideIndex, int rowIndex, int columnIndex,
			CellColor cellColor)
		{
			var newSides = Enum.GetValues(typeof(SideIndex)).Cast<SideIndex>()
				.Select(cube.CloneSide)
				.ToArray();

			newSides[(int)sideIndex].SetColor(cellColor, rowIndex, columnIndex);

			return new RubikCube(newSides[0], newSides[1], newSides[2], newSides[3], newSides[4], newSides[5]);
		}
	}
}
