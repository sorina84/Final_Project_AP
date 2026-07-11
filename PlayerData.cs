using System.Collections.Generic;

namespace GameEntity
{
    public class PlayerData
    {
        public int TotalCoins { get; set; } = 0;
        public int HighScore { get; set; } = 0;

        public bool MusicEnabled { get; set; } = true;
        public bool SfxEnabled { get; set; } = true;

        public string EquippedShipSkin { get; set; } = "DefaultShip";
        public string EquippedBulletStyle { get; set; } = "DefaultBullet";
        public string EquippedBackground { get; set; } = "DefaultBackground";

        public bool ExtraLifeBoosterEnabled { get; set; } = false;

        public List<string> PurchasedItems { get; set; } = new List<string>();
    }
}