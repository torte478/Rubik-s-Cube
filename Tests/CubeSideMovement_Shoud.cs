﻿using System;
using MagicCube;
using MagicCube.Movement;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class CubeSideMovement_Shoud
	{ 
		private CubeSide complexCubeSide;

		[SetUp]
		public void SetUp()
		{
			complexCubeSide = new CubeSide(new[]
			{
				CellColor.White, CellColor.Red, CellColor.Green,
				CellColor.Blue, CellColor.Orange, CellColor.Yellow,
				CellColor.Red, CellColor.Blue, CellColor.Green
			});
		}

		[Test]
		public void ReturnReferenceToThis_FromClockwiseRotateMethod()
		{
			var currentSide = complexCubeSide.MakeClockwiseRotate(TurnTo.Right);

			Assert.That(currentSide.Colors, Is.EqualTo(complexCubeSide.Colors));
		}

		[Test]
		public void ChangeColors_AfterClockwiseRotate()
		{
			complexCubeSide.MakeClockwiseRotate(TurnTo.Right);

			Assert.That(complexCubeSide.Colors, Is.EqualTo(new[]
			{
				CellColor.Red, CellColor.Blue, CellColor.White,
				CellColor.Blue, CellColor.Orange, CellColor.Red,
				CellColor.Green, CellColor.Yellow, CellColor.Green
			}));
		}

		[Test]
		public void MoveColors_AfterNotClockwiseRotate()
		{
			complexCubeSide.MakeClockwiseRotate(TurnTo.Left);

			Assert.That(complexCubeSide.Colors, Is.EqualTo(new[]
			{
				CellColor.Green, CellColor.Yellow, CellColor.Green,
				CellColor.Red, CellColor.Orange, CellColor.Blue,
				CellColor.White, CellColor.Blue, CellColor.Red,
			}));
		}

		[Test]
		public void ChangeColors_AfterTwiceClockwiceRotate()
		{
			complexCubeSide.MakeTwiceClockwiseRotate();

			Assert.That(complexCubeSide.Colors, Is.EqualTo(new[]
			{
				CellColor.Green, CellColor.Blue, CellColor.Red,
				CellColor.Yellow, CellColor.Orange, CellColor.Blue,
				CellColor.Green, CellColor.Red, CellColor.White
			}));
		}

		[Test]
		[TestCase(TurnTo.Up)]
		[TestCase(TurnTo.Down)]
		public void ThrowArgumentOutOfRangeException_ForWrongClockwiseRotateDirection(TurnTo wrongTurnTo)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				complexCubeSide.MakeClockwiseRotate(wrongTurnTo);
			});
		}
	}
}
