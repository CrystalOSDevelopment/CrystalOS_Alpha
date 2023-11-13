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
    class Button
    {
        public static int offset = 0;
        public static Bitmap canvas_Blank;
        public static Bitmap Button_render(Bitmap canvas, int X, int Y, int Width, int Height, int Color, string Text)
        {
            canvas_Blank = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(36, 36, 36), X, Y, Width, Height, false);

            ImprovedVBE.DrawFilledRectangle(canvas, Color, X + 2, Y + 2, Width - 4, Height - 4, false);
            if(Color == System.Drawing.Color.White.ToArgb())
            {
                //BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Global_integers.c, Text, X + (Width / 2) - (Text.Length), Y + Height / 2 - 9);
                offset = BitFont.DrawBitFontString(canvas_Blank, "ArialCustomCharset16", System.Drawing.Color.Black, Text, Width - (Text.Length * 6) - 3, Height / 2 - 8);
                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.Black, Text, X + (Width / 2) - (offset / 2), Y + Height / 2 - 9);
            }
            else
            {
                offset = BitFont.DrawBitFontString(canvas_Blank, "ArialCustomCharset16", System.Drawing.Color.White, Text, Width - (Text.Length * 6) - 3, Height / 2 - 8);
                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.White, Text, X + (Width / 2) - (offset / 2), Y + Height / 2 - 9);
            }
            return canvas;
        }
    }
    class Button_prop
    {

        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Text { get; set; }
        public int Color {get; set;}
        public bool Clickable { get; set;}
        public bool Clicked = false;
        public Button_prop(int X, int Y, int Width, int Height, string Text, int Color)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Text = Text;
            this.Color = Color;
        }
    }
}
