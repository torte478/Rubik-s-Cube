using System;
using MagicCube;
using MagicCube.CubeSolution;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class AcceptanceTests
	{
		private const int accptanceTestsCount = 10;

		private CubeGenerator generator;
		private Solver solver;

		[SetUp]
		public void SetUp()
		{
			generator = new CubeGenerator();
			solver = new Solver();
		}

		[Test]
        [Ignore("very long test")]
		public void AcceptanceTest()
		{
			for (var i = 0; i < accptanceTestsCount; ++i)
			{
				Console.WriteLine($"{i}: ==============");
				SolveCube();
				Console.WriteLine("Ok");
				Console.WriteLine("");
			}
		}

		private void SolveCube()
		{
			var testCube = generator.GetRandomCube();

			Console.Write(testCube.ToString());

			var solution = solver.SolveCube(testCube);

			Assert.That(AlgorithmBase.IsSolvedCube(solution.GoalState), Is.True);
		}
	}
}
