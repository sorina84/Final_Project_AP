using System.Collections.Generic;
using System.Drawing;

namespace GameEntity
{
    public abstract class Enemy : GameEntity
    {
        public int Hp { get; set; }
        public int MaxHp { get; set; }
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

        public virtual List<Bullet> Attack()
        {
            return new List<Bullet>();
        }

        public bool IsOutOfScreen(int screenHeight)
        {
            return Y - Height / 2f > screenHeight + 50;
        }
    }
}