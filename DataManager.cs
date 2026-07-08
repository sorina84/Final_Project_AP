using System;
using System.Collections.Generic;
using System.Text;

namespace GameEntity
{
    public class DataManager
    {
        public PlayerData LoadPlayerData()
        {
            return new PlayerData();
        }

        public void SavePlayerData(PlayerData data)
        {
            // SQLite persistence will be implemented in the next phase.
        }
    }
}