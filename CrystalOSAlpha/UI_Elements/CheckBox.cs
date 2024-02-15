using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.UI_Elements
{
    class CheckBox
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Value { get; set; }
        public bool Clicked { get; set; }
        public string ID { get; set; }
        public string Content { get; set; }
        public CheckBox(int x, int y, int width, int height, bool value, string iD, string content)
        {
            X = x;
            Y = y + 22;
            Width = width;
            Height = height;
            Value = value;
            ID = iD;
            Content = content;
        }

        public void Render(Bitmap Canvas)
        {
            if(Value == true)
            {
                ImprovedVBE.DrawFilledRectangle(Canvas, 1, X, Y, Width, Height, false);
                ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(133, 133, 133), X + 1, Y + 1, Width - 2, Height - 2, false);
                ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(48, 69, 255), X + 2, Y + 2, Width - 4, Height - 4, false);
            }
            else
            {
                ImprovedVBE.DrawFilledRectangle(Canvas, 1, X, Y, Width, Height, false);
                ImprovedVBE.DrawFilledRectangle(Canvas, ImprovedVBE.colourToNumber(133, 133, 133), X + 1, Y + 1, Width - 2, Height - 2, false);
            }
            BitFont.DrawBitFontString(Canvas, "ArialCustomCharset16", Color.White, Content, X + Width + 5, Y + (Height / 2) - 9);
        }

        public bool CheckForClick(int x, int y)
        {
            if (MouseManager.MouseState == MouseState.Left)
            {
                if (MouseManager.X > x + X && MouseManager.X < x + X + Width)
                {
                    if (MouseManager.Y > y + Y && MouseManager.Y < y + Y + Height)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
