using System;
using System.Collections.Generic;

namespace MagicCube
{
	public class PathSearhcer
	{
		public PathSearhcer(RubikCube startCube, CubeCommand[] cubeCommands, Func<RubikCube, bool> finishCondition)
		{
			if (startCube == null)
				throw new ArgumentOutOfRangeException(nameof(startCube));
		}

		public List<CubeCommand> GetPath()
		{
			return new List<CubeCommand>();
		}
	}
}