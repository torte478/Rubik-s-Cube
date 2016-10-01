using MagicCube;
using MagicCube.Movement;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class CommandFactory_Should
	{
		[Test]
		public void ReturnRotationAction_FromGetRotation()
		{
			var command = CommandFactory.GetRotation(TurnTo.Right, Layer.Second);

			var nextState = command.Execute(TestHelper.GetCompleteCube());
			Assert.That(nextState[SideIndex.Left].GetColor(2, 2), Is.EqualTo(CellColor.Yellow));
		}

		[Test]
		public void ReturnClockWiseRotationAction_FromGetClockwiseRotation()
		{
			var command = CommandFactory.GetClockwiseRotation(TurnTo.Left);

			var nextState = command.Execute(TestHelper.GetCompleteCube());
			Assert.That(nextState[SideIndex.Right].GetColor(3, 1), Is.EqualTo(CellColor.Blue));
		}

		[Test]
		public void ReturnTurnAction_FromGetTurn()
		{
			var command = CommandFactory.GetTurn(TurnTo.Down);

			var nextState = command.Execute(TestHelper.GetCompleteCube());
			Assert.That(nextState[SideIndex.Down].IsFill(CellColor.Green), Is.True);
		}

		[Test]
		public void ReturnTurnToCornerAction_ForGetTurnToCorner()
		{
			var command = CommandFactory.GetTurnToCorner(TurnTo.Left);

			var nextState = command.Execute(TestHelper.GetCompleteCube());
			Assert.That(nextState[SideIndex.Top].IsFill(CellColor.Orange), Is.True);
		}
	}
}
