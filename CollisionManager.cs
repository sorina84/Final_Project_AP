using System;
using System.Collections.Generic;
using System.Linq;

namespace GameObject
{
    public static class CollisionManager 
    {
        public static void CheckCollisions(List<Bullet> bullets,List<Enemy> enemies,Player player)
        {
            if (player.IsDestroyed) return;
            PlayerBullet(bullets, enemies);
            EnemyBullet(bullets, player);
            EnemyPlayer(enemies, player);
        }



        //  Bullets --> enemy 
        public static void PlayerBullet(List<Bullet> bs, List<Enemy> es)
        {

            foreach(var bullet in bs.ToList())
            {
                if (!bullet.IsPlayerBullet)
                    continue;
                foreach(var enemy in es.ToList())
                {
                    if (enemy.Hp > 0 && bullet.Bounds.IntersectsWith(enemy.Bounds))
                    {
                        enemy.Hp -= bullet.Damage;
                        bs.Remove(bullet);
                        if (enemy.Hp <= 0)
                        {
                            ScoreManager.AddScore(enemy.ScoreValue);
                            CoinManager.AddCoins(enemy.CoinValue);
                            es.Remove(enemy);
                        }
                        break;
                    }
                }
            }

        }
        // bullets --> Player
        public static void EnemyBullet(List<Bullet> bs, Player player)
        {
            foreach(var bullet in bs.ToList())
            {
                if (bullet.IsPlayerBullet)
                    continue;
                if (bullet.Bounds.IntersectsWith(player.Bounds))
                {
                    player.Hp -= bullet.Damage;
                    bs.Remove(bullet);
                }
            }

        }

        //Enemy --> player
        private static void EnemyPlayer(List<Enemy> es ,Player player)
        {
            foreach (var enemy in es.ToList())
            {
                if (enemy.Hp > 0 && enemy.Bounds.IntersectsWith(player.Bounds))
                {
                    player.Hp -= 20;

                    enemy.Hp = 0;
                    es.Remove(enemy);
                }
            }
        }

    }
}
