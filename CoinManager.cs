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

        public static bool SpendCoins(int amount)
        {
            if (amount <= 0)
                return true;

            if (Coins < amount)
                return false;

            Coins -= amount;
            return true;
        }

        public static void SetCoins(int amount)
        {
            if (amount < 0)
                amount = 0;

            Coins = amount;
        }

        public static void Reset()
        {
            Coins = 0;
        }
    }
}