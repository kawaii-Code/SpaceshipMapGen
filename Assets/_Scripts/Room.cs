using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

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

    public static Room FromData(RoomData data, List<Door> doors)
    {
        var result = new Room
        {
            Name = data.Name,
            Width = data.Width,
            Height = data.Height,
            Data = data,
            Doors = doors,
            DoorCount = data.Doors.Count,
        };
        
        return result;
    }

    public bool Collides(Room room)
    {
        var otherLeftX = room.X - room.Width / 2;
        var otherDownY = room.Y - room.Height / 2;
        var otherRightX = room.X + room.Width / 2;
        var otherUpY = room.Y + room.Height / 2;
        
        var leftX = X - Width / 2;
        var downY = Y - Height / 2;
        var rightX = X + Width / 2;
        var upY = Y + Height / 2;

        if (leftX == otherLeftX && rightX == otherRightX && upY == otherUpY && downY == otherDownY)
            return true;
        
        if (leftX < otherRightX && otherRightX < rightX)
        {
            if (downY < otherDownY && otherDownY < upY)
                return true;
            if (downY < otherUpY && otherUpY < upY)
                return true;
            if (downY >= otherDownY && upY <= otherUpY)
                return true;
        }
        else if (leftX < otherLeftX && otherLeftX < rightX)
        {
            if (downY < otherDownY && otherDownY < upY)
                return true;
            if (downY < otherUpY && otherUpY < upY)
                return true;
            if (downY >= otherDownY && upY <= otherUpY)
                return true;
        }
        else if (leftX > otherLeftX && rightX < otherRightX)
        {
            if (downY < otherDownY && otherDownY < upY)
                return true;
            if (downY < otherUpY && otherUpY < upY)
                return true;
            if (downY >= otherDownY && upY <= otherUpY)
                return true;
        }

        return false;
    }
}