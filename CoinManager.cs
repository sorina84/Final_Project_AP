using System;
using System.Collections.Generic;
using System.Text;

namespace GameEntity
{
    public static class CoinManager
    {
        public static int Coins { get; private set; }

        public static void AddCoins(int amount)
        {
            if (amount <= 0)
                return;

            Coins += amount;
        }

        public static void Reset()
        {
            Coins = 0;
        }
    }
}