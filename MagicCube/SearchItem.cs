using System;

namespace MagicCube
{
	public class SearchItem
	{
		public RubikCube State { get; private set; }
		public int ParentIndex { get; private set; }

		public SearchItem(RubikCube state, int parentIndex)
		{
			if (parentIndex < 0)
				throw new ArgumentOutOfRangeException(nameof(parentIndex));

			State = state;
			ParentIndex = parentIndex;
		}
	}
}