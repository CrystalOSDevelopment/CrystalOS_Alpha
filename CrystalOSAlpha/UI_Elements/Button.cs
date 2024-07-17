using Cosmos.System;
using Cosmos.System.Audio.IO;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System.Drawing;

namespace CrystalOSAlpha.UI_Elements
{
    class Button : UIElementHandler
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Text { get; set; }
        public int Color { get; set; }
        public bool Clickable { get; set; }
        public bool Clicked { get; set; }
        public string ID { get; set; }
        public TypeOfElement TypeOfElement { get; set; }
        public int Pos { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float Sensitivity { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int LockedPos { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int MinVal { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int MaxVal { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int SavedColor = 0;

        public Button(int X, int Y, int Width, int Height, string Text, int Color)
        {
            this.X = X;
            this.Y = Y;
            this.Width = Width;
            this.Height = Height;
            this.Text = Text;
            this.Color = Color;
            this.SavedColor = Color;
            this.TypeOfElement = TypeOfElement.Button;
        }
        public Button(int X, int Y, int Width, int Height, string Text, int Color, string ID)
        {
            this.X = X;
            this.Y = Y + 22;
            this.Width = Width;
            this.Height = Height;
            this.Text = Text;
            this.Color = Color;
            this.ID = ID;
            this.TypeOfElement = TypeOfElement.Button;
        }

        public static int offset = 0;
        public static Bitmap canvas_Blank;
        public void Render(Bitmap canvas)
        {
            canvas_Blank = new Bitmap((uint)Width, (uint)Height, ColorDepth.ColorDepth32);
            ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(36, 36, 36), X, Y, Width, Height, false);

            ImprovedVBE.DrawFilledRectangle(canvas, Color, X + 2, Y + 2, Width - 4, Height - 4, false);

            offset = BitFont.DrawBitFontString(canvas_Blank, "ArialCustomCharset16", System.Drawing.Color.White, Text, Width - (Text.Length * 6) - 3, Height / 2 - 8);//Used to center text
            BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", ComplimentaryColor.Generate(Color), Text, X + (Width / 2) - (offset / 2), Y + Height / 2 - (Text.Split('\n').Length * 9));
        }

        public bool CheckClick(int X, int Y)
        {
            int Left = (int)MouseManager.X - X - this.X;
            int Top = (int)MouseManager.Y - Y - this.Y;
            if (Top >= 0 && Top <= Height && Left >= 0 && Left <= Width)
            {
                if(MouseManager.MouseState == MouseState.Left)
                {
                    if(Clicked == false)
                    {
                        Color = ComplimentaryColor.Generate(Color).ToArgb();
                    }
                    Clicked = true;
                }
                else
                {
                    Color = SavedColor;
                    Clicked = false;
                }
            }
            else
            {
                Clicked = false;
            }
            return Clicked;
        }

        public void SetValue(int X, int Y, string Value, bool writeprotected)
        {
            throw new System.NotImplementedException();
        }

        public string GetValue(int X, int Y)
        {
            throw new System.NotImplementedException();
        }
    }

    class ComplimentaryColor
    {
        public static Color Generate(int rgbInteger)
        {
            int r = (rgbInteger >> 16) & 255;
            int g = (rgbInteger >> 8) & 255;
            int b = rgbInteger & 255;

            // calculate the opposite color
            int rOpp = 255 - r;
            int gOpp = 255 - g;
            int bOpp = 255 - b;

            if (rOpp == 0)
            {
                rOpp = 1;
            }
            if (gOpp == 0)
            {
                gOpp = 1;
            }
            if (bOpp == 0)
            {
                bOpp = 1;
            }
            return Color.FromArgb(rOpp, gOpp, bOpp);
        }
    }
}
