using System.Drawing;

namespace GameEntity
{
    public enum PowerUpType
    {
        HealthPack,
        FireRateBoost,
        Shield,
        TripleShot
    }

    public class PowerUp : GameEntity
    {
        public PowerUpType Type { get; private set; }
        public float Radius { get; private set; }

        public PowerUp(float x, float y, PowerUpType type)
            : base(x, y, 120f)
        {
            Type = type;
            Radius = 12f;
            IsActive = true;
        }

        public RectangleF GetBounds()
        {
            return new RectangleF(X - Radius, Y - Radius, Radius * 2, Radius * 2);
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

            Brush brush;

            switch (Type)
            {
                case PowerUpType.HealthPack:
                    brush = Brushes.LimeGreen;
                    break;

                case PowerUpType.FireRateBoost:
                    brush = Brushes.Gold;
                    break;

                case PowerUpType.Shield:
                    brush = Brushes.DodgerBlue;
                    break;

                case PowerUpType.TripleShot:
                    brush = Brushes.Orange;
                    break;

                default:
                    brush = Brushes.White;
                    break;
            }

            g.FillEllipse(brush, X - Radius, Y - Radius, Radius * 2, Radius * 2);
            g.DrawEllipse(Pens.White, X - Radius, Y - Radius, Radius * 2, Radius * 2);
        }
    }
}