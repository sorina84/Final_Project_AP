using System;
using System.Collections.Generic;

namespace GameEntity
{
    public class WaveManager
    {
        private readonly Random _random = new Random();
        private readonly Player _player;

        public int CurrentWave { get; private set; }
        public List<Enemy> Enemies { get; private set; }

        public bool IsFinished { get; private set; }

        private const int MaxWave = 10;

        public WaveManager(Player player)
        {
            _player = player;

            CurrentWave = 1;
            Enemies = new List<Enemy>();
            IsFinished = false;

            StartWave();
        }

        public void Update(float deltaTime, int screenHeight)
        {
            if (IsFinished)
                return;

            foreach (Enemy enemy in Enemies)
            {
                if (enemy.IsActive)
                    enemy.Update(deltaTime);

                if (enemy.IsActive && enemy.IsOutOfScreen(screenHeight))
                {
                    enemy.ResetPosition(RandomX(), RandomY());
                }
            }

            Enemies.RemoveAll(e => !e.IsActive);

            if (Enemies.Count == 0)
            {
                if (CurrentWave >= MaxWave)
                {
                    IsFinished = true;
                    return;
                }

                CurrentWave++;
                StartWave();
            }
        }

        private void StartWave()
        {
            Enemies.Clear();

            SpawnStandardEnemies(2 + (CurrentWave + 1) / 2);

            if (CurrentWave >= 3)
                SpawnScoutEnemies(1 + (CurrentWave - 3) / 3);

            if (CurrentWave >= 5)
                SpawnShooterEnemies(1 + (CurrentWave - 5) / 3);

            if (CurrentWave >= 8)
                SpawnTerroristEnemies(1);

            if (CurrentWave >= 10)
                SpawnHeavyEnemies(1);
        }

        private void ApplyDifficulty(Enemy enemy)
        {
            enemy.Hp += CurrentWave * 2;
            enemy.MaxHp = enemy.Hp;
            enemy.Speed *= 1f + CurrentWave * 0.1f;
        }

        private void SpawnStandardEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var enemy = new StandardEnemy(RandomX(), RandomY());
                ApplyDifficulty(enemy);
                Enemies.Add(enemy);
            }
        }

        private void SpawnScoutEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var enemy = new ScoutEnemy(RandomX(), RandomY());
                ApplyDifficulty(enemy);
                Enemies.Add(enemy);
            }
        }

        private void SpawnShooterEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var enemy = new ShooterEnemy(RandomX(), RandomY());
                ApplyDifficulty(enemy);
                Enemies.Add(enemy);
            }
        }

        private void SpawnHeavyEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var enemy = new HeavyTankEnemy(RandomX(), RandomY());
                ApplyDifficulty(enemy);
                Enemies.Add(enemy);
            }
        }

        private void SpawnTerroristEnemies(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var enemy = new TerroristEnemy(_player, RandomX(), RandomY());
                ApplyDifficulty(enemy);
                Enemies.Add(enemy);
            }
        }

        private int RandomX()
        {
            return _random.Next(50, 750);
        }

        private int RandomY()
        {
            return _random.Next(-450, -40);
        }
    }
}