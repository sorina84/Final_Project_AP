using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameEntity
{
    public class StandardEnemy : Enemy
    {
        public StandardEnemy(float x, float y)
            : base(x, y, 150f)
        {
            Sprite = AssetLoader.LoadImage("enemy_standard.png");
            Hp = 20;
            MaxHp = Hp;
            ScoreValue = 10;
            CoinValue = 1;
        }

        public override void Update(float deltaTime)
        {
            if (!IsActive)
                return;

            Y += Speed * deltaTime;
        }

        public override void Draw(Graphics g)
        {
            DrawSpriteOrFallback(g, Brushes.Red);
        }
    }

    public class ScoutEnemy : Enemy
    {
        private float _angle;
        private float _startX;

        public ScoutEnemy(float x, float y)
            : base(x, y, 220f)
        {
            Sprite = AssetLoader.LoadImage("enemy_scout.png");

            _startX = x;
            _angle = 0f;

            Hp = 15;
            MaxHp = Hp;
            ScoreValue = 20;
            CoinValue = 2;
        }

        public override void Update(float deltaTime)
        {
            if (!IsActive)
                return;

            _angle += 5f * deltaTime;
            X = _startX + (float)Math.Sin(_angle) * 60f;
            Y += Speed * deltaTime;
        }

        public override void Draw(Graphics g)
        {
            DrawSpriteOrFallback(g, Brushes.DeepSkyBlue, true);
        }

        public override void ResetPosition(float x, float y)
        {
            base.ResetPosition(x, y);
            _startX = x;
            _angle = 0f;
        }
    }

    public class ShooterEnemy : Enemy
    {
        private float _shootTimer;
        private const float ShootInterval = 1.5f;

        public ShooterEnemy(float x, float y)
            : base(x, y, 100f)
        {
            Sprite = AssetLoader.LoadImage("enemy_shooter.png");

            Hp = 40;
            MaxHp = Hp;
            ScoreValue = 40;
            CoinValue = 3;

            _shootTimer = 0f;
        }

        public override void Update(float deltaTime)
        {
            if (!IsActive)
                return;

            Y += Speed * deltaTime;
            _shootTimer += deltaTime;
        }

        public override List<Bullet> Attack()
        {
            List<Bullet> bullets = new List<Bullet>();

            if (_shootTimer >= ShootInterval)
            {
                _shootTimer = 0f;
                bullets.Add(new Bullet(X, Y + Height / 2f, 260f, false));
            }

            return bullets;
        }

        public override void Draw(Graphics g)
        {
            DrawSpriteOrFallback(g, Brushes.Green);
        }
    }

    public class HeavyTankEnemy : Enemy
    {
        private float _shootTimer;
        private const float ShootInterval = 2.5f;

        public HeavyTankEnemy(float x, float y)
            : base(x, y, 60f)
        {
            Sprite = AssetLoader.LoadImage("enemy_heavy.png");

            Hp = 150;
            MaxHp = Hp;
            ScoreValue = 100;
            CoinValue = 10;

            _shootTimer = 0f;
        }

        public override void Update(float deltaTime)
        {
            if (!IsActive)
                return;

            Y += Speed * deltaTime;
            _shootTimer += deltaTime;
        }

        public override List<Bullet> Attack()
        {
            List<Bullet> bullets = new List<Bullet>();

            if (_shootTimer < ShootInterval)
                return bullets;

            _shootTimer = 0f;

            float bulletSpeed = 220f;
            int damage = 20;
            float diagonal = bulletSpeed * 0.7071f;

            bullets.Add(new Bullet(X, Y, 0f, bulletSpeed, false, damage));
            bullets.Add(new Bullet(X, Y, 0f, -bulletSpeed, false, damage));
            bullets.Add(new Bullet(X, Y, bulletSpeed, 0f, false, damage));
            bullets.Add(new Bullet(X, Y, -bulletSpeed, 0f, false, damage));

            bullets.Add(new Bullet(X, Y, diagonal, diagonal, false, damage));
            bullets.Add(new Bullet(X, Y, -diagonal, diagonal, false, damage));
            bullets.Add(new Bullet(X, Y, diagonal, -diagonal, false, damage));
            bullets.Add(new Bullet(X, Y, -diagonal, -diagonal, false, damage));

            return bullets;
        }

        public override void Draw(Graphics g)
        {
            DrawSpriteOrFallback(g, Brushes.DarkGray);
            DrawHealthBar(g);
        }

        private void DrawHealthBar(Graphics g)
        {
            float barWidth = Width;
            float barHeight = 6f;

            float hpPercent = MaxHp <= 0 ? 0f : (float)Hp / MaxHp;

            if (hpPercent < 0f)
                hpPercent = 0f;

            if (hpPercent > 1f)
                hpPercent = 1f;

            g.FillRectangle(
                Brushes.Black,
                X - barWidth / 2f,
                Y - Height / 2f - 12f,
                barWidth,
                barHeight
            );

            g.FillRectangle(
                Brushes.LimeGreen,
                X - barWidth / 2f,
                Y - Height / 2f - 12f,
                barWidth * hpPercent,
                barHeight
            );

            g.DrawRectangle(
                Pens.White,
                X - barWidth / 2f,
                Y - Height / 2f - 12f,
                barWidth,
                barHeight
            );
        }
    }

    public class TerroristEnemy : Enemy
    {
        private readonly Player _targetPlayer;

        public TerroristEnemy(Player targetPlayer, float x, float y)
            : base(x, y, 180f)
        {
            Sprite = AssetLoader.LoadImage("enemy_terrorist.png");
            _targetPlayer = targetPlayer;

            Hp = 80;
            MaxHp = Hp;
            ScoreValue = 70;
            CoinValue = 5;
        }

        public override void Update(float deltaTime)
        {
            if (!IsActive)
                return;

            if (_targetPlayer == null || !_targetPlayer.IsActive)
            {
                Y += Speed * deltaTime;
                return;
            }

            float dx = _targetPlayer.X - X;
            float dy = _targetPlayer.Y - Y;

            float length = (float)Math.Sqrt(dx * dx + dy * dy);

            if (length > 0.001f)
            {
                dx /= length;
                dy /= length;

                X += dx * Speed * deltaTime;
                Y += dy * Speed * deltaTime;
            }
        }

        public override void Draw(Graphics g)
        {
            DrawSpriteOrFallback(g, Brushes.Purple, true);

            using (Pen warningPen = new Pen(Color.FromArgb(180, Color.OrangeRed), 2))
            {
                float radius = Width / 2f + 8f;
                g.DrawEllipse(
                    warningPen,
                    X - radius,
                    Y - radius,
                    radius * 2f,
                    radius * 2f
                );
            }
        }
    }
}