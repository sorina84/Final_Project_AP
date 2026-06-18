using System;
using System.Drawing;

<<<<<<< Updated upstream
public class PowerUp : GameEntity
{
    public enum PowerUpType
    {
        HealthPack,
        FireRateBoost,
        Shield,
        TripleShot
    }
=======
public enum PowerUpType
{
    HealthPack,
    FireRateBoost,
    Shield,
    TripleShot
}

public class PowerUp : GameEntity
{
>>>>>>> Stashed changes
    public PowerUpType Type {get; private set;}
    public float Radius {get;private set;} = 12f;

    public PowerUp(float x , float y , PowerUp type)
    :base(x ,y ,1.5f)
    {
        Type = type;
        IsActive = true;
    }

    public override void Update()
    {
        if(!IsActive)
            return;

        Y += Speed;

        if( Y > 650)
            IsActive = false;
    }

    public override void Draw(Graphics g)
    {
        if(!IsActive) 
            return;

        Brush brush;

        string T = Type.ToString();
        switch(T)
        {
            case "HealthPack":
            {
                brush = Brushes.LimeGreen;//*
                break;
            }
            case "FireRateBoost":
            {
                brush = Brushes.Gold; //*
                break;
            }
            case "Shield":
            {
                brush = Brushes.DodgerBlue; //*
                break;
            }
            case "TripleShot":
            {
                brush = Brushes.Orange;
            }
            default:
                brush = Brushes.White;
                break;
        }
        g.FillEllipse(brush , X -Radius , Y -Radius , Radius*2 , Radius*2);
    }
}