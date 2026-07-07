using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SpaceShooter.Core;

namespace SpaceShooter
{
    public partial class GameForm : Form
    {
        private GameWorld world;
        private Timer gameTimer;
        private DateTime lastUpdate;


        public GameForm()
        {
            InitializeComponent();

            DoubleBuffered = true;
            Width = 800;
            Height = 600;
            KeyPreview = true;
            world = new GameWorld(800, 600);
            gameTimer = new Timer();
            gameTimer.Interval = 16;
            lastUpdate = DateTime.Now;
            gameTimer.Tick += GameLoop;
            gameTimer.Start();
            KeyDown += GameForm_KeyDown;
        }


        private void GameLoop(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            float deltaTime =(float)(now - lastUpdate).TotalSeconds;
            lastUpdate = now;
            world.Update(deltaTime);
            Invalidate();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    world.MovePlayerLeft();
                    break;

                case Keys.Right:
                    world.MovePlayerRight();
                    break;

                case Keys.Up:
                    world.MovePlayerUp();
                    break;

                case Keys.Down:
                    world.MovePlayerDown();
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            world.Render(e.Graphics);
        }
    }
}
