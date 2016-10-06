using System;
using System.Linq;

namespace MagicCube
{
	public class CubeCommand
	{
	    private readonly string description;
	    public Func<RubikCube, RubikCube>[] Actions { get;}

	    public CubeCommand(Func<RubikCube, RubikCube> actionFunc, string description = "Unknown command")
		{
			Actions = new[] {actionFunc};
		    this.description = description;
		}

	    public CubeCommand(Func<RubikCube, RubikCube>[] actions, string description = "Unknown command")
		{
			Actions = actions;
		    this.description = description;
		}

	    public RubikCube Execute(RubikCube startCube)
		{
			return Actions.Aggregate(startCube, (current, action) => action(current));
		}

	    public override string ToString()
	    {
	        return description;
	    }
	}
}