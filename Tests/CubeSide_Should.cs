using System;
using System.Linq;
using MagicCube;
using NUnit.Framework;

namespace Tests
{
	[TestFixture]
	internal class CubeSide_Should
	{
		private CubeSide cubeSide;

		[SetUp]
		public void SetUp()
		{
			cubeSide = new CubeSide(CellColor.White);
		}

		[Test]
		public void ReturnColor_ByIndex()
		{
			CellColor[] colors = {
				CellColor.Yellow, CellColor.Yellow, CellColor.Orange,
				CellColor.Yellow, CellColor.Red, CellColor.Blue,
				CellColor.Green, CellColor.Red, CellColor.Orange
			};

			var side = new CubeSide(colors);

			for (var i = 0; i < colors.Length; ++i)
				Assert.That(side[i], Is.EqualTo(colors[i]));
		}

		[Test]
		[TestCase(-1)]
		[TestCase(12)]
		[TestCase(9)]
		public void ThrowIndexOutOfRangeException_ForWrongIndex(int wrongIndex)
		{
			Assert.Throws<IndexOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var color = cubeSide[wrongIndex];
			});
		}
		
		[Test]
		public void FillSide_ByOneColor()
		{
			Assert.That(cubeSide.Colors.All(color => color == CellColor.White), Is.True);
		}

		[Test]
		public void CreateCopy_FromAnotherCubeSide()
		{ 
			var anotherSide = new CubeSide(cubeSide);

			Assert.That(anotherSide.Colors.All(color => color == CellColor.White), Is.True);
		}

		[Test]
		public void ChangeColor_ByIndex()
		{
			cubeSide[5] = CellColor.Green;

			Assert.That(cubeSide[5], Is.EqualTo(CellColor.Green));
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
			Assert.That(cubeSide.Colors.Count, Is.EqualTo(9));
		}

		[Test]
		public void ReturnColor_FromTwoIndices()
		{
			Assert.That(cubeSide.GetColor(2, 3), Is.EqualTo(CellColor.White));
		}

		[Test]
		[TestCase(0)]
		[TestCase(4)]
		public void ThrowArgumentOutOfRangeException_ForWrongRowIndex(int wrongRowIndex)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var color = cubeSide.GetColor(wrongRowIndex, 2);
			});
		}

		[Test]
		[TestCase(0)]
		[TestCase(4)]
		public void ThrowArgumentOutOfRangeException_ForWrongColumnIndex(int wrongColumnIndex)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				// ReSharper disable once UnusedVariable
				var color = cubeSide.GetColor(1, wrongColumnIndex);
			});
		}

		[Test]
		public void SetColor_FromTwoIndices()
		{
			cubeSide.SetColor(CellColor.Green, 2, 1);

			Assert.That(cubeSide.GetColor(2, 1), Is.EqualTo(CellColor.Green));
		}

		[Test]
		public void ThrowArgumentOutOfRangeException_ForSetColorForWrongCell()
		{
			Assert.Throws<ArgumentOutOfRangeException>(() =>
			{
				cubeSide.SetColor(CellColor.Blue, -4, 10);
			});
		}
	}
}
