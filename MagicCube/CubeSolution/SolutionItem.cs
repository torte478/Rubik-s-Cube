﻿using System.Collections.Generic;

namespace MagicCube.CubeSolution
{
	public struct SolutionItem
	{
		public List<CubeCommand> Actions { get; set; }
		public RubikCube GoalState { get; set; }
        public int MaxHandledElementsCount { get; set; }
        public int MaxRecursionDeep { get; set; }
	}
}