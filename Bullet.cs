
using System;
using System.Drawing;
public enum BulletType
{
    Player,
    Shooter,
    Heavy
}

public class Bullet : GameEntity
{
    public BulletType Type { get; private set; }
    private Image Sprite;
    public int Width { get; private set; }
    public int Height { get; private set; }


    public float VelocityX { get; set; }
    public float VelocityY { get; set; }
    public int Damage { get; set; }
    public bool IsPlayerBullet { get; set; }
    public RectangleF Bounds
    {
        get
        {
            return new RectangleF(X - Width / 2f, Y - Height / 2f, 4, 16);
        }
    }

    public Bullet(float x, float y, float velocityX, float velocityY, bool isPlayerBullet, BulletType type) : base(x, y, 0)
    {
        VelocityX = velocityX;
        VelocityY = velocityY;
        IsPlayerBullet = isPlayerBullet;
        Type = type;
        LoadSprite();
        Damage = 10;
    }

    public override void Update(float deltaTime)
    {
        if (!IsActive)
            return;

        X += VelocityX * deltaTime;
        Y += VelocityY * deltaTime;

        if (X < -20 || X > 820 ||
            Y < -20 || Y > 620)
        {
            IsActive = false;
        }
    }



    public override void Draw(Graphics g)
    {
        if (!IsActive)
            return;

        if (Sprite != null)
        {
            g.DrawImage(Sprite, X - Width / 2f, Y - Height / 2f, Width, Height);
        }
        else
        {
            Brush color = IsPlayerBullet ? Brushes.Yellow : Brushes.Red;
            g.FillRectangle(color, X, Y, 6, 16);
        }
    }




    //new
    private void LoadSprite()
    {
        try
        {
            switch (Type)
            {
                case BulletType.Player:

                    Sprite = Image.FromFile("Assets/Bullet_player.png");
                    Width = 36;
                    Height = 36;
                    break;

                case BulletType.Shooter:

                    Sprite = Image.FromFile("Assets/Bullet_shotter.png");
                    Width = 50;
                    Height = 50;
                    break;

                case BulletType.Heavy:

                    Sprite = Image.FromFile("Assets/Bullet_heavy.png");
                    Width = 40;
                    Height = 40;
                    break;
            }
        }
        catch
        {
            Sprite = null;
        }
    }
}