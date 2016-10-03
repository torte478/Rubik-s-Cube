using System;
using System.Collections.Generic;
using System.Linq;

namespace MagicCube.PathSearch
{
	public class PathSearhcer
	{
		private const int startStateIndex = 0;
		private const int MaxHandledElementsCount = 100000;

		public List<SearchItem> HandledStates { get; }
		public List<CubeCommand> Path { get; }
		public RubikCube GoalState => HandledStates.Last().State;

		public PathSearhcer(RubikCube startState, CubeCommand[] commands, Func<RubikCube, bool> goalCondition)
		{
			HandledStates = new List<SearchItem> { new SearchItem(startState, startStateIndex, null) };
			Path = new List<CubeCommand>();

			MakeSearch(commands, goalCondition);

			ReconstructPath();
		}

		private void MakeSearch(CubeCommand[] commands, Func<RubikCube, bool> goalCondition)
		{
			var nextStateIndices = GetInitializedQueue();

			while (IsFindRespondState(goalCondition) == false)
			{
				if (HandledStates.Count >= MaxHandledElementsCount)
					throw new InvalidOperationException("searcher can not find path");

				var currentStateIndex = nextStateIndices.Dequeue();

				foreach (var command in commands)
				{
					HandledStates.Add(new SearchItem(
						command.Execute(HandledStates[currentStateIndex].State),
						currentStateIndex,
						command));

					if (IsFindRespondState(goalCondition))
						break;

					nextStateIndices.Enqueue(HandledStates.Count - 1);
				}
			}
		}

		private static Queue<int> GetInitializedQueue()
		{
			var nextStateIndices = new Queue<int>();
			nextStateIndices.Enqueue(startStateIndex);
			return nextStateIndices;
		}

		private bool IsFindRespondState(Func<RubikCube, bool> goalCondition)
		{
			return goalCondition(HandledStates.Last().State);
		}

		private void ReconstructPath()
		{
			var currentIndex = HandledStates.Count - 1;

			while (currentIndex != startStateIndex)
			{
				Path.Add(HandledStates[currentIndex].Command);
				currentIndex = HandledStates[currentIndex].ParentIndex;
			}

			Path.Reverse();
		}
	}
}