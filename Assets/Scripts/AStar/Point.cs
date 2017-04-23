
/// <summary>
/// This struct are used for all nodes int he game
/// </summary>
public struct Point
{
    /// <summary>
    /// The x position of the point
    /// </summary>
    public int X { get; set; }

    /// <summary>
    /// The Y position of the struct
    /// </summary>
    public int Y { get; set; }

    /// <summary>
    /// This is a constructor to the the x and y values
    /// </summary>
    /// <param name="x">x val</param>
    /// <param name="y">y val</param>
    public Point(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }

    public static bool operator ==(Point x, Point y)
    {
        return x.X == y.X && x.Y == y.Y;
    }

    public static bool operator !=(Point x, Point y)
    {
        return x.X != y.X || x.Y != y.Y;
    }

    public static Point operator -(Point x, Point y)
    {
        return new Point(x.X - y.X, x.Y - y.Y);
    }

    public static Point operator /(Point x, int value)
    {
        return new Point(x.X / value, x.Y / value);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override bool Equals(object obj)
    {
        Point other = (Point)obj;
        return X == other.X && Y == other.Y;
    }

}
