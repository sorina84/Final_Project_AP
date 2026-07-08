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
        private float shootTimer;
        private const float ShootDelay = 1.5f;
        public ShooterEnemy(float x, float y)
            : base(x, y, 80)
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
            shootTimer += deltaTime;
        }

        public override List<Bullet> Attack()
        {
            shootTimer++;
            if (shootTimer < 60)
                return null;
            shootTimer = 0;
            return new List<Bullet>()
            {
                new Bullet(X + Width/2,Y + Height,0,250,false , BulletType.Shooter)
            };
        }
    }


    public class HeavyTankEnemy : Enemy
    {
        private float shootTimer = 0f;
        private const float ShootInterval = 1f;
        public HeavyTankEnemy(float x, float y)
            : base(x, y, 40)
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
            shootTimer += deltaTime;
        }
        public override List<Bullet> Attack()
        {

            if (shootTimer < ShootInterval)
                return null;
            shootTimer = 0;
            float s = 250;
            float d = s / (float)Math.Sqrt(2);


            return new List<Bullet>()
            {
                new Bullet(X+Width/2,Y+Height/2,0,s,false , BulletType.Heavy),
                new Bullet(X+Width/2,Y+Height/2,s,0,false, BulletType.Heavy),
                new Bullet(X+Width/2,Y+Height/2,-s,0,false, BulletType.Heavy),
                new Bullet(X+Width/2,Y+Height/2,0,-s,false, BulletType.Heavy),

                new Bullet(X+Width/2,Y+Height/2,d,d,false, BulletType.Heavy),
                new Bullet(X+Width/2,Y+Height/2,-d,d,false, BulletType.Heavy),
                new Bullet(X+Width/2,Y+Height/2,d,-d,false, BulletType.Heavy),
                new Bullet(X+Width/2,Y+Height/2,-d,-d,false, BulletType.Heavy)
            };
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
