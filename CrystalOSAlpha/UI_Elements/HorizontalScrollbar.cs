using Cosmos.System;
using Cosmos.System.Graphics;
using System;

namespace CrystalOSAlpha.UI_Elements
{
    class HorizontalScrollbar
    {
        public int x { get; set; }
        public int y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Pos { get; set; }
        public bool Clicked { get; set; }
        public int LockedPos = 0;

        public HorizontalScrollbar(int x, int y, int width, int height, int Pos)
        {
            this.x = x;
            this.y = y;
            this.Width = width;
            this.Height = height;
            this.Pos = Pos;
        }
        public HorizontalScrollbar(int x, int y, int width, int height, int Pos, bool Clicked)
        {
            this.x = x;
            this.y = y;
            this.Width = width;
            this.Height = height;
            this.Pos = Pos;
            this.Clicked = Clicked;
        }
        public void Render(Bitmap canvas)
        {
            Pos = Math.Clamp(Pos, 20, Width - 40);
            ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(36, 36, 36), x, y, Width, Height, false);
            if (Clicked == false)
            {
                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(50, 50, 50), x + Pos, y + 2, 20, Height - 4, false);
            }
            else
            {
                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(100, 100, 100), x + Pos, y + 2, 20, Height - 4, false);
            }
        }
        public bool CheckClick(int X, int Y)
        {
            if(MouseManager.MouseState == MouseState.Left)
            {
                if(X > x + Pos && X < x + Pos + 20 && Clicked == false)
                {
                    LockedPos = X - (x + Pos);
                    Clicked = true;
                }
                if(Clicked == true)
                {
                    Pos = X - x - LockedPos;
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