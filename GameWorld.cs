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

        public bool IsGameOver { get; private set; }
        public bool IsWin { get; private set; }
        public bool IsPaused { get; set; }

        public int ScreenWidth { get; }
        public int ScreenHeight { get; }

        private bool _leftPressed;
        private bool _rightPressed;
        private bool _upPressed;
        private bool _downPressed;
        private bool _shootPressed;

        private float _powerUpSpawnTimer;
        private const float PowerUpSpawnInterval = 10f;

        private readonly Random _random = new Random();

        public GameWorld(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;

            Player = new Player(width / 2f, height - 100f);
            Player.ScreenWidth = width;
            Player.ScreenHeight = height;
            Player.Reset();

            WaveManager = new WaveManager(Player);

            PlayerBullets = new List<Bullet>();
            EnemyBullets = new List<Bullet>();
            PowerUps = new List<PowerUp>();
            Coins = new List<Coin>();

            IsGameOver = false;
            IsWin = false;
            IsPaused = false;

            _powerUpSpawnTimer = 0f;

            ScoreManager.Reset();
            CoinManager.Reset();
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
                var newBullets = Player.Shoot();
                PlayerBullets.AddRange(newBullets);
            }

            WaveManager.Update(deltaTime, ScreenHeight);

            if (WaveManager.IsFinished)
            {
                IsWin = true;
                IsGameOver = true;
            }

            UpdatePlayerBullets(deltaTime);
            UpdateEnemyAttacks();
            UpdateEnemyBullets(deltaTime);
            UpdatePowerUps(deltaTime);
            SpawnPowerUpsByTimer(deltaTime);

            CollisionManager.CheckCollisions(
                Player,
                WaveManager.Enemies,
                PlayerBullets,
                EnemyBullets,
                PowerUps,
                Coins,
                WaveManager.CurrentWave
            );

            CleanupInactiveObjects();

            if (Player.Lives <= 0 || !Player.IsActive)
            {
                IsGameOver = true;
                IsWin = false;
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

                var bullets = enemy.Attack();

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
            WaveManager.Enemies.RemoveAll(e => !e.IsActive);
            Coins.RemoveAll(c => !c.IsActive);
        }

        public void Render(Graphics g)
        {
            g.Clear(Color.Black);

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

            DrawHUD(g);

            if (IsPaused)
                DrawCenteredMessage(g, "PAUSED");

            if (IsGameOver)
            {
                if (IsWin)
                    DrawCenteredMessage(g, "YOU WIN!");
                else
                    DrawCenteredMessage(g, "GAME OVER");
            }
        }

        private void DrawHUD(Graphics g)
        {
            using (var font = new Font("Arial", 14))
            {
                g.DrawString($"Wave: {WaveManager.CurrentWave}", font, Brushes.White, 10, 10);
                g.DrawString($"HP: {Player.Hp}", font, Brushes.White, 10, 35);
                g.DrawString($"Lives: {Player.Lives}", font, Brushes.White, 10, 60);
                g.DrawString($"Score: {ScoreManager.Score}", font, Brushes.White, 10, 85);
                g.DrawString($"Coins: {CoinManager.Coins}", font, Brushes.White, 10, 110);

                int y = 140;

                if (Player.IsShield)
                {
                    g.DrawString($"Shield: {Player.ShieldTimeLeft:0.0}s", font, Brushes.Cyan, 10, y);
                    y += 25;
                }

                if (Player.IsTripleShot)
                {
                    g.DrawString($"Triple Shot: {Player.TripleShotTimeLeft:0.0}s", font, Brushes.Orange, 10, y);
                    y += 25;
                }

                if (Player.IsFireRateBoost)
                {
                    g.DrawString($"Fire Boost: {Player.FireRateBoostTimeLeft:0.0}s", font, Brushes.Gold, 10, y);
                }
            }
        }

        private void DrawCenteredMessage(Graphics g, string message)
        {
            using (var font = new Font("Arial", 32, FontStyle.Bold))
            {
                SizeF size = g.MeasureString(message, font);

                float x = (ScreenWidth - size.Width) / 2f;
                float y = (ScreenHeight - size.Height) / 2f;

                g.DrawString(message, font, Brushes.White, x, y);
            }
        }
    }
}