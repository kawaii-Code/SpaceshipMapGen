using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoomVisualiser : MonoBehaviour
{
    [SerializeField] private GameObject _roomPrefab;
    [SerializeField] private GameObject _doorPrefab;

    private List<GameObject> _rooms;
    private List<GameObject> _doors;

    private void Awake()
    {
		_doors = new List<GameObject>();
        _rooms = new List<GameObject>();
    }

    public void Visualise(IEnumerable<Room> rooms)
    {
        Clear();
        
        foreach (var room in rooms)
        {
            var roomObject = Instantiate(_roomPrefab, transform);
            var roomTransform = roomObject.transform;
            
            roomObject.name = room.Name;
            roomTransform.position = new Vector3(room.X, room.Y);
            roomTransform.localScale = new Vector3(room.Width, room.Height);
            
            foreach (var door in room.Doors)
            {
                var d = Instantiate(_doorPrefab);
                var position = new Vector2(roomTransform.position.x + door.LocalX, roomTransform.position.y + door.LocalY);
                d.transform.localPosition = position;
                d.transform.localScale = Vector2.one * 0.25f;
                
                var spriteRenderer = d.GetComponent<SpriteRenderer>();
                spriteRenderer.sortingOrder = 5;
                spriteRenderer.color = door.Used ? Color.white : Color.red;
                
                _doors.Add(d);

                if (door.Used)
                    Debug.Log($"{door.RoomFrom.Name}->{door.RoomTo.Name}");
            }
            
            var rend = roomObject.GetComponent<SpriteRenderer>();
            rend.color = new Color(Random.Range(0f, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
            
            _rooms.Add(roomObject);
        }
    }

    private void Clear()
    {
        foreach (var room in _rooms) 
            Destroy(room);
        foreach (var door in _doors)
            Destroy(door);
        
        _rooms.Clear();
        _doors.Clear();
    }
}