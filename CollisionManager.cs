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
            List<Explosion> explosions,
            int currentWave)
        {
            CheckPlayerBulletsWithEnemies(enemies, playerBullets, coins, currentWave);
            CheckEnemiesWithPlayer(player, enemies, explosions);
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

                            ScoreManager.AddScore(enemy.ScoreValue);

                            TryDropCoin(enemy, coins, currentWave);
                        }

                        break;
                    }
                }
            }
        }

        private static void TryDropCoin(Enemy enemy, List<Coin> coins, int currentWave)
        {
            double dropChance = 0.25;
            double goldChance = 0.08;
            int count = 1;

            if (enemy is ScoutEnemy)
            {
                dropChance = 0.30;
                goldChance = 0.08;
            }
            else if (enemy is ShooterEnemy)
            {
                dropChance = 0.40;
                goldChance = 0.16;
            }
            else if (enemy is TerroristEnemy)
            {
                dropChance = 0.45;
                goldChance = 0.18;
            }
            else if (enemy is HeavyTankEnemy)
            {
                dropChance = 1.00;
                goldChance = 0.45;
                count = 2;
            }

            dropChance += currentWave * 0.01;
            goldChance += currentWave * 0.005;

            if (dropChance > 0.85)
                dropChance = 0.85;

            if (goldChance > 0.55)
                goldChance = 0.55;

            for (int i = 0; i < count; i++)
            {
                if (_random.NextDouble() > dropChance)
                    continue;

                CoinType type = _random.NextDouble() < goldChance
                    ? CoinType.Gold
                    : CoinType.Silver;

                float offsetX = _random.Next(-12, 13);
                float offsetY = _random.Next(-12, 13);

                coins.Add(new Coin(enemy.X + offsetX, enemy.Y + offsetY, type));
            }
        }

        private static void CheckEnemiesWithPlayer(
            Player player,
            List<Enemy> enemies,
            List<Explosion> explosions)
        {
            foreach (Enemy enemy in enemies)
            {
                if (!enemy.IsActive || !player.IsActive)
                    continue;

                if (!enemy.Bounds.IntersectsWith(player.Bounds))
                    continue;

                if (enemy is TerroristEnemy)
                {
                    ExplodeTerrorist(player, enemy, explosions);
                    continue;
                }

                enemy.IsActive = false;

                if (!player.IsShield)
                    DamagePlayer(player, 20);
            }
        }

        private static void ExplodeTerrorist(
            Player player,
            Enemy enemy,
            List<Explosion> explosions)
        {
            enemy.IsActive = false;

            if (explosions != null)
                explosions.Add(new Explosion(enemy.X, enemy.Y));

            if (!player.IsShield)
                DamagePlayer(player, 50);
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
                        DamagePlayer(player, bullet.Damage);
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
                    AudioManager.PlayCoin();
                    coin.IsActive = false;
                }
            }
        }

        private static void DamagePlayer(Player player, int damage)
        {
            if (!player.IsActive)
                return;

            player.Hp -= damage;

            if (player.Hp > 0)
                return;

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