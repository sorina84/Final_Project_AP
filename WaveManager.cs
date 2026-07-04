using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace GameObject
{
    public class WaveManager
    {
        private readonly Random random = new Random();
        public int CurrentWave { get; private set; }
        public List<Enemy> Enemies { get; private set; }

        private Player player;

        public WaveManager(Player player)
        {
            this.player = player;

            CurrentWave = 1;
            Enemies = new List<Enemy>();

            StartWave();
        }

        public void Update(float deltaTime)
        {
            foreach (Enemy enemy in Enemies)
            {
                if (enemy.IsActive)
                    enemy.Update(deltaTime);
            }

            Enemies.RemoveAll(e => !e.IsActive);

            if (Enemies.Count == 0)
            {
                CurrentWave++;

                OnWaveCompleted();

                StartWave();
            }
        }

        private void StartWave()
        {
            Enemies.Clear();

            SpawnStandardEnemies(CurrentWave * 3);

            if (CurrentWave >= 3)
                SpawnScoutEnemies(CurrentWave - 2);

            if (CurrentWave >= 5)
                SpawnShooterEnemies(CurrentWave - 4);

            if (CurrentWave >= 8)
                SpawnHeavyEnemies(CurrentWave - 7);

            if (CurrentWave >= 10)
                SpawnTerroristEnemies(CurrentWave - 9);
        }

        //--------------------------------------------------

        private void SpawnStandardEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var enemy = new StandardEnemy(RandomX(), RandomY());

                enemy.Hp += CurrentWave * 2;
                enemy.Speed *= (1f + CurrentWave * 0.1f);

                Enemies.Add(enemy);
            }
        }

        private void SpawnScoutEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var enemy = new ScoutEnemy(RandomX(), RandomY());

                enemy.Hp += CurrentWave * 2;
                enemy.Speed *= (1f + CurrentWave * 0.1f);

                Enemies.Add(enemy);
            }
        }

        private void SpawnShooterEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var enemy = new ShooterEnemy(RandomX(), RandomY());

                enemy.Hp += CurrentWave * 2;
                enemy.Speed *= (1f + CurrentWave * 0.1f);

                Enemies.Add(enemy);
            }
        }

        private void SpawnHeavyEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var enemy = new HeavyTankEnemy(RandomX(), RandomY());

                enemy.Hp += CurrentWave * 2;
                enemy.Speed *= (1f + CurrentWave * 0.1f);

                Enemies.Add(enemy);
            }
        }

        private void SpawnTerroristEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var enemy = new TerroristEnemy(player, RandomX(), RandomY());

                enemy.Hp += CurrentWave * 2;
                enemy.Speed *= (1f + CurrentWave * 0.1f);

                Enemies.Add(enemy);
            }
        }

        //--------------------------------------------------

        private int RandomX()
        {
            return random.Next(40, 760);
        }

        private int RandomY()
        {
            return random.Next(-300, -40);
        }

        private void OnWaveCompleted()
        {
            // فعلاً خالی

            // بعداً اینجا می‌توان:
            // Shop
            // Save
            // Bonus
            // Boss Warning
            // را اضافه کرد.
        }
    }
}