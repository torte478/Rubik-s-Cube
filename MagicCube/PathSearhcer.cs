using System;
using System.Collections.Generic;
using System.Linq;

namespace MagicCube
{
	public class PathSearhcer
	{
		private const int MaxHandledElementsCount = 100000;

		public List<SearchItem> HandledStates { get; private set; }
		
		public PathSearhcer(RubikCube startState, CubeCommand[] commands, Func<RubikCube, bool> goalCondition)
		{
			MakeSearch(startState, commands, goalCondition);
		}

		private void MakeSearch(RubikCube startState, CubeCommand[] commands, Func<RubikCube, bool> goalCondition)
		{
			var nextStateIndices = GetInitializedQueue(startState);

			while (IsFindRespondState(goalCondition) == false)
			{
				if (HandledStates.Count >= MaxHandledElementsCount)
					throw new InvalidOperationException("searcher can not find path");

				var currentStateIndex = nextStateIndices.Dequeue();
				var currentState = HandledStates[currentStateIndex].State;

				foreach (var command in commands)
				{
					HandledStates.Add(new SearchItem(
						command.Execute(currentState),
						currentStateIndex));

					if (IsFindRespondState(goalCondition))
						break;

					nextStateIndices.Enqueue(HandledStates.Count - 1);
				}
			}
		}

		private Queue<int> GetInitializedQueue(RubikCube startState)
		{
			HandledStates = new List<SearchItem> { new SearchItem(startState, 0) };
			var nextStateIndices = new Queue<int>();
			nextStateIndices.Enqueue(0);
			return nextStateIndices;
		}

		private bool IsFindRespondState(Func<RubikCube, bool> goalCondition)
		{
			return goalCondition(HandledStates.Last().State);
		}
	}
}