using System;
using System.Drawing;


namespace GameObject
{
    public abstract class Enemy : GameEntity
    {
        public Image Sprite { get; protected set; }
        public int Width { get; protected set; } = 50;
        public int Height { get; protected set; } = 50;

        public int Hp { get; set; }
        public int ScoreValue { get; set; }
        public int CoinValue { get; set; }


        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(X, Y, Width, Height);
            }
        }

        protected Enemy(float x, float y, float speed) : base(x, y, speed)
        {
        }

        public override void Draw(Graphics g)
        {
            if (!IsActive)
                return;
            if (Sprite != null)
            {
                g.DrawImage(Sprite, X, Y, Width, Height);
            }
            else
            {
                g.FillRectangle(Brushes.Red, X, Y, Width, Height);
            }
        }

        public virtual void Attack()
        {
            // بعداً برای دشمن‌های تیرانداز استفاده می‌شود.
        }
    }
}
