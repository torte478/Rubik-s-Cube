using System;
using System.Collections.Generic;
using System.Linq;
using MagicCube.Movement;

namespace MagicCube
{
	public class CubeGenerator
	{
		private const int rotationNumber = 40;

		private readonly Random random = new Random();

		public RubikCube GetRandomCube()
		{
			var actions = new List<CubeCommand>();

			for (var i = 0; i < rotationNumber / 2; ++i)
			{
				actions.Add(GetRandomRotation());
				actions.Add(GetRandomRotation());
				actions.Add(GetRandomTurn());
			}

			return actions.Aggregate(
				GetSolvedCube(), 
				(current, command) => command.Execute(current));
		}

		private CubeCommand GetRandomRotation()
		{
			var actionIndex = random.Next(2);

			if (actionIndex == 0)
			{
				var turnTo = random.Next(2) == 0
					? TurnTo.Left
					: TurnTo.Right;
				return CommandFactory.GetClockwiseRotation(turnTo);
			}
			else
			{
				var turnTo = GetRandomTurnTo();
				var layer = GetRandomLayer();
				return CommandFactory.GetRotation(turnTo, layer);
			}
		}

		private TurnTo GetRandomTurnTo()
		{
			return (TurnTo)random.Next(4);
		}

		private Layer GetRandomLayer()
		{
			return (Layer)random.Next(3);
		}

		private CubeCommand GetRandomTurn()
		{
			var turnTo = GetRandomTurnTo();
			
			return random.Next(2) == 0 
				? CommandFactory.GetTurnToCorner(turnTo) 
				: CommandFactory.GetTurn(turnTo);
		}

		public RubikCube GetSolvedCube()
		{
			return new RubikCube(
				new CubeSide(CellColor.Green),
				new CubeSide(CellColor.White),
				new CubeSide(CellColor.Orange),
				new CubeSide(CellColor.Yellow),
				new CubeSide(CellColor.Blue),
				new CubeSide(CellColor.Red));
		}
	}
}