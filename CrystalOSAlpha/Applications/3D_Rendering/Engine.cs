using Cosmos.System.Graphics;
using CrystalOSAlpha;
using System;
using System.Drawing;
using System.Numerics;

namespace _3DRendering
{
    public class Simple3DRendering
    {
        public static float rotationAngleZ = 0;
        public static float rotationAngleY = 0;
        public static float rotationAngleX = 0;

        public static void DrawCube()
        {
            // Camera position
            Vector3 cameraPosition = new Vector3(0, 0, 5);

            // Create a bitmap to render the cube
            Bitmap bitmap = new Bitmap(512, 512, ColorDepth.ColorDepth32);

            // Define the 3D points of the cube
            Vector3[] cubePoints = {
            new Vector3(-1, -1, -1), // Front bottom left
            new Vector3(-1, 1, -1), // Front top left
            new Vector3(1, 1, -1), // Front top right
            new Vector3(1, -1, -1), // Front bottom right
            new Vector3(-1, -1, 1), // Back bottom left
            new Vector3(-1, 1, 1), // Back top left
            new Vector3(1, 1, 1), // Back top right
            new Vector3(1, -1, 1) // Back bottom right
        };

            rotationAngleZ += 0.01f; // Adjust the rotation speed as needed
            rotationAngleY += 0.01f; // Adjust the rotation speed as needed
            rotationAngleX += 0.01f; // Adjust the rotation speed as needed

            // Rotate the entire scene (including camera position)
            RotateScene(cubePoints, cameraPosition, rotationAngleZ, rotationAngleY, rotationAngleX);

            // Render the rotated scene
            RenderScene(bitmap, cubePoints, Color.Red);

            // Rendering the wireframe
            RenderScene_Wireframe(bitmap, cubePoints, Color.Green);
        }

        //public static void DrawLineDDA(int x1, int y1, int x2, int y2)
        //{
        //    int dx = x2 - x1;
        //    int dy = y2 - y1;
        //    int steps = Math.Max(Math.Abs(dx), Math.Abs(dy));
        //    float xIncrement = (float)dx / steps;
        //    float yIncrement = (float)dy / steps;
        //    float x = x1;
        //    float y = y1;
        //    for (int i = 0; i <= steps; i++)
        //    {
        //        ImprovedVBE.DrawPixel(ImprovedVBE.cover, (int)x, (int)y, Color.Red.ToArgb());
        //        x += xIncrement;
        //        y += yIncrement;
        //    }
        //}
        public static void DrawLineBresenham(int x1, int y1, int x2, int y2)
        {
            int dx = Math.Abs(x2 - x1);
            int dy = Math.Abs(y2 - y1);
            int sx = (x1 < x2) ? 1 : -1;
            int sy = (y1 < y2) ? 1 : -1;
            int err = dx - dy;

            while (true)
            {
                ImprovedVBE.DrawPixel(ImprovedVBE.cover, x1, y1, Color.Red.ToArgb());

                if (x1 == x2 && y1 == y2) break;
                int e2 = 2 * err;
                if (e2 > -dy)
                {
                    err -= dy;
                    x1 += sx;
                }
                if (e2 < dx)
                {
                    err += dx;
                    y1 += sy;
                }
            }
        }

        public static void RenderScene_Wireframe(Bitmap bitmap, Vector3[] points, Color color)
        {
            // Define the order of connecting points to form the cube
            int[][] cubeSides = new int[][]
            {
        new int[] { 0, 1 },   // Front face
        new int[] { 1, 2 },   // Front face continued
        new int[] { 2, 3 },   // Front face continued
        new int[] { 3, 0 },   // Front face continued
        new int[] { 4, 5 },   // Back face
        new int[] { 5, 6 },   // Back face continued
        new int[] { 6, 7 },   // Back face continued
        new int[] { 7, 4 },   // Back face continued
        new int[] { 0, 4 },   // Top face
        new int[] { 1, 5 },   // Top face continued
        new int[] { 2, 6 },   // Right face
        new int[] { 3, 7 },   // Right face continued
            };

            // Loop through each side of the cube
            for (int i = 0; i < cubeSides.Length; i++)
            {
                int[] sidePoints = cubeSides[i];

                // Get the current point and the next point in the side
                int currentPointIndex = sidePoints[0];
                int nextPointIndex = sidePoints[1];

                // Convert 3D point to 2D point for both points
                Point currentPoint = ConvertTo2D(points[currentPointIndex], (int)bitmap.Width, (int)bitmap.Height);
                Point nextPoint = ConvertTo2D(points[nextPointIndex], (int)bitmap.Width, (int)bitmap.Height);

                // Connect current point to next point
                DrawLineBresenham(currentPoint.X + 200, currentPoint.Y + 200, nextPoint.X + 200, nextPoint.Y + 200);
            }
        }

