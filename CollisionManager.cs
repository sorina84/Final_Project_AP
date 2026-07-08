using System;
using System.Collections.Generic;

namespace GameEntity
{
    public static class CollisionManager
    {
        private static readonly Random _random = new Random();

        public static void CheckCollisions(
            Player player,
            List<Enemy> enemies,
            List<Bullet> playerBullets,
            List<Bullet> enemyBullets,
            List<PowerUp> powerUps,
            List<Coin> coins,
            int currentWave)
        {
            CheckPlayerBulletsWithEnemies(enemies, playerBullets, coins, currentWave);
            CheckEnemiesWithPlayer(player, enemies);
            CheckEnemyBulletsWithPlayer(player, enemyBullets);
            CheckPowerUpsWithPlayer(player, powerUps);
            CheckCoinsWithPlayer(player, coins);
        }

        private static void CheckPlayerBulletsWithEnemies(
            List<Enemy> enemies,
            List<Bullet> playerBullets,
            List<Coin> coins,
            int currentWave)
        {
            foreach (Bullet bullet in playerBullets)
            {
                if (!bullet.IsActive || !bullet.IsPlayerBullet)
                    continue;

                foreach (Enemy enemy in enemies)
                {
                    if (!enemy.IsActive)
                        continue;

                    if (bullet.Bounds.IntersectsWith(enemy.Bounds))
                    {
                        enemy.Hp -= bullet.Damage;
                        bullet.IsActive = false;

                        if (enemy.Hp <= 0)
                        {
                            enemy.IsActive = false;

                            // امتیاز مستقیم اضافه می‌شود.
                            ScoreManager.AddScore(enemy.ScoreValue);

                            // سکه مستقیم اضافه نمی‌شود؛ روی صفحه می‌افتد.
                            TryDropCoins(enemy, coins, currentWave);
                        }

                        break;
                    }
                }
            }
        }

        private static void TryDropCoins(Enemy enemy, List<Coin> coins, int currentWave)
        {
            double dropChance = 0.25 + currentWave * 0.02;
            double goldChance = 0.05 + currentWave * 0.015;
            int dropCount = 1;

            if (enemy is ScoutEnemy)
            {
                dropChance += 0.10;
            }
            else if (enemy is ShooterEnemy)
            {
                dropChance += 0.20;
                goldChance += 0.08;
            }
            else if (enemy is HeavyTankEnemy)
            {
                dropChance += 0.50;
                goldChance += 0.25;
                dropCount = 2;
            }
            else if (enemy is TerroristEnemy)
            {
                dropChance += 0.30;
                goldChance += 0.15;
            }

            if (dropChance > 0.95)
                dropChance = 0.95;

            if (goldChance > 0.70)
                goldChance = 0.70;

            for (int i = 0; i < dropCount; i++)
            {
                if (_random.NextDouble() > dropChance)
                    continue;

                CoinType type = _random.NextDouble() < goldChance
                    ? CoinType.Gold
                    : CoinType.Silver;

                float offsetX = _random.Next(-15, 16);
                float offsetY = _random.Next(-10, 11);

                coins.Add(new Coin(enemy.X + offsetX, enemy.Y + offsetY, type));
            }
        }

        private static void CheckEnemiesWithPlayer(Player player, List<Enemy> enemies)
        {
            foreach (Enemy enemy in enemies)
            {
                if (!enemy.IsActive || !player.IsActive)
                    continue;

                if (enemy.Bounds.IntersectsWith(player.Bounds))
                {
                    enemy.IsActive = false;

                    if (!player.IsShield)
                    {
                        int damage = enemy is TerroristEnemy ? 50 : 20;
                        DamagePlayer(player, damage);
                    }
                }
            }
        }

        private static void CheckEnemyBulletsWithPlayer(Player player, List<Bullet> enemyBullets)
        {
            foreach (Bullet bullet in enemyBullets)
            {
                if (!bullet.IsActive || bullet.IsPlayerBullet || !player.IsActive)
                    continue;

                if (bullet.Bounds.IntersectsWith(player.Bounds))
                {
                    bullet.IsActive = false;

                    if (!player.IsShield)
                    {
                        DamagePlayer(player, bullet.Damage);
                    }
                }
            }
        }

        private static void CheckPowerUpsWithPlayer(Player player, List<PowerUp> powerUps)
        {
            foreach (PowerUp powerUp in powerUps)
            {
                if (!powerUp.IsActive || !player.IsActive)
                    continue;

                if (powerUp.GetBounds().IntersectsWith(player.Bounds))
                {
                    player.ActivatePowerUp(powerUp.Type);
                    powerUp.IsActive = false;
                }
            }
        }

        private static void CheckCoinsWithPlayer(Player player, List<Coin> coins)
        {
            foreach (Coin coin in coins)
            {
                if (!coin.IsActive || !player.IsActive)
                    continue;

                if (coin.Bounds.IntersectsWith(player.Bounds))
                {
                    CoinManager.AddCoins(coin.Value);
                    coin.IsActive = false;
                }
            }
        }

        private static void DamagePlayer(Player player, int damage)
        {
            player.Hp -= damage;

            if (player.Hp <= 0)
            {
                player.Lives--;

                if (player.Lives > 0)
                {
                    player.Reset();
                }
                else
                {
                    player.Hp = 0;
                    player.IsActive = false;
                }
            }
        }
    }
}