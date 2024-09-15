using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.UI_Elements
{
    class label : UIElementHandler
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string Text { get; set; }
        public string ID { get; set; }
        public string FontType { get; set; }
        public bool Clicked { get; set; }
        public int Color { get; set; }
        public TypeOfElement TypeOfElement { get; set; }
        public int Pos { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public float Sensitivity { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int LockedPos { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int MinVal { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public int MaxVal { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public List<Point> Points { get; set; }

        public label(int X, int Y, string Text, int TextColor, string iD)
        {
            this.X = X;
            this.Y = Y;
            this.Text = Text;
            this.Color = TextColor;
            this.ID = iD;
            this.TypeOfElement = TypeOfElement.Label;
        }
        public label(int X, int Y, string Text, string fontType, int TextColor, string iD)
        {
            this.X = X;
            this.Y = Y;
            this.Text = Text;
            this.Color = TextColor;
            this.FontType = fontType;
            this.ID = iD;
            this.TypeOfElement = TypeOfElement.Label;
        }
        public void Render(Bitmap canvas)
        {
            if(FontType != null)
            {
                BitFont.DrawBitFontString(canvas, FontType, System.Drawing.Color.FromArgb(Color), Text, X, Y + 22);
            }
            else
            {
                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", System.Drawing.Color.FromArgb(Color), Text, X, Y + 22);
            }
        }

        public bool CheckClick(int X, int Y)
        {
            throw new System.NotImplementedException();
        }

        public void SetValue(int X, int Y, string Value, bool writeprotected)
        {
            throw new System.NotImplementedException();
        }

        string UIElementHandler.GetValue(int X, int Y)
        {
            throw new System.NotImplementedException();
        }
    }
}
