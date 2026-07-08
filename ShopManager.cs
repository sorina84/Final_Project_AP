using System.Collections.Generic;

namespace GameEntity
{
    public enum ShopItem
    {
        FalconShip,
        CyberShip,
        GreenBullet,
        PlasmaBullet,
        GalaxyBackground,
        MarsBackground,
        ExtraLife
    }

    public class PlayerData
    {
        public int Coins { get; set; }
        public int HighScore { get; set; }

        public string ShipSkin { get; set; }
        public string BulletSkin { get; set; }
        public string BackgroundSkin { get; set; }

        public int ExtraLives { get; set; }

        public HashSet<ShopItem> PurchasedItems { get; private set; }

        public PlayerData()
        {
            Coins = 0;
            HighScore = 0;

            ShipSkin = "Default";
            BulletSkin = "Default";
            BackgroundSkin = "Default";

            ExtraLives = 0;
            PurchasedItems = new HashSet<ShopItem>();
        }
    }

    public class ShopManager
    {
        private readonly Dictionary<ShopItem, int> _prices;

        public ShopManager()
        {
            _prices = new Dictionary<ShopItem, int>
            {
                { ShopItem.FalconShip, 500 },
                { ShopItem.CyberShip, 800 },
                { ShopItem.GreenBullet, 300 },
                { ShopItem.PlasmaBullet, 600 },
                { ShopItem.GalaxyBackground, 400 },
                { ShopItem.MarsBackground, 700 },
                { ShopItem.ExtraLife, 200 }
            };
        }

        public int GetPrice(ShopItem item)
        {
            return _prices[item];
        }

        public bool IsPurchased(PlayerData data, ShopItem item)
        {
            return data.PurchasedItems.Contains(item);
        }

        public bool CanBuy(PlayerData data, ShopItem item)
        {
            if (item != ShopItem.ExtraLife && IsPurchased(data, item))
                return false;

            return data.Coins >= GetPrice(item);
        }

        public bool BuyItem(PlayerData data, ShopItem item)
        {
            if (!CanBuy(data, item))
                return false;

            data.Coins -= GetPrice(item);

            if (item != ShopItem.ExtraLife)
                data.PurchasedItems.Add(item);

            EquipItem(data, item);

            return true;
        }

        public void EquipItem(PlayerData data, ShopItem item)
        {
            switch (item)
            {
                case ShopItem.FalconShip:
                    data.ShipSkin = "Falcon";
                    break;

                case ShopItem.CyberShip:
                    data.ShipSkin = "Cyber";
                    break;

                case ShopItem.GreenBullet:
                    data.BulletSkin = "Green";
                    break;

                case ShopItem.PlasmaBullet:
                    data.BulletSkin = "Plasma";
                    break;

                case ShopItem.GalaxyBackground:
                    data.BackgroundSkin = "Galaxy";
                    break;

                case ShopItem.MarsBackground:
                    data.BackgroundSkin = "Mars";
                    break;

                case ShopItem.ExtraLife:
                    data.ExtraLives++;
                    break;
            }
        }
    }
}