using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameEntity
{
    public class GameWorld
    {
        //Entitrs -->
        public Player Player { get; private set; }
        public WaveManager WaveManager { get; private set; }
        public List<Bullet> PlayerBullets { get; private set; }
        public List<Bullet> EnemyBullets { get; private set; }
        public List<PowerUp> PowerUps { get; private set; }

        //Timers -->
        private float _powerUpSpawnTimer = 0f;
        private const float PowerUpSpawnInterval = 10f;
        private Random _random = new Random();

        // Game Status
        public bool IsGameOver { get; private set; }
        public bool IsPaused { get; set; }

        //screen
        public int ScreenWidth { get; }
        public int ScreenHeight { get; }


        public GameWorld(int width, int height)
        {
            ScreenWidth = width;
            ScreenHeight = height;

            Player = new Player(width / 2f, height - 100);
            WaveManager = new WaveManager(Player);
            PlayerBullets = new List<Bullet>();
            EnemyBullets = new List<Bullet>();
            PowerUps = new List<PowerUp>();

            IsGameOver = false;
            IsPaused = false;
        }

        public void Update(float deltaTime)
        {
            if (IsGameOver || IsPaused)
                return;

            Player.Update(deltaTime);

            if (Player.CanShoot())
            {
                var newBullets = Player.Shoot();
                PlayerBullets.AddRange(newBullets);
            }

            WaveManager.Update(deltaTime);

            foreach (var bullet in PlayerBullets)
                bullet.Update(deltaTime);
            PlayerBullets.RemoveAll(b => !b.IsActive);

            foreach (var enemy in WaveManager.Enemies)
            {
                var bullets = enemy.Attack();
                if (bullets != null)
                    EnemyBullets.AddRange(bullets);
            }

            foreach (var bullet in EnemyBullets)
                bullet.Update(deltaTime);
            EnemyBullets.RemoveAll(b => !b.IsActive);

            foreach (var p in PowerUps)
                p.Update(deltaTime);
            PowerUps.RemoveAll(p => !p.IsActive);

            _powerUpSpawnTimer += deltaTime;
            if (_powerUpSpawnTimer >= PowerUpSpawnInterval)
            {
                _powerUpSpawnTimer = 0f;
                SpawnRandomPowerUp();
            }

            CheckCollisions();

            if (Player.Lives <= 0 || !Player.IsActive)
                IsGameOver = true;
        }

        private void CheckCollisions()
        {
            CollisionManager.CheckCollisions(Player, WaveManager.Enemies, PlayerBullets);

            foreach (var bullet in EnemyBullets)
            {
                if (!bullet.IsActive || bullet.IsPlayerBullet)
                    continue;

                if (bullet.Bounds.IntersectsWith(Player.Bounds))
                {
                    bullet.IsActive = false;
                    if (!Player.IsShield)
                    {
                        Player.Hp -= bullet.Damage;
                        if (Player.Hp <= 0)
                        {
                            Player.Lives--;
                            if (Player.Lives > 0)
                                Player.Reset();
                            else
                                Player.IsActive = false;
                        }
                    }
                }
            }

            foreach (var powerUp in PowerUps)
            {
                if (!powerUp.IsActive)
                    continue;

                if (powerUp.GetBounds().IntersectsWith(Player.Bounds))
                {
                    Player.ActivatePowerUp(powerUp.Type);
                    powerUp.IsActive = false;
                }
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

        public void MovePlayerLeft() => Player.MoveLeft();
        public void MovePlayerRight() => Player.MoveRight();
        public void MovePlayerUp() => Player.MoveUp();
        public void MovePlayerDown() => Player.MoveDown();

        public void Render(Graphics g)
        {
            g.Clear(Color.Black);

            foreach (var p in PowerUps)
                p.Draw(g);

            foreach (var b in EnemyBullets)
                b.Draw(g);

            foreach (var b in PlayerBullets)
                b.Draw(g);

            foreach (var e in WaveManager.Enemies)
                e.Draw(g);

            Player.Draw(g);

            DrawHUD(g);
        }

        private void DrawHUD(Graphics g)
        {
            using (var font = new Font("Arial", 14))
            {
                g.DrawString($"Wave: {WaveManager.CurrentWave}", font, Brushes.White, 10, 10);
                g.DrawString($"HP: {Player.Hp}", font, Brushes.White, 10, 30);
                g.DrawString($"Lives: {Player.Lives}", font, Brushes.White, 10, 50);
                g.DrawString($"Score: {ScoreManager.Score}", font, Brushes.White, 10, 70);
                g.DrawString($"Coins: {CoinManager.Coins}", font, Brushes.White, 10, 90);
            }
        }
    }
}