using System;
using System.Linq;

namespace MagicCube
{
	public class CubeCommand
	{
		private readonly Func<RubikCube, RubikCube>[] actions;

		public CubeCommand(Func<RubikCube, RubikCube> actionFunc)
		{
			actions = new[] {actionFunc};
		}

		public CubeCommand(Func<RubikCube, RubikCube>[] actions)
		{
			this.actions = actions;
		}

		public RubikCube Execute(RubikCube startCube)
		{
			return actions.Aggregate(startCube, (current, action) => action(current));
		}
	}
}