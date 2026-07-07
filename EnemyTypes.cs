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


            Width = 50;
            Height = 50;
            try
            {
                Sprite = Image.FromFile("Assets/enemy_standard.png");

            }
            catch
            {
                Sprite = null;
            }
        }

        public override void Update(float deltaTime)
        {
            Y += Speed * deltaTime;
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

            Width = 45;
            Height = 45;

            try
            {
                Sprite = Image.FromFile("Assets/enemy_scout.png");

            }
            catch
            {
                Sprite = null;
            }
        }
        public override void Update(float deltaTime)
        {
            angle += 3f * deltaTime;
            X += (float)System.Math.Sin(angle) * 80 * deltaTime;
            Y += Speed * deltaTime;
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

            Width = 55;
            Height = 55;
            try
            {
                Sprite = Image.FromFile("Assets/enemy_shooter.png");
            }
            catch
            {
                Sprite = null;
            }
        }
        public override void Update(float deltaTime)
        {
            Y += Speed * deltaTime;
        }
        public override List<Bullet> Attack()
        {
            return null;
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

            Width = 80;
            Height = 80;

            try
            {
                Sprite = Image.FromFile("Assets/enemy_heavy.png");

            }
            catch
            {
                Sprite = null;
            }
        }
        public override void Update(float deltaTime)
        {
            Y += Speed * deltaTime;
        }
        public override List<Bullet> Attack()
        {
            return null;
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

            Width = 60;
            Height = 60;
            try
            {
                Sprite = Image.FromFile("Assets/enemy_terrorist.png");

            }
            catch
            {
                Sprite = null;
            }
        }

        public override void Update(float deltaTime)
        {
            Y += Speed * deltaTime;

        }
    }
}
