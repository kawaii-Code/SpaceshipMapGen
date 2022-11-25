using System.Collections.Generic;
using UnityEngine;

class RoomVisualiser : MonoBehaviour
{
    public GameObject roomPrefab;
    public List<Room> rooms;
    
    public void Awake()
    {
        Visualise(rooms);
    }

    public void Visualise(List<Room> rooms)
    {
        foreach (var room in rooms)
        {
            var r = Instantiate(roomPrefab);

            r.transform.localScale = new Vector3(
                room.Width, room.Height);

            r.name = room.Name;

            r.transform.position = new Vector3(
                room.X,
                room.Y);
            
            var rend = r.GetComponent<SpriteRenderer>();
            rend.color = new Color(Random.Range(0f, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }
    }
}