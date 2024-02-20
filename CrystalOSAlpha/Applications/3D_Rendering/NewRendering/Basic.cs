

using Cosmos.System.Graphics;
using CrystalOSAlpha;
using System.Drawing;

public struct Point3D
{
    public float X, Y, Z;
    public Point3D(float x, float y, float z) { X = x; Y = y; Z = z; }
}

public struct Point2D
{
    public int X, Y;
    public Point2D(int x, int y) { X = x; Y = y; }
}

public class Renderer
{
    private float focalLength;

    public Renderer(float focalLength)
    {
        this.focalLength = focalLength;
    }

    public Point2D Project(Bitmap bitmap, Point3D point3D)
    {
        int x = (int)(point3D.X * (focalLength / point3D.Z) + bitmap.Width / 2);
        int y = (int)(point3D.Y * (focalLength / point3D.Z) + bitmap.Height / 2);
        return new Point2D(x, y);
    }

    public void DrawPoint(Bitmap bitmap, Point2D point2D, Color color)
    {
        if (point2D.X >= 0 && point2D.X < bitmap.Width && point2D.Y >= 0 && point2D.Y < bitmap.Height)
        {
            ImprovedVBE.DrawPixelfortext(bitmap, point2D.X, point2D.Y, color.ToArgb());
        }
    }
}