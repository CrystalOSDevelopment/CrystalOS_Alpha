using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOS_Alpha;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.TaskBar;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;
using Image = Cosmos.System.Graphics.Image;

namespace CrystalOSAlpha
{
    public class ImprovedVBE
    {
        #region Wallpaper & Resolution
        //Change this if you want to change wallpapers!!! Also, make sure that it matches your current selected resolution!!! (Most cases it's 1920x1080x32)
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Engine.Wallp1.bmp")] public static byte[] VBECanvas;

        //These are black images matching the size exacly the resolution
        /// <summary>
        /// The base canvas
        /// </summary>
        public static Bitmap cover = new Bitmap(VBECanvas);
        /// <summary>
        /// To reset the base canvas
        /// </summary>
        public static Bitmap data = new Bitmap(VBECanvas);
        /// <summary>
        /// To reset the base canvas
        /// </summary>
        public static Bitmap Temp = new Bitmap(VBECanvas);

        public static Bitmap CursorBack = new Bitmap(18, 30, ColorDepth.ColorDepth32);

        public static int width = (int)cover.Width;
        public static int height = (int)cover.Height;
        public static bool Res = true;
        #endregion Wallpaper & Resolution

        #region Render to the screen and Clear
        public static int Counter = 0;
        public static bool isMoving = false;
        public static int Clock = -99;
        public static bool RequestRedraw = true;

        /// <summary>
        /// To display the rendered frame.
        /// </summary>
        /// <param name="c"></param>
        public static void Display(VBECanvas c, bool EnableCursor = true)
        {
            if(isMoving == false)
            {
                if (Counter == 7)
                {
                    c.DrawImage(cover, 0, 0);
                    if (EnableCursor)
                    {
                        c.DrawImageAlpha(CrystalOS_Alpha.Kernel.C, (int)MouseManager.X, (int)MouseManager.Y);
                    }
                    c.Display();
                    if (Res == true)
                    {
                        try
                        {
                            data = ScaleImageStock(Temp, (uint)width, (uint)height);
                            cover = ScaleImageStock(Temp, (uint)width, (uint)height);

                            MouseManager.ScreenWidth = (uint)width;
                            MouseManager.ScreenHeight = (uint)height;

                            TaskManager.Top = height;
                            TaskManager.Left = width / 2 + 60;
                        }
                        catch
                        {

                        }

                        Res = false;
                    }
                    Counter = 0;
                }
                else
                {
                    Counter++;
                }
                Clear();
            }
            else
            {
                //This is an artificial delay part which only gets activated when a window is being dragged to avoid tearing
                if(Counter == 7)
                {
                    if(Res == true)
                    {
                        try
                        {
                            data = ScaleImageStock(Temp, (uint)width, (uint)height);
                            cover = ScaleImageStock(Temp, (uint)width, (uint)height);

                            MouseManager.ScreenWidth = (uint)width;
                            MouseManager.ScreenHeight = (uint)height;

                            TaskManager.Top = height;
                            TaskManager.Left = width / 2 + 60;
                        }
                        catch
                        {

                        }

                        Res = false;
                    }
                    c.DrawImage(cover, 0, 0);
                    c.DrawImageAlpha(CrystalOS_Alpha.Kernel.C, (int)MouseManager.X, (int)MouseManager.Y);
                    c.Display();
                    Counter = 0;
                }
                else
                {
                    Counter++;
                }
                Clear();
            }
        }

        /// <summary>
        /// Clear the frame by copying a fresh version from the backbuffer onto the main canvas.
        /// </summary>
        /// <param name="col"></param>
        public static void Clear(bool RequestClear = false)
        {
            // Start of original code
            //data.RawData.CopyTo(cover.RawData, 0);
            // End of original code

            if (MouseManager.MouseState == MouseState.Left || MouseManager.MouseState == MouseState.Right || RequestClear)//Separate the mouse update from window movement for the love of God!
            {
                data.RawData.CopyTo(cover.RawData, 0);
                RequestRedraw = true;
                SideNav.RequestDrawLocal = true;    //If you don't have a SideNav, you can remove this line
            }
        }
        #endregion Render to the screen and Clear

        #region Graphics
        /// <summary>
        /// Draws a pixel to a specific location
        /// </summary>
        /// <param name="Canvas">The image you want to draw the pixel onto</param>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <param name="color">Color of the pixel in integer</param>
        public static void DrawPixel(Bitmap Canvas, int x, int y, int color)
        {
            switch(x > 0 && x < Canvas.Width && y >= 0 && y < Canvas.Height)
            {
                case true:
                    Canvas.RawData[y * Canvas.Width + x] = color;
                    break;
            }
        }

        public static int GetPixel(Bitmap Canvas, int x, int y)
        {
            if (x >= 0 && x < Canvas.Width && y >= 0 && y < Canvas.Height)
            {
                return Canvas.RawData[y * Canvas.Width + x];
            }
            else
            {
                throw new Exception("Out of bounds");
            }
        }

