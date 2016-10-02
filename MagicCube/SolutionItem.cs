using System.Collections.Generic;

namespace MagicCube
{
	public struct SolutionItem
	{
		public List<CubeCommand> Actions { get; set; }
		public RubikCube GoalState { get; set; }
	}
}