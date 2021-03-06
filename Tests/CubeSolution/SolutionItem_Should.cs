﻿using System.Collections.Generic;
using MagicCube;
using MagicCube.CubeSolution;
using NUnit.Framework;

namespace Tests.CubeSolution
{
	[TestFixture]
	internal class SolutionItem_Should
	{
		[Test]
		public void HaveActionField()
		{
			var item = new SolutionItem
			{
				Actions = new List<CubeCommand>()
			};

			Assert.That(item.Actions, Is.Not.Null);
		}

		[Test]
		public void HaveGoalStateField()
		{
			var item = new SolutionItem
			{
				GoalState = TestHelper.GetCompleteCube()
			};

			Assert.That(item.GoalState, Is.Not.Null);
		}

	    [Test]
	    public void HaveMaxRecursionDeepField()
	    {
	        var item = new SolutionItem
	        {
	            MaxRecursionDeep = 123
	        };

            Assert.That(item.MaxRecursionDeep, Is.Not.Null);
	    }

	    [Test]
	    public void HaveMaxHandledElementsCount()
	    {
	        var item = new SolutionItem
	        {
	            MaxHandledElementsCount = 321
	        };

            Assert.That(item.MaxHandledElementsCount, Is.Not.Null);
	    }
	}
}
