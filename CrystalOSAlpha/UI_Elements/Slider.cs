using Cosmos.System;
using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.UI_Elements
{
    class Slider : UIElementHandler
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Value { get; set; }
        public string ID { get; set; }
        public int Height { get; set; }
        public string Text { get; set; }
        public bool Clicked { get; set; }
        public int Color { get; set; }
        public TypeOfElement TypeOfElement { get; set; }
        public int Pos { get; set; }
        float UIElementHandler.Sensitivity { get; set; }
        public int LockedPos { get; set; }
        public int MinVal { get; set; }
        public int MaxVal { get; set; }
        public List<Point> Points { get; set; }

        private float Sensitivity;

        public Slider(int x, int y, int width, int minimumValue, int maximumValue, int value, string ID)
        {
            X = x;
            Y = y + 22;
            Width = width;
            Height = 12;
            Value = value;
            MinVal = minimumValue;
            MaxVal = maximumValue;
            this.ID = ID;
            Sensitivity = (float)Width / (MaxVal - MinVal);
            TypeOfElement = TypeOfElement.Slider;
        }

        public void Render(Bitmap Canvas)
        {
            ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(1, 1, 1), X, Y, Width, 7, false);

            // Calculate the position of the slider's circle based on the value
            int circlePosX = X + (int)((Value - MinVal) * Sensitivity);

            ImprovedVBE.DrawFilledEllipse(Canvas, circlePosX, Y + 4, 6, 6, ImprovedVBE.colourToNumber(30, 30, 30));
        }

        public bool CheckClick(int x, int y)
        {
            if (MouseManager.MouseState == MouseState.Left)
            {
                int circlePosX = X + (int)((Value - MinVal) * Sensitivity);

                // Check if the mouse is over the slider's circle or if it's already clicked
                if (Clicked || (MouseManager.X > x + circlePosX - 6 && MouseManager.X < x + circlePosX + 6 && MouseManager.Y > y + Y - 6 && MouseManager.Y < y + Y + 12))
                {
                    Clicked = true;
                    int Temp = Value;
                    UpdateValue(x);
                    if(Temp != Value)
                    {
                        return true;
                    }
                }
            }

            if (MouseManager.MouseState == MouseState.None)
            {
                Clicked = false;
            }

            return false;
        }

        public void UpdateValue(int x)
        {
            Value = MinVal + (int)((MouseManager.X - x - X) / Sensitivity);
            Value = Math.Clamp(Value, MinVal, MaxVal);
        }

        public void SetValue(int X, int Y, string Value, bool writeprotected)
        {
            throw new NotImplementedException();
        }

        string UIElementHandler.GetValue(int X, int Y)
        {
            throw new NotImplementedException();
        }
    }
}
