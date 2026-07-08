using System.Collections.Generic;

namespace GameEntity
{
    public static class CollisionManager
    {
        public static void CheckCollisions(
            Player player,
            List<Enemy> enemies,
            List<Bullet> playerBullets,
            List<Bullet> enemyBullets,
            List<PowerUp> powerUps)
        {
            CheckPlayerBulletsWithEnemies(enemies, playerBullets);
            CheckEnemiesWithPlayer(player, enemies);
            CheckEnemyBulletsWithPlayer(player, enemyBullets);
            CheckPowerUpsWithPlayer(player, powerUps);
        }

        private static void CheckPlayerBulletsWithEnemies(List<Enemy> enemies, List<Bullet> playerBullets)
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
                            CoinManager.AddCoins(enemy.CoinValue);
                        }

                        break;
                    }
                }
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
                    player.IsActive = false;
                }
            }
        }
    }
}