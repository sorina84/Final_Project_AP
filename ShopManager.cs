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
        public string PreviewFileName { get; private set; }

        public ShopItem(
            string id,
            string name,
            string category,
            string description,
            int price,
            string previewFileName)
        {
            Id = id;
            Name = name;
            Category = category;
            Description = description;
            Price = price;
            PreviewFileName = previewFileName;
        }
    }

    public static class ShopManager
    {
        private static readonly List<ShopItem> _items = new List<ShopItem>
        {
            new ShopItem(
                "DefaultShip",
                "Default Ship",
                "Ship Skins",
                "Original player ship.",
                0,
                "player.png"
            ),

            new ShopItem(
                "FalconShip",
                "Red Eagle Ship",
                "Ship Skins",
                "A red and white fighter ship skin.",
                40,
                "ship_red_eagle.png"
            ),

            new ShopItem(
                "CyberShip",
                "Cyber Neon Ship",
                "Ship Skins",
                "A dark cyber ship with purple neon lights.",
                75,
                "ship_cyber_neon.png"
            ),

            new ShopItem(
                "DefaultBullet",
                "Default Bullet",
                "Bullet Styles",
                "Original player bullet.",
                0,
                "Bullet_player.png"
            ),

            new ShopItem(
                "GreenBullet",
                "Green Bullet",
                "Bullet Styles",
                "Changes player bullets to green energy shots.",
                30,
                "bullet_player_green.png"
            ),

            new ShopItem(
                "PlasmaBullet",
                "Plasma Bullet",
                "Bullet Styles",
                "Changes player bullets to purple plasma shots.",
                55,
                "bullet_player_plasma.png"
            ),

            new ShopItem(
                "DefaultBackground",
                "Default Space",
                "Background Themes",
                "Original space background.",
                0,
                "backgound_defult.jpg"
            ),

            new ShopItem(
                "BlueNebulaBackground",
                "Blue Nebula",
                "Background Themes",
                "A blue space background theme.",
                50,
                "back1.jpg"
            ),

            new ShopItem(
                "PinkGalaxyBackground",
                "Pink Galaxy",
                "Background Themes",
                "A pink and purple galaxy background.",
                60,
                "back2.jpg"
            ),

            new ShopItem(
                "MarsBackground",
                "Mars Desert",
                "Background Themes",
                "A warm orange space background.",
                65,
                "back3.jpg"
            ),

            new ShopItem(
                "ExtraLifeBooster",
                "Extra Life",
                "Boosters",
                "Start the next game with 4 lives.",
                100,
                "icon_extra_life.png"
            )
        };

        private static readonly HashSet<string> _purchasedItems = new HashSet<string>
        {
            "DefaultShip",
            "DefaultBullet",
            "DefaultBackground"
        };

        public static IReadOnlyList<ShopItem> Items
        {
            get { return _items; }
        }

        public static bool IsPurchased(string itemId)
        {
            if (itemId == "DefaultShip" ||
                itemId == "DefaultBullet" ||
                itemId == "DefaultBackground")
                return true;

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

            if (item.Id == "ExtraLifeBooster")
            {
                if (!CoinManager.SpendCoins(item.Price))
                {
                    message = "Not enough coins.";
                    return false;
                }

                GameSettings.ExtraLifeBoosterEnabled = true;
                message = "Extra Life booster activated for the next game.";
                return true;
            }

            if (IsPurchased(item.Id))
            {
                EquipItem(item.Id);
                message = "Item equipped.";
                return true;
            }

            if (!CoinManager.SpendCoins(item.Price))
            {
                message = "Not enough coins.";
                return false;
            }

            _purchasedItems.Add(item.Id);
            EquipItem(item.Id);

            message = "Item purchased and equipped.";
            return true;
        }

        public static void EquipItem(string itemId)
        {
            switch (itemId)
            {
                case "DefaultShip":
                case "FalconShip":
                case "CyberShip":
                    GameSettings.EquippedShipSkin = itemId;
                    break;

                case "DefaultBullet":
                case "GreenBullet":
                case "PlasmaBullet":
                    GameSettings.EquippedBulletStyle = itemId;
                    break;

                case "DefaultBackground":
                case "BlueNebulaBackground":
                case "PinkGalaxyBackground":
                case "MarsBackground":
                    GameSettings.EquippedBackground = itemId;
                    break;

                case "ExtraLifeBooster":
                    GameSettings.ExtraLifeBoosterEnabled = true;
                    break;
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
                return GameSettings.ExtraLifeBoosterEnabled;

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