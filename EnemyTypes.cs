using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace GameObject
{
    public class StandardEnemy : Enemy
    {
        public StandardEnemy(float x, float y)
            : base(x, y, 150)
        {
            Hp = 20;
            ScoreValue = 10;
            CoinValue = 1;
        }

        public override void Update(float deltaTime)
        {
            Y += Speed * deltaTime;
        }

        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Red, X, Y, 40, 40);
        }
    }



    public class ScoutEnemy : Enemy
    {
        private float angle;
        public ScoutEnemy(float x, float y)
            : base(x, y, 220)
        {
            Hp = 15;
            ScoreValue = 20;
            CoinValue = 2;
        }
        public override void Update(float deltaTime)
        {
            angle += 3f * deltaTime;
            X += (float)System.Math.Sin(angle) * 80 * deltaTime;
            Y += Speed * deltaTime;
        }
        public override void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.Blue, X, Y, 35, 35);
        }
    }

    public class ShooterEnemy : Enemy
    {
        public ShooterEnemy(float x, float y)
            : base(x, y, 100)
        {
            Hp = 40;
            ScoreValue = 40;
            CoinValue = 3;
        }
        public override void Update(float deltaTime)
        {
            Y += Speed * deltaTime;
        }
        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.Green, X, Y, 45, 45);
        }
        public override void Attack()
        {
            // بعداً گلوله دشمن اینجا ساخته می‌شود.
        }
    }


    public class HeavyTankEnemy : Enemy
    {
        public HeavyTankEnemy(float x, float y)
            : base(x, y, 60)
        {
            Hp = 150;
            ScoreValue = 100;
            CoinValue = 10;
        }
        public override void Update(float deltaTime)
        {
            Y += Speed * deltaTime;
        }
        public override void Draw(Graphics g)
        {
            g.FillRectangle(Brushes.DarkGray, X, Y, 55, 55);
        }
        public override void Attack()
        {
        }
    }

    public class TerroristEnemy : Enemy
    {
        public TerroristEnemy(float x, float y)
            : base(x, y, 170)
        {
            Hp = 80;
            ScoreValue = 70;
            CoinValue = 5;
        }

        public override void Update(float deltaTime)
        {
            Y += Speed * deltaTime;

        }

        public override void Draw(Graphics g)
        {
            g.FillEllipse(Brushes.Purple, X, Y, 45, 45);
        }
    }
}
