using System;
using System.Drawing;


namespace GameObject
{
    public abstract class Enemy : GameObject
    {
        public float Speed { get; set; } //5.3
        public Image Sprite { get; set; } //5.3
        public int ScoreValue { get; set; } //5.2
        public int CoinValue { get; set; } //5.2
        public abstract void Move();
        public virtual void Attack()
        {

        }
    }
}
