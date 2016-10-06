using System.Drawing;
using MagicCube;
using MetroFramework.Forms;

namespace UserInterface
{
    public partial class Form1 : MetroForm
    {
        private readonly CubeGenerator cubeGenerator;
        private readonly CubePainter cubePainter;

        public Form1()
        {
            InitializeComponent();
            canvasPictureBox.Image = new Bitmap(canvasPictureBox.Width, canvasPictureBox.Height);
            cubePainter = new CubePainter(Graphics.FromImage(canvasPictureBox.Image), canvasPictureBox);
            cubeGenerator = new CubeGenerator();

            cubePainter.Draw(cubeGenerator.GetRandomCube());
        }
    }
}
