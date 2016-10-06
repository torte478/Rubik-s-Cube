using System;
using System.Linq;

namespace MagicCube
{
	public class CubeCommand
	{
		private readonly Func<RubikCube, RubikCube>[] actions;

	    private readonly string description;

		public CubeCommand(Func<RubikCube, RubikCube> actionFunc, string description = "Unknown command")
		{
			actions = new[] {actionFunc};
		    this.description = description;
		}

		public CubeCommand(Func<RubikCube, RubikCube>[] actions, string description = "Unknown command")
		{
			this.actions = actions;
		    this.description = description;
		}

	    public RubikCube Execute(RubikCube startCube)
		{
			return actions.Aggregate(startCube, (current, action) => action(current));
		}

	    public override string ToString()
	    {
	        return description;
	    }
	}
}