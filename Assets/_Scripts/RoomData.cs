using System.Collections.Generic;
using UnityEngine;

public class RoomData : MonoBehaviour
{
    [field: SerializeField] public float Width { get; private set; }
    [field: SerializeField] public float Height { get; private set; }
    [field: SerializeField] public string Name { get; private set; }
    
    [SerializeField] private List<DoorData> _doors;

    public IReadOnlyList<DoorData> Doors => _doors;
}