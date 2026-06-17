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

