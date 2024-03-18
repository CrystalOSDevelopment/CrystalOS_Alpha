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
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha
{
    public class ImprovedVBE
    {
        #region Wallpaper & Resolution
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Engine.Wallp1.bmp")] public static byte[] VBECanvas;

        //These are black images matching the size exacly the resolution
        public static Bitmap cover = new Bitmap(VBECanvas);//The base canvas
        public static Bitmap data = new Bitmap(VBECanvas);//To reset the base canvas
        public static Bitmap Temp = new Bitmap(VBECanvas);//To reset the base canvas

        public static int width = 1920;
        public static int height = 1080;
        public static bool Res = true;
        #endregion Wallpaper & Resolution

        #region Render to the screen and Clear
        public static void display(VBECanvas c)
        {
            c.DrawImage(cover, 0, 0);
            //clear(c, Global_Integers.c);
            //cover.RawData = data.RawData;
            Clear(0);
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

                    //width = 1920;
                    //height = 1080;

                    c.Clear(Color.Black);
                }
                catch
                {

                }

                Res = false;
            }
        }
        public static void Clear(int col)
        {
            data.RawData.CopyTo(cover.RawData, col);
        }
        #endregion Render to the screen and Clear
        
        #region Graphics
        //Draws a pixel to a specific location
        public static void DrawPixel(Bitmap Canvas, int x, int y, int color)
        {
            if (x > 0 && x < Canvas.Width && y >= 0 && y < Canvas.Height)
            {
                Canvas.RawData[y * Canvas.Width + x] = color;
            }
        }

        //Draws a line to the screen
        public static void DrawLine(Bitmap Canvas, float x1, float y1, float x2, float y2, int color)
        {
            float dx = x2 - x1;
            float dy = y2 - y1;

            float length = (float)Math.Sqrt(dx * dx + dy * dy);

            float angle = (float)Math.Atan2(dy, dx);

            for (float i = 0; i < length; i++)
            {
                DrawPixel(Canvas, (int)(x1 + Math.Cos(angle) * i), (int)(y1 + Math.Sin(angle) * i), color);
            }
        }

        //Draws a filled recatngle to a given canvas
        //NOTE: if transparent parameter is true, it may cause severe performance loss
        public static void DrawFilledRectangle(Bitmap Canvas, int color, int X, int Y, int Width, int Height, bool transparent)
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
                        //Color c = Color.FromArgb(cover.RawData[j * width + i]);

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
                        if(i < height)
                        {
                            Array.Copy(line, 0, Canvas.RawData, (i * Canvas.Width) + X, line.Length);
                        }
                    }
                }
            }
        }

        //Draws a filled Ellipse to the screen
        public static Bitmap DrawFilledEllipse(Bitmap input, int xCenter, int yCenter, int yR, int xR, int color)
        {
            for (int y = -yR; y <= yR; y++)
            {
                for (int x = -xR; x <= xR; x++)
                {
                    if ((x * x * yR * yR) + (y * y * xR * xR) <= yR * yR * xR * xR)
                    {
                        if (xCenter + x > 0 && xCenter + x < input.Width - 1 && yCenter + y > 0 && yCenter + y < input.Height)
                        {
                            input.RawData[(yCenter + y) * input.Width + xCenter + x] = color;
                        }
                    }
                }
            }
            return input;
        }

        //Draws an image to a given canvas
        //NOTE: This method ignores Alpha values
        public static void DrawImage(Bitmap image, int x, int y, Bitmap Into)
        {
            int counter = 0;
            int scan_line = 0;
            int[] line = new int[image.Width];
            for (int _y = y; _y < y + image.Height; _y++)
            {
                if (x < 0)
                {
                    line = new int[image.Width + x];
                    x = 0;
                }
                else if (x + image.Width > Into.Width)
                {
                    line = new int[image.Width - (x + image.Width - Into.Width)];
                }
                Array.Copy(Into.RawData, scan_line * image.Width, line, 0, image.Width);

                if(_y > 0)
                {
                    line.CopyTo(Into.RawData, (_y - 1) * width + x);
                }
                counter += (int)image.Width;
            }
        }

        //Draws an image to a given canvas
        //NOTE: Not actually blending colors by their alpha value! Works like this: if RGB value is 0, 0, 0 => pixel is not rendered(transparent)
        public static Bitmap DrawImageAlpha(Bitmap image, int x, int y, Bitmap into)
        {
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
                        Array.Copy(image.RawData, scan_line * image.Width, line, 0, line.Length);
                        bool found = false;
                        for(int i = 0; i < line.Length && found == false; i++)
                        {
                            if (line[i] == 0)
                            {
                                found = true;
                            }
                        }
                        if (found == false)
                        {
                            if (_y < into.Height - 1)
                            {
                                line.CopyTo(into.RawData, _y * into.Width + x);
                                counter += (int)image.Width;
                            }
                            else
                            {
                                counter += (int)image.Width;
                            }
                        }
                        else
                        {
                            for (int _x = x; _x < x + image.Width; _x++)
                            {
                                if (_y < into.Height - 1)
                                {
                                    if (_x <= into.Width)
                                    {
                                        if (image.RawData[counter] == 0)
                                        {
                                            counter++;
                                        }
                                        else
                                        {
                                            into.RawData[_y * into.Width + _x] = image.RawData[counter];
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
                    }
                    catch
                    {

                    }
                }
            }
            return into;
        }

        //Scale an image
        //NOTE: This image scaler comes with COSMOS by default
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

        //Scale image(my not so great, but faster implementation)
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

        //Used to make windows(canvases) slightly transparent
        public static Bitmap EnableTransparency(Bitmap image, int x, int y, Bitmap output)
        {
            int r = Global_integers.R;
            int g = Global_integers.G;
            int b = Global_integers.B;

            float blendFactor = 0.85f;
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
                                //output.RawData[counter] = cover.RawData[_y * width + _x];
                                counter++;
                            }
                            else
                            {
                                try
                                {
                                    int r3 = (cover.RawData[_y * width + _x] & 0xff0000) >> 16;
                                    int g3 = (cover.RawData[_y * width + _x] & 0x00ff00) >> 8;
                                    int b3 = (cover.RawData[_y * width + _x] & 0x0000ff);
                                    //Color c = Color.FromArgb(cover.RawData[j * width + i]);

                                    int r2 = (int)(inverseBlendFactor * r3 + blendFactor * r);
                                    int g2 = (int)(inverseBlendFactor * g3 + blendFactor * g);
                                    int b2 = (int)(inverseBlendFactor * b3 + blendFactor * b);

                                    //DrawPixel(image, _x, _y, colourToNumber(r2, g2, b2));
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

        //Does the same thing as EnableTransparency, except with pre-existing RGB values
        public static Bitmap EnableTransparencyPreRGB(Bitmap image, int x, int y, Bitmap output, int R, int G, int B, Bitmap back)
        {
            int r = R;
            int g = G;
            int b = B;

            float blendFactor = 0.85f;
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
            if (y > 5 && y + output.Height < height)
            {
                output = Blur(output, 5);
            }
            return output;
        }

        //Blurs an image
        public static Bitmap Blur(Bitmap image, int blurAmount)
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

        //Converts a color to it's integer value
        public static int colourToNumber(int r, int g, int b)
        {
            return (r << 16) + (g << 8) + (b);
        }

        //For 3D rendering in the future
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
        
        //For 3D rendering in the future
        public static void DrawFilledPollygon(Bitmap Canvas, List<Point> Points, int Color)
        {
            int XMax = 0;
            int YMax = 0;
            for(int i = 0; i < Points.Count; i++)
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

            for(int i = 0; i < Points.Count; i++)
            {
                if(i < Points.Count - 1)
                {
                    DrawLine(Temp, Points[i].X, Points[i].Y, Points[i + 1].X, Points[i + 1].Y, Color);
                }
                else
                {
                    DrawLine(Temp, Points[i].X, Points[i].Y, Points[0].X, Points[0].Y, Color);
                }
            }

            for(int i = 0; i < Temp.Height - 1; i++)
            {
                int StartX = 0;
                int EndX = 0;
                bool Found = false;

                for(int j = 0; j < Temp.Width && Found == false; j++)
                {
                    if (Temp.RawData[i * Temp.Width + j] != 0)
                    {
                        StartX = j;
                        Found = true;
                    }
                }

                Found = false;
                for (int j = (int)Temp.Width - 1; j >= 0 && Found == false; j--)
                {
                    if (Temp.RawData[i * Temp.Width + j] != 0)
                    {
                        EndX = j;
                        Found = true;
                    }
                }
                int Distance = EndX - StartX;
                Array.Fill(Temp.RawData, Color, (int)(i * Temp.Width + StartX), Distance);
            }
            DrawImageAlpha(Temp, 0, 0, Canvas);
        }
        #endregion Graphics

        #region For Window look
        public static int GetGradientColor(int x, int y, int width, int height)
        {
            int r = (int)((double)x / width * 255);
            int g = (int)((double)y / height * 255);
            int b = 255;

            return ImprovedVBE.colourToNumber(r, g, b);
        }
        public static void DrawGradientLeftToRight(Bitmap Input)
        {
            int x_1 = 0;

            int gradientColorStart = GetGradientColor(0, 0, (int)Input.Width, (int)Input.Height);
            int gradientColorEnd = GetGradientColor((int)Input.Width, 0, (int)Input.Width, (int)Input.Height);

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
    }
}
