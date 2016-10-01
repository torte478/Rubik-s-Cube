using System;

namespace MagicCube.Movement
{
	public static partial class RubikCubeMovement
	{
		public static RubikCube MakeTurn(this RubikCube rubikCube, TurnTo turnTo)
		{
			return turnTo == TurnTo.Right || turnTo == TurnTo.Left
				? rubikCube.MakeHorizontalTurn(turnTo)
				: rubikCube.MakeVerticalTurn(turnTo);
		}

		public static RubikCube MakeTurnToCorner(this RubikCube rubikCube, TurnTo turnTo)
		{
			if (turnTo == TurnTo.Up || turnTo == TurnTo.Down)
			{
				return rubikCube.MakeTurn(turnTo);
			}
			else
				return rubikCube
					.MakeTurn(TurnTo.Down)
					.MakeTurn(turnTo)
					.MakeTurn(TurnTo.Up);
		}

		public static RubikCube MakeRotation(this RubikCube rubikCube, TurnTo turn, Layer layer)
		{
			if (turn == TurnTo.Left || turn == TurnTo.Right)
				return rubikCube.MakeHorizontalRotation(turn, layer);
			else
				return rubikCube.MakeVerticalRotation(turn, layer);
		}


		/// <param name="rubikCube"></param>
		/// <param name="turnTo">left or right</param>
		public static RubikCube MakeClockwiseRotation(this RubikCube rubikCube, TurnTo turnTo)
		{
			if (turnTo == TurnTo.Up || turnTo == TurnTo.Down)
				throw new InvalidOperationException("wrong turnTo direction");

			return rubikCube
				.MakeTurn(TurnTo.Down)
				.MakeRotation(turnTo, Layer.Third)
				.MakeTurn(TurnTo.Up);
		}
	}
}
