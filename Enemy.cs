using System;
using System.Drawing;

public abstract class Enemy : GameEntity
{
    public int Health {get ; protected set;}
    public int Damage {get ; protected set;}

    protected Enemy(float x , float y, float speed , int health , int damage)
    :base(x ,y ,speed)
    {
        Health = health;
        Damage = damage;
    }
}

public class BasicEnemy : Enemy
{
    public BasicEnemy(float x ,float y)
    :base(x ,y ,2f , health = 30 , damage = 10 )
    {
    }

    public override void Update()
    {
        if(!IsActive)
            return;
        
        Y += speed ;
        if(Y > 650)
            IsActive =false;
    }

    public override void Draw(Graphic g)
    {
        if(!IsActive)
            return;

        g.FillRectangle(Brushes.Red , x -15 , y-15 , 30 , 30);
    }
}