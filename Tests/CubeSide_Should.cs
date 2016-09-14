﻿using System;
using System.Linq;
using MagicCube;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class CubeSide_Should
	{
		private readonly CellColor[] colors = {
				CellColor.Yellow, CellColor.Yellow, CellColor.Orange,
				CellColor.Yellow, CellColor.Red, CellColor.Blue,
				CellColor.Green, CellColor.Red, CellColor.Orange
			};

		[Test]
		public void ReturnColor_ByIndex()
		{
			var cubeSide = new CubeSide(colors);

			for (var i = 0; i < colors.Length; ++i)
				Assert.That(cubeSide[i], Is.EqualTo(colors[i]));
		}

		[Test]
		[TestCase(-1)]
		[TestCase(12)]
		[TestCase(9)]
		public void ThrowIndexOutOfRangeException_ForWrongIndex(int wrongIndex)
		{
			var cubeSide = new CubeSide(colors);

			Assert.Throws<IndexOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var color = cubeSide[wrongIndex];
			});
		}

		[Test]
		public void FillSide_ByOneColor()
		{
			const CellColor sideColor = CellColor.Green;

			var cubeSide = new CubeSide(sideColor);

			Assert.That(cubeSide.All(color => color == sideColor), Is.True);
		}

		[Test]
		public void CreateCopy_FromAnotherCubeSide()
		{
			const CellColor sideColor = CellColor.Blue;
			var side = new CubeSide(sideColor);

			var anotherSide = new CubeSide(side);

			Assert.That(anotherSide.All(color => color == sideColor), Is.True);
		}

		[Test]
		public void ChangeColor_ByIndex()
		{
			var side = new CubeSide(CellColor.White) {[5] = CellColor.Green};

			Assert.That(side[5], Is.EqualTo(CellColor.Green));
		}
	}
}
