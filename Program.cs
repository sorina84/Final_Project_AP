using System;
using System.Drawing;
namespace GameObject
{
    public abstract class GameObject
    {
        //public float X { get; set; }
        //public float Y { get; set; }
        //public int Width { get; set; } = 40;
        //public int Height { get; set; } = 40;
        //public Rectangle Bounds => new Rectangle((int)X, (int)Y, Width, Height);
        public Rectangle Bounds { get; set; }
        public int Hp { get; set; }
        public bool IsDestroyed => Hp <= 0;        
    }
}
