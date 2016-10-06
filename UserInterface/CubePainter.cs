using System.Drawing;
using System.Windows.Forms;
using MagicCube;

namespace UserInterface
{
    public class CubePainter
    {
        private const int lineNumber = 4;

        private const int leftBorder = 60;
        private const int upBorder = 75;

        private const int xCell = 50;
        private const int x = xCell*3;
        private const int yCell = 60;
        private const int y = yCell*3;
        private const int yStep = yCell/3;

        private readonly Graphics graphics;
        private readonly PictureBox pictureBox;
        private readonly Pen linePen;

        public CubePainter(Graphics graphics, PictureBox pictureBox)
        {
            this.graphics = graphics;
            this.pictureBox = pictureBox;
            linePen = new Pen(Color.Black, 5);
        }

        public void Draw(RubikCube cube)
        {
            DrawColors(cube);
            DrawBase();
            pictureBox.Invalidate();
        }

        private void DrawColors(RubikCube cube)
        {
            DrawFrontColors(cube[SideIndex.Front]);
            DrawRightColors(cube[SideIndex.Right]);
            DrawTopColors(cube[SideIndex.Top]);
        }

        private void DrawFrontColors(CubeSide cubeSide)
        {
            for (var i = 0; i < 3; ++i)
                for (var j = 0; j < 3; ++j)
                {
                    var color = GetColor(cubeSide.GetColor(i + 1, j + 1));
                    using (var brush = new SolidBrush(color))
                        graphics.FillPolygon(brush, new[]
                        {
                            new Point(leftBorder + xCell * j, upBorder + yStep * j + yCell * i),
                            new Point(leftBorder + xCell * j + xCell, upBorder + yStep + yStep*j  + yCell * i),  
                            new Point(leftBorder + xCell * j + xCell, upBorder + yStep + yStep*j + yCell  + yCell * i),
                            new Point(leftBorder + xCell * j, upBorder + yStep * j + yCell  + yCell * i), 
                        });
                }
        }

        private Color GetColor(CellColor getColor)
        {
            switch (getColor)
            {
                case CellColor.Blue:
                    return Color.Blue;
                case CellColor.Red:
                    return Color.Red;
                case CellColor.Green:
                    return Color.Green;
                case CellColor.Orange:
                    return Color.Orange;
                case CellColor.White:
                    return Color.White;
                case CellColor.Yellow:
                    return Color.Yellow;
            }
            return Color.MediumVioletRed;
        }

        private void DrawRightColors(CubeSide cubeSide)
        {
            for (var i = 0; i < 3; ++i)
                for (var j = 0; j < 3; ++j)
                {
                    var color = GetColor(cubeSide.GetColor(i + 1, j + 1));
                    using (var brush = new SolidBrush(color))
                        graphics.FillPolygon(brush, new[]
                        {
                            new Point(leftBorder + xCell * j + x, upBorder - yStep * j + yCell * i + yCell),
                            new Point(leftBorder + xCell * j + xCell + x, upBorder - yStep - yStep*j  + yCell * i + yCell),
                            new Point(leftBorder + xCell * j + xCell + x, upBorder - yStep - yStep*j + yCell + yCell * i + yCell),
                            new Point(leftBorder + xCell * j + x, upBorder- yStep * j + yCell  + yCell * i + yCell),
                        });
                }
        }

        private void DrawTopColors(CubeSide cubeSide)
        {
            for (var i = 0; i < 3; ++i)
                for (var j = 0; j < 3; ++j)
                {
                    var color = GetColor(cubeSide.GetColor(i + 1, j + 1));
                    using (var brush = new SolidBrush(color))
                        graphics.FillPolygon(brush, new[]
                        {
                            new Point(leftBorder + x + xCell * j - xCell * i, upBorder - yCell + yStep * j + yStep * i),
                            new Point(leftBorder + x + xCell + xCell * j - xCell * i, upBorder - yCell + yStep + yStep * j + yStep * i),
                            new Point(leftBorder + x + xCell * j - xCell * i, upBorder - yCell + 2 * yStep + yStep * j + yStep * i),
                            new Point(leftBorder + x - xCell + xCell * j - xCell * i, upBorder - yCell + yStep + yStep * j + yStep * i),
                        });
                }
        }

        private void DrawBase()
        {
            for (var i = 0; i < lineNumber; ++i)
                graphics.DrawLine(linePen, 
                    leftBorder + xCell * i, 
                    upBorder + yStep * i, 
                    leftBorder + xCell * i, 
                    upBorder + yStep * i + y);

            for (var i = 0; i < lineNumber; ++i)
                graphics.DrawLine(linePen,
                    leftBorder + x + xCell * i,
                    upBorder + yCell - yStep * i,
                    leftBorder + x + xCell * i,
                    upBorder + yCell - yStep * i + y);

            for (var i = 0; i < lineNumber; ++i)
                graphics.DrawLine(linePen,
                    leftBorder,
                    upBorder + yCell * i,
                    leftBorder + x,
                    upBorder + yCell * i + yCell);

            for (var i = 0; i < lineNumber; ++i)
                graphics.DrawLine(linePen,
                    leftBorder + x,
                    upBorder + yCell + yCell * i,
                    leftBorder + x + x,
                    upBorder + yCell + yCell * i - yCell);

            for (var i = 0; i < lineNumber; ++i)
                graphics.DrawLine(linePen,
                    leftBorder + xCell * i,
                    upBorder + yStep * i,
                    leftBorder + x + xCell * i,
                    upBorder + yStep * i - yCell);

            for (var i = 0; i < lineNumber; ++i)
                graphics.DrawLine(linePen,
                    leftBorder + xCell * i,
                    upBorder - yStep * i,
                    leftBorder + x + xCell * i,
                    upBorder - yStep * i + yCell);
        }
    }
}