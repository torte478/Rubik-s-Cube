﻿using System;

namespace MagicCube.Movement
{
	public static class CubeSideMovement
	{
		public static CubeSide MakeClockwiseTurn(this CubeSide cubeSide, TurnTo turnTo)
		{
			if (turnTo == TurnTo.Up || turnTo == TurnTo.Down)
				throw new ArgumentOutOfRangeException(nameof(turnTo));

			var oldSide = new CubeSide(cubeSide);
			var isClockwiseTurn = turnTo == TurnTo.Right;

			for (var i = 1; i <= 3; ++i)
				for (var j = 1; j <= 3; ++j)
				{
					var color = oldSide.GetColor(i, j);
					var newRow = isClockwiseTurn ? j : 4 - j;
					var newColumn = isClockwiseTurn ? 4 - i : i;

					cubeSide.SetColor(color, newRow, newColumn);
				}

			return cubeSide;
		}

		public static CubeSide MakeTwiceClockwiseTurn(this CubeSide cubeSide)
		{
			return cubeSide
				.MakeClockwiseTurn(TurnTo.Right)
				.MakeClockwiseTurn(TurnTo.Right);
		}
	}
}