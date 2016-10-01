using System;
using MagicCube;
using MagicCube.Movement;
using NUnit.Framework;

using Condition = System.Func<MagicCube.RubikCube, bool>;

namespace Tests
{
	[TestFixture]
	internal class PathSearcher_Should
	{
		[Test]
		public void HaveHandledState_WhenStartStateRespondsToCondition()
		{
			var startState = TestHelper.GetCompleteCube();
			var commands = new CubeCommand[] {};
			
			var searcher = new PathSearhcer(startState, commands, cube => true);

			Assert.That(searcher.HandledStates.Count, Is.EqualTo(1));
		}

		[Test]
		public void HandleAllCommands_WhileNotFindGoal()
		{
			var startState = TestHelper.GetCompleteCube();
			var commands = new[] {CommandFactory.GetRotation(TurnTo.Right, Layer.First)};
			Condition condition = cube => cube[SideIndex.Left].GetColor(1, 1) == CellColor.Green;

			var searcher = new PathSearhcer(startState, commands, condition);

			Assert.That(searcher.HandledStates.Count, Is.EqualTo(4));
		}

		[Test]
		public void StopHandling_WhenTargetWasFinded()
		{
			var startState = TestHelper.GetCompleteCube();
			var commands = new[]
			{
				CommandFactory.GetRotation(TurnTo.Right, Layer.Second),
				CommandFactory.GetRotation(TurnTo.Left, Layer.Second)
			};
			Condition condition = cube => cube[SideIndex.Left].GetColor(2, 2) == CellColor.Green;

			var searcher = new PathSearhcer(startState, commands, condition);

			Assert.That(searcher.HandledStates.Count, Is.EqualTo(3));
		}

		[Test]
		public void ThrowInvalidOperationException_WhenCanNotFindTarget()
		{
			var startState = TestHelper.GetCompleteCube();
			var commands = new[] {CommandFactory.GetRotation(TurnTo.Right, Layer.First)};
			Condition condtition = cube => cube[SideIndex.Front].GetColor(3, 3) != CellColor.Green;

			Assert.Throws<InvalidOperationException>(() =>
			{
				var searcher = new PathSearhcer(startState, commands, condtition);
			});
		}
	}
}
