using System;
using UnityEngine;

public class DoorData : MonoBehaviour
{
    [field: SerializeField] public Direction Direction { get; private set; }
    public Vector2 LocalPosition => transform.localPosition;
    
}