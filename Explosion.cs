using System.Drawing;

namespace GameEntity
{
    public class Explosion : GameEntity
    {
        private float _age;
        private readonly float _lifeTime;
        private readonly float _maxRadius;

        public Explosion(float x, float y)
            : base(x, y, 0f)
        {
            _age = 0f;
            _lifeTime = 0.45f;
            _maxRadius = 58f;
        }

        public override void Update(float deltaTime)
        {
            if (!IsActive)
                return;

            _age += deltaTime;

            if (_age >= _lifeTime)
                IsActive = false;
        }

        public override void Draw(Graphics g)
        {
            if (!IsActive)
                return;

            float progress = _age / _lifeTime;

            if (progress < 0f)
                progress = 0f;

            if (progress > 1f)
                progress = 1f;

            float radius = 8f + _maxRadius * progress;
            int alpha = (int)(220 * (1f - progress));

            if (alpha < 0)
                alpha = 0;

            using (SolidBrush orangeBrush = new SolidBrush(Color.FromArgb(alpha, 255, 130, 20)))
            using (Pen yellowPen = new Pen(Color.FromArgb(alpha, 255, 230, 80), 3))
            using (Pen redPen = new Pen(Color.FromArgb(alpha, 255, 40, 20), 2))
            {
                g.FillEllipse(
                    orangeBrush,
                    X - radius,
                    Y - radius,
                    radius * 2f,
                    radius * 2f
                );

                g.DrawEllipse(
                    yellowPen,
                    X - radius,
                    Y - radius,
                    radius * 2f,
                    radius * 2f
                );

                g.DrawEllipse(
                    redPen,
                    X - radius * 0.65f,
                    Y - radius * 0.65f,
                    radius * 1.3f,
                    radius * 1.3f
                );
            }
        }
    }
}