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

        public HorizontalScrollbar(int X, int y, int width, int height, int Pos, int MinVal, int MaxVal, string ID)
        {
            this.X = X;
            this.Y = y;
            this.Width = width;
            this.Height = height;
            this.Pos = Pos;
            this.MinVal = MinVal;
            this.MaxVal = MaxVal;
            this.Sensitivity = (float)(((float)MaxVal - (float)MinVal) / ((float)this.Width - 60.0));
            this.ID = ID;
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
            Sensitivity = (float)(((float)MaxVal - (float)MinVal) / ((float)this.Width - 60.0));
            Value = (int)((Pos - 20) * Sensitivity);
        }
        public bool CheckClick(int X, int Y)
        {
            if (MouseManager.MouseState == MouseState.Left)
            {
                // Check if the mouse is over the scrollbar handle
                if (X > this.X + Pos && X < this.X + Pos + 20 && Y > this.Y && Y < this.Y + Height)
                {
                    if (Clicked == false)
                    {
                        LockedPos = X - (this.X + Pos);
                        Clicked = true;
                    }
                    Pos = X - this.X - LockedPos;
                }

                // Adjust the position and clamp it within bounds
                Pos = Math.Clamp(Pos, 20, Width - 40);

                return Clicked;
            }
            else
            {
                // Release the click when the mouse is not pressed
                if (Clicked == true)
                {
                    Clicked = false;
                    return true;
                }
                return Clicked;
            }
        }


        public void SetValue(int X, int Y, string Value, bool writeprotected)
        {
            throw new NotImplementedException();
        }

        public string GetValue(int X, int Y)
        {
            throw new NotImplementedException();
        }
    }
}