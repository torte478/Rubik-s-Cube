using System.Linq;

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

		public RubikCube Turn(Turn right, int i)
		{
			var newSides = new[] {Side.Front, Side.Left, Side.Back, Side.Right};
			var newStateSides = newSides.Select(side => this[side]).ToArray();

			for (var sideIndex = 0; sideIndex < 4; ++sideIndex)
			{
				var oldSide = this[newSides[(sideIndex + 3)%4]];
				for (var cell = 0; cell < 3; ++cell)
					newStateSides[sideIndex][cell] = oldSide[cell];
			}

			return new RubikCube(
				newStateSides[0],
				this[Side.Top],
				newStateSides[3],
				newStateSides[2],
				this[Side.Down],
				newStateSides[1]);
		}
	}
}