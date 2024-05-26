using Cosmos.System.Graphics;
using Cosmos.System;
using System;

namespace CrystalOSAlpha.UI_Elements
{
    class VerticalScrollbar : UIElementHandler
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Pos { get; set; }
        public int MinVal { get; set; }
        public int MaxVal { get; set; }
        public int LockedPos { get; set; }
        public int Value { get; set; }
        public bool Clicked { get; set; }
        public string ID { get; set; }
        public int Color { get; set; }
        public string Text { get; set; }
        public TypeOfElement TypeOfElement { get; set; }

        public float Sensitivity { get; set; }
        public VerticalScrollbar(int x, int y, int width, int height, int Pos, float Sensitivity)
        {
            this.X = x;
            this.X = y;
            this.Width = width;
            this.Height = height;
            this.Pos = Pos;
            this.Sensitivity = Sensitivity;
            this.TypeOfElement = TypeOfElement.VerticalScrollbar;
        }
        public VerticalScrollbar(int x, int y, int width, int height, int Pos, int MinVal, int MaxVal, string ID)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Pos = Pos;
            this.MinVal = MinVal;
            this.MaxVal = MaxVal;
            this.Sensitivity = (float)(((float)MaxVal - (float)MinVal) / ((float)this.Height - 60.0));
            this.ID = ID;
            this.TypeOfElement = TypeOfElement.VerticalScrollbar;
        }
        public void Render(Bitmap canvas)
        {
            Pos = Math.Clamp(Pos, 20, Height - 40);
            ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(36, 36, 36), X, Y, Width, Height, false);
            if (Clicked == false)
            {
                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(50, 50, 50), X + 2, Y + Pos, Width - 4, 20, false);
            }
            else
            {
                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(100, 100, 100), X + 2, Y + Pos, Width - 4, 20, false);
            }
            Sensitivity = (float)(((float)MaxVal - (float)MinVal) / ((float)this.Height - 60.0));
            Value = (int)((Pos - 20) * Sensitivity);
        }
        public bool CheckClick(int X, int Y)
        {
            if (MouseManager.MouseState == MouseState.Left)
            {
                if ((Y > this.Y + Pos && Y < this.Y + Pos + 20) || Clicked == true)
                {
                    if (X > this.X && X < this.X + Width && Clicked == false)
                    {
                        LockedPos = Y - (this.Y + Pos);
                        Clicked = true;
                        return true;
                    }
                    if (Clicked == true)
                    {
                        if (Y - this.Y - LockedPos != Pos)
                        {
                            Pos = Y - this.Y - LockedPos;
                            Pos = Math.Clamp(Pos, 20, Height - 40);
                            return true;
                        }
                    }
                    return false;
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
