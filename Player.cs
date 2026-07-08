using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameEntity
{
    public class Player : GameEntity
    {
        public Image Sprite { get; private set; }

        public int Width => 60;
        public int Height => 60;

        public int Lives { get; set; }
        public int Hp { get; set; }

        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }

        public bool IsShield { get; private set; }
        public bool IsTripleShot { get; private set; }
        public bool IsFireRateBoost { get; private set; }

        public float ShieldTimeLeft => _shieldTimer;
        public float TripleShotTimeLeft => _tripleShotTimer;
        public float FireRateBoostTimeLeft => _fireRateBoostTimer;

        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }

        private const float Acceleration = 900f;
        private const float Friction = 0.92f;
        private const float MaxSpeed = 420f;

        private float _shootCooldown = 0.2f;
        private float _timeSinceLastShot = 0.2f;

        private float _shieldTimer = 0f;
        private float _tripleShotTimer = 0f;
        private float _fireRateBoostTimer = 0f;

        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(X - Width / 2f, Y - Height / 2f, Width, Height);
            }
        }

        public Player(float x, float y) : base(x, y, 0f)
        {
            Hp = 100;
            Lives = 3;

            VelocityX = 0f;
            VelocityY = 0f;

            ScreenWidth = 800;
            ScreenHeight = 600;

            LoadSprite();
        }

        public void LoadSprite()
        {
            try
            {
                Sprite = AssetLoader.LoadImage("player.png");
            }
            catch
            {
                Sprite = null;
            }
        }

        public void AccelerateLeft(float deltaTime)
        {
            VelocityX -= Acceleration * deltaTime;
        }

        public void AccelerateRight(float deltaTime)
        {
            VelocityX += Acceleration * deltaTime;
        }

        public void AccelerateUp(float deltaTime)
        {
            VelocityY -= Acceleration * deltaTime;
        }

        public void AccelerateDown(float deltaTime)
        {
            VelocityY += Acceleration * deltaTime;
        }

        public bool CanShoot()
        {
            return _timeSinceLastShot >= _shootCooldown;
        }

        public List<Bullet> Shoot()
        {
            var bullets = new List<Bullet>();

            if (!CanShoot())
                return bullets;

            _timeSinceLastShot = 0f;

            float bulletSpeed = 520f;

            if (IsTripleShot)
            {
                bullets.Add(new Bullet(X, Y - Height / 2f, 0f, -bulletSpeed, true, 10));
                bullets.Add(new Bullet(X - 12f, Y - Height / 2f, -140f, -bulletSpeed, true, 10));
                bullets.Add(new Bullet(X + 12f, Y - Height / 2f, 140f, -bulletSpeed, true, 10));
            }
            else
            {
                bullets.Add(new Bullet(X, Y - Height / 2f, 0f, -bulletSpeed, true, 10));
            }

            return bullets;
        }

        public void ActivatePowerUp(PowerUpType type)
        {
            switch (type)
            {
                case PowerUpType.HealthPack:
                    Hp = Math.Min(Hp + 40, 100);
                    break;

                case PowerUpType.Shield:
                    IsShield = true;
                    _shieldTimer = 5f;
                    break;

                case PowerUpType.TripleShot:
                    IsTripleShot = true;
                    _tripleShotTimer = 10f;
                    break;

                case PowerUpType.FireRateBoost:
                    IsFireRateBoost = true;
                    _fireRateBoostTimer = 10f;
                    _shootCooldown = 0.1f;
                    break;
            }
        }

        public override void Update(float deltaTime)
        {
            if (!IsActive)
                return;

            VelocityX *= Friction;
            VelocityY *= Friction;

            VelocityX = Clamp(VelocityX, -MaxSpeed, MaxSpeed);
            VelocityY = Clamp(VelocityY, -MaxSpeed, MaxSpeed);

            if (Math.Abs(VelocityX) < 1f)
                VelocityX = 0f;

            if (Math.Abs(VelocityY) < 1f)
                VelocityY = 0f;

            X += VelocityX * deltaTime;
            Y += VelocityY * deltaTime;

            ClampToScreen();

            _timeSinceLastShot += deltaTime;

            UpdatePowerUpTimers(deltaTime);
        }

        private void UpdatePowerUpTimers(float deltaTime)
        {
            if (IsShield)
            {
                _shieldTimer -= deltaTime;

                if (_shieldTimer <= 0f)
                {
                    _shieldTimer = 0f;
                    IsShield = false;
                }
            }

            if (IsTripleShot)
            {
                _tripleShotTimer -= deltaTime;

                if (_tripleShotTimer <= 0f)
                {
                    _tripleShotTimer = 0f;
                    IsTripleShot = false;
                }
            }

            if (IsFireRateBoost)
            {
                _fireRateBoostTimer -= deltaTime;

                if (_fireRateBoostTimer <= 0f)
                {
                    _fireRateBoostTimer = 0f;
                    IsFireRateBoost = false;
                    _shootCooldown = 0.2f;
                }
            }
        }

        private void ClampToScreen()
        {
            float halfWidth = Width / 2f;
            float halfHeight = Height / 2f;

            if (X - halfWidth < 0)
            {
                X = halfWidth;
                VelocityX = 0f;
            }

            if (Y - halfHeight < 0)
            {
                Y = halfHeight;
                VelocityY = 0f;
            }

            if (X + halfWidth > ScreenWidth)
            {
                X = ScreenWidth - halfWidth;
                VelocityX = 0f;
            }

            if (Y + halfHeight > ScreenHeight)
            {
                Y = ScreenHeight - halfHeight;
                VelocityY = 0f;
            }
        }

        public override void Draw(Graphics g)
        {
            if (!IsActive)
                return;

            if (IsShield)
            {
                float radius = Math.Max(Width, Height) / 2f + 8f;
                g.DrawEllipse(Pens.Cyan, X - radius, Y - radius, radius * 2f, radius * 2f);
            }

            if (Sprite != null)
            {
                g.DrawImage(Sprite, X - Width / 2f, Y - Height / 2f, Width, Height);
            }
            else
            {
                PointF[] ship =
                {
                    new PointF(X, Y - 24f),
                    new PointF(X - 18f, Y + 22f),
                    new PointF(X + 18f, Y + 22f)
                };

                g.FillPolygon(Brushes.Cyan, ship);
            }
        }

        public void Reset()
        {
            Hp = 100;
            Lives = 3;

            X = ScreenWidth / 2f;
            Y = ScreenHeight - 100f;

            VelocityX = 0f;
            VelocityY = 0f;

            IsShield = false;
            IsTripleShot = false;
            IsFireRateBoost = false;

            _shieldTimer = 0f;
            _tripleShotTimer = 0f;
            _fireRateBoostTimer = 0f;
            _shootCooldown = 0.2f;
            _timeSinceLastShot = 0.2f;

            IsActive = true;
        }

        private float Clamp(float value, float min, float max)
        {
            if (value < min)
                return min;

            if (value > max)
                return max;

            return value;
        }
    }
}