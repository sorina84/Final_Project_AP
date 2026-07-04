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

            if (Coins < 0)
                Coins = 0;
        }

        public static bool SpendCoins(int amount)
        {
            if (Coins < amount)
                return false;

            Coins -= amount;

            return true;
        }

        public static void SetCoins(int amount)
        {
            Coins = amount;

            if (Coins < 0)
                Coins = 0;
        }

        public static void Reset()
        {
            Coins = 0;
        }
    }
}
