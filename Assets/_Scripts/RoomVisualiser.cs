using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomVisualiser : MonoBehaviour // Rename and make it like an entry point
{
    [SerializeField] private GameObject _roomPrefab;

    private List<GameObject> _rooms;

    private void Awake()
    {
        _rooms = new List<GameObject>();
    }

    public void Visualise(IEnumerable<Room> rooms)
    {
        Clear();
        
        foreach (var room in rooms)
        {
            var roomObject = Instantiate(_roomPrefab, transform);

            roomObject.name = room.Name;
            roomObject.transform.position = new Vector3(room.X, room.Y);
            roomObject.transform.localScale = new Vector3(room.Width, room.Height);
            
            var rend = roomObject.GetComponent<SpriteRenderer>();
            rend.color = new Color(Random.Range(0f, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            
            _rooms.Add(roomObject);
        }
    }

    public void Clear()
    {
        foreach (var room in _rooms) 
            Destroy(room);
        _rooms.Clear();
    }
}