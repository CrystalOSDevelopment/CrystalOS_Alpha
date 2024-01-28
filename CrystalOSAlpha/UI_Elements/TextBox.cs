
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.UI_Elements
{
    public class TextBox
    {
        public TextBox(int X, int Y, int Width, int Height, int Color, string Text, string PlaceHolder, Options opt, string ID)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Color = Color;
            this.Text = Text;
            this.PlaceHolder = PlaceHolder;
            this.opt = opt;
            this.ID = ID;
        }
        public static Bitmap canvas;
        public static Bitmap canvas_Blank;
        public static int offset = 0;
        public enum Options
        {
            left,
            right,
            top,
            bottom
        }
        public Options opt { get; set; }
        public string PlaceHolder { get; set; }
        public string Text { get; set; }
        public int Color { get; set; }
        public int Height { get; set; }
        public int Width { get; set; }
        public int Y { get; set; }
        public int X { get; set; }
        public string ID { get; set; }
        public static Bitmap Box(Bitmap Canvas, int X, int Y, int Width, int Height, int Color, string Text, string PlaceHolder, Options opt)
        {
            canvas = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            canvas_Blank = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            Array.Fill(canvas.RawData, ImprovedVBE.colourToNumber(36, 36, 36));

            ImprovedVBE.DrawFilledRectangle(canvas, Color, 2, 2, Width - 4, Height - 4, false);

            switch (opt)
            {
                case Options.left:
                    if (Text == "")
                    {
                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.Gray, PlaceHolder, (Width / 2) - (PlaceHolder.Length * 4), Height / 2 - 8);
                    }
                    else
                    {
                        offset = 0;
                        for(int i = Text.Length; i > 0; i--)
                        {
                            offset += BitFont.DrawBitFontString(canvas_Blank, "ArialCustomCharset16", System.Drawing.Color.Black, Text[i].ToString(), 0, 0);
                            if(offset > Width - 15)
                            {
                                Text = Text.Remove(0, i);
                                break;
                            }
                        }
                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.White, Text, 5, Height / 2 - 8);
                    }
                    break;
                case Options.right:
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
                    break;
            }

            return ImprovedVBE.DrawImageAlpha(canvas, X, Y, Canvas);
        }
        public Bitmap Box(Bitmap Canvas, int X, int Y)
        {
            canvas = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            canvas_Blank = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            Array.Fill(canvas.RawData, ImprovedVBE.colourToNumber(36, 36, 36));

            ImprovedVBE.DrawFilledRectangle(canvas, Color, 2, 2, Width - 4, Height - 4, false);

            switch (opt)
            {
                case Options.left:
                    if (Text == "")
                    {
                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.Gray, PlaceHolder, (Width / 2) - (PlaceHolder.Length * 4), Height / 2 - 8);
                    }
                    else
                    {
                        offset = 0;
                        for (int i = Text.Length; i > 0; i--)
                        {
                            offset += BitFont.DrawBitFontString(canvas_Blank, "ArialCustomCharset16", System.Drawing.Color.Black, Text[i].ToString(), 0, 0);
                            if (offset > Width - 15)
                            {
                                Text = Text.Remove(0, i);
                                break;
                            }
                        }
                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.White, Text, 5, Height / 2 - 8);
                    }
                    break;
                case Options.right:
                    if (Text == "")
                    {
                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.Gray, PlaceHolder, (Width / 2) - (PlaceHolder.Length * 4), Height / 2 - 8);
                    }
                    else
                    {
                        if (Text.Length > 30)
                        {
                            Text = Text.Remove(0, Text.Length - 30);
                        }
                        offset = BitFont.DrawBitFontString(canvas_Blank, "ArialCustomCharset16", System.Drawing.Color.White, Text, Width - (Text.Length * 6) - 3, Height / 2 - 8);
                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.White, Text, Width - offset - 3, Height / 2 - 8);
                    }
                    break;
            }

            return ImprovedVBE.DrawImageAlpha(canvas, X, Y, Canvas);
        }
    }
}
