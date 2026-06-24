using System;
using System.Collections.Generic;
using System.Text;

namespace GameObject
{
    public static class ScoreManager
    {
        public static int Score { get; private set; }
        public static void AddScore(int amount)
        {
            Score += amount;
        }
        public static void Reset()
        {
            Score = 0;
        }
    }
}
