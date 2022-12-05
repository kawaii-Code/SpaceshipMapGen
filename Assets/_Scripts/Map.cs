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
        var startingRoom = Room.FromData(startingRoomData, startingRoomData.Doors.Select(Door.FromData).ToList());

        _generatedCount = 1;
        Generate(startingRoom);
    }

    private List<T> Shuffle<T>(List<T> list)
    {
        var lastIndex = list.Count;
        while (lastIndex > 1)
        {
            var next = _random.Next(lastIndex);
            (list[next], list[lastIndex-1]) = (list[lastIndex-1], list[next]);
            lastIndex--;
        }
        
        return list;
    }
    
    private void Generate(Room roomFrom)
    {
        _generatedRooms.Add(roomFrom); 
        _generatedCount += roomFrom.DoorCount;

        var shuffledDoors = Shuffle(roomFrom.Doors);
        var freeDoors = shuffledDoors.Where(d => !d.Used);
        foreach (var doorFrom in freeDoors)
        {
            var fittingRooms = new List<(RoomData, DoorData)>();
            
            foreach (var candidateRoom in _preMadeRooms)
            foreach (var candidateDoor in candidateRoom.Doors)
            {
                if (candidateDoor.IsOppositeTo(doorFrom.Data))
                {
                    var candidateCoordinates = GetNextCoordinates(
                        roomFrom, doorFrom, candidateRoom, candidateDoor
                    );

                    var r = Room.FromData(candidateRoom, null);
                    r.X = candidateCoordinates.x;
                    r.Y = candidateCoordinates.y;
                    if (HasCollisions(r))
                    {
                        Debug.Log("Oh no! I have collisions!");
                        continue;
                    }
                    
                    if (RoomsLeft > 0)
                    {
                        if (candidateRoom.Doors.Count > 1 && candidateRoom.Doors.Count - 1 <= RoomsLeft) // ♥Rewrite
                            fittingRooms.Add((candidateRoom, candidateDoor)); // Cool ‼
                        else if (RoomsLeft < 4)
                            fittingRooms.Add((candidateRoom, candidateDoor));
                    }
                    else if (candidateRoom.Doors.Count == 1)
                    {
                        fittingRooms.Add((candidateRoom, candidateDoor));
                    }
                }
            }

            if (fittingRooms.Count == 0)
                continue;

            var (roomToData, doorToData) = fittingRooms[_random.Next(fittingRooms.Count)];
            var doorTo = Door.FromData(doorToData);
            
            var doors = roomToData.Doors.Where(d => d != doorToData).Select(Door.FromData).ToList();
            doors.Add(doorTo);
            var resultRoom = Room.FromData(roomToData, doors);

            var resultCoordinates = GetNextCoordinates(roomFrom, doorFrom, roomToData, doorToData);
            resultRoom.X = resultCoordinates.x;
            resultRoom.Y = resultCoordinates.y;
            
            doorTo.Used = true;
            doorFrom.Used = true;
            
            Generate(resultRoom);
        }
    }

    private static (float x, float y) GetNextCoordinates(Room roomFrom, Door doorFrom, RoomData roomTo, DoorData doorTo)
    {
        (float X, float Y) result = (0, 0);
        
        switch (doorFrom.Data.Direction) // ♥Rewrite
        {
            case Direction.North:
                result.X = roomFrom.X + doorFrom.LocalX - doorTo.LocalPosition.x;
                result.Y = roomFrom.Y + roomFrom.Height / 2 + roomTo.Height / 2;
                break;
            case Direction.East:
                result.X = roomFrom.X + roomFrom.Width / 2 + roomTo.Width / 2;
                result.Y = roomFrom.Y + doorFrom.LocalY - doorTo.LocalPosition.y;
                break;
            case Direction.South:
                result.X = roomFrom.X + doorFrom.LocalX - doorTo.LocalPosition.x;
                result.Y = roomFrom.Y - roomFrom.Height / 2 - roomTo.Height / 2;
                break;
            case Direction.West:
                result.X = roomFrom.X - roomFrom.Width / 2 - roomTo.Width / 2;
                result.Y = roomFrom.Y + doorFrom.LocalY - doorTo.LocalPosition.y;
                break;
        }

        return result;
    }

    private void Clear() // ☺ ☻
    {
        _generatedRooms.Clear();
        _generatedCount = 0;
    }
    
    private bool HasCollisions(Room room) //♥Right
    {
        foreach (var other in _generatedRooms)
            if (other.Collides(room))
                return true;
        return false;
    }
}