using System;
using UnityEngine;

public class DoorData : MonoBehaviour
{
    [field: SerializeField] public Direction Direction { get; private set; }
    public Vector2 LocalPosition => transform.localPosition;

    public int Id { get; private set; }
    private static int _id;

    private void Awake()
    {
        Id = _id;
        _id++;
    }

    public bool IsOppositeTo(DoorData other)
    {
        return Direction switch
        {
            Direction.North => other.Direction == Direction.South,
            Direction.East => other.Direction == Direction.West,
            Direction.South => other.Direction == Direction.North,
            Direction.West => other.Direction == Direction.East,
        
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}