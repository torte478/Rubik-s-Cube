using System;
using System.Linq;
using MagicCube;
using MagicCube.Movement;
using MagicCube.PathSearch;
using NUnit.Framework;
using Condition = System.Func<MagicCube.RubikCube, bool>;

namespace Tests.PathSearch
{
	[TestFixture]
	internal class PathSearcher_Should
	{
		[Test]
		public void HaveHandledState_WhenStartStateRespondsToCondition()
		{
			var commands = new CubeCommand[] {};
			
			var searcher = new PathSearhcer(TestHelper.GetCompleteCube(), commands, cube => true);

			Assert.That(searcher.HandledStates.Count, Is.EqualTo(1));
		}

		[Test]
		public void HandleAllCommands_WhileNotFindGoal()
		{
			var commands = new[] {CommandFactory.GetRotation(TurnTo.Right, Layer.First)};
			Condition condition = cube => cube[SideIndex.Left].GetColor(1, 1) == CellColor.Green;

			var searcher = new PathSearhcer(TestHelper.GetCompleteCube(), commands, condition);

			Assert.That(searcher.HandledStates.Count, Is.EqualTo(4));
		}

		[Test]
		public void StopHandling_WhenTargetWasFinded()
		{
			var commands = new[]
			{
				CommandFactory.GetRotation(TurnTo.Right, Layer.Second),
				CommandFactory.GetRotation(TurnTo.Left, Layer.Second)
			};
			Condition condition = cube => cube[SideIndex.Left].GetColor(2, 2) == CellColor.Green;

			var searcher = new PathSearhcer(TestHelper.GetCompleteCube(), commands, condition);

			Assert.That(searcher.HandledStates.Count, Is.EqualTo(3));
		}

		[Test]
		public void ThrowInvalidOperationException_WhenCanNotFindTarget()
		{
			var commands = new[] {CommandFactory.GetRotation(TurnTo.Right, Layer.First)};
			Condition condtition = cube => cube[SideIndex.Front].GetColor(3, 3) != CellColor.Green;

			Assert.Throws<InvalidOperationException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var searcher = new PathSearhcer(TestHelper.GetCompleteCube(), commands, condtition);
			});
		}

		[Test]
		public void ReturnEmptyPath_WhenStartPositionRespondsToCondition()
		{
			var searcher = new PathSearhcer(TestHelper.GetCompleteCube(), new CubeCommand[] {}, cube => true);

			Assert.That(searcher.Path.Count, Is.EqualTo(0));
		}

		[Test]
		public void ReturnPathFromOneElement_ForSimpleCase()
		{
			var commands = new[] {CommandFactory.GetRotation(TurnTo.Right, Layer.First)};
			Condition condtiion = cube => cube[SideIndex.Right].GetColor(1, 1) == CellColor.Green;

			var searcher = new PathSearhcer(TestHelper.GetCompleteCube(), commands, condtiion);

			Assert.That(searcher.Path.Count, Is.EqualTo(1));
		}

		[Test]
		public void HaveSensiblePath_AfterInitialization()
		{
			var commands = new[] { CommandFactory.GetRotation(TurnTo.Right, Layer.First) };
			Condition condtiion = cube => cube[SideIndex.Right].GetColor(1, 1) == CellColor.Green;
			var searcher = new PathSearhcer(TestHelper.GetCompleteCube(), commands, condtiion);

			var testCube = TestHelper.GetCompleteCube();
			testCube = searcher.Path.Aggregate(testCube, (current, cubeCommand) => cubeCommand.Execute(current));

			Assert.That(condtiion(testCube), Is.True);
		}
	}
}
