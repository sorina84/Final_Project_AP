using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GameEntity;

namespace SpaceShooter
{
    public class MainMenuForm : Form
    {
        private readonly Color _darkBlue = Color.FromArgb(15, 18, 32);
        private readonly Color _panelGray = Color.FromArgb(35, 39, 55);
        private readonly Color _neonBlue = Color.FromArgb(0, 210, 255);
        private readonly Color _neonPink = Color.FromArgb(255, 70, 180);
        private readonly Color _softGray = Color.FromArgb(185, 190, 210);

        public MainMenuForm()
        {
            Text = "Space Shooter - Main Menu";
            ClientSize = new Size(800, 600);
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            BuildMenu();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (LinearGradientBrush backgroundBrush = new LinearGradientBrush(
                ClientRectangle,
                Color.FromArgb(12, 14, 28),
                Color.FromArgb(45, 48, 70),
                45f))
            {
                e.Graphics.FillRectangle(backgroundBrush, ClientRectangle);
            }

            using (Pen bluePen = new Pen(Color.FromArgb(90, _neonBlue), 2))
            using (Pen pinkPen = new Pen(Color.FromArgb(90, _neonPink), 2))
            {
                e.Graphics.DrawEllipse(bluePen, -120, 60, 300, 300);
                e.Graphics.DrawEllipse(pinkPen, 620, 320, 260, 260);
            }

            using (SolidBrush glowBlue = new SolidBrush(Color.FromArgb(40, _neonBlue)))
            using (SolidBrush glowPink = new SolidBrush(Color.FromArgb(35, _neonPink)))
            {
                e.Graphics.FillEllipse(glowBlue, 80, 120, 90, 90);
                e.Graphics.FillEllipse(glowPink, 635, 80, 120, 120);
            }

            Rectangle panelRect = new Rectangle(220, 135, 360, 345);

            using (SolidBrush panelBrush = new SolidBrush(Color.FromArgb(150, _panelGray)))
            using (Pen borderPen = new Pen(Color.FromArgb(160, _neonBlue), 2))
            {
                e.Graphics.FillRectangle(panelBrush, panelRect);
                e.Graphics.DrawRectangle(borderPen, panelRect);
            }
        }

        private void BuildMenu()
        {
            Label title = new Label();
            title.Text = "SPACE SHOOTER";
            title.ForeColor = Color.White;
            title.Font = new Font("Arial", 34, FontStyle.Bold);
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.Width = 760;
            title.Height = 70;
            title.Left = 20;
            title.Top = 45;
            title.BackColor = Color.Transparent;
            Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "GALAXY DEFENSE";
            subtitle.ForeColor = _softGray;
            subtitle.Font = new Font("Arial", 12, FontStyle.Bold);
            subtitle.TextAlign = ContentAlignment.MiddleCenter;
            subtitle.Width = 760;
            subtitle.Height = 30;
            subtitle.Left = 20;
            subtitle.Top = 105;
            subtitle.BackColor = Color.Transparent;
            Controls.Add(subtitle);

            Button playButton = CreateMenuButton("PLAY", 165, _neonBlue);
            playButton.Click += PlayButton_Click;
            Controls.Add(playButton);

            Button shopButton = CreateMenuButton("SHOP", 225, _neonPink);
            shopButton.Click += ShopButton_Click;
            Controls.Add(shopButton);

            Button optionsButton = CreateMenuButton("OPTIONS", 285, _neonBlue);
            optionsButton.Click += OptionsButton_Click;
            Controls.Add(optionsButton);

            Button aboutButton = CreateMenuButton("ABOUT", 345, _neonPink);
            aboutButton.Click += AboutButton_Click;
            Controls.Add(aboutButton);

            Button quitButton = CreateMenuButton("QUIT", 405, Color.FromArgb(180, 190, 210));
            quitButton.Click += QuitButton_Click;
            Controls.Add(quitButton);

            Label footer = new Label();
            footer.Text = "Use PLAY to start a new mission";
            footer.ForeColor = Color.FromArgb(160, 170, 190);
            footer.Font = new Font("Arial", 9);
            footer.TextAlign = ContentAlignment.MiddleCenter;
            footer.Width = 760;
            footer.Height = 25;
            footer.Left = 20;
            footer.Top = 535;
            footer.BackColor = Color.Transparent;
            Controls.Add(footer);
        }

        private Button CreateMenuButton(string text, int top, Color borderColor)
        {
            Button button = new Button();
            button.Text = text;
            button.Width = 240;
            button.Height = 45;
            button.Left = (ClientSize.Width - button.Width) / 2;
            button.Top = top;
            button.Font = new Font("Arial", 13, FontStyle.Bold);
            button.BackColor = Color.FromArgb(28, 31, 46);
            button.ForeColor = Color.White;
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderColor = borderColor;
            button.FlatAppearance.BorderSize = 2;
            button.Cursor = Cursors.Hand;

            button.MouseEnter += (sender, e) =>
            {
                button.BackColor = Color.FromArgb(55, 58, 80);
                button.ForeColor = borderColor;
            };

            button.MouseLeave += (sender, e) =>
            {
                button.BackColor = Color.FromArgb(28, 31, 46);
                button.ForeColor = Color.White;
            };

            return button;
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            Hide();

            using (GameForm gameForm = new GameForm())
            {
                gameForm.StartPosition = FormStartPosition.CenterScreen;
                gameForm.ShowDialog();
            }

            Show();
        }

        private void ShopButton_Click(object sender, EventArgs e)
        {
            using (ShopForm shopForm = new ShopForm())
            {
                shopForm.ShowDialog(this);
            }
        }

        private void OptionsButton_Click(object sender, EventArgs e)
        {
            using (OptionsForm optionsForm = new OptionsForm())
            {
                optionsForm.ShowDialog(this);
            }
        }

        private void AboutButton_Click(object sender, EventArgs e)
        {
            using (AboutForm aboutForm = new AboutForm())
            {
                aboutForm.ShowDialog(this);
            }
        }

        private void QuitButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}