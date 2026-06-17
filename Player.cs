using System;
using System.Drawing;

public class Player : GameEntity
{
    public int Heath {get; private set ; }
    public int Score {get; private set ; }
    public float VelocityX {get; private set ; }
    public float VelocityY {get; private set ; }

    private const float Acceleration = 0.5f;
    private const float Friction = 0.92f;
    private const float MaxSpeed = 8f;

    public Player(float x , float y) :base(x ,y ,of)
    {
        Health = 100 ;
        Score =0;
        VelocityX =0f;
        VelocityY =0f;
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

   public override void Update()
    {
        if(!IsActive)
            return;

        VelocityX *= Friction;
        VelocityY *= Friction;

        VelocityX = Math.Clamp(VelocityX , _MaxSpeed , MaxSpeed); //*
        VelocityY = Math.Clamp(VelocityY , _MaxSpeed , MaxSpeed);

        X += VelocityX;
        Y += VelocityY;

        X = Math.Clamp(X , 0 , 800);
        Y = Math.Clamp(Y , 0 , 600);
    }

    public override void Draw(Graphic g)
    {
        if(!IsActive)
            return;

        pointF[] ship = {new pointF(X , Y -20),
            new pointF(X -15 , Y +20 ),
            new pointF(X +15 , Y +20)
        };

        g.FillPolygon(Brushes.Cyan , Ship);
    }
}