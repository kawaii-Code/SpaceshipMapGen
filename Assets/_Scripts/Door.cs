using System;

public class Door
{
    private bool _used;
    public bool Used
    {
        get => _used;
        set
        {
            if (_used)
                throw new InvalidOperationException();
            _used = value;
        }
    }
    public float LocalX { get; private set; }
    public float LocalY { get; private set; }
    public int Id { get; private set; }
    public DoorData Data { get; private set; }
    
    public static Door FromData(DoorData data)
    {
        return new Door
        {
            LocalX = data.LocalPosition.x,
            LocalY = data.LocalPosition.y,
            Id = data.Id,
            Data = data,
        };
    }
}