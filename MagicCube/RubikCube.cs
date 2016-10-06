using System.Text;

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

		public override string ToString()
		{
			var toString = new StringBuilder();
			toString.AppendLine("Front:");
			toString.AppendLine(sides[0].ToString());
			toString.AppendLine("Top:");
			toString.AppendLine(sides[1].ToString());
			toString.AppendLine("Right:");
			toString.AppendLine(sides[2].ToString());
			toString.AppendLine("Back:");
			toString.AppendLine(sides[3].ToString());
			toString.AppendLine("Down:");
			toString.AppendLine(sides[4].ToString());
			toString.AppendLine("Left:");
			toString.AppendLine(sides[5].ToString());
			return toString.ToString();
		}
	}
}