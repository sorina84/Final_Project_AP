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

        private bool _isClosingAfterGameEnd;
        private Button _backToMenuButton;

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
            _isClosingAfterGameEnd = false;

            KeyDown += GameForm_KeyDown;
            KeyUp += GameForm_KeyUp;

            CreateBackToMenuButton();

            _gameTimer.Start();
        }

        private void CreateBackToMenuButton()
        {
            _backToMenuButton = new Button();

            _backToMenuButton.Text = "BACK TO MENU";
            _backToMenuButton.Width = 220;
            _backToMenuButton.Height = 50;
            _backToMenuButton.Left = (ClientSize.Width - _backToMenuButton.Width) / 2;
            _backToMenuButton.Top = ClientSize.Height / 2 + 125;

            _backToMenuButton.Font = new Font("Arial", 13, FontStyle.Bold);
            _backToMenuButton.BackColor = Color.FromArgb(28, 31, 46);
            _backToMenuButton.ForeColor = Color.White;
            _backToMenuButton.FlatStyle = FlatStyle.Flat;
            _backToMenuButton.FlatAppearance.BorderColor = Color.FromArgb(255, 70, 180);
            _backToMenuButton.FlatAppearance.BorderSize = 2;
            _backToMenuButton.Cursor = Cursors.Hand;
            _backToMenuButton.Visible = false;

            _backToMenuButton.MouseEnter += (sender, e) =>
            {
                _backToMenuButton.BackColor = Color.FromArgb(55, 58, 80);
                _backToMenuButton.ForeColor = Color.FromArgb(0, 210, 255);
            };

            _backToMenuButton.MouseLeave += (sender, e) =>
            {
                _backToMenuButton.BackColor = Color.FromArgb(28, 31, 46);
                _backToMenuButton.ForeColor = Color.White;
            };

            _backToMenuButton.Click += (sender, e) =>
            {
                Close();
            };

            Controls.Add(_backToMenuButton);
            _backToMenuButton.BringToFront();
        }

        private void GameLoop(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            float deltaTime = (float)(now - _lastUpdate).TotalSeconds;
            _lastUpdate = now;

            _world.SetInput(_leftPressed, _rightPressed, _upPressed, _downPressed, _shootPressed);
            _world.Update(deltaTime);

            if (_world.IsGameOver && !_isClosingAfterGameEnd)
            {
                _isClosingAfterGameEnd = true;
                _gameTimer.Stop();

                _backToMenuButton.Visible = true;
                _backToMenuButton.BringToFront();

                Invalidate();
                return;
            }

            Invalidate();
        }

        private void GameForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (_world.IsGameOver)
                return;

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

            if (_backToMenuButton != null)
                _backToMenuButton.BringToFront();
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            if (_gameTimer != null)
            {
                _gameTimer.Stop();
                _gameTimer.Dispose();
            }

            base.OnFormClosed(e);
        }
    }
}