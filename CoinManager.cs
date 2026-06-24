using System;
using System.Collections.Generic;
using System.Text;

namespace GameObject
{
    public static class CoinManager
    {
        public static int Coins { get; private set; }

        public static void AddCoins(int amount)
        {
            Coins += amount;
        }

        public static void Reset()
        {
            Coins = 0;
        }
    }
}
