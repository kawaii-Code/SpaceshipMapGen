static class DirectionExtensions
{
    public static Direction Opposite(this Direction direction) => 
        (Direction) (((int)direction + 2) % 4);
}