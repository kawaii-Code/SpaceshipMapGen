using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

class Map : MonoBehaviour
{
    public int TotalRooms;
    
    public List<Room> PreMadeRooms; // 5 rooms 2 - (2 -> 1)

    private int roomsGenerated;
    private Random random;
    private List<Room> Rooms;
    public RoomVisualiser visualier;

    public int RoomsLeft => TotalRooms - roomsGenerated;
    private float x;
    private float y;
    
    private void Awake()
    {
        random = new Random();
        var startingRoom = PreMadeRooms[random.Next(PreMadeRooms.Count)].Clone();

        roomsGenerated++;
        Rooms = new List<Room>();

        Generate(startingRoom);
        visualier.Visualise(Rooms);
    }

    private void Generate(Room room)
    {
        // Non sense collision
        // Stupid generation??
        
        (room.X, room.Y) = (x, y);
        Rooms.Add(room);
        
        roomsGenerated += room.Doors.Count(d => !d.Used);
        
        foreach (var door in room.Doors.Where(d => !d.Used))
        {
            var fittingRooms = new List<(Room rm, Door dr)>();
            
            foreach (var rm in PreMadeRooms)
            foreach (var d in rm.Doors)
                if (d.Direction == door.Direction.Opposite())
                    if (RoomsLeft > 0)
                    {
                        if (rm.Doors.Count > 1 && rm.Doors.Count - 1 <= RoomsLeft)
                            fittingRooms.Add((rm.Clone(), d));
                    }
                    else if (rm.Doors.Count - 1 <= RoomsLeft)
                        fittingRooms.Add((rm.Clone(), d));

            if (fittingRooms.Count == 0)
            {
                Debug.Log($"Not found for door {door.Direction}, {door.Used}, at room {room.Name}, roomsLeft: {RoomsLeft}");
                continue;
            }

            var (r, to) = fittingRooms[random.Next(fittingRooms.Count)];

            x = room.X + door.X - to.X;
            y = room.Y + door.Y - to.Y;
            
            var resultRoom = r.Clone(to);
            door.Used = true;
            
            Generate(resultRoom);
        }
    }

    private bool HasCollisions(Room room)
    {
        foreach (var other in Rooms)
            if (other.Collides(room))
                return true;
        return false;
    }
}