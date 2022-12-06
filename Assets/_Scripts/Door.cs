public class Door
{
    private Door()
    {
    }
    
    public bool Used { get; set; }
    public float LocalX { get; private set; }
    public float LocalY { get; private set; }
    public DoorData Data { get; private set; }
    
    public static Door FromData(DoorData data)
    {
        return new Door
        {
            LocalX = data.LocalPosition.x,
            LocalY = data.LocalPosition.y,
            Data = data,
        };
    }
}