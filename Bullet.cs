
using System;
using System.Drawing;

public class Bullet : GameEntity
{
    public bool IsPlayerBullet {get ;set;}

    public Bullet(float x ,float y , float speedY , bool isPlayerBullet)
    :base ( x ,y , Math.Abs(speedY))
    {
        IsPlayerBullet = isPlayerBullet;
        Speed = isPlayerBullet ? -Math.Abs(speedY) : Math.Abs(speedY);
    }

    public override void Update(float deltaTime)
    {
        if(!IsActive)
            return;

        Y += Speed*deltaTime;

        if(Y < -10 || Y > 650 )
            IsActive = false;
    }

    public override void Draw(Graphics g)
    {
        if(!IsActive)
            return;

        Brush color = isPlayerBullet ? Brushes.Yellow : Brushes.OrangeRed;
        g.FillRectangle ( color , X-2 , Y-8 , 4 ,16);
    }
}