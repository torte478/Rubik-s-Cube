using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MagicCube;
using MagicCube.Movement;
using MetroFramework.Forms;

namespace UserInterface
{
    public partial class Form1 : MetroForm
    {
        private readonly RubikCubeAPI cubeAPI = new RubikCubeAPI();
        private readonly CubePainter cubePainter;

        private RubikCube startCube;
        private int currentStateIndex;

        public Form1()
        {
            InitializeComponent();
            canvasPictureBox.Image = new Bitmap(canvasPictureBox.Width, canvasPictureBox.Height);
            cubePainter = new CubePainter(Graphics.FromImage(canvasPictureBox.Image), canvasPictureBox);

            startCube = cubeAPI.GetSolvedCube();
            cubePainter.Draw(startCube);
        }

        private void ChangeCube(RubikCube cube)
        {
            startCube = cube;
            cubePainter.Draw(startCube);
        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            ChangeCube(cubeAPI.GetSolvedCube());
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {
            ChangeCube(cubeAPI.GetRandomCube());
        }

        private void pictureBox16_Click(object sender, EventArgs e)
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

        private void metroButton3_Click(object sender, EventArgs e)
        {
            metroLabel3.Text = @"Выполняется поиск...";
            
            //TODO: make a pause

            if (cubeAPI.SolveCube(startCube) == false)
            {
                File.WriteAllText("error log.txt", startCube.ToString());
                MessageBox.Show(@"Произошла неизвестная ошибка! Попробуйте другую конфигурацию кубика");
                return;
            }

            currentStateIndex = 0;
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            metroLabel3.Text = $@"Шаг {currentStateIndex} из {cubeAPI.Actions.Length}";
            metroLabel1.Text = $@"Требуется вращений: {cubeAPI.RotationCount}";
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ++currentStateIndex;
            if (currentStateIndex >= cubeAPI.States.Length)
            {
                timer1.Enabled = false;
                MessageBox.Show(@"Кубик собран!");
                return;
            }
            ChangeCube(cubeAPI.States[currentStateIndex]);
            UpdateLabels();
        }

        private void pictureBox22_Click(object sender, EventArgs e)
        {
            timer1.Enabled = !timer1.Enabled;
        }

        private void metroTrackBar1_Scroll(object sender, ScrollEventArgs e)
        {
            timer1.Enabled = false;
            timer1.Interval = metroTrackBar1.Value;
            timer1.Enabled = true;
        }
    }
}
