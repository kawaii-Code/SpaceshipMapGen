using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public class Map : MonoBehaviour
{
    [FormerlySerializedAs("_preMadeRooms")] [SerializeField] private List<RoomData> _preMadeRoomsData;
    [SerializeField] private int _totalRooms;

    private List<Room> _preMadeRooms;
    private List<Room> _generatedRooms;
    private int _generatedCount;

    private int RoomsLeft => _totalRooms - _generatedCount;
    public IEnumerable<Room> GeneratedRooms => _generatedRooms;

    public void Generate()
    {
        _preMadeRooms = new List<Room>();
        foreach (var data in _preMadeRoomsData) 
            _preMadeRooms.Add(Room.FromData(data));
        
        do
        {
            Refresh();
            var startingRoom = PickStartingRoom();
            _generatedCount = 1;
            Generate(startingRoom);
        } 
        while (_generatedCount < _totalRooms);
    }

    private Room PickStartingRoom()
    {
        var startingRoomData = _preMadeRoomsData.RandomElement();
        var startingDoors = startingRoomData.Doors.Select(Door.FromData).ToList();
        var startingRoom = Room.FromDataAndDoors(startingRoomData, startingDoors);
        
        return startingRoom;
    }

    private void Generate(Room roomFrom)
    {
        _generatedRooms.Add(roomFrom); 
        _generatedCount += roomFrom.Doors.Count(d => !d.Used);

        var shuffledDoors = roomFrom.Doors.Shuffle();
        var freeDoors = shuffledDoors.Where(d => !d.Used);

        CreateBarrierRooms(roomFrom, freeDoors);

        foreach (var doorFrom in freeDoors)
        {
            RemoveBarrierRoom(doorFrom);

            var fittingRooms = GetFittingRooms(roomFrom, doorFrom);
            if (fittingRooms.Count == 0)
                continue;

            var (roomTo, doorTo) = fittingRooms.RandomElement();
            var position = NextRoomPosition(roomFrom, doorFrom, roomTo, doorTo);
            roomTo.SetPosition(position);
            
            doorTo.Used = true;
            doorFrom.Used = true;
            doorFrom.RoomFrom = roomFrom;
            doorFrom.RoomTo = roomTo;
            
            Generate(roomTo);
        }
    }

    private void CreateBarrierRooms(Room roomFrom, IEnumerable<Door> doors)
    {
        foreach (var doorFrom in doors)
        {
            var barrierDoor = Door.FromRawData(roomFrom.X + doorFrom.LocalX, roomFrom.Y + doorFrom.LocalY);
            var barrierRoomPosition = NextRoomPosition(roomFrom, doorFrom, roomFrom, barrierDoor);
            var barrierRoom = Room.FromRawData(
                roomFrom.Width, roomFrom.Height,
                roomFrom.X, roomFrom.Y,
                "Barrier", new List<Door>() { doorFrom });

            barrierRoom.SetPosition(barrierRoomPosition);

            _generatedRooms.Add(barrierRoom);
        }
    }

    private void RemoveBarrierRoom(Door doorFrom)
    {
        var barrierRoom = _generatedRooms
            .Where(room => room.Name == "Barrier")
            .First(room => room.Doors.Contains(doorFrom));
        _generatedRooms.Remove(barrierRoom);
    }

    private List<(Room fittingRoom, Door fittingDoor)> GetFittingRooms(Room roomFrom, Door doorFrom)
    {
        var result = new List<(Room, Door)>();
        
        foreach (var candidateRoom in _preMadeRooms)
        foreach (var candidateDoor in candidateRoom.Doors)
        {
            if (!candidateDoor.IsOppositeTo(doorFrom)) 
                continue;
            
            var candidateCoordinates = NextRoomPosition(roomFrom, doorFrom, candidateRoom, candidateDoor);
            candidateRoom.SetPosition(candidateCoordinates);
            if (HasCollisions(candidateRoom))
                continue;

            if (RoomsLeft > 0)
            {
                if (candidateRoom.Doors.Count > 1 && candidateRoom.Doors.Count - 1 <= RoomsLeft)
                {
                    result.Add(CopyRoomWithDoor(candidateRoom, candidateDoor));
                }
                else if (RoomsLeft < 4)
                {
                    result.Add(CopyRoomWithDoor(candidateRoom, candidateDoor));
                }
            }
            else if (candidateRoom.Doors.Count == 1)
            {
                result.Add(CopyRoomWithDoor(candidateRoom, candidateDoor));
            }
        }

        return result;
    }

    private static Vector2 NextRoomPosition(Room roomFrom, Door doorFrom, Room roomTo, Door doorTo)
    {
        var result = Vector2.zero;
        
        switch (doorFrom.Direction)
        {
            case Direction.North:
                result.x = roomFrom.X + doorFrom.LocalX - doorTo.LocalX;
                result.y = roomFrom.Y + roomFrom.Height / 2 + roomTo.Height / 2;
                break;
            case Direction.East:
                result.x = roomFrom.X + roomFrom.Width / 2 + roomTo.Width / 2;
                result.y = roomFrom.Y + doorFrom.LocalY - doorTo.LocalY;
                break;
            case Direction.South:
                result.x = roomFrom.X + doorFrom.LocalX - doorTo.LocalX;
                result.y = roomFrom.Y - roomFrom.Height / 2 - roomTo.Height / 2;
                break;
            case Direction.West:
                result.x = roomFrom.X - roomFrom.Width / 2 - roomTo.Width / 2;
                result.y = roomFrom.Y + doorFrom.LocalY - doorTo.LocalY;
                break;
        }

        return result;
    }

    private static (Room room, Door candidateDoorCopy) CopyRoomWithDoor(Room candidateRoom, Door candidateDoor)
    {
        var doors = candidateRoom.Doors
            .Where(door => door != candidateDoor)
            .Select(door => door.Copy())
            .ToList();
        var candidateDoorCopy = candidateDoor.Copy();
        doors.Add(candidateDoorCopy);
        var room = Room.CopyWithDoors(candidateRoom, doors);
        
        return (room, candidateDoorCopy);
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