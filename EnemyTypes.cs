using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
namespace GameObject
{
    public class StandardEnemy : Enemy
    {
        public StandardEnemy()
        {
            Hp = 20;
            Speed = 4;
            ScoreValue = 10;
            CoinValue = 1;
        }

        public override void Move()
        {
            Bounds = new Rectangle (Bounds.X,Bounds.Y + (int)Speed ,Bounds.Width ,Bounds.Height);
        }
    }
    public class ScoutEnemy : Enemy
    {
        private float angle;
        //private int initialX;
        public ScoutEnemy()
        {
            Hp = 15;
            Speed = 6;
            ScoreValue = 20;
            CoinValue = 2;
        }

        public override void Move()
        {
            angle += 0.1f;

            int xOffset = (int)(Math.Sin(angle) * 30);

            Bounds = new Rectangle(Bounds.X + xOffset,Bounds.Y + (int)Speed,Bounds.Width,Bounds.Height);
        }
    }




    public class ShooterEnemy : Enemy
    {
        public ShooterEnemy()
        {
            Hp = 40;
            Speed = 2;
            ScoreValue = 40;
            CoinValue = 3;
        }

        public override void Move()
        {
            Bounds = new Rectangle(Bounds.X,Bounds.Y + (int)Speed,Bounds.Width,Bounds.Height);
        }

        public override void Attack()
        {
            // ساخت گلوله دشمن
        }
    }
    public class HeavyTankEnemy : Enemy
    {
        public HeavyTankEnemy()
        {
            Hp = 150;
            Speed = 1;
            ScoreValue = 100;
            CoinValue = 10;
        }

        public override void Move()
        {
            Bounds = new Rectangle(Bounds.X,Bounds.Y + (int)Speed,Bounds.Width,Bounds.Height);
        }

        public override void Attack()
        {
            // 8 جهت
        }
    }
    public class TerroristEnemy : Enemy
    {
        private Player target;

        public TerroristEnemy(Player player)
        {
            target = player;

            Hp = 80;
            Speed = 3;

            ScoreValue = 70;
            CoinValue = 5;
        }

        public override void Move()
        {
            if (target.Bounds.X < Bounds.X)
                Bounds = new Rectangle(Bounds.X - (int)Speed,Bounds.Y + (int)Speed,Bounds.Width,Bounds.Height);

            else
                Bounds = new Rectangle(Bounds.X + (int)Speed,Bounds.Y + (int)Speed,Bounds.Width,Bounds.Height);
        }

            // ۱. ابتدا ابعاد فعلی رو حفظ می‌کنیم تا غیب نشن (اگر توی حلقه اصلی مقداردهی می‌شن)
            //int currentWidth = Bounds.Width == 0 ? 45 : Bounds.Width;
            //int currentHeight = Bounds.Height == 0 ? 45 : Bounds.Height;
            //int newX = Bounds.X;
            //int newY = Bounds.Y + (int)Speed;
            //int deltaX = target.Bounds.X - Bounds.X;
            //if (Math.Abs(deltaX) > (int)Speed)
            //{
            //    if (target.Bounds.X < Bounds.X)
              //      newX -= (int)Speed; // حرکت به چپ
                //else
                  //  newX += (int)Speed; // حرکت به راست
            //}
            //Bounds = new Rectangle(newX, newY, currentWidth, currentHeight);
        
    }
}
