using System;
using System.Collections.Generic;
using System.Text;

namespace GameEntity
{
    public static class ScoreManager
    {
        public static int Score { get; private set; }

        public static void AddScore(int amount)
        {
            if (amount <= 0)
                return;

            Score += amount;
        }

        public static void Reset()
        {
            Score = 0;
        }
    }
}