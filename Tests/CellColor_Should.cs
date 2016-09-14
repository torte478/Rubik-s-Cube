using MagicCube;
using NUnit.Framework;
#pragma warning disable 219

namespace Tests
{
    [TestFixture]
    internal class CellColor_Should
    {
        [Test]
        public void Contains_SixColors()
        {
            Assert.DoesNotThrow(() =>
            {
                var yellow = CellColor.Yellow;
                var orange = CellColor.Orange;
                var red = CellColor.Red;
                var blue = CellColor.Blue;
                var green = CellColor.Green;
                var white = CellColor.White;
            });
        }
    }
}
