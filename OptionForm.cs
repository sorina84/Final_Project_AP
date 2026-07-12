using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using GameEntity;

namespace SpaceShooter
{
    public partial class OptionsForm : Form
    {
        private readonly Color _neonBlue = Color.FromArgb(0, 210, 255);
        private readonly Color _neonPink = Color.FromArgb(255, 70, 180);

        public OptionsForm()
        {
            Text = "Options";
            ClientSize = new Size(560, 440);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            DoubleBuffered = true;

            BuildOptions();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            using (LinearGradientBrush brush = new LinearGradientBrush(
                ClientRectangle,
                Color.FromArgb(18, 20, 35),
                Color.FromArgb(48, 50, 72),
                45f))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }

            using (Pen bluePen = new Pen(Color.FromArgb(120, _neonBlue), 2))
            using (Pen pinkPen = new Pen(Color.FromArgb(120, _neonPink), 2))
            {
                e.Graphics.DrawRectangle(bluePen, 25, 25, ClientSize.Width - 50, ClientSize.Height - 50);
                e.Graphics.DrawEllipse(pinkPen, 390, 45, 110, 110);
            }
        }

        private void BuildOptions()
        {
            Label title = CreateLabel("OPTIONS", 0, 35, 560, 50, 28, Color.White);
            Controls.Add(title);

            CheckBox musicCheckBox = CreateCheckBox("Music Enabled", 90, 115);
            musicCheckBox.Checked = AudioManager.MusicEnabled;
            musicCheckBox.CheckedChanged += (sender, e) =>
            {
                AudioManager.MusicEnabled = musicCheckBox.Checked;

                if (AudioManager.MusicEnabled)
                {
                    AudioManager.PlayMenuMusic();
                }
                else
                {
                    AudioManager.StopMusic();
                }
            };
            Controls.Add(musicCheckBox);

            CheckBox sfxCheckBox = CreateCheckBox("Sound Effects Enabled", 90, 160);
            sfxCheckBox.Checked = AudioManager.SoundEnabled;
            sfxCheckBox.CheckedChanged += (sender, e) =>
            {
                AudioManager.SoundEnabled = sfxCheckBox.Checked;
            };
            Controls.Add(sfxCheckBox);

            Label guide = new Label();
            guide.Text =
                "Controls Guide:\n\n" +
                "Move: Arrow Keys or WASD\n" +
                "Shoot: Space\n" +
                "Pause: Esc";
            guide.ForeColor = Color.White;
            guide.Font = new Font("Arial", 12, FontStyle.Bold);
            guide.BackColor = Color.FromArgb(60, 30, 34, 50);
            guide.TextAlign = ContentAlignment.MiddleLeft;
            guide.Width = 380;
            guide.Height = 125;
            guide.Left = 90;
            guide.Top = 220;
            guide.Padding = new Padding(18);
            Controls.Add(guide);

            Button backButton = CreateButton("BACK", 190, 370, _neonPink);
            backButton.Click += (sender, e) => Close();
            Controls.Add(backButton);
        }

        private Label CreateLabel(string text, int left, int top, int width, int height, int fontSize, Color color)
        {
            Label label = new Label();
            label.Text = text;
            label.Left = left;
            label.Top = top;
            label.Width = width;
            label.Height = height;
            label.ForeColor = color;
            label.BackColor = Color.Transparent;
            label.Font = new Font("Arial", fontSize, FontStyle.Bold);
            label.TextAlign = ContentAlignment.MiddleCenter;
            return label;
        }

        private CheckBox CreateCheckBox(string text, int left, int top)
        {
            CheckBox checkBox = new CheckBox();
            checkBox.Text = text;
            checkBox.Left = left;
            checkBox.Top = top;
            checkBox.Width = 300;
            checkBox.Height = 35;
            checkBox.ForeColor = Color.White;
            checkBox.BackColor = Color.Transparent;
            checkBox.Font = new Font("Arial", 12, FontStyle.Bold);
            return checkBox;
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
            return button;
        }
    }
}