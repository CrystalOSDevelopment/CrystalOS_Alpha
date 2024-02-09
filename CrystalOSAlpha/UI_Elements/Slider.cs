using Cosmos.HAL;
using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.UI_Elements
{
    class Slider
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Value { get; set; }
        public int MinValue { get; set; }
        public int MaxValue { get; set; }
        public string ID { get; set; }
        public bool Clicked = false;

        public Slider(int x, int y, int width, int value, string ID)
        {
            X = x;
            Y = y + 22;
            Width = width;
            Value = value;
            this.ID = ID;
        }

        public void Render(Bitmap Canvas)
        {
            ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B), X, Y, Width, 7, false);

            DrawFilledEllipse(Canvas, X + Value, Y + 4, 6, 6, ImprovedVBE.colourToNumber(Global_integers.R + 20, Global_integers.G + 20, Global_integers.B + 20));
        }

        public bool CheckForClick(int x, int y)
        {
            if(MouseManager.MouseState == MouseState.Left)
            {
                if(MouseManager.X > x + X + Value - 6 && MouseManager.X < x + X + Value + 6)
                {
                    if (MouseManager.Y > y + Y - 6 && MouseManager.Y < y + Y + 6)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void UpdateValue(int x)
        {
            Value = (int)(MouseManager.X - x - X);
            if(Value > Width)
            {
                Value = Width;
            }
            if(Value < 0)
            {
                Value = 0;
            }
        }

        public void DrawFilledEllipse(Bitmap Canvas, int xCenter, int yCenter, int yR, int xR, int color)
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
                    if (x * x * yR * yR + y * y * xR * xR <= yR * yR * xR * xR)
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
                        if (xCenter + x > 0 && xCenter + x < Canvas.Width - 1 && yCenter + y > 0 && yCenter + y < Canvas.Height)
                        {
                            Canvas.RawData[(yCenter + y) * Canvas.Width + xCenter + x] = color;
                        }

                        //DrawPixelfortext(xCenter + x, yCenter + y, GetGradientColor(x, 0, width, height));

                        //DrawPixelfortext(xCenter + x, yCenter + y, color);
                    }
                }
            }
        }
    }
}
