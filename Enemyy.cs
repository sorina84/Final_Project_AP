using System;
using System.Drawing;


namespace GameObject
{
    public abstract class Enemy : GameObject
    {
        public int Hp { get; set; }
        public int ScoreValue { get; set; }
        public int CoinValue { get; set; }
        public virtual int Width => 40;
        public virtual int Height => 40;

        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(X - Width / 2f, Y - Height / 2f, Width, Height);
            }
        }

        protected Enemy(float x, float y, float speed) : base(x, y, speed)
        {
        }

        public virtual void Attack()
        {
            // بعداً برای دشمن‌های تیرانداز استفاده می‌شود.
        }
    }
}
