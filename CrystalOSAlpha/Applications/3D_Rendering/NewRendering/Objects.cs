public class Cube
{
    public Point3D[] Vertices { get; }

    public Cube(float sideLength)
    {
        float halfSideLength = sideLength / 2;
        Vertices = new Point3D[]
        {
            new Point3D(-halfSideLength, -halfSideLength, -halfSideLength),
            new Point3D(halfSideLength, -halfSideLength, -halfSideLength),
            new Point3D(halfSideLength, halfSideLength, -halfSideLength),
            new Point3D(-halfSideLength, halfSideLength, -halfSideLength),
            new Point3D(-halfSideLength, -halfSideLength, halfSideLength),
            new Point3D(halfSideLength, -halfSideLength, halfSideLength),
            new Point3D(halfSideLength, halfSideLength, halfSideLength),
            new Point3D(-halfSideLength, halfSideLength, halfSideLength)
        };
    }
}