        public static Point ConvertTo2D(Vector3 point3D, int width, int height)
        {
            // Apply scaling and translation to the 3D point
            int x = (int)(point3D.X * width / 4 + width / 2);
            int y = (int)(point3D.Y * height / 4 + height / 2);

            return new Point(x, y);
        }

        public static void RenderScene(Bitmap bitmap, Vector3[] points, Color color)
        {
            // Define the order of connecting points to form the cube
            int[][] cubeSides = new int[][]
            {
        new int[] { 0, 1, 2 },   // Front face
        new int[] { 2, 3, 0 },   // Front face continued
        new int[] { 4, 5, 6 },   // Back face
        new int[] { 6, 7, 4 },   // Back face continued
        new int[] { 0, 4, 1 },   // Top face
        new int[] { 1, 5, 2 },   // Top face continued
        new int[] { 2, 6, 3 },   // Right face
        new int[] { 3, 7, 0 },   // Right face continued
            };

            // Loop through each side of the cube
            for (int i = 0; i < cubeSides.Length; i++)
            {
                int[] sidePoints = cubeSides[i];

                // Get the 3 points in the side
                int point1Index = sidePoints[0];
                int point2Index = sidePoints[1];
                int point3Index = sidePoints[2];

                // Convert 3D points to 2D points for the triangle
                Point point1 = ConvertTo2D(points[point1Index], (int)bitmap.Width, (int)bitmap.Height);
                Point point2 = ConvertTo2D(points[point2Index], (int)bitmap.Width, (int)bitmap.Height);
                Point point3 = ConvertTo2D(points[point3Index], (int)bitmap.Width, (int)bitmap.Height);

                // Connect the 3 points together using a triangle
                DrawFilledTriangle(new Point(point1.X + 200, point1.Y + 200), new Point(point2.X + 200, point2.Y + 200), new Point(point3.X + 200, point3.Y + 200), ImprovedVBE.colourToNumber(255, 255, 255));
            }
        }

        public static void DrawFilledTriangle(Point p1, Point p2, Point p3, int color)
        {
            // Get the bounding rectangle of the triangle
            int minX = Math.Min(p1.X, Math.Min(p2.X, p3.X));
            int maxX = Math.Max(p1.X, Math.Max(p2.X, p3.X));
            int minY = Math.Min(p1.Y, Math.Min(p2.Y, p3.Y));
            int maxY = Math.Max(p1.Y, Math.Max(p2.Y, p3.Y));

            int start_x = 0;
            int end_x = 0;

            Bitmap line = new Bitmap(512, 2, ColorDepth.ColorDepth32);
            Array.Fill(line.RawData, color);

            // Iterate through each pixel within the bounding rectangle
            for (int y = minY; y < maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (IsPointInTriangle(x, y, p1, p2, p3))
                    {
                        start_x = x;
                        break;
                    }
                }
                for (int x = maxX; x > minX; x--)
                {
                    if (IsPointInTriangle(x, y, p1, p2, p3))
                    {
                        end_x = x;
                        break;
                    }
                }
                if (end_x > start_x)
                {
                    Array.Copy(line.RawData, 0, ImprovedVBE.cover.RawData, (y * ImprovedVBE.width) + start_x, end_x - start_x);
                }
            }
        }

        static bool IsPointInTriangle(int x, int y, Point p1, Point p2, Point p3)
        {
            int p2YMinusp3Y = p2.Y - p3.Y;
            int p3XMinusp2X = p3.X - p2.X;
            int denom = (p2YMinusp3Y) * (p1.X - p3.X) + (p3XMinusp2X) * (p1.Y - p3.Y);

            // Calculate the barycentric coordinates of the point
            float alpha = ((float)((p2YMinusp3Y) * (x - p3.X) + (p3XMinusp2X) * (y - p3.Y))) / denom;
            float beta = ((float)((p3.Y - p1.Y) * (x - p3.X) + (p1.X - p3.X) * (y - p3.Y))) / denom;
            float gamma = 1.0f - alpha - beta;

            // Check if the point is inside the triangle
            return alpha >= 0 && beta >= 0 && gamma >= 0;
        }

