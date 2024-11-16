using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.UI_Elements
{
    class TextBox : UIElementHandler
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
            this.TypeOfElement = TypeOfElement.TextBox;
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
        public bool Clicked { get; set; }
        public TypeOfElement TypeOfElement { get; set; }
        public int Pos { get; set; }
        public int Value { get; set; }
        public float Sensitivity { get; set; }
        public int LockedPos { get; set; }
        public int MinVal { get; set; }
        public int MaxVal { get; set; }
        public List<Point> Points { get; set; }

        public void Render(Bitmap Canvas)
        {
            canvas = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            canvas_Blank = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            Array.Fill(canvas.RawData, ImprovedVBE.colourToNumber(36, 36, 36));

            ImprovedVBE.DrawFilledRectangle(canvas, Color, 2, 2, Width - 4, Height - 4, false);

            string displayText = Text;  // Use displayText for rendering, keeping Text unchanged

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
                            offset += BitFont.DrawBitFontString(canvas_Blank, "ArialCustomCharset16", System.Drawing.Color.Black, Text[i - 1].ToString(), 0, 0);
                            if (offset > Width - 15)
                            {
                                displayText = Text.Substring(i - 1);  // Adjust displayText instead of modifying Text
                                break;
                            }
                        }
                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", ComplimentaryColor.Generate(Color), displayText, 5, Height / 2 - 8);
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
                            displayText = Text.Substring(Text.Length - 30);  // Trim only for display, keep full Text
                        }
                        offset = BitFont.DrawBitFontString(canvas_Blank, "ArialCustomCharset16", System.Drawing.Color.White, displayText, Width - (displayText.Length * 6) - 3, Height / 2 - 8);
                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", ComplimentaryColor.Generate(Color), displayText, Width - offset - 3, Height / 2 - 8);
                    }
                    break;
            }

            ImprovedVBE.DrawImage(canvas, X, Y, Canvas);
        }


        public bool CheckClick(int X, int Y)
        {
            if(MouseManager.MouseState == MouseState.Left)
            {
                if(MouseManager.X > this.X + X && MouseManager.X < this.X + X + Width)
                {
                    if(MouseManager.Y > this.Y + Y && MouseManager.Y < this.Y + Y + Height)
                    {
                        Clicked = true;
                        return true;
                    }
                    Clicked = false;
                }
                Clicked = false;
            }
            return false;
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
