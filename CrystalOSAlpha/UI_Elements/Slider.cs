﻿using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;

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

            ImprovedVBE.DrawFilledEllipse(Canvas, X + Value, Y + 4, 6, 6, ImprovedVBE.colourToNumber(Global_integers.R + 20, Global_integers.G + 20, Global_integers.B + 20));
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
    }
}
