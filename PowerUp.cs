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

        public int Radius { get; private set; } = 14;

        public RectangleF Bounds
        {
            get
            {
                return new RectangleF(
                    X - Radius,
                    Y - Radius,
                    Radius * 2,
                    Radius * 2
                );
            }
        }

        public RectangleF GetBounds()
        {
            return Bounds;
        }

        public PowerUp(float x, float y, PowerUpType type)
            : base(x, y, 120f)
        {
            Type = type;
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

            Brush fillBrush = Brushes.LimeGreen;
            Pen borderPen = Pens.White;
            string text = "?";

            switch (Type)
            {
                case PowerUpType.HealthPack:
                    fillBrush = Brushes.LimeGreen;
                    text = "HP";
                    break;

                case PowerUpType.Shield:
                    fillBrush = Brushes.DeepSkyBlue;
                    text = "S";
                    break;

                case PowerUpType.TripleShot:
                    fillBrush = Brushes.MediumPurple;
                    text = "3X";
                    break;

                case PowerUpType.FireRateBoost:
                    fillBrush = Brushes.Orange;
                    text = "FR";
                    break;
            }

            g.FillEllipse(fillBrush, Bounds);
            g.DrawEllipse(borderPen, Bounds.X, Bounds.Y, Bounds.Width, Bounds.Height);

            using (Font font = new Font("Arial", 8, FontStyle.Bold))
            using (Brush textBrush = new SolidBrush(Color.Black))
            using (StringFormat format = new StringFormat())
            {
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                g.DrawString(text, font, textBrush, Bounds, format);
            }
        }
    }
}