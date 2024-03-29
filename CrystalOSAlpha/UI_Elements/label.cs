﻿using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System.Drawing;

namespace CrystalOSAlpha.UI_Elements
{
    class label
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Text { get; set; }
        public int TextColor { get; set; }
        public string ID { get; set; }
        public string FontType { get; set; }
        public label(int X, int Y, string Text, int TextColor, string iD)
        {
            this.X = X;
            this.Y = Y;
            this.Text = Text;
            this.TextColor = TextColor;
            ID = iD;
        }
        public label(int X, int Y, string Text, string fontType, int TextColor, string iD)
        {
            this.X = X;
            this.Y = Y;
            this.Text = Text;
            this.TextColor = TextColor;
            this.FontType = fontType;
            ID = iD;
        }
        public void Label(Bitmap canvas)
        {
            if(FontType != null)
            {
                BitFont.DrawBitFontString(canvas, FontType, Color.FromArgb(TextColor), Text, X, Y + 22);
            }
            else
            {
                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.FromArgb(TextColor), Text, X, Y + 22);
            }
        }
    }
}
