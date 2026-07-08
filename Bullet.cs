using System.Drawing;

namespace GameEntity
{
    public class Bullet : GameEntity
    {
        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }

        public int Damage { get; private set; }
        public bool IsPlayerBullet { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        private Image _sprite;

        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(X - Width / 2f, Y - Height / 2f, Width, Height);
            }
        }

        public Bullet(float x, float y, float speed, bool isPlayerBullet)
            : base(x, y, speed)
        {
            IsPlayerBullet = isPlayerBullet;
            Damage = isPlayerBullet ? 10 : 15;

            VelocityX = 0f;
            VelocityY = isPlayerBullet ? -speed : speed;

            SetSizeAndSprite();
        }

        public Bullet(float x, float y, float velocityX, float velocityY, bool isPlayerBullet, int damage)
            : base(x, y, 0f)
        {
            IsPlayerBullet = isPlayerBullet;
            Damage = damage;

            VelocityX = velocityX;
            VelocityY = velocityY;

            SetSizeAndSprite();
        }

        private void SetSizeAndSprite()
        {
            if (IsPlayerBullet)
            {
                Width = 32;
                Height = 52;
                _sprite = AssetLoader.LoadImage("Bullet_player.png");
            }
            else if (Damage >= 20)
            {
                Width = 42;
                Height = 42;
                _sprite = AssetLoader.LoadImage("Bullet_heavy.png");
            }
            else
            {
                Width = 30;
                Height = 46;
                _sprite = AssetLoader.LoadImage("Bullet_shotter.png");
            }
        }

        public override void Update(float deltaTime)
        {
            if (!IsActive)
                return;

            X += VelocityX * deltaTime;
            Y += VelocityY * deltaTime;

            if (X < -100 || X > 1000 || Y < -100 || Y > 900)
                IsActive = false;
        }

        public override void Draw(Graphics g)
        {
            if (!IsActive)
                return;

            if (_sprite != null)
            {
                g.DrawImage(_sprite, Bounds);
                return;
            }

            Brush brush = IsPlayerBullet ? Brushes.Cyan : Brushes.OrangeRed;
            g.FillEllipse(brush, Bounds);
        }
    }
}