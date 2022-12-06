using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] private List<RoomData> _preMadeRooms;
    [SerializeField] private int _totalRooms;

    private List<Room> _generatedRooms;
    private int _generatedCount;

    private int RoomsLeft => _totalRooms - _generatedCount;
    public IEnumerable<Room> GeneratedRooms => _generatedRooms;

    public void Generate()
    {
        do
        {
            Refresh();

            var startingRoomData = _preMadeRooms.RandomElement();
            var startingDoors = startingRoomData.Doors.Select(Door.FromData).ToList();
            var startingRoom = Room.FromDataAndDoors(startingRoomData, startingDoors);
            _generatedCount = 1;

            Generate(startingRoom);
        } while (_generatedCount < _totalRooms);
    }
    
    private void Generate(Room roomFrom)
    {
        _generatedRooms.Add(roomFrom); 
        _generatedCount += roomFrom.Doors.Count(d => !d.Used);

        var shuffledDoors = roomFrom.Doors.Shuffle();
        var freeDoors = shuffledDoors.Where(d => !d.Used);
        
        foreach (var doorFrom in freeDoors)
        {
            var fittingRooms = GetFittingRooms(roomFrom, doorFrom);
            if (fittingRooms.Count == 0)
                continue;

            var (roomToData, doorToData) = fittingRooms.RandomElement();
            
            var doorTo = Door.FromData(doorToData);
            var doors = roomToData.Doors.Where(d => d != doorToData).Select(Door.FromData).ToList();
            doors.Add(doorTo);
            
            var roomTo = Room.FromDataAndDoors(roomToData, doors);

            var position = NextRoomPosition(roomFrom, doorFrom, roomToData, doorToData);
            roomTo.SetPosition(position);
            
            doorTo.Used = true;
            doorFrom.Used = true;
            
            Generate(roomTo);
        }
    }

    private List<(RoomData, DoorData)> GetFittingRooms(Room roomFrom, Door doorFrom)
    {
        var result = new List<(RoomData, DoorData)>();
        
        foreach (var candidateRoom in _preMadeRooms)
        foreach (var candidateDoor in candidateRoom.Doors)
        {
            if (candidateDoor.IsOppositeTo(doorFrom.Data))
            {
                var candidateCoordinates = NextRoomPosition(
                    roomFrom, doorFrom, candidateRoom, candidateDoor
                );
                var r = Room.FromDataAndDoors(candidateRoom, null);
                r.SetPosition(candidateCoordinates);
                if (HasCollisions(r))
                    continue;

                if (RoomsLeft > 0)
                {
                    if (candidateRoom.Doors.Count > 1 && candidateRoom.Doors.Count - 1 <= RoomsLeft)
                        result.Add((candidateRoom, candidateDoor));
                    else if (RoomsLeft < 4)
                        result.Add((candidateRoom, candidateDoor));
                }
                else if (candidateRoom.Doors.Count == 1)
                {
                    result.Add((candidateRoom, candidateDoor));
                }
            }
        }

        return result;
    }

    private static Vector2 NextRoomPosition(Room roomFrom, Door doorFrom, RoomData roomTo, DoorData doorTo)
    {
        var result = Vector2.zero;
        
        switch (doorFrom.Data.Direction)
        {
            case Direction.North:
                result.x = roomFrom.X + doorFrom.LocalX - doorTo.LocalPosition.x;
                result.y= roomFrom.Y + roomFrom.Height / 2 + roomTo.Height / 2;
                break;
            case Direction.East:
                result.x = roomFrom.X + roomFrom.Width / 2 + roomTo.Width / 2;
                result.y = roomFrom.Y + doorFrom.LocalY - doorTo.LocalPosition.y;
                break;
            case Direction.South:
                result.x = roomFrom.X + doorFrom.LocalX - doorTo.LocalPosition.x;
                result.y = roomFrom.Y - roomFrom.Height / 2 - roomTo.Height / 2;
                break;
            case Direction.West:
                result.x = roomFrom.X - roomFrom.Width / 2 - roomTo.Width / 2;
                result.y = roomFrom.Y + doorFrom.LocalY - doorTo.LocalPosition.y;
                break;
        }

        return result;
    }

    private bool HasCollisions(Room room)
    {
        foreach (var other in _generatedRooms)
            if (other.CollidesWith(room))
                return true;
        return false;
    }

    private void Refresh()
    {
        _generatedRooms = new List<Room>();
        _generatedCount = 0;
    }
}