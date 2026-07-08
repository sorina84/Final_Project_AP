using System;
using System.Drawing;

namespace GameEntity
{
    public enum CoinType
    {
        Silver,
        Gold
    }

    public class Coin : GameEntity
    {
        public CoinType Type { get; private set; }
        public int Value { get; private set; }
        public int Size { get; private set; }

        private Image _sprite;

        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(X - Size / 2f, Y - Size / 2f, Size, Size);
            }
        }

        public Coin(float x, float y, CoinType type)
            : base(x, y, 90f)
        {
            Type = type;

            if (type == CoinType.Gold)
            {
                Value = 5;
                Size = 26;
            }
            else
            {
                Value = 1;
                Size = 22;
            }

            LoadSprite();
        }

        private void LoadSprite()
        {
            try
            {
                if (Type == CoinType.Gold)
                    _sprite = Image.FromFile("Assets/coin_gold.png");
                else
                    _sprite = Image.FromFile("Assets/coin_silver.png");
            }
            catch
            {
                _sprite = null;
            }
        }

        public override void Update(float deltaTime)
        {
            if (!IsActive)
                return;

            Y += Speed * deltaTime;

            if (Y > 650)
                IsActive = false;
        }

        public override void Draw(Graphics g)
        {
            if (!IsActive)
                return;

            if (_sprite != null)
            {
                g.DrawImage(_sprite, X - Size / 2f, Y - Size / 2f, Size, Size);
                return;
            }

            Brush fillBrush = Type == CoinType.Gold ? Brushes.Gold : Brushes.Silver;
            Pen borderPen = Type == CoinType.Gold ? Pens.Orange : Pens.White;

            g.FillEllipse(fillBrush, Bounds);
            g.DrawEllipse(borderPen, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
        }
    }
}