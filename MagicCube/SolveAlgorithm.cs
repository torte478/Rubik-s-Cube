using MagicCube.Movement;

using CubeAction = System.Func<MagicCube.RubikCube, MagicCube.RubikCube>;

namespace MagicCube
{
	public static class SolveAlgorithm
	{
		public static readonly CubeCommand MoveMiddleUpperFrontToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
			cube => cube.MakeClockwiseRotation(TurnTo.Right),
		});

		public static readonly CubeCommand MoveMiddleUpperRightToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeClockwiseRotation(TurnTo.Right)
		});

		public static readonly CubeCommand MoveMiddleUpperLeftToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeRotation(TurnTo.Down, Layer.First),
			cube => cube.MakeClockwiseRotation(TurnTo.Left)
		});

		public static readonly CubeCommand MoveMiddleUpperBackToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeTurn(TurnTo.Left),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeTurn(TurnTo.Right)
		});

		public static readonly CubeCommand MoveMiddleSecondRightToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeTurn(TurnTo.Left),
			cube => cube.MakeRotation(TurnTo.Down, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Left, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Up, Layer.Third),
			cube => cube.MakeTurn(TurnTo.Right)
		});

		public static readonly CubeCommand MoveMiddleSecondLeftToLower = new CubeCommand(new CubeAction[]
		{
			cube => cube.MakeTurn(TurnTo.Right),
			cube => cube.MakeRotation(TurnTo.Down, Layer.First),
			cube => cube.MakeRotation(TurnTo.Right, Layer.Third),
			cube => cube.MakeRotation(TurnTo.Up, Layer.First),
			cube => cube.MakeTurn(TurnTo.Left)
		});
	}
}
