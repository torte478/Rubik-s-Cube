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

        private static bool IsEqualCubes(RubikCube state, RubikCube nextState)
        {
            var movementState = nextState.MakeTurn(TurnTo.Right);
            for (var i = 0; i < 4; ++i)
                if (HasEqialOrientation(state, movementState) == false)
                {
                    movementState = movementState.MakeTurn(TurnTo.Left);
                }
            for (var i = 0; i < 4; ++i)
                if (HasEqialOrientation(state, movementState) == false)
                {
                    movementState = movementState.MakeTurn(TurnTo.Up);
                }

            for (var i = 0; i < 6; ++i)
                for (var j = 0; j < 9; ++j)
                {
                    if (state[(SideIndex) i].Colors[j] != movementState[(SideIndex) i].Colors[j])
                        return false;
                }

            return true;
        }

        private static bool HasEqialOrientation(RubikCube state, RubikCube movementCube)
        {
            return movementCube[SideIndex.Front].GetCenterColor() == state[SideIndex.Front].GetCenterColor()
                   && movementCube[SideIndex.Top].GetCenterColor() == state[SideIndex.Top].GetCenterColor();
        }
    }
}