using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using System;

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
        public float Sensitivity = 0;

        public Slider(int x, int y, int width, int value, string ID)
        {
            X = x;
            Y = y + 22;
            Width = width;
            Value = value;
            this.ID = ID;
        }
        public Slider(int x, int y, int width, int MinValue, int MaxValue, int value, string ID)
        {
            X = x;
            Y = y + 22;
            Width = width;
            Value = value;
            this.MinValue = MinValue;
            this.MaxValue = MaxValue;
            this.ID = ID;
            Sensitivity = (float)((float)MaxValue - (float)MinValue) / (float)width;
        }

        public void Render(Bitmap Canvas)
        {
            ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(1, 1, 1), X, Y, Width, 7, false);

            ImprovedVBE.DrawFilledEllipse(Canvas, (int)(X + Value / Sensitivity), Y + 4, 6, 6, ImprovedVBE.colourToNumber(30, 30, 30));
        }

        public bool CheckForClick(int x, int y)
        {
            if(MouseManager.MouseState == MouseState.Left)
            {
                if(MouseManager.X > x + X + (Value / Sensitivity) - 6 && MouseManager.X < x + X + (Value / Sensitivity) + 6)
                {
                    if (MouseManager.Y > y + Y - 6 && MouseManager.Y < y + Y + 6)
                    {
                        Clicked = true;
                        return true;
                    }
                }
            }
            if(Clicked == true && MouseManager.MouseState == MouseState.Left && (int)((MouseManager.X - x - X) * Sensitivity) != Value)
            {
                return true;
            }
            Clicked = false;
            return false;
        }

        public void UpdateValue(int x)
        {
            Value = (int)((MouseManager.X - x - X) * Sensitivity);
            Value = Math.Clamp(Value, MinValue, MaxValue);
        }
    }
}