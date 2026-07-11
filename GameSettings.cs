namespace GameEntity
{
    public static class GameSettings
    {
        public static bool MusicEnabled { get; set; } = true;
        public static bool SfxEnabled { get; set; } = true;

        public static string EquippedShipSkin { get; set; } = "DefaultShip";
        public static string EquippedBulletStyle { get; set; } = "DefaultBullet";
        public static string EquippedBackground { get; set; } = "DefaultBackground";

        public static bool ExtraLifeBoosterEnabled { get; set; } = false;
    }
}