﻿using System.Linq;
using MagicCube;
using MagicCube.CubeSolution;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    internal class RubikCubeAPI_Should
    {
        private RubikCubeAPI cubeAPI;

        [SetUp]
        public void SetUp()
        {
            cubeAPI = new RubikCubeAPI();
        }

        [Test]
        public void ReturnNotNull_FromGetSolvedCube()
        {
            Assert.That(cubeAPI.GetSolvedCube(), Is.Not.Null);
        }

        [Test]
        public void ReturnNotNull_FromGetRandomCube()
        {
            Assert.That(cubeAPI.GetRandomCube(), Is.Not.Null);
        }

        [Test]
        public void ReturnFalse_WhenCanNotSolveCube()
        {
            var wrongCube = TestHelper.GetCompleteCube().SetColor(SideIndex.Front, 2, 2, CellColor.Red);

            Assert.That(cubeAPI.SolveCube(wrongCube), Is.False);
        }

        [Test]
        public void ReturnTrue_WhenCanSolveCube()
        {
            var cube = TestHelper.GetCompleteCube();

            Assert.That(cubeAPI.SolveCube(cube), Is.True);
        }

        [Test]
        public void NotHaveActions_WhenStarting()
        {
            Assert.That(cubeAPI.Actions.Length, Is.EqualTo(0));
        }

        [Test]
        public void HaveActions_AfterSolving()
        {
            cubeAPI.SolveCube(TestHelper.GetCompleteCube());

            Assert.That(cubeAPI.Actions.Length, Is.Not.EqualTo(0));
        }

        [Test]
        public void HaveCorrectActions_AfterSolving()
        {
            var testCube = TestHelper.GetNotSolvedCube();

            cubeAPI.SolveCube(testCube);

            testCube = cubeAPI.Actions.Aggregate(testCube, (current, action) => action(current));
            Assert.That(AlgorithmBase.IsSolvedCube(testCube));
        }

        [Test]
        public void HaveZeroRotationCount_WhenStarting()
        {
            Assert.That(cubeAPI.RotationCount, Is.EqualTo(0));
        }

        [Test]
        public void HaveRotationCount_AfterSolving()
        {
            cubeAPI.SolveCube(TestHelper.GetCompleteCube());

            Assert.That(cubeAPI.RotationCount, Is.Not.EqualTo(0));
        }

        [Test]
        public void NotHaveStates_WhenStarting()
        {
            Assert.That(cubeAPI.States.Length, Is.EqualTo(0));
        }

        [Test]
        public void HaveStates_AfterSolving()
        {
            cubeAPI.SolveCube(TestHelper.GetCompleteCube());

            Assert.That(cubeAPI.States.Length, Is.Not.EqualTo(0));
        }

        [Test]
        public void CalculateRotationCount_FormActions()
        {
            cubeAPI.SolveCube(TestHelper.GetCompleteCube());

            Assert.That(cubeAPI.RotationCount, Is.LessThan(cubeAPI.Actions.Length));
        }
    }
}