using MagicCube.Movement;

namespace MagicCube
{
	public static class CommandFactory
	{
		public static CubeCommand GetRotation(TurnTo turnTo, Layer layer)
		{
			return new CubeCommand(cube => cube.MakeRotation(turnTo, layer), $"Rotate to {turnTo} {layer} layer");
		}

		public static CubeCommand GetClockwiseRotation(TurnTo turnTo)
		{
			return new CubeCommand(cube => cube.MakeClockwiseRotation(turnTo), $"Clockwise rotate to {turnTo}");
		}

		public static CubeCommand GetTurn(TurnTo turnTo)
		{
			return new CubeCommand(cube => cube.MakeTurn(turnTo), $"Turn to {turnTo}");
		}

		public static CubeCommand GetTurnToCorner(TurnTo turnTo)
		{
			return new CubeCommand(cube => cube.MakeTurnToCorner(turnTo), $"Turn to corner to {turnTo}");
		}
	}
}