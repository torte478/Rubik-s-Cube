using System;

namespace MagicCube.PathSearch
{
	public class SearchItem
	{
		public RubikCube State { get; private set; }
		public int ParentIndex { get; private set; }
		public CubeCommand Command { get; private set; }

		public SearchItem(RubikCube state, int parentIndex, CubeCommand command)
		{
			if (parentIndex < 0)
				throw new ArgumentOutOfRangeException(nameof(parentIndex));

			State = state;
			ParentIndex = parentIndex;
			Command = command;
		}
	}
}