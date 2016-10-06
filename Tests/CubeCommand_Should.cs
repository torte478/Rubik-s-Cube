using System;
using MagicCube;
using MagicCube.Movement;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class CubeCommand_Should
	{
		[Test]
		public void ReturnStartCube_ForSimpleCommand()
		{
			var command = new CubeCommand(cube => cube);

			var newState = command.Execute(TestHelper.GetCompleteCube());

			Assert.That(newState[SideIndex.Back].IsFill(CellColor.Yellow), Is.True);
		}

		[Test]
		public void ReturnNextState_ForRotationCommand()
		{
			var command = new CubeCommand(cube => cube.MakeRotation(TurnTo.Right, Layer.Second));

			var newState = command.Execute(TestHelper.GetCompleteCube());

			Assert.That(newState[SideIndex.Right].GetColor(2, 2), Is.EqualTo(CellColor.Green));
		}

		[Test]
		public void ExecuteActionRange_ForArrayOfActions()
		{
			var command = new CubeCommand(new Func<RubikCube, RubikCube>[]
			{
			    cube => cube.MakeRotation(TurnTo.Right, Layer.First),
			    cube => cube.MakeRotation(TurnTo.Down, Layer.Third)
			});

			var newState = command.Execute(TestHelper.GetCompleteCube());

			Assert.That(newState[SideIndex.Down].GetColor(1, 3), Is.EqualTo(CellColor.Red));
		}

	    [Test]
	    public void HaveDefaultName_AfterInitialization()
	    {
	        var command = new CubeCommand(new Func<RubikCube, RubikCube>[] {});

            Assert.That(command.ToString(), Is.EqualTo("Unknown command"));
	    }

	    [Test]
	    public void InitializeName_FromConstructor()
	    {
	        var command = new CubeCommand(new Func<RubikCube, RubikCube>[] {}, "Command 1");

            Assert.That(command.ToString(), Is.EqualTo("Command 1"));
	    }

	    [Test]
	    public void InitializeName_FromUpdatedConstructor()
	    {
	        var command = new CubeCommand(x => null, "Command 2");
            
            Assert.That(command.ToString(), Is.EqualTo("Command 2"));
	    }
	}
}
