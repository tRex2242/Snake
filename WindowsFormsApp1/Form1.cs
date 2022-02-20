using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private List<Circle> Snake = new List<Circle>();
        private Circle food = new Circle();

        public Form1()
        {
            InitializeComponent();
            new Settings();
            timer.Interval = 1000 / Settings.Speed;
            timer.Start();

            Start_game();

        }
    

        void MovePLayer()
        {
            for(int i = Snake.Count - 1; i >= 0; i--)
            {
                if (i == 0)
                {
                    switch (Settings.direction)
                    {
                        case Direction.Up:
                            Snake[i].Y--;
                            break;
                        case Direction.Down:
                            Snake[i].Y++;
                            break;
                        case Direction.Left:
                            Snake[i].X--;
                            break;
                        case Direction.Right:
                            Snake[i].X++;
                            break;
                    }

                    int X = Snake[i].X;
                    int Y = Snake[i].Y;
                    int maxXPos = pbCanvas.Width / Settings.Wight;
                    int maxYPos = pbCanvas.Height / Settings.Height;

                    if ((X < 0) || (X >= maxXPos) || (Y < 0) || (Y >= maxYPos))
                    {
                        Die();
                        return;

                    }

                    for (int a = 1; a < Snake.Count; a++)
                    {
                        if (X == Snake[a].X && Y == Snake[a].Y)
                        {
                            Die();
                            return;
                        }
                    }

                    if (X == food.X && Y == food.Y)
                    {
                        Eat();
                    }

                }
                else
                {
                    Snake[i].X = Snake[i - 1].X;
                    Snake[i].Y = Snake[i - 1].Y;
                }
            }
        }

        void Start_game()
        {
            lblGameOver.Visible = false;
            new Settings();

            Snake.Clear();
            Circle head = new Circle();
            head.X = 10;
            head.Y = 5;
            Snake.Add(head);

            score.Text = $"Score: {Settings.Score}";
            Generate_food();
        }

        void Generate_food()
        {
            int maxXPos = pbCanvas.Width / Settings.Wight;
            int maxYPos = pbCanvas.Height/ Settings.Height;

            Random random = new Random();
            food = new Circle();
            food.X = random.Next(0, maxXPos);
            food.Y = random.Next(0, maxYPos);
        }

        void Die()
        {
            Settings.GameOver = true;
        }

        void Eat()
        {
            Circle tail = new Circle();
            tail.X = Snake[Snake.Count - 1].X;
            tail.Y = Snake[Snake.Count - 1].Y;
            Snake.Add(tail);

            Settings.Score += Settings.Points;
            score.Text = $"Score: {Settings.Score}";
            Generate_food();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Settings.GameOver)
            {
                if (e.KeyCode == Keys.Enter)
                {
                    Start_game();
                }
            }
            else
            {
                switch (e.KeyCode)
                {
                    case Keys.Left:
                        if(Settings.direction != Direction.Right){
                            Settings.direction = Direction.Left;}
                        break;
                    case Keys.Right:
                        if (Settings.direction != Direction.Left)
                        {
                            Settings.direction = Direction.Right;
                        }//у меня сдесь гит хаб не подключен, да
                        break;
                    case Keys.Up:
                        if (Settings.direction != Direction.Down)
                        {
                            Settings.direction = Direction.Up;
                        }
                        break;
                    case Keys.Down:
                        if (Settings.direction != Direction.Up)
                        {
                            Settings.direction = Direction.Down;
                        }
                        break;
                }
            }
        }

        private void pbCanvas_Paint(object sender, PaintEventArgs e)
        {
            Graphics canvas = e.Graphics;
            if (!Settings.GameOver)
            {
                Brush color;
                for (int i = 0; i < Snake.Count; i++)
                {
                    if (i == 0)
                    {
                        color = Brushes.Red;
                    }
                    else
                    {
                        color = Brushes.Black;
                    }
                    canvas.FillEllipse(color, new Rectangle(
                        Snake[i].X * Settings.Wight,
                        Snake[i].Y * Settings.Height,
                        Settings.Height,
                        Settings.Wight));
                }
                canvas.FillEllipse(Brushes.Green, new Rectangle(
                            food.X * Settings.Wight,
                            food.Y * Settings.Height,
                            Settings.Height,
                            Settings.Wight));
            }
            else
            {
                string message = $"Game Over \nYou final score: {Settings.Score}";
                lblGameOver.Text = message;
                lblGameOver.Visible = true;
            }
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!Settings.GameOver)
            {
                MovePLayer();
            }
            pbCanvas.Invalidate();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

}
