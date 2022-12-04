using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

public class Map : MonoBehaviour
{
    [SerializeField] private List<RoomData> _preMadeRooms;
    [SerializeField] private int _totalRooms;

    private List<Room> _generatedRooms;
    private int _generatedCount;
    private Random _random;

    private int RoomsLeft => _totalRooms - _generatedCount;
    public IEnumerable<Room> GeneratedRooms => _generatedRooms;

    private void Awake()
    {
        _random = new Random();
        _generatedRooms = new List<Room>();
    }

    public void Generate()
    {
        Clear();

        var startingRoomData = _preMadeRooms[_random.Next(_preMadeRooms.Count)];
        var startingRoom = Room.FromData(startingRoomData);

        _generatedCount = 1;
        Generate(startingRoom);
    }

    private void Generate(Room room)
    {
        _generatedRooms.Add(room);
        _generatedCount += room.DoorCount;

        var freeDoors = room.Doors.Where(d => !d.Used).Select(d => d.Data);
        foreach (var door in freeDoors)
        {
            var fittingRooms = new List<(RoomData, DoorData)>();
            
            foreach (var rm in _preMadeRooms)
            foreach (var d in rm.Doors)
            {
                if (d.IsOppositeTo(door))
                {
                    if (RoomsLeft > 0)
                    {
                        if (rm.Doors.Count > 1 && rm.Doors.Count - 1 <= RoomsLeft)
                            fittingRooms.Add((rm, d));
                    }
                    else if (rm.Doors.Count - 1 <= RoomsLeft)
                    {
                        fittingRooms.Add((rm, d));
                    }
                }
            }

            if (fittingRooms.Count == 0)
                continue;

            var (roomData, doorData) = fittingRooms[_random.Next(fittingRooms.Count)];
            var resultRoom = Room.FromData(roomData);
            
            var doorTo = resultRoom.Doors.First(d => d.Id == doorData.Id);
            var doorFrom = room.Doors.First(d => d.Id == door.Id);

            switch (door.Direction)
            {
                case Direction.North:
                    resultRoom.X = room.X + doorFrom.LocalX - doorTo.LocalX;
                    resultRoom.Y = room.Y + room.Height / 2 + resultRoom.Height / 2;
                    break;
                case Direction.East:
                    resultRoom.X = room.X + room.Width / 2 + resultRoom.Width / 2;
                    resultRoom.Y = room.Y + doorFrom.LocalY - doorTo.LocalY;
                    break;
                case Direction.South:
                    resultRoom.X = room.X + doorFrom.LocalX - doorTo.LocalX;
                    resultRoom.Y = room.Y - room.Height / 2 - resultRoom.Height / 2;
                    break;
                case Direction.West:
                    resultRoom.X = room.X - room.Width / 2 - resultRoom.Width / 2;
                    resultRoom.Y = room.Y + doorFrom.LocalY - doorTo.LocalY;
                    break;
            }
            
            doorTo.Used = true;
            doorFrom.Used = true;
            
            Generate(resultRoom);
        }
    }

    private void Clear()
    {
        _generatedRooms.Clear();
        _generatedCount = 0;
    }
    
    private bool HasCollisions(Room room) // Do not touch, this is to be done tomorrow
    {
        foreach (var other in _generatedRooms)
            if (other.Collides(room))
                return true;
        return false;
    }
}