        /// <summary>
        /// Draws a line to the screen
        /// </summary>
        /// <param name="Canvas">The image you want to draw the line onto</param>
        /// <param name="x1">Starting X coordinate</param>
        /// <param name="y1">Starting Y coordinate</param>
        /// <param name="x2">Ending X coordinate</param>
        /// <param name="y2">Ending Y coordinate</param>
        /// <param name="color">Color of the line in integer</param>
        public static void DrawLine(Bitmap Canvas, float x1, float y1, float x2, float y2, int color)
        {
            switch(y1 == y2)
            {
                case false:
                    float dx = x2 - x1;
                    float dy = y2 - y1;

                    float length = (float)Math.Sqrt(dx * dx + dy * dy);

                    float angle = (float)Math.Atan2(dy, dx);

                    for (float i = 0; i < length; i++)
                    {
                        DrawPixel(Canvas, (int)(x1 + Math.Cos(angle) * i), (int)(y1 + Math.Sin(angle) * i), color);
                    }
                    break;
                case true:
                    DrawFilledRectangle(Canvas, color, (int)x1, (int)y1, (int)(x2 - x1), 1);
                    break;
            }
        }

        /// <summary>
        /// Draws an unfilled triangle on the screen using a list of points
        /// </summary>
        /// <param name="Canvas">The image you want to draw the triangle onto</param>
        /// <param name="Points">List containing the three vertices of the triangle</param>
        /// <param name="color">Color of the triangle's lines in integer</param>
        public static void DrawTriangle(Bitmap Canvas, List<Point> Points, int color)
        {
            // Ensure the list contains exactly 3 points to form a triangle
            if (Points.Count == 3)
            {
                // Draw the three sides of the triangle
                DrawLine(Canvas, Points[0].X, Points[0].Y, Points[1].X, Points[1].Y, color); // Side 1: Points[0] to Points[1]
                DrawLine(Canvas, Points[1].X, Points[1].Y, Points[2].X, Points[2].Y, color); // Side 2: Points[1] to Points[2]
                DrawLine(Canvas, Points[2].X, Points[2].Y, Points[0].X, Points[0].Y, color); // Side 3: Points[2] to Points[0]
            }

        }

        public static void DrawFilledTriangle(Bitmap Canvas, List<Point> Points, int color)
        {
            if (Points.Count != 3) return; // Ensure it's a triangle

            // Manually sort the points by their Y-coordinate
            Point p1 = Points[0];
            Point p2 = Points[1];
            Point p3 = Points[2];

            // Sort points in place by Y-coordinate (ascending order)
            if (p1.Y > p2.Y) { Swap(ref p1, ref p2); }
            if (p1.Y > p3.Y) { Swap(ref p1, ref p3); }
            if (p2.Y > p3.Y) { Swap(ref p2, ref p3); }

            // Helper function to swap two points
            void Swap(ref Point a, ref Point b)
            {
                Point temp = a;
                a = b;
                b = temp;
            }

            // Function to interpolate between two points
            void FillLine(Point a, Point b)
            {
                int startX = Math.Min(a.X, b.X);
                int endX = Math.Max(a.X, b.X);

                for (int x = startX; x <= endX; x++)
                {
                    DrawPixel(Canvas, x, a.Y, color); // Draw a horizontal line
                }
            }

            // Helper function to draw horizontal lines between points vertically
            void FillFlatBottom(Point top, Point left, Point right)
            {
                float slopeLeft = (float)(left.X - top.X) / (left.Y - top.Y);
                float slopeRight = (float)(right.X - top.X) / (right.Y - top.Y);

                for (int y = top.Y; y <= left.Y; y++)
                {
                    int startX = (int)(top.X + (y - top.Y) * slopeLeft);
                    int endX = (int)(top.X + (y - top.Y) * slopeRight);
                    FillLine(new Point(startX, y), new Point(endX, y));
                }
            }

            void FillFlatTop(Point left, Point right, Point bottom)
            {
                float slopeLeft = (float)(bottom.X - left.X) / (bottom.Y - left.Y);
                float slopeRight = (float)(bottom.X - right.X) / (bottom.Y - right.Y);

                for (int y = left.Y; y <= bottom.Y; y++)
                {
                    int startX = (int)(left.X + (y - left.Y) * slopeLeft);
                    int endX = (int)(right.X + (y - right.Y) * slopeRight);
                    FillLine(new Point(startX, y), new Point(endX, y));
                }
            }

            // Split triangle into two flat triangles (flat-bottom and flat-top)
            if (p2.Y == p3.Y) // Flat-bottom triangle
            {
                FillFlatBottom(p1, p2, p3);
            }
            else if (p1.Y == p2.Y) // Flat-top triangle
            {
                FillFlatTop(p1, p2, p3);
            }
            else
            {
                // General case - split into two triangles
                Point splitPoint = new Point(
                    p1.X + (int)((float)(p2.Y - p1.Y) / (p3.Y - p1.Y) * (p3.X - p1.X)),
                    p2.Y
                );

                FillFlatBottom(p1, p2, splitPoint);
                FillFlatTop(p2, splitPoint, p3);
            }
        }


