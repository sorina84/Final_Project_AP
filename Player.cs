using System;
using System.Drawing;

public class Player : GameEntity
{
    public Image Sprite {get ; private set;}//*

    public float X {get; set ;}
    public float Y {get; set;}

    public int Width => Sprite?.Width ?? 50;
    public int Hieght => Sprite?.Hieght ?? 50;
    
    public int Health {get; private set ; }
    public int Score {get; private set ; }
    public float VelocityX {get; private set ; }
    public float VelocityY {get; private set ; }

    public Player(float x , float y) :base(x ,y ,0f)
    {
        Health = 100;
        Score =0;
        VelocityX =0f;
        VelocityY =0f;
        LoadSprite();
    }

    public void LoadSprite()
    {
        try
        {
            Sprite = Image.FromFile("Assets/player_ship.png");
        }
        catch
        {
            Sprite = null;
        }
    }

    private const float Acceleration = 0.5f;
    private const float Friction = 0.92f;
    private const float MaxSpeed = 8f;

    private const float ShootCooldown = 0.2f;
    private float _timeSinceLastShot = 0f;

    public bool CanShoot()
    {
        return _timeSinceLastShot >= ShootCooldown;
    }

    public Bullet Shoot()
    {
        _timeSinceLastShot = 0f;
        return new Bullet (X ,Y - Hieght /2f , 10f , isPlayerBullet = true);
    }

    public void MoveLeft()
    {
        VelocityX -= Acceleration;
    }
    public void MoveRight()
    {
        VelocityX += Acceleration;
    }
    public void MoveUp()
    {
        VelocityY -= Acceleration;
    }
    public void MoveDown()
    {
        VelocityY += Acceleration;
    }

   public override void Update(float deltaTime)
    {
        if(!IsActive)
            return;

        VelocityX *= Friction;
        VelocityY *= Friction;

        VelocityX = Math.Clamp(VelocityX , _MaxSpeed , MaxSpeed); //*
        VelocityY = Math.Clamp(VelocityY , _MaxSpeed , MaxSpeed);

        X += VelocityX;
        Y += VelocityY;

        float halfWidth = Width / 2f;
        float halfHiegth = Hieght / 2f;

        if(X - halfWidth < 0)
        {
            X = halfWidth ; 
            VelocityX = 0;
        }
        if(Y - halfHiegth < 0)
        {
            Y = halfHiegth;
            VelocityY = 0;
        }
        if(X + halfWidth > 800)
        {
            X = 800 - halfWidth;
            VelocityX = 0;
        }
        if(Y + halfHiegth > 600)
        {
            Y = 600 - halfHiegth;
            VelocityY = 0;
        }

        _timeSinceLastShot += deltaTime;
    }
    public override void Draw(Graphic g)
    {
        if(!IsActive)
            return;

        if(Sprite != null)
        {
            g.DrawImage(Sprite , X - Width/2f , Y - Hieght/2f , Width ,Hieght);
        }
        else{
            pointF[] ship = {new pointF(X , Y -20),
                new pointF(X -15 , Y +20 ),
                new pointF(X +15 , Y +20)
            };
            g.FillPolygon(Brushes.Cyan , Ship);
        }
    }
}