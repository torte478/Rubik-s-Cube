namespace MagicCube
{
	public class RubikCube
	{
		private readonly CubeSide[] sides;

		public RubikCube(CubeSide frontSide, CubeSide topSide, CubeSide rightSide, CubeSide backSide, CubeSide downSide, CubeSide leftSide)
		{
			sides = new[]
			{
				frontSide,
				topSide,
				rightSide,
				backSide,
				downSide,
				leftSide
			};
		}

		public CubeSide this[Side side] => new CubeSide(sides[(int)side]);
	}
}