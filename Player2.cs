using System;
using System.Collections.Generic;
using System.Text;

namespace GameObject
{
    using System.Drawing;

    public class Player : GameObject
    {
        public int Lives { get; set; }
        public int FireRate { get; set; }
        public Player()
        {
            Hp = 100;
            Lives = 3;
            FireRate = 200;
        }
    }
}
