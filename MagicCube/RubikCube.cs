namespace MagicCube
{
	public class RubikCube
	{
		private readonly CubeSide[] sides;

		public RubikCube(
			CubeSide frontSide, CubeSide topSide,  CubeSide rightSide, 
			CubeSide backSide,  CubeSide downSide, CubeSide leftSide)
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

		public CubeSide this[SideIndex sideIndex] => sides[(int)sideIndex];

		public CubeSide CloneSide(SideIndex sideIndex)
		{
			return new CubeSide(this[sideIndex]);
		}

		public CellColor GetColor(SideIndex sideIndex, int rowIndex, int columnIndex)
		{
			return this[sideIndex].GetColor(rowIndex, columnIndex);
		}
	}
}