using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Graphics.Widgets
{
    public class Base
    {
        public static int width = 0;
        public static int height = 0;
        public static Bitmap canvas;
        public static Bitmap Widget_Back(int width, int height, int CurrentColor)
        {
            canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32); //new int[width * height];
            Array.Fill(canvas.RawData, 0);
            Base.width = width;
            Base.height = height;

            DrawFilledEllipse(10, 10, 10, 10, CurrentColor);
            DrawFilledEllipse(width - 11, 10, 10, 10, CurrentColor);
            DrawFilledEllipse(10, height - 10, 10, 10, CurrentColor);
            DrawFilledEllipse(width - 11, height - 10, 10, 10, CurrentColor);

            DrawFilledRectangle(CurrentColor, 0, 10, width, height - 20);
            DrawFilledRectangle(CurrentColor, 5, 0, width - 10, 15);
            DrawFilledRectangle(CurrentColor, 5, height - 15, width - 10, 15);
            return canvas;
        }

        public static void DrawFilledEllipse(int xCenter, int yCenter, int yR, int xR, int color)
        {
            /*
            int r = (color & 0xff0000) >> 16;
            int g = (color & 0x00ff00) >> 8;
            int b = (color & 0x0000ff);

            float blendFactor = 0.5f;
            float inverseBlendFactor = 1 - blendFactor;
            */
            for (int y = -yR; y <= yR; y++)
            {
                for (int x = -xR; x <= xR; x++)
                {
                    if ((x * x * yR * yR) + (y * y * xR * xR) <= yR * yR * xR * xR)
                    {
                        /*
                        int r3 = (cover.RawData[(yCenter + y) * width + xCenter + x] & 0xff0000) >> 16;
                        int g3 = (cover.RawData[(yCenter + y) * width + xCenter + x] & 0x00ff00) >> 8;
                        int b3 = (cover.RawData[(yCenter + y) * width + xCenter + x] & 0x0000ff);
                        //Color c = Color.FromArgb(cover.RawData[j * width + i]);

                        int r2 = (int)(inverseBlendFactor * r3 + blendFactor * r);
                        int g2 = (int)(inverseBlendFactor * g3 + blendFactor * g);
                        int b2 = (int)(inverseBlendFactor * b3 + blendFactor * b);
                        */
                        if (xCenter + x > 0 && xCenter + x < width - 1 && yCenter + y > 0 && yCenter + y < height)
                        {
                            canvas.RawData[(yCenter + y) * width + xCenter + x] = color;
                        }

                        //DrawPixel(xCenter + x, yCenter + y, GetGradientColor(x, 0, width, height));

                        //DrawPixel(xCenter + x, yCenter + y, color);
                    }
                }
            }
        }

        public static void DrawFilledRectangle(int color, int X, int Y, int Width, int Height)
        {
            /*
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

                    DrawPixel(i, j, colourToNumber(r2, g2, b2));
                }
            }*/

            if (X < width && Y < height)
            {
                int[] line = new int[Width];
                if (X < 0)
                {
                    line = new int[Width + X];
                }
                else if (X + Width > width)
                {
                    line = new int[Width - (X + Width - width)];
                }
                Array.Fill(line, color);

                for (int i = Y; i < Y + Height; i++)
                {
                    Array.Copy(line, 0, canvas.RawData, (i * width) + X, line.Length);
                }
            }

        }
    }
}
