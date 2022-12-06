using System.Collections.Generic;
using UnityEngine;

public class Room
{
    public List<Door> Doors;

    private Room()
    {
    }
    
    public float X { get; private set; }
    public float Y { get; private set; }
    public float Width { get; private set; }
    public float Height { get; private set; }
    public string Name { get; private set; }

    public static Room FromDataAndDoors(RoomData data, List<Door> doors)
    {
        var result = new Room
        {
            Name = data.Name,
            Width = data.Width,
            Height = data.Height,
            Doors = doors,
        };

        return result;
    }
    
    public static Room FromRawData(float width, float height, float x, float y, string name, List<Door> doors)
    {
        return new Room
        {
            X = x,
            Y = y,
            Width = width,
            Height = height,
            Name = name,
            Doors = doors,
        };
    }

    public bool CollidesWith(Room room)
    {
        var otherLeftX = room.X - room.Width / 2;
        var otherDownY = room.Y - room.Height / 2;
        var otherRightX = room.X + room.Width / 2;
        var otherUpY = room.Y + room.Height / 2;
        
        var leftX = X - Width / 2 + 0.001;
        var downY = Y - Height / 2 + 0.001;
        var rightX = X + Width / 2 - 0.001;
        var upY = Y + Height / 2 - 0.001;

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

    public void SetPosition(Vector2 position)
    {
        X = position.x;
        Y = position.y;
    }
}