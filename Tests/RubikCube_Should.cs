using System.Linq;
using MagicCube;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class RubikCube_Should
	{
		private readonly CubeSide frontSide = new CubeSide(CellColor.Green);
		private readonly CubeSide topSide = new CubeSide(CellColor.White);
		private readonly CubeSide rightSide = new CubeSide(CellColor.Orange);
		private readonly CubeSide leftSide = new CubeSide(CellColor.Red);
		private readonly CubeSide backSide = new CubeSide(CellColor.Yellow);
		private readonly CubeSide downSide = new CubeSide(CellColor.Blue);

		private RubikCube cube;

		[SetUp]
		public void SetUp()
		{
			cube = new RubikCube(frontSide, topSide, rightSide, backSide, downSide, leftSide);
		}

		[Test]
		[TestCase(Side.Front, CellColor.Green)]
		[TestCase(Side.Top, CellColor.White)]
		[TestCase(Side.Right, CellColor.Orange)]
		[TestCase(Side.Back, CellColor.Yellow)]
		[TestCase(Side.Down, CellColor.Blue)]
		[TestCase(Side.Left, CellColor.Red)]
		public void HaveCorrectSide_AfterInitialization(Side side, CellColor sideColor)
		{
			Assert.That(cube[side].Colors.All(c => c == sideColor), Is.True);
		}

		[Test]
		public void CorrectChangeSides_AfterTurnToLeftFirstLayer()
		{
			var expectedColors = Enumerable
				.Range(1, 3)
				.Select(i => CellColor.Orange)
				.Concat(Enumerable
					.Range(1, 6)
					.Select(i => CellColor.Green))
				.ToArray();

			var nextState = cube.Turn(Turn.Left, 1);

			Assert.That(nextState[Side.Front].Colors, Is.EqualTo(expectedColors));
		}
	}
}
