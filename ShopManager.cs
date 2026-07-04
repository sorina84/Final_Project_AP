using System;
using System.Collections.Generic;
using System.Text;

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
        // اطلاعات ذخیره‌شونده
        //public int Coins { get; set; }
        //public int HighScore { get; set; }
        // آیتم‌های خریداری شده
        public string ShipSkin { get; set; }
        public string BulletSkin { get; set; }
        public string BackgroundSkin { get; set; }
        // آیتم مصرفی
        public int ExtraLives { get; set; }
        public PlayerData()
        {
            //Coins = 0;
            //HighScore = 0;
            ShipSkin = "Default";
            BulletSkin = "Default";
            BackgroundSkin = "Default";
            ExtraLives = 0;
        }
    }


    public class ShopManager
    {
        private readonly Dictionary<ShopItem, int> prices;

        public ShopManager()
        {
            prices = new Dictionary<ShopItem, int>()
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

        //--------------------------------------------------

        public int GetPrice(ShopItem item)
        {
            return prices[item];
        }

        //--------------------------------------------------

        public bool CanBuy(ShopItem item)
        {
            return CoinManager.Coins >= GetPrice(item);
        }

        //--------------------------------------------------

        public bool BuyItem(PlayerData data, ShopItem item)
        {
            if (!CoinManager.SpendCoins(GetPrice(item)))
                return false;

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

            // ذخیره تغییرات
            DataManager.Save(data);

            return true;
        }
    }
}