        public static Point3D[] RotateX(Point3D[] vertices, double angle)
        {
            Point3D[] newVertices = new Point3D[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                int y = vertices[i].Y;
                int z = vertices[i].Z;
                newVertices[i] = new Point3D(vertices[i].X, (int)(y * Math.Cos(angle) - z * Math.Sin(angle)), (int)(y * Math.Sin(angle) + z * Math.Cos(angle)));
            }
            return newVertices;
        }

        public static void RotateScene(Vector3[] points, Vector3 cameraPosition, float angleZ, float angleY, float angleX)
        {
            // Translate all points by the negative camera position
            //Translate(points, -cameraPosition.X, -cameraPosition.Y, -cameraPosition.Z);

            // Rotate around Z-axis
            RotateZ(points, angleZ);

            // Rotate around Y-axis
            RotateY(points, angleY);

            // Rotate around X-axis
            RotateX(points, angleX);

            // Translate all points back to the original position
            Translate(points, cameraPosition.X, cameraPosition.Y, cameraPosition.Z);
        }
        public static void Translate(Vector3[] points, float dx, float dy, float dz)
        {
            for (int i = 0; i < points.Length; i++)
            {
                points[i].X += dx;
                points[i].Y += dy;
                points[i].Z += dz;
            }
        }

        public static void RotateZ(Vector3[] points, float angle)
        {
            // Loop through each point and rotate it around the z-axis
            for (int i = 0; i < points.Length; i++)
            {
                float x = points[i].X * (float)Math.Cos(angle) - points[i].Y * (float)Math.Sin(angle);
                float y = points[i].X * (float)Math.Sin(angle) + points[i].Y * (float)Math.Cos(angle);
                points[i] = new Vector3(x, y, points[i].Z);
            }
        }
        public static void RotateY(Vector3[] points, float angle)
        {
            // Loop through each point and rotate it around the y-axis
            for (int i = 0; i < points.Length; i++)
            {
                float x = points[i].X * (float)Math.Cos(angle) + points[i].Z * (float)Math.Sin(angle);
                float z = -points[i].X * (float)Math.Sin(angle) + points[i].Z * (float)Math.Cos(angle);
                points[i] = new Vector3(x, points[i].Y, z);
            }
        }

        public static void RotateX(Vector3[] points, float angle)
        {
            // Loop through each point and rotate it around the x-axis
            for (int i = 0; i < points.Length; i++)
            {
                float y = points[i].Y * (float)Math.Cos(angle) - points[i].Z * (float)Math.Sin(angle);
                float z = points[i].Y * (float)Math.Sin(angle) + points[i].Z * (float)Math.Cos(angle);
                points[i] = new Vector3(points[i].X, y, z);
            }
        }

        public static void DrawFilledPolygon(Point[] points, Color color)
        {
            // Find the bounding rectangle
            int minY = 0, maxY = 0;
            foreach (var point in points)
            {
                if (point.Y < minY)
                    minY = point.Y;
                if (point.Y > maxY)
                    maxY = point.Y;
            }

            // Draw lines between points
            for (int y = minY; y <= maxY; y++)
            {
                int[] interX = new int[points.Length];
                int k = 0;
                for (int i = 0; i < points.Length; i++)
                {
                    int j = (i + 1) % points.Length;
                    if ((points[i].Y <= y && points[j].Y > y) || (points[j].Y <= y && points[i].Y > y))
                    {
                        interX[k] = (int)(points[i].X + 1.0 * (y - points[i].Y) / (points[j].Y - points[i].Y) * (points[j].X - points[i].X));
                        k++;
                    }
                }

                // Bubble sort interX array
                for (int c = 0; c < k - 1; c++)
                {
                    for (int d = 0; d < k - c - 1; d++)
                    {
                        if (interX[d] > interX[d + 1])
                        {
                            int swap = interX[d];
                            interX[d] = interX[d + 1];
                            interX[d + 1] = swap;
                        }
                    }
                }

                // Draw horizontal lines
                for (int i = 0; i < k; i += 2)
                {
                    ImprovedVBE.DrawLine(interX[i], y, interX[i + 1], y, color.ToArgb());
                }
            }
        }
    }

    public class Point3D
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Point3D(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}