using System;
using System.Drawing;
using MagicCube;
using MagicCube.Movement;
using MetroFramework.Forms;

namespace UserInterface
{
    public partial class Form1 : MetroForm
    {
        private readonly CubeGenerator cubeGenerator;
        private readonly CubePainter cubePainter;
        private RubikCube startCube;

        public Form1()
        {
            InitializeComponent();
            canvasPictureBox.Image = new Bitmap(canvasPictureBox.Width, canvasPictureBox.Height);
            cubePainter = new CubePainter(Graphics.FromImage(canvasPictureBox.Image), canvasPictureBox);
            cubeGenerator = new CubeGenerator();

            startCube = cubeGenerator.GetSolvedCube();
            cubePainter.Draw(startCube);
        }

        private void ChangeCube(RubikCube cube)
        {
            startCube = cube;
            cubePainter.Draw(startCube);
        }

        private void metroButton2_Click(object sender, System.EventArgs e)
        {
            ChangeCube(cubeGenerator.GetSolvedCube());
        }

        private void metroButton1_Click(object sender, System.EventArgs e)
        {
            ChangeCube(cubeGenerator.GetRandomCube());
        }

        private void pictureBox16_Click(object sender, System.EventArgs e)
        {
            ChangeCube(startCube.MakeTurn(TurnTo.Up));
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeTurn(TurnTo.Down));
        }

        private void pictureBox13_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeTurn(TurnTo.Right));
        }

        private void pictureBox15_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeTurn(TurnTo.Left));
        }

        private void pictureBox18_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeTurnToCorner(TurnTo.Right));
        }

        private void pictureBox17_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeTurnToCorner(TurnTo.Left));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeRotation(TurnTo.Up, Layer.First));
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeRotation(TurnTo.Up, Layer.Second));
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeRotation(TurnTo.Up, Layer.Third));
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeRotation(TurnTo.Down, Layer.First));
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeRotation(TurnTo.Down, Layer.Second));
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeRotation(TurnTo.Down, Layer.Third));
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeRotation(TurnTo.Right, Layer.First));
        }

        private void pictureBox11_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeRotation(TurnTo.Right, Layer.Second));
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeRotation(TurnTo.Right, Layer.Third));
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeRotation(TurnTo.Left, Layer.First));
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeRotation(TurnTo.Left, Layer.Second));
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeRotation(TurnTo.Left, Layer.Third));
        }

        private void pictureBox19_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeClockwiseRotation(TurnTo.Right));
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            ChangeCube(startCube.MakeClockwiseRotation(TurnTo.Left));
        }
    }
}
