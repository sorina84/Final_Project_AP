using System;
using System.Collections.Generic;

namespace GameEntity
{
    public class WaveManager
    {
        private enum WaveState
        {
            Starting,
            Spawning,
            Clearing,
            Finished
        }

        private readonly Random _random = new Random();
        private readonly Player _player;
        private readonly Queue<int> _spawnQueue = new Queue<int>();

        private const int MaxWave = 10;

        private const int TypeStandard = 0;
        private const int TypeScout = 1;
        private const int TypeShooter = 2;
        private const int TypeTerrorist = 3;
        private const int TypeHeavy = 4;

        private const float SpawnInterval = 0.85f;
        private const float WaveStartDelay = 2.2f;

        private WaveState _state;
        private float _spawnTimer;
        private float _stateTimer;
        private float _statusMessageTimer;

        public int CurrentWave { get; private set; }
        public List<Enemy> Enemies { get; private set; }

        public bool IsFinished { get; private set; }

        public string StatusMessage { get; private set; }
        public bool IsShowingStatusMessage
        {
            get { return _statusMessageTimer > 0f; }
        }

        public int TotalEnemiesInWave { get; private set; }
        public int SpawnedEnemiesInWave { get; private set; }

        public int WaitingToSpawnCount
        {
            get { return _spawnQueue.Count; }
        }

        public int AliveEnemiesCount
        {
            get { return Enemies.Count; }
        }

        public WaveManager(Player player)
        {
            _player = player;

            CurrentWave = 1;
            Enemies = new List<Enemy>();
            IsFinished = false;

            BeginWaveIntro();
        }

        public void Update(float deltaTime, int screenHeight)
        {
            if (IsFinished)
                return;

            UpdateStatusMessage(deltaTime);
            UpdateActiveEnemies(deltaTime, screenHeight);

            if (_state == WaveState.Starting)
            {
                _stateTimer -= deltaTime;

                if (_stateTimer <= 0f)
                {
                    _state = WaveState.Spawning;
                    _spawnTimer = 0f;
                }

                return;
            }

            if (_state == WaveState.Spawning)
            {
                _spawnTimer -= deltaTime;

                if (_spawnTimer <= 0f)
                {
                    SpawnNextEnemy();
                    _spawnTimer = SpawnInterval;
                }

                if (_spawnQueue.Count == 0)
                    _state = WaveState.Clearing;

                return;
            }

            if (_state == WaveState.Clearing)
            {
                bool allEnemiesSpawned = _spawnQueue.Count == 0;
                bool allEnemiesDestroyed = Enemies.Count == 0;

                if (allEnemiesSpawned && allEnemiesDestroyed)
                {
                    if (CurrentWave >= MaxWave)
                    {
                        IsFinished = true;
                        _state = WaveState.Finished;
                        ShowStatusMessage("ALL WAVES CLEARED!");
                        return;
                    }

                    CurrentWave++;
                    BeginWaveIntro();
                }
            }
        }

        private void UpdateStatusMessage(float deltaTime)
        {
            if (_statusMessageTimer > 0f)
            {
                _statusMessageTimer -= deltaTime;

                if (_statusMessageTimer <= 0f)
                {
                    _statusMessageTimer = 0f;
                }
            }
        }

        private void UpdateActiveEnemies(float deltaTime, int screenHeight)
        {
            foreach (Enemy enemy in Enemies)
            {
                if (!enemy.IsActive)
                    continue;

                enemy.Update(deltaTime);

                if (enemy.IsActive && enemy.IsOutOfScreen(screenHeight))
                {
                    enemy.ResetPosition(RandomX(), RandomSpawnY());
                }
            }

            Enemies.RemoveAll(e => !e.IsActive);
        }

        private void BeginWaveIntro()
        {
            Enemies.Clear();
            BuildSpawnQueueForCurrentWave();

            SpawnedEnemiesInWave = 0;

            _state = WaveState.Starting;
            _stateTimer = WaveStartDelay;
            _spawnTimer = 0f;

            ShowStatusMessage("WAVE " + CurrentWave + " STARTING...");
        }

        private void ShowStatusMessage(string message)
        {
            StatusMessage = message;
            _statusMessageTimer = 2.1f;
        }

        private void BuildSpawnQueueForCurrentWave()
        {
            _spawnQueue.Clear();

            List<int> types = new List<int>();

            AddEnemyTypes(types, TypeStandard, 2 + (CurrentWave + 1) / 2);

            if (CurrentWave >= 3)
                AddEnemyTypes(types, TypeScout, 1 + (CurrentWave - 3) / 3);

            if (CurrentWave >= 5)
                AddEnemyTypes(types, TypeShooter, 1 + (CurrentWave - 5) / 3);

            if (CurrentWave >= 8)
                AddEnemyTypes(types, TypeTerrorist, 1);

            if (CurrentWave >= 10)
                AddEnemyTypes(types, TypeHeavy, 1);

            Shuffle(types);

            foreach (int type in types)
                _spawnQueue.Enqueue(type);

            TotalEnemiesInWave = _spawnQueue.Count;
        }

        private void AddEnemyTypes(List<int> list, int type, int count)
        {
            for (int i = 0; i < count; i++)
                list.Add(type);
        }

        private void Shuffle(List<int> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = _random.Next(i, list.Count);

                int temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        private void SpawnNextEnemy()
        {
            if (_spawnQueue.Count == 0)
                return;

            int type = _spawnQueue.Dequeue();

            Enemy enemy = CreateEnemy(type);

            ApplyDifficulty(enemy);

            Enemies.Add(enemy);
            SpawnedEnemiesInWave++;
        }

        private Enemy CreateEnemy(int type)
        {
            float x = RandomX();
            float y = RandomSpawnY();

            switch (type)
            {
                case TypeScout:
                    return new ScoutEnemy(x, y);

                case TypeShooter:
                    return new ShooterEnemy(x, y);

                case TypeTerrorist:
                    return new TerroristEnemy(_player, x, y);

                case TypeHeavy:
                    return new HeavyTankEnemy(x, y);

                default:
                    return new StandardEnemy(x, y);
            }
        }

        private void ApplyDifficulty(Enemy enemy)
        {
            enemy.Speed *= 1f + CurrentWave * 0.1f;

            enemy.Hp += CurrentWave * 2;
            enemy.MaxHp = enemy.Hp;
        }

        private int RandomX()
        {
            return _random.Next(50, 750);
        }

        private int RandomSpawnY()
        {
            return _random.Next(-90, -45);
        }
    }
}