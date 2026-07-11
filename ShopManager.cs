using System.Collections.Generic;

namespace GameEntity
{
    public class ShopItem
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Category { get; private set; }
        public string Description { get; private set; }
        public int Price { get; private set; }

        public ShopItem(string id, string name, string category, string description, int price)
        {
            Id = id;
            Name = name;
            Category = category;
            Description = description;
            Price = price;
        }
    }

    public static class ShopManager
    {
        private static readonly List<ShopItem> _items = new List<ShopItem>
        {
            new ShopItem(
                "FalconShip",
                "Falcon Ship",
                "Ship Skins",
                "A sharp and fast-looking player ship skin.",
                40
            ),

            new ShopItem(
                "CyberShip",
                "Cyber Ship",
                "Ship Skins",
                "A futuristic neon player ship skin.",
                75
            ),

            new ShopItem(
                "GreenBullet",
                "Green Bullet",
                "Bullet Styles",
                "Changes player bullets to a green energy style.",
                30
            ),

            new ShopItem(
                "PlasmaBullet",
                "Plasma Bullet",
                "Bullet Styles",
                "Changes player bullets to a stronger plasma visual style.",
                55
            ),

            new ShopItem(
                "BlueNebulaBackground",
                "Blue Nebula",
                "Background Themes",
                "A blue galaxy style background for gameplay.",
                50
            ),

            new ShopItem(
                "PinkGalaxyBackground",
                "Pink Galaxy",
                "Background Themes",
                "A pink and purple galaxy background theme.",
                60
            ),

            new ShopItem(
                "ExtraLifeBooster",
                "Extra Life",
                "Boosters",
                "Start the next game with 4 lives instead of 3.",
                100
            )
        };

        private static readonly HashSet<string> _purchasedItems = new HashSet<string>();

        public static IReadOnlyList<ShopItem> Items => _items;

        public static bool IsPurchased(string itemId)
        {
            return _purchasedItems.Contains(itemId);
        }

        public static bool BuyItem(string itemId, out string message)
        {
            ShopItem item = FindItem(itemId);

            if (item == null)
            {
                message = "Item not found.";
                return false;
            }

            if (IsPurchased(itemId))
            {
                message = "You already own this item.";
                return false;
            }

            if (!CoinManager.SpendCoins(item.Price))
            {
                message = "Not enough coins.";
                return false;
            }

            _purchasedItems.Add(itemId);
            EquipItem(itemId);

            message = "Item purchased successfully.";
            return true;
        }

        public static void EquipItem(string itemId)
        {
            ShopItem item = FindItem(itemId);

            if (item == null)
                return;

            if (item.Category == "Ship Skins")
            {
                GameSettings.EquippedShipSkin = item.Id;
            }
            else if (item.Category == "Bullet Styles")
            {
                GameSettings.EquippedBulletStyle = item.Id;
            }
            else if (item.Category == "Background Themes")
            {
                GameSettings.EquippedBackground = item.Id;
            }
            else if (item.Category == "Boosters")
            {
                if (item.Id == "ExtraLifeBooster")
                    GameSettings.ExtraLifeBoosterEnabled = true;
            }
        }

        public static bool IsEquipped(ShopItem item)
        {
            if (item.Category == "Ship Skins")
                return GameSettings.EquippedShipSkin == item.Id;

            if (item.Category == "Bullet Styles")
                return GameSettings.EquippedBulletStyle == item.Id;

            if (item.Category == "Background Themes")
                return GameSettings.EquippedBackground == item.Id;

            if (item.Category == "Boosters")
                return item.Id == "ExtraLifeBooster" && GameSettings.ExtraLifeBoosterEnabled;

            return false;
        }

        private static ShopItem FindItem(string itemId)
        {
            foreach (ShopItem item in _items)
            {
                if (item.Id == itemId)
                    return item;
            }

            return null;
        }
    }
}