using System;
using System.Collections.Generic;
using System.Linq;

namespace GameObject
{
    public static class CollisionManager
    {
        public static void CheckCollisions(Player player, List<Enemy> enemies, List<Bullet> bullets)
        {
            //  Bullets --> enemy 
            foreach (Bullet bullet in bullets)
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

            //Enemy --> player
            foreach (Enemy enemy in enemies)
            {
                if (!enemy.IsActive)
                    continue;

                if (enemy.Bounds.IntersectsWith(player.Bounds))
                {
                    enemy.IsActive = false;

                    if (!player.IsShield)
                    {
                        player.Hp -= 20;

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

            // bullets --> Player
            foreach (Bullet bullet in bullets)
            {
                if (!bullet.IsActive || bullet.IsPlayerBullet)
                    continue;

                if (bullet.Bounds.IntersectsWith(player.Bounds))
                {
                    bullet.IsActive = false;

                    if (!player.IsShield)
                    {
                        player.Hp -= bullet.Damage;

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
        }
    }
}
