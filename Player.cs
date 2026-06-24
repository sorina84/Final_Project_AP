using System;
using System.Collections.Generic;
using System.Drawing;

public class Player : GameEntity
{
    public Image Sprite {get ; private set;}//*

    public int Width => Sprite?.Width ?? 50;
    public int Height => Sprite?.Height ?? 50;

    public int Lives { get; set; }
    public int FireRate { get; set; }

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
        Lives = 3;
        FireRate = 200;
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

    private float ShootCooldown = 0.2f;
    private float _timeSinceLastShot = 0f;

    public bool CanShoot()
    {
        return _timeSinceLastShot >= ShootCooldown;
    }

    public List<Bullet> Shoot()
    {
        _timeSinceLastShot = 0f;
        var bullets = new List<Bullet>();

        if(IsTripleShot)
        {
            bullets.Add(new Bullet (X , Y-25 , 10f , true)); //*
            bullets.Add(new Bullet (X-12 , Y-20 , 10f , true));
            bullets.Add(new Bullet (X +12 ,Y -20 ,10f , true));
        }
        else{
            bullets.Add(new Bullet(X , Y-25 , 10f , true));
        }

        return bullets;
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

    public bool IsShield {get;private set;}
    public bool IsTripleShot {get; private set;}
    public bool IsFireRateBoost {get; private set;}

    private float _shieldTimer = 0f;
    private float _tripleShotTimer = 0f;
    private float _fireARateBoostTimer = 0f;

    public void ActivatePowerUp(PowerUpType type)
    {
        switch(type)
        {
            case PowerUpType.HealthPack:
            {
                Health = Math.Min(Health +40 , 100);
                break;
            }
            case PowerUpType.Shield:
            {
                IsShield = true;
                _shieldTimer = 5f;
                break;
            }
            case PowerUpType.TripleShot:
            {
                IsTripleShot = true;
                _tripleShotTimer = 10f;
                break;
            }
            case PowerUpType.FireRateBoost:
            {
                IsFireRateBoost = true;
                _fireRateBoostTimer = 10f;
                ShootCooldown = 0.1f;
                break;
            }
        }
    }

    public override void Update(float deltaTime)
    {
        if(!IsActive)
            return;

        VelocityX *= Friction;
        VelocityY *= Friction;

        VelocityX = Math.Clamp(VelocityX , -MaxSpeed , MaxSpeed); //*
        VelocityY = Math.Clamp(VelocityY , -MaxSpeed , MaxSpeed);

        X += VelocityX;
        Y += VelocityY;

        float halfWidth = Width / 2f;
        float halfHeight = Height / 2f;

        if(X - halfWidth < 0)
        {
            X = halfWidth ; 
            VelocityX = 0;
        }
        if(Y - halfHeight < 0)
        {
            Y = halfHeight;
            VelocityY = 0;
        }
        if(X + halfWidth > 800)
        {
            X = 800 - halfWidth;
            VelocityX = 0;
        }
        if(Y + halfHeight > 600)
        {
            Y = 600 - halfHeight;
            VelocityY = 0;
        }

        _timeSinceLastShot += deltaTime;

        if(IsShield)
        {
            _shieldTimer -= deltaTime;
            if(_shieldTimer <= 0 ) IsShield = false;
        }
        if(IsTripleShot)
        {
            _tripleShotTimer -= deltaTime;
            if(_tripleShotTimer <= 0 ) IsTripleShot = false;
        }
        if(IsFireRateBoost)
        {
            _fireARateBoostTimer -= deltaTime;
            if(_fireARateBoostTimer <= 0 )
            {
                IsFireRateBoost = false;
                ShootCooldown = 0.2f;
            }
        }
    }
    public override void Draw(Graphics g)
    {
        if(!IsActive)
            return;

        if(IsShield)
        {
            g.DrawEllipse(Pens.Cyan , X -3 , Y -3 , 60 , 60);
        }

        if(Sprite != null)
        {
            g.DrawImage(Sprite , X - Width/2f , Y - Height/2f , Width ,Height);
        }
        else{
            PointF[] ship = {new PointF(X , Y -20),
                new PointF(X -15 , Y +20 ),
                new PointF(X +15 , Y +20)
            };
            g.FillPolygon(Brushes.Cyan , ship);
        }
    }
}