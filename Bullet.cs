
using System;
using System.Drawing;

public class Bullet : GameEntity
{
    public bool IsPlayerBullet {get ;set;}

    public Bullet(float x ,float y , float speedY , bool isPlayerBullet)
    :base ( x ,y , Math.abs(speedY))
    {
        IsPlayerBullet = isPlayerBullet;
        speed = isPlayerBullet ? -Math.abs(speedY) : Math.abs(speedY);
    }

    public override void Update()
    {
        if(!IsActive)
            return;

        Y += Speed;

        if(Y < -10 || Y > 650 )
            IsActive = false;
    }

    public override void Draw(Graphic g)
    {
        if(!IsActive)
            return;

        Brush color = isPlayerBullet ? Brushes.Yellow : Brushes.OrangeRed;
        g.FillRectangle ( color , X-2 , Y-8 , 4 ,16);
    }
}