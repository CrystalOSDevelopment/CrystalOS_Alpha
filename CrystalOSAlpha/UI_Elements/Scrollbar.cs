using Cosmos.System.Graphics;
using CrystalOSAlpha;
using CrystalOSAlpha.UI_Elements;
using System.Collections.Generic;
using System.Drawing;

namespace CrYstalOSAlpha.UI_Elements
{
    class Scrollbar : UIElementHandler
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Pos { get; set; }
        public bool Clicked { get; set; }
        public string Text { get; set; }
        public int Color { get; set; }
        public TypeOfElement TypeOfElement { get; set; }
        public string ID { get; set; }
        public int Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float Sensitivity { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int LockedPos { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int MinVal { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int MaxVal { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public List<Point> Points { get; set; }

        public Scrollbar(int X, int Y, int width, int height, int Pos)
        {
            this.X = X;
            this.Y = Y;
            this.Width = width;
            this.Height = height;
            this.Pos = Pos;
        }

        public void Render(Bitmap canvas)
        {
            ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(36, 36, 36), X, Y + 22, Width, Height, false);
            if(Clicked == false)
            {
                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(50, 50, 50), X + 2, Y + 42 + Pos, Width - 4, 20, false);
            }
            else
            {
                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(100, 100, 100), X + 2, Y + 42 + Pos, Width - 4, 20, false);
            }
        }

        public bool CheckClick(int X, int Y)
        {
            throw new System.NotImplementedException();
        }

        public void SetValue(int X, int Y, string Value, bool writeprotected)
        {
            throw new System.NotImplementedException();
        }

        string UIElementHandler.GetValue(int X, int Y)
        {
            throw new System.NotImplementedException();
        }
    }
}
