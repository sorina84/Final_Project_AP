using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GameObject
{
    public class WaveManager
    {
        public int CurrentWave { get; set; }
        private Random rand; 

        public WaveManager()
        {
            CurrentWave = 1;
            rand = new Random();
        }

        public void SpawnWave(List<Enemy> enemies, Player player)
        {
            enemies.Clear();

            for (int i = 0; i < CurrentWave * 3; i++)
            {
                enemies.Add(createstandardEnemy());
            }


            if (CurrentWave >= 3)
            {
                int scoutCount = CurrentWave - 2;
                for (int i = 0; i < scoutCount; i++)
                {
                    enemies.Add(createScoutEnemy());
                }
            }

            if (CurrentWave >= 5)
            {
                int shooterCount = CurrentWave - 4;
                for (int i = 0; i < shooterCount; i++)
                {
                    enemies.Add(createShooterEnemy());
                }
            }

            if (CurrentWave >= 8)
            {
                int tankCount = CurrentWave - 7;
                for (int i = 0; i < tankCount; i++)
                {
                    enemies.Add(createHeavyTankEnemy());
                }
            }

            if (CurrentWave >= 10)
            {
                int terroristCount = CurrentWave - 9;
                for (int i = 0; i < terroristCount; i++)
                {
                    enemies.Add(createTerroristEnemy(player));
                }
            }
        }

        private void ApplyDifficulty(Enemy e, int width, int height)
        {

            e.Speed = e.Speed * (1 + 0.1f * CurrentWave);
            e.Hp = e.Hp + (2 * CurrentWave);
            int randomX = rand.Next(0, 800 - width);
            int randomY = rand.Next(-150, 0);
            e.Bounds = new Rectangle(randomX, randomY, width, height);
        }

        private StandardEnemy createstandardEnemy()
        {
            StandardEnemy e = new StandardEnemy();
            ApplyDifficulty(e, 40, 40); 
            return e;
        }

        private ScoutEnemy createScoutEnemy()
        {
            ScoutEnemy e = new ScoutEnemy();
            ApplyDifficulty(e, 45, 45);
            return e;
        }

        private ShooterEnemy createShooterEnemy()
        {
            ShooterEnemy e = new ShooterEnemy();
            ApplyDifficulty(e, 50, 50);
            return e;
        }

        private TerroristEnemy createTerroristEnemy(Player p)
        {
            TerroristEnemy e = new TerroristEnemy(p);
            ApplyDifficulty(e, 45, 45); 
            return e;
        }

        private HeavyTankEnemy createHeavyTankEnemy()
        {
            HeavyTankEnemy e = new HeavyTankEnemy();
            ApplyDifficulty(e, 80, 80); 
            return e;
        }
    }
}