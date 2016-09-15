using System;
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
		public void ProvideAccess_ToColors()
		{
			var cubeSide = new CubeSide(CellColor.Orange);

			cubeSide.Colors[4] = CellColor.Blue;
		}

		[Test]
		public void FillSide_ByOneColor()
		{
			const CellColor sideColor = CellColor.Green;

			var cubeSide = new CubeSide(sideColor);

			Assert.That(cubeSide.Colors.All(color => color == sideColor), Is.True);
		}

		[Test]
		public void CreateCopy_FromAnotherCubeSide()
		{
			const CellColor sideColor = CellColor.Blue;
			var side = new CubeSide(sideColor);

			var anotherSide = new CubeSide(side);

			Assert.That(anotherSide.Colors.All(color => color == sideColor), Is.True);
		}

		[Test]
		public void ChangeColor_ByIndex()
		{
			var side = new CubeSide(CellColor.White) {[5] = CellColor.Green};

			Assert.That(side[5], Is.EqualTo(CellColor.Green));
		}

		[Test]
		public void ThrowOutOfRangeException_IfColorsCountNotEqualto9()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var side = new CubeSide(new[] {CellColor.White, CellColor.White});
			});
		}

		[Test]
		public void HaveNineColors_AfterOneColorFill()
		{
			var side = new CubeSide(CellColor.Red);

			Assert.That(side.Colors.Count, Is.EqualTo(9));
		}

		[Test]
		public void ReturnColor_FromTwoIndices()
		{
			var side = new CubeSide(CellColor.Orange);

			Assert.That(side.GetColor(2, 3), Is.EqualTo(CellColor.Orange));
		}

		[Test]
		[TestCase(0)]
		[TestCase(4)]
		public void ThrowArgumentOutOfRangeException_ForWrongRowIndex(int wrongRowIndex)
		{
			var side = new CubeSide(CellColor.White);

			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var color = side.GetColor(wrongRowIndex, 2);
			});
		}

		[Test]
		[TestCase(0)]
		[TestCase(4)]
		public void ThrowArgumentOutOfRangeException_ForWrongColumnIndex(int wrongColumnIndex)
		{
			var side = new CubeSide(CellColor.White);

			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var color = side.GetColor(1, wrongColumnIndex);
			});
		}

		[Test]
		public void SetColor_FromTwoIndices()
		{
			var side = new CubeSide(CellColor.Blue);

			side.SetColor(CellColor.Green, 2, 1);

			Assert.That(side.GetColor(2, 1), Is.EqualTo(CellColor.Green));
		}

		[Test]
		public void ThrowArgumentOutOfRangeException_ForSetColorForWrongCell()
		{
			var side = new CubeSide(CellColor.Red);

			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				side.SetColor(CellColor.Blue, -4, 10);
			});
		}
	}
}