        /// <summary>
        /// Draw a non-filled rectangle
        /// </summary>
        /// <param name="Canvas">The image you want to draw the non-filled rectangle onto</param>
        /// <param name="x">X coordiante (on canvas)</param>
        /// <param name="y">Y coordiante (on canvas)</param>
        /// <param name="width">Width of the non-filled rectangle</param>
        /// <param name="height">Height of the non-filled rectangle</param>
        /// <param name="Color">Color of the non-filled rectangle in integer</param>
        public static void DrawRectangle(Bitmap Canvas, int x, int y, int width, int height, int Color)
        {
            DrawLine(Canvas, x, y, x + width, y, Color);
            DrawLine(Canvas, x, y, x, y + height, Color);
            DrawLine(Canvas, x, y + height, x + width, y + height, Color);
            DrawLine(Canvas, x + width, y + height, x + width, y, Color);
        }

        /// <summary>
        /// Draws a filled recatngle to a given canvas<br></br>
        /// NOTE: If transparent parameter is true, it may cause severe performance loss
        /// </summary>
        /// <param name="Canvas">The image you want to draw the filled rectangle onto</param>
        /// <param name="color">Color of the filled rectangle in integer</param>
        /// <param name="X">X coordiante (on canvas)</param>
        /// <param name="Y">Y coordiante (on canvas)</param>
        /// <param name="Width">Width of the filled rectangle</param>
        /// <param name="Height">Height of the filled rectangle</param>
        /// <param name="transparent"></param>
        public static void DrawFilledRectangle(Bitmap Canvas, int color, int X, int Y, int Width, int Height, bool transparent = false)
        {
            if(X == 0 && Width == Canvas.Width)
            {
                Bitmap Temp = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
                Array.Fill(Temp.RawData, color);
                Array.Copy(Temp.RawData, 0, Canvas.RawData, Y * Canvas.Width, Temp.RawData.Length);
            }
            else
            {
                if (transparent == true)
                {
                    int r = (color & 0xff0000) >> 16;
                    int g = (color & 0x00ff00) >> 8;
                    int b = (color & 0x0000ff);

                    float blendFactor = 0.5f;
                    float inverseBlendFactor = 1 - blendFactor;

                    for (int j = Y; j < Y + Height; j++)
                    {
                        for (int i = X; i < X + Width; i++)
                        {
                            int r3 = (cover.RawData[j * width + i] & 0xff0000) >> 16;
                            int g3 = (cover.RawData[j * width + i] & 0x00ff00) >> 8;
                            int b3 = (cover.RawData[j * width + i] & 0x0000ff);

                            int r2 = (int)(inverseBlendFactor * r3 + blendFactor * r);
                            int g2 = (int)(inverseBlendFactor * g3 + blendFactor * g);
                            int b2 = (int)(inverseBlendFactor * b3 + blendFactor * b);

                            DrawPixel(Canvas, i, j, colourToNumber(r2, g2, b2));
                        }
                    }
                }
                else
                {
                    if (X <= width)
                    {
                        int[] line = new int[Width];
                        if (X < 0)
                        {
                            line = new int[Width + X];
                        }
                        else if (X + Width > Canvas.Width)
                        {
                            line = new int[Width - (X + Width - Canvas.Width)];
                        }
                        Array.Fill(line, color);

                        for (int i = Y; i < Y + Height; i++)
                        {
                            if(i < Canvas.Height && i >= 0)
                            {
                                Array.Copy(line, 0, Canvas.RawData, (i * Canvas.Width) + X, line.Length);
                            }
                        }
                    }
                }
            }
        }
        public static void DrawCircle(Bitmap canvas, int centerX, int centerY, int radius, int color)
        {
            int x = radius;
            int y = 0;
            int p = 1 - radius;

            // Use symmetry to draw points in all eight octants
            void DrawCirclePoints(int x, int y)
            {
                DrawPixel(canvas, centerX + x, centerY + y, color);
                DrawPixel(canvas, centerX - x, centerY + y, color);
                DrawPixel(canvas, centerX + x, centerY - y, color);
                DrawPixel(canvas, centerX - x, centerY - y, color);
                DrawPixel(canvas, centerX + y, centerY + x, color);
                DrawPixel(canvas, centerX - y, centerY + x, color);
                DrawPixel(canvas, centerX + y, centerY - x, color);
                DrawPixel(canvas, centerX - y, centerY - x, color);
            }

            while (x >= y)
            {
                DrawCirclePoints(x, y);
                y++;

                if (p <= 0)
                {
                    p += 2 * y + 1;
                }
                else
                {
                    x--;
                    p += 2 * y - 2 * x + 1;
                }
            }
        }

