using System;
using System.Drawing;

public class PowerUp : GameEntity
{
    public enum PowerUpType
    {
        Health,
        SpeedBoost,
        Shield
    }
    public PowerUpType Type {get; private set;}

    public PowerUp(float x , float y , PowerUp type)
    :base(x ,y ,1.5f)
    {
        Type = type;
    }

    public override void Update()
    {
        if(!IsActive)
            return;

        Y += Speed;

        if( Y > 650)
            IsActive = false;
    }

    public override void Draw(Graphic g)
    {
        if(!IsActive) 
            return;

        Brush brush;

        string T = Type.ToString();
        switch(T)
        {
            case "Health":
            {
                brush = Brushes.LimeGreen;//*
                break;
            }
            case "SpeedBoost":
            {
                brush = Brushes.Gold; //*
                break;
            }
            case "Shield":
            {
                brush = Brushes.DodgerBlue; //*
                break;
            }
            default:
                brush = Brushes.White;
                break;
        }
        g.FillEllipse(brush , X -12 , Y -12 , 24 ,24);
    }
}