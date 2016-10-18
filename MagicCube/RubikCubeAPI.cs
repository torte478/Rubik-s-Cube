using System;
using System.Collections.Generic;
using System.Linq;
using MagicCube.CubeSolution;
using MagicCube.Movement;

namespace MagicCube
{
    public class RubikCubeAPI
    {
        private readonly CubeGenerator cubeGenerator = new CubeGenerator();
        private readonly Solver solver = new Solver();

        public Func<RubikCube, RubikCube>[] Actions { get; private set; }
        public RubikCube[] States { get; private set; }
        public int RotationCount { get; private set; }

        public RubikCubeAPI()
        {
            Actions = new Func<RubikCube, RubikCube>[] {};
            States = new RubikCube[] {};
        }

        public RubikCube GetSolvedCube()
        {
            return cubeGenerator.GetSolvedCube();
        }

        public RubikCube GetRandomCube()
        {
            return cubeGenerator.GetRandomCube();
        }

        public bool SolveCube(RubikCube cube)
        {
            SolutionItem solutionItem;
            try
            {
                solutionItem = solver.SolveCube(cube);
            }
            catch (InvalidOperationException)
            {
                return false;
            }

            ConvertCommandsToAction(cube, solutionItem.Actions);
            RotationCount = GetRotationCount();

            return true;
        }

        private void ConvertCommandsToAction(RubikCube cube, IEnumerable<CubeCommand> commands)
        {
            var actions = new List<Func<RubikCube, RubikCube>>();
            var states = new List<RubikCube> {cube};

            foreach (var cubeCommand in commands)
            {
                foreach (var commandAction in cubeCommand.Actions)
                {
                    actions.Add(commandAction);
                    states.Add(commandAction(states.Last()));
                }
            }

            Actions = actions.ToArray();
            States = states.ToArray();
        }

        private int GetRotationCount()
        {
            var rotationCount = 0;

            for (var i = 0; i < States.Length - 1; ++i)
            {
                var state = States[i];
                var nextState = States[i + 1];

                if (IsEqualCubes(state, nextState) == false)
                    ++rotationCount;
            }

            return rotationCount;
        }

        public static bool IsEqualCubes(RubikCube cube, RubikCube otherCube)
        {
            var tempCube = OrientateToCube(cube, otherCube);
            return HaveEqualCells(cube, tempCube);
        }

        private static RubikCube OrientateToCube(RubikCube goalCube, RubikCube cube)
        {
            var tempCube = OrientateTopToCorrectSide(goalCube, cube);

            while (HasFrontOnCorrectSide(goalCube, tempCube) == false)
            {
                tempCube = tempCube.MakeTurn(TurnTo.Right);
            }

            return tempCube;
        }

        private static RubikCube OrientateTopToCorrectSide(RubikCube goalCube, RubikCube cube)
        {
            var tempCube = cube.MakeTurn(TurnTo.Left).MakeTurn(TurnTo.Right);

            if (HasTopSideOnTopOfDown(goalCube, tempCube) == false)
            {
                while (HasCorrectOrientedTopSide(goalCube, tempCube) == false)
                {
                    tempCube = tempCube.MakeTurn(TurnTo.Right);
                }
            }

            while (HasTopOnCorrectSide(goalCube, tempCube) == false)
            {
                tempCube = tempCube.MakeTurn(TurnTo.Up);
            }

            return tempCube;
        }

        private static bool HasFrontOnCorrectSide(RubikCube goalCube, RubikCube tempCube)
        {
            return tempCube[SideIndex.Front].GetCenterColor() == goalCube[SideIndex.Front].GetCenterColor();
        }

        private static bool HasTopOnCorrectSide(RubikCube goalCube, RubikCube tempCube)
        {
            return tempCube[SideIndex.Top].GetCenterColor() == goalCube[SideIndex.Top].GetCenterColor();
        }

        private static bool HasCorrectOrientedTopSide(RubikCube goalCube, RubikCube tempCube)
        {
            return tempCube[SideIndex.Front].GetCenterColor() == goalCube[SideIndex.Top].GetCenterColor();
        }

        private static bool HasTopSideOnTopOfDown(RubikCube goalCube, RubikCube tempCube)
        {
            return tempCube[SideIndex.Top].GetCenterColor() == goalCube[SideIndex.Top].GetCenterColor()
                   || tempCube[SideIndex.Down].GetCenterColor() == goalCube[SideIndex.Down].GetCenterColor();
        }

        private static bool HaveEqualCells(RubikCube cube, RubikCube otherCube)
        {
            for (var side = 0; side < 6; ++side)
                for (var cell = 0; cell < 9; ++cell)
                {
                    if (IsEqualCellColors(cube, otherCube, side, cell) == false)
                        return false;
                }

            return true;
        }

        private static bool IsEqualCellColors(
            RubikCube cube, 
            RubikCube otherCube, 
            int side, 
            int cell)
        {
            var sideIndex = (SideIndex) side;
            return cube[sideIndex][cell] == otherCube[sideIndex][cell];
        }
    }
}