        /// <summary>
        /// Draws a filled ellipse to the screen
        /// </summary>
        /// <param name="input">The image you want your ellipse to be rendered onto</param>
        /// <param name="xCenter">X coordinate on input image (x center of ellipse)</param>
        /// <param name="yCenter">Y coordinate on input image (y center of ellipse)</param>
        /// <param name="yR">Height of the ellipse (into both directions)</param>
        /// <param name="xR">Width of the ellipse (into both directions)</param>
        /// <param name="color">Color of the ellipse in integer</param>
        /// <returns></returns>
        public static Bitmap DrawFilledEllipse(Bitmap input, int xCenter, int yCenter, int yR, int xR, int color)
        {
            for (int y = -yR; y <= yR; y++)
            {
                for (int x = -xR; x <= xR; x++)
                {
                    double ellipseEquation = ((double)(x * x) / (xR * xR)) + ((double)(y * y) / (yR * yR));
                    if (ellipseEquation <= 1.0)
                    {
                        // Set pixel ensuring it's within bounds
                        int pixelX = xCenter + x;
                        int pixelY = yCenter + y;

                        if (pixelX >= 0 && pixelX < input.Width && pixelY >= 0 && pixelY < input.Height)
                        {
                            DrawPixel(input, pixelX, pixelY, color); // Change color as desired
                        }
                    }
                }
            }

            return input;
        }

