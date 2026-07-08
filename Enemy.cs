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
        protected Image Sprite { get; set; }

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

        public virtual void ResetPosition(float x, float y)
        {
            X = x;
            Y = y;
        }

        protected void DrawSpriteOrFallback(Graphics g, Brush fallbackBrush, bool ellipseFallback = false)
        {
            if (!IsActive)
                return;

            RectangleF rect = Bounds;

            if (Sprite != null)
            {
                g.DrawImage(Sprite, rect);
                return;
            }

            if (ellipseFallback)
                g.FillEllipse(fallbackBrush, rect);
            else
                g.FillRectangle(fallbackBrush, rect);
        }
    }
}