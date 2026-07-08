using System;
using System.Drawing;
using System.Windows.Forms;
using GameEntity;

namespace SpaceShooter
{
    public partial class GameForm : Form
    {
        private GameWorld _world;
        private System.Windows.Forms.Timer _gameTimer;
        private DateTime _lastUpdate;

        private bool _leftPressed;
        private bool _rightPressed;
        private bool _upPressed;
        private bool _downPressed;
        private bool _shootPressed;

        public GameForm()
        {
            DoubleBuffered = true;
            Width = 800;
            Height = 600;
            Text = "Space Shooter";
            StartPosition = FormStartPosition.CenterScreen;
            KeyPreview = true;

            _world = new GameWorld(ClientSize.Width, ClientSize.Height);

            _gameTimer = new System.Windows.Forms.Timer();
            _gameTimer.Interval = 16;
            _gameTimer.Tick += GameLoop;

            _lastUpdate = DateTime.Now;

            KeyDown += GameForm_KeyDown;
            KeyUp += GameForm_KeyUp;

            _gameTimer.Start();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            float deltaTime = (float)(now - _lastUpdate).TotalSeconds;
            _lastUpdate = now;

            _world.SetInput(_leftPressed, _rightPressed, _upPressed, _downPressed, _shootPressed);
            _world.Update(deltaTime);

            Invalidate();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.A:
                    _leftPressed = true;
                    break;

                case Keys.Right:
                case Keys.D:
                    _rightPressed = true;
                    break;

                case Keys.Up:
                case Keys.W:
                    _upPressed = true;
                    break;

                case Keys.Down:
                case Keys.S:
                    _downPressed = true;
                    break;

                case Keys.Space:
                    _shootPressed = true;
                    break;

                case Keys.Escape:
                    _world.IsPaused = !_world.IsPaused;
                    break;
            }
        }

        private void GameForm_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.A:
                    _leftPressed = false;
                    break;

                case Keys.Right:
                case Keys.D:
                    _rightPressed = false;
                    break;

                case Keys.Up:
                case Keys.W:
                    _upPressed = false;
                    break;

                case Keys.Down:
                case Keys.S:
                    _downPressed = false;
                    break;

                case Keys.Space:
                    _shootPressed = false;
                    break;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            _world.Render(e.Graphics);
        }
    }
}