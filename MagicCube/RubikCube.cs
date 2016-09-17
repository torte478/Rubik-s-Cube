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

		public CubeSide CloneSide(Side side)
		{
			return new CubeSide(this[side]);
		}

		public CellColor GetColor(Side side, int row, int column)
		{
			return this[side].GetColor(row, column);
		}
	}
}