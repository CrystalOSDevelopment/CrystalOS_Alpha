using Cosmos.System;
using Cosmos.System.Graphics;
using System;

namespace CrystalOSAlpha.UI_Elements
{
    class HorizontalScrollbar : UIElementHandler
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
        public int Value { get; set; }
        public float Sensitivity { get; set; }
        int UIElementHandler.LockedPos { get; set; }
        public int MinVal { get; set; }
        public int MaxVal { get; set; }

        public int LockedPos = 0;

        public HorizontalScrollbar(int X, int y, int width, int height, int Pos)
        {
            this.X = X;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Pos = Pos;
            this.TypeOfElement = TypeOfElement.HorizontalScrollbar;
        }
        public void Render(Bitmap canvas)
        {
            Pos = Math.Clamp(Pos, 20, Width - 40);
            ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(36, 36, 36), X, Y, Width, Height, false);
            if (Clicked == false)
            {
                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(50, 50, 50), X + Pos, Y + 2, 20, Height - 4, false);
            }
            else
            {
                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(100, 100, 100), X + Pos, Y + 2, 20, Height - 4, false);
            }
        }
        public bool CheckClick(int X, int Y)
        {
            if(MouseManager.MouseState == MouseState.Left)
            {
                if(X > X + Pos && X < X + Pos + 20 && Clicked == false)
                {
                    LockedPos = X - (X + Pos);
                    Clicked = true;
                }
                if(Clicked == true)
                {
                    Pos = X - X - LockedPos;
                }
                Pos = Math.Clamp(Pos, 20, Width - 40);
                return true;
            }
            else
            {
                if(Clicked == true)
                {
                    Clicked = false;
                    return true;
                }
                return false;
            }
        }
    }
}