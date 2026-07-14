using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GameEntity;

namespace SpaceShooter
{
    public partial class ShopForm : Form
    {
        private readonly Color _neonBlue = Color.FromArgb(0, 210, 255);
        private readonly Color _neonPink = Color.FromArgb(255, 70, 180);
        private readonly Color _panelColor = Color.FromArgb(28, 31, 46);

        private Label _coinsLabel;
        private TabControl _tabControl;

        public ShopForm()
        {
            Text = "Shop";
            ClientSize = new Size(760, 560);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            BuildShop();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                ClientRectangle,
                Color.FromArgb(14, 16, 30),
                Color.FromArgb(55, 48, 78),
                45f))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }

            using (SolidBrush blueGlow = new SolidBrush(Color.FromArgb(35, _neonBlue)))
            using (SolidBrush pinkGlow = new SolidBrush(Color.FromArgb(35, _neonPink)))
            {
                e.Graphics.FillEllipse(blueGlow, -80, 60, 200, 200);
                e.Graphics.FillEllipse(pinkGlow, 610, 370, 220, 220);
            }

            using (Pen bluePen = new Pen(Color.FromArgb(150, _neonBlue), 2))
            using (Pen pinkPen = new Pen(Color.FromArgb(120, _neonPink), 2))
            {
                e.Graphics.DrawRectangle(bluePen, 24, 24, ClientSize.Width - 48, ClientSize.Height - 48);
                e.Graphics.DrawRectangle(pinkPen, 34, 34, ClientSize.Width - 68, ClientSize.Height - 68);
            }
        }

        private void BuildShop()
        {
            Controls.Clear();

            Label title = new Label();
            title.Text = "SHOP";
            title.ForeColor = Color.White;
            title.Font = new Font("Arial", 30, FontStyle.Bold);
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.BackColor = Color.Transparent;
            title.Width = 760;
            title.Height = 55;
            title.Left = 0;
            title.Top = 30;
            Controls.Add(title);

            AddCoinsHeader();

            Label equippedLabel = new Label();
            equippedLabel.Text =
                "Ship: " + GameSettings.EquippedShipSkin + "   |   " +
                "Bullet: " + GameSettings.EquippedBulletStyle + "   |   " +
                "BG: " + GameSettings.EquippedBackground;

            equippedLabel.ForeColor = Color.Gainsboro;
            equippedLabel.Font = new Font("Arial", 9, FontStyle.Bold);
            equippedLabel.BackColor = Color.Transparent;
            equippedLabel.Width = 430;
            equippedLabel.Height = 30;
            equippedLabel.Left = 270;
            equippedLabel.Top = 95;
            equippedLabel.TextAlign = ContentAlignment.MiddleRight;
            Controls.Add(equippedLabel);

            _tabControl = new TabControl();
            _tabControl.Left = 55;
            _tabControl.Top = 135;
            _tabControl.Width = 650;
            _tabControl.Height = 335;
            _tabControl.Font = new Font("Arial", 10, FontStyle.Bold);

            AddCategoryTab("Ship Skins");
            AddCategoryTab("Bullet Styles");
            AddCategoryTab("Background Themes");
            AddCategoryTab("Boosters");

            Controls.Add(_tabControl);

            Button backButton = CreateButton("BACK", 290, 495, _neonPink);
            backButton.Click += (sender, e) => Close();
            Controls.Add(backButton);
        }

        private void AddCoinsHeader()
        {
            PictureBox goldCoinIcon = new PictureBox();
            goldCoinIcon.Image = AssetLoader.LoadImage("coin_gold.png");
            goldCoinIcon.SizeMode = PictureBoxSizeMode.Zoom;
            goldCoinIcon.BackColor = Color.Transparent;
            goldCoinIcon.Width = 28;
            goldCoinIcon.Height = 28;
            goldCoinIcon.Left = 55;
            goldCoinIcon.Top = 95;
            Controls.Add(goldCoinIcon);

            PictureBox silverCoinIcon = new PictureBox();
            silverCoinIcon.Image = AssetLoader.LoadImage("coin_silver.png");
            silverCoinIcon.SizeMode = PictureBoxSizeMode.Zoom;
            silverCoinIcon.BackColor = Color.Transparent;
            silverCoinIcon.Width = 26;
            silverCoinIcon.Height = 26;
            silverCoinIcon.Left = 87;
            silverCoinIcon.Top = 96;
            Controls.Add(silverCoinIcon);

            _coinsLabel = new Label();
            _coinsLabel.Text = "Coins: " + CoinManager.Coins;
            _coinsLabel.ForeColor = Color.Gold;
            _coinsLabel.Font = new Font("Arial", 13, FontStyle.Bold);
            _coinsLabel.BackColor = Color.Transparent;
            _coinsLabel.Width = 180;
            _coinsLabel.Height = 30;
            _coinsLabel.Left = 120;
            _coinsLabel.Top = 95;
            Controls.Add(_coinsLabel);
        }

        private void AddCategoryTab(string category)
        {
            TabPage tabPage = new TabPage(category);
            tabPage.BackColor = Color.FromArgb(22, 25, 38);

            FlowLayoutPanel panel = new FlowLayoutPanel();
            panel.Dock = DockStyle.Fill;
            panel.Padding = new Padding(18);
            panel.AutoScroll = true;
            panel.BackColor = Color.FromArgb(22, 25, 38);

            foreach (ShopItem item in ShopManager.Items)
            {
                if (item.Category == category)
                    panel.Controls.Add(CreateItemCard(item));
            }

            tabPage.Controls.Add(panel);
            _tabControl.TabPages.Add(tabPage);
        }

        private Panel CreateItemCard(ShopItem item)
        {
            Panel card = new Panel();
            card.Width = 185;
            card.Height = 250;
            card.Margin = new Padding(10);
            card.BackColor = Color.FromArgb(32, 35, 52);

            card.Paint += (sender, e) =>
            {
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

                Rectangle rect = new Rectangle(0, 0, card.Width - 1, card.Height - 1);
                Color accent = GetCategoryColor(item.Category);

                using (Pen borderPen = new Pen(ShopManager.IsEquipped(item) ? _neonPink : accent, 2))
                {
                    e.Graphics.DrawRectangle(borderPen, rect);
                }
            };

            PictureBox preview = CreatePreviewPictureBox(item);
            card.Controls.Add(preview);

            Label nameLabel = new Label();
            nameLabel.Text = item.Name;
            nameLabel.ForeColor = Color.White;
            nameLabel.Font = new Font("Arial", 11, FontStyle.Bold);
            nameLabel.TextAlign = ContentAlignment.MiddleCenter;
            nameLabel.BackColor = card.BackColor;
            nameLabel.Width = 165;
            nameLabel.Height = 30;
            nameLabel.Left = 10;
            nameLabel.Top = 90;
            card.Controls.Add(nameLabel);

            Label descLabel = new Label();
            descLabel.Text = item.Description;
            descLabel.ForeColor = Color.Gainsboro;
            descLabel.Font = new Font("Arial", 8);
            descLabel.TextAlign = ContentAlignment.MiddleCenter;
            descLabel.BackColor = card.BackColor;
            descLabel.Width = 155;
            descLabel.Height = 52;
            descLabel.Left = 15;
            descLabel.Top = 122;
            card.Controls.Add(descLabel);

            Label priceLabel = new Label();

            if (item.Price <= 0)
                priceLabel.Text = "Price: Free";
            else
                priceLabel.Text = "Price: " + item.Price + " Coins";

            priceLabel.ForeColor = Color.Gold;
            priceLabel.Font = new Font("Arial", 9, FontStyle.Bold);
            priceLabel.TextAlign = ContentAlignment.MiddleCenter;
            priceLabel.BackColor = card.BackColor;
            priceLabel.Width = 155;
            priceLabel.Height = 24;
            priceLabel.Left = 15;
            priceLabel.Top = 176;
            card.Controls.Add(priceLabel);

            Button actionButton = new Button();
            actionButton.Width = 125;
            actionButton.Height = 32;
            actionButton.Left = 30;
            actionButton.Top = 208;
            actionButton.Font = new Font("Arial", 9, FontStyle.Bold);
            actionButton.BackColor = _panelColor;
            actionButton.ForeColor = Color.White;
            actionButton.FlatStyle = FlatStyle.Flat;
            actionButton.FlatAppearance.BorderColor = ShopManager.IsEquipped(item) ? _neonPink : GetCategoryColor(item.Category);
            actionButton.FlatAppearance.BorderSize = 2;
            actionButton.Cursor = Cursors.Hand;
            actionButton.Text = GetButtonText(item);

            actionButton.Click += (sender, e) =>
            {
                HandleItemButton(item);
            };

            card.Controls.Add(actionButton);

            return card;
        }

        private PictureBox CreatePreviewPictureBox(ShopItem item)
        {
            PictureBox pictureBox = new PictureBox();

            Rectangle rect = GetPreviewRectangle(item);

            pictureBox.Left = rect.Left;
            pictureBox.Top = rect.Top;
            pictureBox.Width = rect.Width;
            pictureBox.Height = rect.Height;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.BackColor = Color.FromArgb(18, 20, 32);

            Image image = AssetLoader.LoadImage(item.PreviewFileName);

            if (image != null)
            {
                pictureBox.Image = image;
            }
            else
            {
                if (item.Category == "Boosters")
                    pictureBox.Image = CreateBoosterFallbackBitmap(pictureBox.Width, pictureBox.Height);
                else
                    pictureBox.Image = CreateMissingImageBitmap(pictureBox.Width, pictureBox.Height, item.PreviewFileName);
            }

            return pictureBox;
        }

        private Bitmap CreateMissingImageBitmap(int width, int height, string fileName)
        {
            Bitmap bitmap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bitmap))
            using (SolidBrush backgroundBrush = new SolidBrush(Color.FromArgb(50, 20, 20)))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            using (Pen borderPen = new Pen(Color.Red, 2))
            using (Font font = new Font("Arial", 7, FontStyle.Bold))
            {
                g.Clear(Color.FromArgb(18, 20, 32));
                g.FillRectangle(backgroundBrush, 0, 0, width, height);
                g.DrawRectangle(borderPen, 1, 1, width - 3, height - 3);

                string text = "MISSING\n" + fileName;

                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    g.DrawString(text, font, textBrush, new RectangleF(2, 2, width - 4, height - 4), format);
                }
            }

            return bitmap;
        }

        private Bitmap CreateBoosterFallbackBitmap(int width, int height)
        {
            Bitmap bitmap = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bitmap))
            using (SolidBrush goldBrush = new SolidBrush(Color.Gold))
            using (SolidBrush darkBrush = new SolidBrush(Color.FromArgb(18, 20, 32)))
            using (Pen whitePen = new Pen(Color.White, 2))
            using (Font font = new Font("Arial", 28, FontStyle.Bold))
            {
                g.Clear(Color.FromArgb(18, 20, 32));
                g.FillEllipse(goldBrush, 10, 5, width - 20, height - 10);
                g.DrawEllipse(whitePen, 10, 5, width - 20, height - 10);

                using (StringFormat format = new StringFormat())
                {
                    format.Alignment = StringAlignment.Center;
                    format.LineAlignment = StringAlignment.Center;

                    g.DrawString("+", font, darkBrush, new RectangleF(0, 0, width, height), format);
                }
            }

            return bitmap;
        }

        private Rectangle GetPreviewRectangle(ShopItem item)
        {
            if (item.Category == "Ship Skins")
                return new Rectangle(35, 14, 115, 72);

            if (item.Category == "Bullet Styles")
                return new Rectangle(62, 12, 60, 76);

            if (item.Category == "Background Themes")
                return new Rectangle(28, 16, 128, 68);

            if (item.Category == "Boosters")
                return new Rectangle(55, 18, 75, 68);

            return new Rectangle(45, 18, 95, 68);
        }

        private string GetButtonText(ShopItem item)
        {
            if (ShopManager.IsEquipped(item))
            {
                if (item.Category == "Boosters")
                    return "ACTIVE";

                return "EQUIPPED";
            }

            if (ShopManager.IsPurchased(item.Id))
                return "EQUIP";

            return "BUY";
        }

        private void HandleItemButton(ShopItem item)
        {
            if (ShopManager.IsPurchased(item.Id) && item.Category != "Boosters")
            {
                ShopManager.EquipItem(item.Id);
                BuildShop();
                SaveData();
                return;
            }

            string message;
            bool success = ShopManager.BuyItem(item.Id, out message);

            MessageBox.Show(
                message,
                success ? "Shop" : "Purchase Failed",
                MessageBoxButtons.OK,
                success ? MessageBoxIcon.Information : MessageBoxIcon.Warning
            );

            BuildShop();
        }

        private Color GetCategoryColor(string category)
        {
            if (category == "Ship Skins")
                return _neonBlue;

            if (category == "Bullet Styles")
                return Color.FromArgb(80, 255, 150);

            if (category == "Background Themes")
                return _neonPink;

            if (category == "Boosters")
                return Color.Gold;

            return Color.White;
        }

        private Button CreateButton(string text, int left, int top, Color borderColor)
        {
            Button button = new Button();
            button.Text = text;
            button.Left = left;
            button.Top = top;
            button.Width = 180;
            button.Height = 42;
            button.Font = new Font("Arial", 12, FontStyle.Bold);
            button.BackColor = _panelColor;
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = borderColor;
            button.FlatAppearance.BorderSize = 2;
            button.Cursor = Cursors.Hand;

            return button;
        }
        private void SaveData()
        {
            DataManager manager = new DataManager();
            PlayerData data = new PlayerData();
            data.TotalCoins = CoinManager.Coins;
            data.EquippedShipSkin = GameSettings.EquippedShipSkin;
            data.EquippedBulletStyle = GameSettings.EquippedBulletStyle;

            data.EquippedBackground = GameSettings.EquippedBackground;


            manager.SavePlayerData(data);
        }
    }
}