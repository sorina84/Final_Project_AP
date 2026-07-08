using System;
using System.Drawing;

namespace GameEntity
{
    public class Bullet : GameEntity
    {
        public int Damage { get; set; }
        public bool IsPlayerBullet { get; set; }

        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }

        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(X - 3, Y - 8, 6, 16);
            }
        }

        public Bullet(float x, float y, float speedY, bool isPlayerBullet)
            : base(x, y, Math.Abs(speedY))
        {
            IsPlayerBullet = isPlayerBullet;

            VelocityX = 0f;
            VelocityY = isPlayerBullet ? -Math.Abs(speedY) : Math.Abs(speedY);

            Damage = isPlayerBullet ? 10 : 15;
        }

        public Bullet(float x, float y, float velocityX, float velocityY, bool isPlayerBullet, int damage)
            : base(x, y, Math.Abs(velocityY))
        {
            IsPlayerBullet = isPlayerBullet;
            VelocityX = velocityX;
            VelocityY = velocityY;
            Damage = damage;
        }

        public override void Update(float deltaTime)
        {
            if (!IsActive)
                return;

            X += VelocityX * deltaTime;
            Y += VelocityY * deltaTime;

            if (X < -20 || X > 820 || Y < -20 || Y > 680)
                IsActive = false;
        }

        public override void Draw(Graphics g)
        {
            if (!IsActive)
                return;

            Brush color = IsPlayerBullet ? Brushes.Yellow : Brushes.OrangeRed;
            g.FillRectangle(color, X - 3, Y - 8, 6, 16);
        }
    }
}