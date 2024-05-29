using Cosmos.System.Graphics;
using System;

namespace CrystalOSAlpha.UI_Elements
{
    class Progressbar : UIElementHandler
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Color { get; set; }
        public int Pos { get; set; }
        public int Value { get; set; }
        public float Sensitivity { get; set; }
        public int LockedPos { get; set; }
        public int MinVal { get; set; }
        public int MaxVal { get; set; }
        public bool Clicked { get; set; }
        public string Text { get; set; }
        public string ID { get; set; }
        public TypeOfElement TypeOfElement { get; set; }
        public Progressbar(int x, int y, int width, int height, int color, int value, string iD)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Color = color;
            Value = value;
            ID = iD;
            TypeOfElement = TypeOfElement.ProgressBar;
        }
        public void Render(Bitmap Canvas)
        {
            ImprovedVBE.DrawFilledRectangle(Canvas, 1, X, Y, Width, Height);
            ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(255, 255, 255), X + 2, Y + 2, Width - 4, Height - 4);
            Value = Math.Clamp(Value, 0, 100);
            double Range = Width - 4;
            double PercentPerPixel = Range / 100.0;
            if(PercentPerPixel * Value > 0)
            {
                ImprovedVBE.DrawFilledRectangle(Canvas, Color, X + 2, Y + 2, (int)(PercentPerPixel * Value), Height - 4);
            }
        }

        public bool CheckClick(int X, int Y)
        {
            return false;
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
