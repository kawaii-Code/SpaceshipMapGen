using System.Collections.Generic;
using System.Linq;

public class Room
{
    public List<Door> Doors;

    public float X { get; set; }
    public float Y { get; set; }
    public RoomData Data { get; private set; }
    public int DoorCount { get; private set; }
    public float Width { get; private set; }
    public float Height { get; private set; }
    public string Name { get; private set; }

    public static Room FromData(RoomData data)
    {
        var result = new Room
        {
            Name = data.Name,
            Width = data.Width,
            Height = data.Height,
            Data = data,
            Doors = new List<Door>(data.Doors.Select(d => Door.FromData(d))),
            DoorCount = data.Doors.Count,
        };
        
        return result;
    }

    public bool Collides(Room room)
    {
        return false;
    }
}