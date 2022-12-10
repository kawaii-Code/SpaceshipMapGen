using System;

public class Door
{
    private Door()
    {
    }
    
    public bool Used { get; set; }
    public float LocalX { get; private set; }
    public float LocalY { get; private set; }
    public Direction Direction { get; private set; }
    public Room RoomFrom { get; set; }
    public Room RoomTo { get; set; }
    
    public Door Copy()
    {
        return new Door
        {
            LocalX = LocalX,
            LocalY = LocalY, 
            Direction = Direction,
        };
    }
    
    public static Door FromData(DoorData data)
    {
        return new Door
        {
            LocalX = data.LocalPosition.x,
            LocalY = data.LocalPosition.y,
            Direction = data.Direction,
        };
    }

    public static Door FromRawData(float localX, float localY)
    {
        return new Door
        {
            LocalX = localX,
            LocalY = localY,
        };
    }

    public bool IsOppositeTo(Door other)
    {
        return Direction switch
        {
            Direction.North => other.Direction == Direction.South,
            Direction.East => other.Direction == Direction.West,
            Direction.South => other.Direction == Direction.North,
            Direction.West => other.Direction == Direction.East,
        
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}