        /// <summary>
        /// Draws an image to a given canvas
        /// <br></br>
        /// NOTE: This method ignores Alpha values
        /// </summary>
        /// <param name="image">The image you want to draw out</param>
        /// <param name="x">X coordinate (on into image)</param>
        /// <param name="y">Y coordinate (on into image)</param>
        /// <param name="into">The image you want your image to be rendered onto</param>
        public static void DrawImage(Bitmap image, int x, int y, Bitmap into)
        {
            int TempX = x;
            int[] line = new int[image.Width];
            if (x < into.Width)
            {
                if (x < 0)
                {
                    line = new int[image.Width + x];
                    x = 0;
                }
                else if (x + image.Width > into.Width)
                {
                    line = new int[image.Width - (x + image.Width - into.Width)];
                }

                int counter = 0;
                int scan_line = 0;
                for (int _y = y; _y < y + image.Height; _y++, scan_line++)
                {
                    try
                    {
                        if (TempX < 0)
                        {
                            Array.Copy(image.RawData, scan_line * image.Width - TempX, line, 0, line.Length);
                        }
                        else
                        {
                            Array.Copy(image.RawData, scan_line * image.Width, line, 0, line.Length);
                        }
                        if (_y < into.Height - 1 && _y > 0)
                        {
                            if (x < 0)
                            {
                                x = 0;
                            }
                            line.CopyTo(into.RawData, _y * into.Width + x);
                            counter += (int)image.Width;
                        }
                        else
                        {
                            counter += (int)image.Width;
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        /// <summary>
        /// Draws an image to a given canvas
        /// <br></br>
        /// NOTE: Not actually blending colors by their alpha value! Works like this: if RGB value is 0, 0, 0 => pixel is not rendered(transparent)
        /// </summary>
        /// <param name="image">The alpha image you want to draw out</param>
        /// <param name="x">X coordinate (on into image)</param>
        /// <param name="y">Y coordinate (on into image)</param>
        /// <param name="into">The image you want your alpha image to be rendered onto</param>
        /// <returns></returns>
        public static Bitmap DrawImageAlpha(Bitmap image, int x, int y, Bitmap into)
        {
            int TempX = x;
            int[] line = new int[image.Width];
            switch(x < into.Width && x + image.Width >= 0)
            {
                case true:
                    switch(x < 0)
                    {
                        case true:
                            line = new int[image.Width + x];
                            x = 0;
                            break;
                        case false:
                            switch (x + image.Width > into.Width)
                            {
                                case true:
                                    line = new int[image.Width - (x + image.Width - into.Width)];
                                    break;
                            }
                            break;
                    }

                    int counter = 0;
                    int scan_line = 0;
                    for (int _y = y; _y < y + image.Height; _y++, scan_line++)
                    {
                        try
                        {
                            switch(TempX < 0)
                            {
                                case true:
                                    Array.Copy(image.RawData, scan_line * image.Width - TempX, line, 0, line.Length);
                                    break;
                                case false:
                                    Array.Copy(image.RawData, scan_line * image.Width, line, 0, line.Length);
                                    break;
                            }
                            bool found = false;
                            for(int i = 0; i < line.Length && found == false; i++)
                            {
                                switch (line[i])
                                {
                                    case 0:
                                        found = true;
                                        break;
                                    default:
                                        i = line.Length;
                                        for(int j = line.Length - 1; j >= 0; j--)
                                        {
                                            switch (line[j])
                                            {
                                                case 0:
                                                    found = true;
                                                    break;
                                                default:
                                                    j = -1;
                                                    break;
                                            }
                                        }
                                        break;
                                }
                            }
                            switch (found == false)
                            {
                                case true:
                                    if (_y < into.Height - 1 && _y > 0)
                                    {
                                        if(x < 0)
                                        {
                                            x = 0;
                                        }
                                        line.CopyTo(into.RawData, _y * into.Width + x);
                                        counter += (int)image.Width;
                                    }
                                    else
                                    {
                                        counter += (int)image.Width;
                                    }
                                    break;
                                case false:
                                    x = TempX;
                                    for (int _x = x; _x < x + image.Width; _x++)
                                    {
                                        if (_y < into.Height - 1)
                                        {
                                            if (_x <= into.Width && _x >= 0)
                                            {
                                                if (image.RawData[counter] == 0)
                                                {
                                                    counter++;
                                                }
                                                else
                                                {
                                                    DrawPixel(into, _x, _y, image.RawData[counter]);
                                                    counter++;
                                                }
                                            }
                                            else
                                            {
                                                counter++;
                                            }
                                        }
                                        else
                                        {
                                            counter += (int)image.Width;
                                        }
                                    }

                                    break;
                            }
                        }
                        catch
                        {

                        }
                    }
                    break;
            }
            return into;
        }

        /// <summary>
        /// Scale an image
        /// <br></br>
        /// NOTE: This image scaler comes with COSMOS by default
        /// </summary>
        /// <param name="image">Image you want to resize</param>
        /// <param name="newWidth">New width</param>
        /// <param name="newHeight">New height</param>
        /// <returns></returns>
        public static Bitmap ScaleImageStock(Bitmap image, uint newWidth, uint newHeight)
        {
            Bitmap pixels = image;

            int w1 = (int)image.Width;
            int h1 = (int)image.Height;
            Bitmap temp = new Bitmap(newWidth, newHeight, ColorDepth.ColorDepth32);
            int xRatio = (int)((w1 << 16) / newWidth) + 1;
            int yRatio = (int)((h1 << 16) / newHeight) + 1;
            int x2, y2;

            for (int i = 0; i < newHeight; i++)
            {
                for (int j = 0; j < newWidth; j++)
                {
                    x2 = (j * xRatio) >> 16;
                    y2 = (i * yRatio) >> 16;
                    temp.RawData[(i * newWidth) + j] = pixels.RawData[(y2 * w1) + x2];
                }
            }

            return temp;
        }

        /// <summary>
        /// Scale image, but you can only go bigger and actually just multiply the sides
        /// <br></br>
        /// Example: 400 width image can only scale up to 800; 1200; 1600 etc.
        /// <br></br>
        /// Renders on main canvas only
        /// </summary>
        /// <param name="image">Image you want to scale</param>
        /// <param name="x">X coordinate (on main canvas)</param>
        /// <param name="y">Y coordinate (on main canvas)</param>
        public static void ScaleImage(Image image, int x, int y)
        {
            int counter = 0;
            int counter2 = 1;
            int prewy = y;
            for (int _y = y; _y < y + image.Height * 3; _y++)
            {
                for (int _x = x; _x < x + image.Width * 3; _x++)
                {
                    if ((_y * width) - (width - _x) < width * height)
                    {
                        if (_x > width)
                        {
                            counter++;
                        }
                        else
                        {
                            if (counter2 % 3 == 0)
                            {
                                cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
                                counter++;
                                counter2++;
                            }
                            else
                            {
                                cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
                                counter2++;
                            }
                        }
                    }
                }
                counter -= (int)image.Width;
                _y += 1;
                for (int _x = x; _x < x + image.Width * 3; _x++)
                {
                    if ((_y * width) - (width - _x) < width * height)
                    {
                        if (_x > width)
                        {
                            counter++;
                        }
                        else
                        {
                            if (counter2 % 3 == 0)
                            {
                                cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
                                counter++;
                                counter2++;
                            }
                            else
                            {
                                cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
                                counter2++;
                            }
                        }
                    }
                }
                counter -= (int)image.Width;
                _y += 1;
                for (int _x = x; _x < x + image.Width * 3; _x++)
                {
                    if ((_y * width) - (width - _x) < width * height)
                    {
                        if (_x > width)
                        {
                            counter++;
                        }
                        else
                        {
                            if (counter2 % 3 == 0)
                            {
                                cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
                                counter++;
                                counter2++;
                            }
                            else
                            {
                                cover.RawData[((_y * width) - (width - _x))] = image.RawData[counter];
                                counter2++;
                            }
                        }
                    }
                }
                counter2 = 1;
                prewy++;
            }
        }

        /// <summary>
        /// Used to make windows(canvases) slightly transparent
        /// <br></br>
        /// If you plan to use this outside of CrystalOS Alpha (why?) blendFactor is normally equal to 0.85f.
        /// </summary>
        /// <param name="image">Image you want to make transparent</param>
        /// <param name="x">X coordinate (on main canvas)</param>
        /// <param name="y">Y coordinate (on main canvas)</param>
        /// <param name="output">The image you want as output</param>
        /// <returns></returns>
        public static Bitmap EnableTransparency(Bitmap image, int x, int y, Bitmap output)
        {
            int r = GlobalValues.R;
            int g = GlobalValues.G;
            int b = GlobalValues.B;

            float blendFactor = (float)GlobalValues.LevelOfTransparency / 100.0f;
            float inverseBlendFactor = 1 - blendFactor;

            int counter = 0;
            for (int _y = y; _y < y + image.Height; _y++)
            {
                for (int _x = x; _x < x + image.Width; _x++)
                {
                    if (_y < height)
                    {
                        if (_x <= width)
                        {
                            if (image.RawData[counter] == 0)
                            {
                                counter++;
                            }
                            else
                            {
                                try
                                {
                                    int r3 = (cover.RawData[_y * width + _x] & 0xff0000) >> 16;
                                    int g3 = (cover.RawData[_y * width + _x] & 0x00ff00) >> 8;
                                    int b3 = (cover.RawData[_y * width + _x] & 0x0000ff);

                                    int r2 = (int)(inverseBlendFactor * r3 + blendFactor * r);
                                    int g2 = (int)(inverseBlendFactor * g3 + blendFactor * g);
                                    int b2 = (int)(inverseBlendFactor * b3 + blendFactor * b);

                                    output.RawData[counter] = colourToNumber(r2, g2, b2);
                                }
                                catch
                                {

                                }
                                counter++;
                            }
                        }
                        else
                        {
                            counter++;
                        }
                    }
                    else
                    {
                        counter += (int)image.Width;
                    }
                }
            }
            if(y > 5 && y + output.Height < height)
            {
                output = Blur(output, 5);
            }
            return output;
        }

        /// <summary>
        /// Does the same thing as EnableTransparency, except with pre-existing RGB values
        /// </summary>
        /// <param name="image">The image you want to add transparency to</param>
        /// <param name="x">X coordinate (on the back image)</param>
        /// <param name="y">Y coordinate (on the back image)</param>
        /// <param name="output">The image you want as output</param>
        /// <param name="R">Red</param>
        /// <param name="G">Green</param>
        /// <param name="B">Blue</param>
        /// <param name="back">The back canvas from where it takes the pixel data from before blending</param>
        /// <param name="Bluring">Enable bluring is on by default, because of windows</param>
        /// <returns></returns>
        public static Bitmap EnableTransparencyPreRGB(Bitmap image, int x, int y, Bitmap output, int R, int G, int B, Bitmap back, bool Bluring = true)
        {
            int r = R;
            int g = G;
            int b = B;

            float blendFactor = (float)GlobalValues.LevelOfTransparency / 100.0f;
            float inverseBlendFactor = 1 - blendFactor;

            int counter = 0;

            for (int _y = y; _y < y + image.Height; _y++)
            {
                for (int _x = x; _x < x + image.Width; _x++, counter++)
                {
                    if (_y < back.Height)
                    {
                        if (_x < back.Width)
                        {
                            if (image.RawData[counter] == 0)
                            {
                                //counter++;
                            }
                            else
                            {
                                try
                                {
                                    int r3 = (back.RawData[_y * back.Width + _x] & 0xff0000) >> 16;
                                    int g3 = (back.RawData[_y * back.Width + _x] & 0x00ff00) >> 8;
                                    int b3 = (back.RawData[_y * back.Width + _x] & 0x0000ff);

                                    int r2 = (int)(inverseBlendFactor * r3 + blendFactor * r);
                                    int g2 = (int)(inverseBlendFactor * g3 + blendFactor * g);
                                    int b2 = (int)(inverseBlendFactor * b3 + blendFactor * b);

                                    output.RawData[counter] = colourToNumber(r2, g2, b2);
                                }
                                catch
                                {

                                }
                                //counter++;
                            }
                        }
                        else
                        {
                            //counter++;
                        }
                    }
                    else
                    {
                        counter += (int)image.Width - 1;
                    }
                }
            }
            if (y > 5 && y + output.Height < height && Bluring == true)
            {
                output = Blur(output, 5);
            }
            return output;
        }

        /// <summary>
        /// Blurs an image
        /// </summary>
        /// <param name="image">The image you want to blur</param>
        /// <param name="blurAmount">Level of bluring</param>
        /// <returns></returns>
        private static Bitmap Blur(Bitmap image, int blurAmount)
        {
            for(int x = blurAmount; x < image.Width - blurAmount; x++)
            {
                for (int y = blurAmount; y < image.Height; y++)
                {
                    try
                    {
                        if((y + blurAmount) < height)
                        {
                            Color prevX = Color.FromArgb(image.RawData[y * image.Width + (x - 1)]);
                            Color nextX = Color.FromArgb(image.RawData[y * image.Width + (x + 1)]);
                            Color prevY = Color.FromArgb(image.RawData[(y - blurAmount) * image.Width + x]);
                            Color nextY = Color.FromArgb(image.RawData[(y + blurAmount) * image.Width + x]);

                            int avgR = (int)((prevX.R + nextX.R + prevY.R + nextY.R) / 4);
                            int avgG = (int)((prevX.G + nextX.G + prevY.G + nextY.G) / 4);
                            int avgB = (int)((prevX.B + nextX.B + prevY.B + nextY.B) / 4);

                            image.RawData[y * image.Width + x] = colourToNumber(avgR, avgG, avgB);
                        }
                    }
                    catch
                    {

                    }
                }
            }
            return image;
        }

        /// <summary>
        /// Converts a color to it's integer value
        /// </summary>
        /// <param name="r">Red</param>
        /// <param name="g">Green</param>
        /// <param name="b">Blue</param>
        /// <returns></returns>
        public static int colourToNumber(int r, int g, int b)
        {
            return (r << 16) + (g << 8) + (b);
        }

        /// <summary>
        /// For 3D rendering in the future
        /// </summary>
        /// <param name="Canvas">The canvas you want to render it on</param>
        /// <param name="Points">A list of points</param>
        /// <param name="Color">Color of the sides</param>
        public static void DrawPollygonFrame(Bitmap Canvas, List<Point> Points, int Color)
        {
            int XMax = 0;
            int YMax = 0;
            for (int i = 0; i < Points.Count; i++)
            {
                if (Points[i].X > XMax)
                {
                    XMax = Points[i].X;
                }
                if (Points[i].Y > YMax)
                {
                    YMax = Points[i].Y;
                }
            }
            Bitmap Temp = new Bitmap((uint)XMax, (uint)YMax, ColorDepth.ColorDepth32);
            Array.Fill(Temp.RawData, 0);

            for (int i = 0; i < Points.Count; i++)
            {
                if (i < Points.Count - 1)
                {
                    DrawLine(Temp, Points[i].X, Points[i].Y, Points[i + 1].X, Points[i + 1].Y, Color);
                }
                else
                {
                    DrawLine(Temp, Points[i].X, Points[i].Y, Points[0].X, Points[0].Y, Color);
                }
            }
            ImprovedVBE.DrawImageAlpha(Temp, 10, 10, Canvas);
        }

        /// <summary>
        /// For 3D rendering in the future
        /// </summary>
        /// <param name="Canvas">The canvas you want to render it on</param>
        /// <param name="Points">A list of points</param>
        /// <param name="Color">Color of the filled pollygon</param>

        public static void DrawFilledPollygon(Bitmap canvas, List<Point> points, int color)
        {
            int width = (int)canvas.Width;
            int height = (int)canvas.Height;

            // Step 1: Find the bounding box of the polygon
            int yMin = points[0].Y;
            int yMax = points[0].Y;
            for (int i = 1; i < points.Count; i++)
            {
                yMin = Math.Min(yMin, points[i].Y);
                yMax = Math.Max(yMax, points[i].Y);
            }

            // Step 2: Create and reuse the edge table
            List<Edge>[] edgeTable = new List<Edge>[yMax - yMin + 1];
            for (int i = 0; i < edgeTable.Length; i++)
            {
                edgeTable[i] = new List<Edge>();
            }

            // Step 3: Populate the edge table
            for (int i = 0; i < points.Count; i++)
            {
                Point start = points[i];
                Point end = points[(i + 1) % points.Count];

                if (start.Y != end.Y) // Ignore horizontal edges
                {
                    if (start.Y > end.Y)
                    {
                        (start, end) = (end, start); // Swap start and end
                    }

                    int deltaY = end.Y - start.Y;
                    float slope = (float)(end.X - start.X) / deltaY;

                    edgeTable[start.Y - yMin].Add(new Edge(start.X, end.Y, slope));
                }
            }

            // Step 4: Fill the polygon
            List<Edge> activeEdges = new List<Edge>();

            for (int y = yMin; y <= yMax; y++)
            {
                // Add new edges to the active edge list from the edge table, inserting in the correct position
                foreach (var edge in edgeTable[y - yMin])
                {
                    // Insert the new edge in the correct sorted position
                    int insertIndex = activeEdges.Count;
                    for (int i = 0; i < activeEdges.Count; i++)
                    {
                        if (activeEdges[i].X > edge.X)
                        {
                            insertIndex = i;
                            break;
                        }
                    }
                    activeEdges.Insert(insertIndex, edge);
                }

                // Remove edges where the Ymax has been exceeded
                for (int i = activeEdges.Count - 1; i >= 0; i--)
                {
                    if (activeEdges[i].YMax <= y)
                    {
                        activeEdges.RemoveAt(i);
                    }
                }

                // Step 5: Fill between pairs of active edges
                int Height = (int)(y * canvas.Width);
                for (int i = 0; i < activeEdges.Count; i += 2)
                {
                    int xStart = (int)activeEdges[i].X;
                    int xEnd = (int)activeEdges[i + 1].X;

                    if (xStart <= xEnd)
                    {
                        for (int x = xStart; x <= xEnd; x++)
                        {
                            canvas.RawData[Height + x] = color;
                        }
                    }
                }

                // Step 6: Update the X values for the next scanline
                foreach (var edge in activeEdges)
                {
                    edge.X += edge.Slope;
                }
            }
        }

        #endregion Graphics

        #region For Window look
        public static int GetGradientColor(int x, int y, int width, int height, Color startColor, Color endColor)
        {
            int rStart = startColor.R;
            int gStart = startColor.G;
            int bStart = startColor.B;

            int rEnd = endColor.R;
            int gEnd = endColor.G;
            int bEnd = endColor.B;

            int r = (int)((double)x / width * (rEnd - rStart)) + rStart;
            int g = (int)((double)x / width * (gEnd - gStart)) + gStart;
            int b = (int)((double)x / width * (bEnd - bStart)) + bStart;

            return colourToNumber(r, g, b);
        }
        public static void DrawGradientLeftToRight(Bitmap Input)
        {
            int x_1 = 0;

            int gradientColorStart = GetGradientColor(0, 0, (int)Input.Width, (int)Input.Height, GlobalValues.StartColor, GlobalValues.EndColor);
            int gradientColorEnd = GetGradientColor((int)Input.Width, 0, (int)Input.Width, (int)Input.Height, GlobalValues.StartColor, GlobalValues.EndColor);

            int rStart = Color.FromArgb(gradientColorStart).R;
            int gStart = Color.FromArgb(gradientColorStart).G;
            int bStart = Color.FromArgb(gradientColorStart).B;

            int rEnd = Color.FromArgb(gradientColorEnd).R;
            int gEnd = Color.FromArgb(gradientColorEnd).G;
            int bEnd = Color.FromArgb(gradientColorEnd).B;

            for (int i = 0; i < Input.RawData.Length; i++)
            {
                if (x_1 == Input.Width - 1)
                {
                    x_1 = 0;
                }
                else
                {
                    x_1++;
                }
                int r = (int)((double)x_1 / Input.Width * (rEnd - rStart)) + rStart;
                int g = (int)((double)x_1 / Input.Width * (gEnd - gStart)) + gStart;
                int b = (int)((double)x_1 / Input.Width * (bEnd - bStart)) + bStart;
                if (Input.RawData[i] != 0)
                {
                    Input.RawData[i] = colourToNumber(r, g, b);
                }
                if (i / Input.Width > 20)
                {
                    break;
                }
            }
            x_1 = 0;
        }
        #endregion

        #region UI Elements
        #region Glassy Effect
        public static void DrawCircularGradient(Bitmap canvas, Color startColor, Color endColor, int centerX, int centerY, int diameter)
        {
            int radius = diameter / 2;
            for (int r = radius; r > 0; r--)
            {
                // Calculate interpolation factor (0.0 at edge to 1.0 at center)
                float t = (float)r / radius;

                // Linearly interpolate between startColor and endColor
                int red = (int)(startColor.R * (1 - t) + endColor.R * t);
                int green = (int)(startColor.G * (1 - t) + endColor.G * t);
                int blue = (int)(startColor.B * (1 - t) + endColor.B * t);

                // Draw the filled circle at each radius step
                DrawFilledEllipse(canvas, centerX, centerY, r, r, colourToNumber(red, green, blue));
            }
        }
        public static void DrawFilledArc(Bitmap canvas, int color, int centerX, int centerY, int width, int height, int startAngle, int endAngle)
        {
            int radius = width / 2;
            int arcHeight = height / 2; // Limiting the arc to the upper half

            for (int y = 0; y <= arcHeight; y++)
            {
                int rowRadius = (int)Math.Sqrt(radius * radius - y * y); // Row width based on the circle equation
                int startX = centerX - rowRadius;
                int endX = centerX + rowRadius;

                for (int x = startX; x <= endX; x++)
                {
                    try
                    {
                        canvas.RawData[canvas.Width * (centerY - y) + x] = color;
                    }
                    catch { }
                    //canvas.SetPixel(x, centerY - y, color); // Draw only in the upper half
                }
            }
        }
        #endregion Glassy Effect
        #endregion UI Elements
    }

    // Helper class to store edge data
    public class Edge
    {
        public float X;      // Current X position of the edge
        public int YMax;     // Maximum Y value of the edge
        public float Slope;  // 1 / slope of the edge

        public Edge(float x, int yMax, float slope)
        {
            X = x;
            YMax = yMax;
            Slope = slope;
        }
    }
}
