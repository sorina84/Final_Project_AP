using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SpaceShooter
{
    public class AboutForm : Form
    {
        private readonly Color _neonBlue = Color.FromArgb(0, 210, 255);
        private readonly Color _neonPink = Color.FromArgb(255, 70, 180);

        public AboutForm()
        {
            Text = "About Project";
            ClientSize = new Size(760, 620);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            BuildAbout();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                ClientRectangle,
                Color.FromArgb(12, 15, 30),
                Color.FromArgb(55, 48, 78),
                55f))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }

            using (SolidBrush blueGlow = new SolidBrush(Color.FromArgb(35, _neonBlue)))
            using (SolidBrush pinkGlow = new SolidBrush(Color.FromArgb(35, _neonPink)))
            {
                e.Graphics.FillEllipse(blueGlow, -70, 60, 180, 180);
                e.Graphics.FillEllipse(pinkGlow, 610, 430, 210, 210);
            }

            using (Pen bluePen = new Pen(Color.FromArgb(160, _neonBlue), 2))
            using (Pen pinkPen = new Pen(Color.FromArgb(140, _neonPink), 2))
            {
                e.Graphics.DrawRectangle(bluePen, 24, 24, ClientSize.Width - 48, ClientSize.Height - 48);
                e.Graphics.DrawRectangle(pinkPen, 34, 34, ClientSize.Width - 68, ClientSize.Height - 68);
            }
        }

        private void BuildAbout()
        {
            Label title = new Label();
            title.Text = "PROJECT REPORT";
            title.ForeColor = Color.White;
            title.Font = new Font("Arial", 30, FontStyle.Bold);
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.BackColor = Color.Transparent;
            title.Width = 760;
            title.Height = 55;
            title.Left = 0;
            title.Top = 35;
            Controls.Add(title);

            Label subtitle = new Label();
            subtitle.Text = "SPACE SHOOTER - C# WINDOWS FORMS";
            subtitle.ForeColor = Color.FromArgb(190, 200, 220);
            subtitle.Font = new Font("Arial", 11, FontStyle.Bold);
            subtitle.TextAlign = ContentAlignment.MiddleCenter;
            subtitle.BackColor = Color.Transparent;
            subtitle.Width = 760;
            subtitle.Height = 28;
            subtitle.Left = 0;
            subtitle.Top = 88;
            Controls.Add(subtitle);

            TextBox reportBox = new TextBox();
            reportBox.Multiline = true;
            reportBox.ReadOnly = true;
            reportBox.ScrollBars = ScrollBars.Vertical;
            reportBox.BorderStyle = BorderStyle.FixedSingle;
            reportBox.BackColor = Color.FromArgb(25, 28, 45);
            reportBox.ForeColor = Color.Gainsboro;
            reportBox.Font = new Font("Consolas", 10);
            reportBox.Left = 65;
            reportBox.Top = 135;
            reportBox.Width = 630;
            reportBox.Height = 380;
            reportBox.Text = GetProjectReport();
            Controls.Add(reportBox);

            Button backButton = CreateButton("BACK", 290, 540, _neonPink);
            backButton.Click += (sender, e) => Close();
            Controls.Add(backButton);
        }

        private string GetProjectReport()
        {
            return
            @"SPACE SHOOTER - PROJECT REPORT
            ================================

            Student Information
            -------------------
            Name: Sorina Afshary
            Student Code: [403433033]

            Name: Negar Ehsanipoor
            Student Code: [403471006]

            Course: Advanced Programming
            Technology: C# Windows Forms


            Project Overview
            ----------------
            Space Shooter is a 2D arcade-style shooting game developed with C# and Windows Forms.
            The player controls a spaceship, moves inside the game window, shoots enemies, survives
            enemy waves, collects score and coins, and uses collected coins in the shop system.

            The game starts from a Main Menu instead of opening the gameplay directly. The menu
            contains Play, Shop, Options, About, and Quit sections.


            Main Features
            -------------
            1. Player Movement:
            - Movement in four directions: up, down, left, right
            - Support for WASD and Arrow Keys
            - Boundary checking to prevent the player from leaving the game window

            2. Shooting System:
            - Player shooting with Space key
            - Controlled fire rate to prevent unlimited rapid fire
            - Player bullets and enemy bullets are separated

            3. Enemy System:
            - Standard Enemy: moves straight downward
            - Scout Enemy: moves in zigzag/sinusoidal pattern
            - Shooter Enemy: moves and shoots bullets toward the player
            - Heavy Tank Enemy: high HP and shoots in multiple directions
            - Terrorist Enemy: follows the player and causes heavy damage on collision

            4. Wave System:
            - The game progresses through multiple waves
            - Difficulty increases over time
            - Enemy speed and HP scale with wave number
            - The player wins after clearing the final wave

            5. Collision System:
            - Player bullets damage enemies
            - Enemy bullets damage the player
            - Direct collision between enemy and player causes damage
            - Destroyed enemies increase score and may drop coins

            6. Coin System:
            - Coins are different from score
            - Score is used for game performance and high score
            - Coins are used for buying shop items
            - Silver Coin value: 1
            - Gold Coin value: 5

            7. Power-Up System:
            - Health Pack
            - Shield
            - Triple Shot
            - Fire Rate Boost

            8. Shop System:
            - Ship skins
            - Bullet styles
            - Background themes
            - Extra Life booster

            9. Options System:
            - Music On/Off
            - SFX On/Off
            - Controls Guide

            10. Data Persistence:
            - SQLite will be used to save:
            * Total Coins
            * High Score
            * Purchased Items
            * Equipped Items


            Object-Oriented Design
            ----------------------
            The project follows OOP principles:

            Encapsulation:
            Game logic is separated from UI forms. Classes such as Player, Enemy, Bullet,
            PowerUp, WaveManager, CollisionManager, ScoreManager, and CoinManager handle the
            core logic.

            Inheritance:
            GameEntity is used as a base class for moving and drawable game objects such as
            Player, Enemy, Bullet, Coin, and PowerUp.

            Polymorphism:
            Different enemy types override movement, drawing, and attack behavior. The game
            loop can update enemies without needing to know their exact type.


            Technical Structure
            -------------------
            MainMenuForm:
            Handles navigation between Play, Shop, Options, About, and Quit.

            GameForm:
            Runs the game loop using Windows Forms Timer and renders the game world.

            GameWorld:
            Manages player, bullets, enemies, power-ups, coins, HUD, waves, and gameplay state.

            WaveManager:
            Controls enemy spawning and wave progression.

            CollisionManager:
            Checks collisions between bullets, enemies, player, power-ups, and coins.

            ShopManager:
            Controls shop item prices and purchase logic.

            AssetLoader:
            Loads image assets safely from the Assets folder.


            Controls
            --------
            Move: Arrow Keys or WASD
            Shoot: Space
            Pause: Esc


            Conclusion
            ----------
            This project demonstrates practical use of C# Windows Forms, object-oriented design,
            game loop structure, collision detection, wave management, UI navigation, and basic
            data persistence planning through SQLite.";
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
            button.BackColor = Color.FromArgb(30, 32, 48);
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
                button.BackColor = Color.FromArgb(30, 32, 48);
                button.ForeColor = Color.White;
            };

            return button;
        }
    }
}