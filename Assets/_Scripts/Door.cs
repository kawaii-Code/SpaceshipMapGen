using System;

[Serializable]
class Door
{
    public Direction Direction;
    public bool Used;
    
    public float X;
    public float Y;

    public Door Clone()
    {
        return new Door
        {
            Direction = Direction,
            Used = Used,
            X = X,
            Y = Y,
        };
    }
}