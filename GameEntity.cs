using System;
using System.Drawing;

public abstract class GameEntity
{
    private float _x;
    private float _y;
    private float _speed;
    private bool _isActive;

    public float X
    {
        get => _x;
        set => _x = value;
    }

    public float Y
    {
        get => _y;
        set => _y = value;
    }

    public float Speed
    {
        get => _speed;
        protected set => _speed = value;
    }

    public bool IsActive
    {
        get => _isActive;
        set => _isActive = value;
    }
    protected GameEntity(float x, float y, float speed)
    {
        _x = x;
        _y = y;
        _speed = speed;
        _isActive = true;
    }
    public abstract void Update();
    public abstract void Draw();
}