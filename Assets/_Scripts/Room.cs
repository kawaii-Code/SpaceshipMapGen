using System;
using System.Collections.Generic;

[Serializable]
class Room
{
    public List<Door> Doors;

    public float X;
    public float Y;
    public float Width;
    public float Height;

    public string Name;
    
    public Room Clone()
    {
        var newDoors = new List<Door>();
        foreach (var door in Doors)
            newDoors.Add(door.Clone());

        return new Room()
        {
            Doors = newDoors,
            X = X,
            Y = Y,
            Width = Width,
            Height = Height,
            Name = Name,
        };
    }
    
    public Room Clone(Door transitioingDoor)
    {
        var newDoors = new List<Door>();
        foreach (var door in Doors)
            if (door.Direction == transitioingDoor.Direction &&
                door.X == transitioingDoor.X &&
                door.Y == transitioingDoor.Y)
            {
                var newDoor = door.Clone();
                newDoor.Used = true;
                newDoors.Add(newDoor);
            }
            else 
                newDoors.Add(door.Clone());

        return new Room()
        {
            Doors = newDoors,
            X = X,
            Y = Y,
            Width = Width,
            Height = Height,
            Name = Name,
        };
    }

    public bool Collides(Room room)
    {
        return false;
    }
}