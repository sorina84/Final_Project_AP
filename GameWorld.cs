using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameEntity
{
    public class GameWorld
    {
        public Player Player { get; private set; }
        public WaveManager WaveManager { get; private set; }

        public List<Bullet> PlayerBullets { get; private set; }
        public List<Bullet> EnemyBullets { get; private set; }
        public List<PowerUp> PowerUps { get; private set; }
        public List<Coin> Coins { get; private set; }
        public List<Explosion> Explosions { get; private set; }

        public bool IsGameOver { get; private set; }
        public bool IsWin { get; private set; }
        public bool IsPaused { get; set; }

        public int ScreenWidth { get; private set; }
        public int ScreenHeight { get; private set; }

        private bool _leftPressed;
        private bool _rightPressed;
        private bool _upPressed;
        private bool _downPressed;
        private bool _shootPressed;

        private float _powerUpSpawnTimer;
        private const float PowerUpSpawnInterval = 10f;

        private readonly Random _random = new Random();
        private Image _background;

        public GameWorld(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;

            LoadBackground();

            Player = new Player(width / 2f, height - 100f);
            Player.ScreenWidth = width;
            Player.ScreenHeight = height;
            Player.Reset();

            WaveManager = new WaveManager(Player);

            PlayerBullets = new List<Bullet>();
            EnemyBullets = new List<Bullet>();
            PowerUps = new List<PowerUp>();
            Coins = new List<Coin>();
            Explosions = new List<Explosion>();

            IsGameOver = false;
            IsWin = false;
            IsPaused = false;

            _powerUpSpawnTimer = 0f;

            ScoreManager.Reset();
        }

        private void LoadBackground()
        {
            switch (GameSettings.EquippedBackground)
            {
                case "BlueNebulaBackground":
                    _background = AssetLoader.LoadImage("back1.jpg");
                    break;

                case "PinkGalaxyBackground":
                    _background = AssetLoader.LoadImage("back2.jpg");
                    break;

                case "MarsBackground":
                    _background = AssetLoader.LoadImage("back3.jpg");
                    break;

                default:
                    _background = AssetLoader.LoadImage("backgound_defult.jpg");
                    break;
            }
        }

        public void SetInput(bool left, bool right, bool up, bool down, bool shoot)
        {
            _leftPressed = left;
            _rightPressed = right;
            _upPressed = up;
            _downPressed = down;
            _shootPressed = shoot;
        }

        public void Update(float deltaTime)
        {
            if (IsGameOver || IsPaused)
                return;

            UpdatePlayerInput(deltaTime);

            Player.Update(deltaTime);

            if (_shootPressed && Player.CanShoot())
            {
                List<Bullet> newBullets = Player.Shoot();
                PlayerBullets.AddRange(newBullets);
            }

            WaveManager.Update(deltaTime, ScreenHeight);

            UpdatePlayerBullets(deltaTime);
            UpdateEnemyAttacks();
            UpdateEnemyBullets(deltaTime);
            UpdatePowerUps(deltaTime);
            UpdateCoins(deltaTime);
            UpdateExplosions(deltaTime);
            SpawnPowerUpsByTimer(deltaTime);

            CollisionManager.CheckCollisions(
                Player,
                WaveManager.Enemies,
                PlayerBullets,
                EnemyBullets,
                PowerUps,
                Coins,
                Explosions,
                WaveManager.CurrentWave
            );

            CleanupInactiveObjects();

            if (Player.Lives <= 0 || !Player.IsActive)
            {
                IsGameOver = true;
                IsWin = false;
                return;
            }

            if (WaveManager.IsFinished)
            {
                IsGameOver = true;
                IsWin = true;
                return;
            }
        }

        private void UpdatePlayerInput(float deltaTime)
        {
            if (_leftPressed)
                Player.AccelerateLeft(deltaTime);

            if (_rightPressed)
                Player.AccelerateRight(deltaTime);

            if (_upPressed)
                Player.AccelerateUp(deltaTime);

            if (_downPressed)
                Player.AccelerateDown(deltaTime);
        }

        private void UpdatePlayerBullets(float deltaTime)
        {
            foreach (Bullet bullet in PlayerBullets)
                bullet.Update(deltaTime);
        }

        private void UpdateEnemyAttacks()
        {
            foreach (Enemy enemy in WaveManager.Enemies)
            {
                if (!enemy.IsActive)
                    continue;

                List<Bullet> bullets = enemy.Attack();

                if (bullets != null && bullets.Count > 0)
                    EnemyBullets.AddRange(bullets);
            }
        }

        private void UpdateEnemyBullets(float deltaTime)
        {
            foreach (Bullet bullet in EnemyBullets)
                bullet.Update(deltaTime);
        }

        private void UpdatePowerUps(float deltaTime)
        {
            foreach (PowerUp powerUp in PowerUps)
                powerUp.Update(deltaTime);
        }

        private void UpdateCoins(float deltaTime)
        {
            foreach (Coin coin in Coins)
                coin.Update(deltaTime);
        }

        private void UpdateExplosions(float deltaTime)
        {
            foreach (Explosion explosion in Explosions)
                explosion.Update(deltaTime);
        }

        private void SpawnPowerUpsByTimer(float deltaTime)
        {
            _powerUpSpawnTimer += deltaTime;

            if (_powerUpSpawnTimer >= PowerUpSpawnInterval)
            {
                _powerUpSpawnTimer = 0f;
                SpawnRandomPowerUp();
            }
        }

        private void SpawnRandomPowerUp()
        {
            float x = _random.Next(40, ScreenWidth - 40);
            float y = -30;

            Array values = Enum.GetValues(typeof(PowerUpType));
            PowerUpType randomType = (PowerUpType)values.GetValue(_random.Next(values.Length));

            PowerUps.Add(new PowerUp(x, y, randomType));
        }

        private void CleanupInactiveObjects()
        {
            PlayerBullets.RemoveAll(b => !b.IsActive);
            EnemyBullets.RemoveAll(b => !b.IsActive);
            PowerUps.RemoveAll(p => !p.IsActive);
            Coins.RemoveAll(c => !c.IsActive);
            Explosions.RemoveAll(e => !e.IsActive);
            WaveManager.Enemies.RemoveAll(e => !e.IsActive);
        }

        public void Render(Graphics g)
        {
            DrawBackground(g);

            foreach (PowerUp powerUp in PowerUps)
                powerUp.Draw(g);

            foreach (Coin coin in Coins)
                coin.Draw(g);

            foreach (Bullet bullet in EnemyBullets)
                bullet.Draw(g);

            foreach (Bullet bullet in PlayerBullets)
                bullet.Draw(g);

            foreach (Enemy enemy in WaveManager.Enemies)
                enemy.Draw(g);

            Player.Draw(g);

            foreach (Explosion explosion in Explosions)
                explosion.Draw(g);

            DrawHUD(g);

            if (WaveManager.IsShowingStatusMessage)
                DrawWaveStatusMessage(g, WaveManager.StatusMessage);

            if (IsPaused)
                DrawCenteredMessage(g, "PAUSED");

            if (IsGameOver)
                DrawEndGameOverlay(g);
        }

        private void DrawBackground(Graphics g)
        {
            if (_background != null)
                g.DrawImage(_background, 0, 0, ScreenWidth, ScreenHeight);
            else
                g.Clear(Color.Black);
        }

        private void DrawHUD(Graphics g)
        {
            using (Font font = new Font("Arial", 14, FontStyle.Bold))
            {
                g.DrawString("Wave: " + WaveManager.CurrentWave, font, Brushes.White, 10, 10);
                g.DrawString("HP: " + Player.Hp, font, Brushes.White, 10, 35);
                g.DrawString("Lives: " + Player.Lives, font, Brushes.White, 10, 60);
                g.DrawString("Score: " + ScoreManager.Score, font, Brushes.White, 10, 85);
                g.DrawString("Coins: " + CoinManager.Coins, font, Brushes.Gold, 10, 110);

                g.DrawString(
                    "Enemies: " + WaveManager.SpawnedEnemiesInWave + "/" + WaveManager.TotalEnemiesInWave +
                    "  Alive: " + WaveManager.AliveEnemiesCount,
                    font,
                    Brushes.White,
                    10,
                    135
                );

                int y = 165;

                if (Player.IsShield)
                {
                    g.DrawString("Shield: " + Player.ShieldTimeLeft.ToString("0.0") + "s", font, Brushes.Cyan, 10, y);
                    y += 25;
                }

                if (Player.IsTripleShot)
                {
                    g.DrawString("Triple Shot: " + Player.TripleShotTimeLeft.ToString("0.0") + "s", font, Brushes.Orange, 10, y);
                    y += 25;
                }

                if (Player.IsFireRateBoost)
                {
                    g.DrawString("Fire Boost: " + Player.FireRateBoostTimeLeft.ToString("0.0") + "s", font, Brushes.Gold, 10, y);
                }
            }
        }

        private void DrawWaveStatusMessage(Graphics g, string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                return;

            Rectangle rect = new Rectangle(
                ScreenWidth / 2 - 230,
                70,
                460,
                70
            );

            using (SolidBrush panelBrush = new SolidBrush(Color.FromArgb(190, 10, 15, 32)))
            using (Pen borderPen = new Pen(Color.FromArgb(0, 210, 255), 2))
            using (Font font = new Font("Arial", 22, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                g.FillRectangle(panelBrush, rect);
                g.DrawRectangle(borderPen, rect);
                g.DrawString(message, font, textBrush, rect, format);
            }
        }

        private void DrawCenteredMessage(Graphics g, string message)
        {
            using (Font font = new Font("Arial", 32, FontStyle.Bold))
            {
                SizeF size = g.MeasureString(message, font);
                float x = (ScreenWidth - size.Width) / 2f;
                float y = (ScreenHeight - size.Height) / 2f;

                g.DrawString(message, font, Brushes.White, x, y);
            }
        }

        private void DrawEndGameOverlay(Graphics g)
        {
            using (SolidBrush overlayBrush = new SolidBrush(Color.FromArgb(210, 5, 8, 18)))
            {
                g.FillRectangle(overlayBrush, 0, 0, ScreenWidth, ScreenHeight);
            }

            Rectangle panelRect = new Rectangle(
                ScreenWidth / 2 - 260,
                ScreenHeight / 2 - 170,
                520,
                270
            );

            Color accentColor = IsWin
                ? Color.FromArgb(0, 210, 255)
                : Color.FromArgb(255, 70, 180);

            using (SolidBrush panelBrush = new SolidBrush(Color.FromArgb(230, 25, 28, 45)))
            using (Pen borderPen = new Pen(accentColor, 3))
            using (Pen innerPen = new Pen(Color.FromArgb(120, Color.White), 1))
            {
                g.FillRectangle(panelBrush, panelRect);
                g.DrawRectangle(borderPen, panelRect);
                g.DrawRectangle(innerPen, panelRect.X + 10, panelRect.Y + 10, panelRect.Width - 20, panelRect.Height - 20);
            }

            string title = IsWin ? "YOU WIN!" : "GAME OVER";
            string subtitle = IsWin ? "ALL WAVES CLEARED" : "MISSION FAILED";

            using (Font titleFont = new Font("Arial", 40, FontStyle.Bold))
            using (Font subtitleFont = new Font("Arial", 15, FontStyle.Bold))
            using (Font statsFont = new Font("Arial", 16, FontStyle.Bold))
            using (SolidBrush titleBrush = new SolidBrush(accentColor))
            using (SolidBrush whiteBrush = new SolidBrush(Color.White))
            using (SolidBrush grayBrush = new SolidBrush(Color.Gainsboro))
            using (SolidBrush goldBrush = new SolidBrush(Color.Gold))
            {
                DrawCenteredText(g, title, titleFont, titleBrush,
                    new Rectangle(panelRect.X, panelRect.Y + 35, panelRect.Width, 60));

                DrawCenteredText(g, subtitle, subtitleFont, grayBrush,
                    new Rectangle(panelRect.X, panelRect.Y + 95, panelRect.Width, 35));

                DrawCenteredText(g, "Score: " + ScoreManager.Score, statsFont, whiteBrush,
                    new Rectangle(panelRect.X, panelRect.Y + 145, panelRect.Width, 35));

                DrawCenteredText(g, "Coins: " + CoinManager.Coins, statsFont, goldBrush,
                    new Rectangle(panelRect.X, panelRect.Y + 180, panelRect.Width, 35));
            }
        }

        private void DrawCenteredText(Graphics g, string text, Font font, Brush brush, Rectangle rect)
        {
            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;
                g.DrawString(text, font, brush, rect, format);
            }
        }
    }
}