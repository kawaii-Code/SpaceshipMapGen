using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SpaceshipMapGenerator : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private RoomVisualiser _visualiser;
    
    public void Update()
    {
        if (Input.GetMouseButtonDown(0) ||
            Input.GetKeyDown(KeyCode.Space))
        {
            var timer = new Stopwatch();
            timer.Start();
            _map.Generate();
            timer.Stop();
            Debug.Log($"Generation took: {timer.Elapsed.Milliseconds}ms");
            _visualiser.Visualise(_map.GeneratedRooms);        
        }
    }
}
