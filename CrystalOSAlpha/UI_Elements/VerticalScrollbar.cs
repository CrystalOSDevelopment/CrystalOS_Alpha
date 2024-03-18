using Cosmos.System.Graphics;
using Cosmos.System;
using System;

namespace CrystalOSAlpha.UI_Elements
{
    class VerticalScrollbar
    {
        public int x { get; set; }
        public int y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Pos { get; set; }
        public int LockedPos = 0;
        public int Value = 0;
        public bool Clicked { get; set; }
        public string ID { get; set; }
        public float Sensitivity { get; set; }
        public VerticalScrollbar(int x, int y, int width, int height, int Pos, float Sensitivity)
        {
            this.x = x;
            this.y = y;
            this.Width = width;
            this.Height = height;
            this.Pos = Pos;
            this.Sensitivity = Sensitivity;
        }
        public void Render(Bitmap canvas)
        {
            Pos = Math.Clamp(Pos, 20, Height - 40);
            ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(36, 36, 36), x, y, Width, Height, false);
            if (Clicked == false)
            {
                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(50, 50, 50), x + 2, y + Pos, Width - 4, 20, false);
            }
            else
            {
                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(100, 100, 100), x + 2, y + Pos, Width - 4, 20, false);
            }
            Value = (int)((Pos - 20) * Sensitivity);
        }
        public bool CheckClick(int X, int Y)
        {
            if (MouseManager.MouseState == MouseState.Left)
            {
                if((Y > y + Pos && Y < Pos + 20) || Clicked == true)
                {
                    if (X > x && X < x + Width && Clicked == false)
                    {
                        LockedPos = Y - (y + Pos);
                        Clicked = true;
                    }
                    if (Clicked == true)
                    {
                        Pos = Y - y - LockedPos;
                    }
                    Pos = Math.Clamp(Pos, 20, Height - 40);
                    return true;
                }
            }
            else
            {
                if (Clicked == true)
                {
                    Clicked = false;
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
