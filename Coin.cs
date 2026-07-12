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
        private float _lifeTimer;

        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(
                    X - Size / 2f,
                    Y - Size / 2f,
                    Size,
                    Size
                );
            }
        }

        public Coin(float x, float y, CoinType type)
            : base(x, y, 0f)
        {
            Type = type;
            _lifeTimer = 8f;

            if (type == CoinType.Gold)
            {
                Value = 5;
                Size = 28;
                _sprite = AssetLoader.LoadImage("coin_gold.png");
            }
            else
            {
                Value = 1;
                Size = 24;
                _sprite = AssetLoader.LoadImage("coin_silver.png");
            }
        }

        public override void Update(float deltaTime)
        {
            if (!IsActive)
                return;

            _lifeTimer -= deltaTime;

            if (_lifeTimer <= 0f)
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

            Brush brush = Type == CoinType.Gold ? Brushes.Gold : Brushes.Silver;
            Pen pen = Type == CoinType.Gold ? Pens.Orange : Pens.White;

            g.FillEllipse(brush, Bounds);
            g.DrawEllipse(pen, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);
        }
    }
}