using UnityEngine;

public class SpaceshipMapGenerator : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private RoomVisualiser _visualiser;
    
    public void Update()
    {
        if (Input.GetMouseButtonDown(0) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            _map.Generate();
            _visualiser.Visualise(_map.GeneratedRooms);        
        }
    }
}
