
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.UI_Elements
{
    public class TextBox
    {
        public static Bitmap canvas;
        public static Bitmap canvas_Blank;
        public static int offset = 0;
        public static Bitmap Box(Bitmap Canvas, int X, int Y, int Width, int Height, int Color, string Text, string PlaceHolder)
        {
            canvas = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            canvas_Blank = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            Array.Fill(canvas.RawData, ImprovedVBE.colourToNumber(36, 36, 36));

            ImprovedVBE.DrawFilledRectangle(canvas, Color, 2, 2, Width - 4, Height - 4, false);

            if(Text == "")
            {
                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.Gray, PlaceHolder, (Width / 2) - (PlaceHolder.Length * 4), Height / 2 - 8);
            }
            else
            {
                if(Text.Length > 30)
                {
                    Text = Text.Remove(0, Text.Length - 30);
                }
                offset = BitFont.DrawBitFontString(canvas_Blank, "ArialCustomCharset16", System.Drawing.Color.White, Text, Width - (Text.Length * 6) - 3, Height / 2 - 8);
                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.White, Text, Width - offset - 3, Height / 2 - 8);
            }
            return ImprovedVBE.DrawImageAlpha(canvas, X, Y, Canvas);
        }
    }
}
