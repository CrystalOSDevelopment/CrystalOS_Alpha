using Cosmos.System.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.UI_Elements
{
    class PictureBox : UIElementHandler
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string ID { get; set; }
        public bool Visible = true;
        public Bitmap image {get; set;}
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Clicked { get; set; }
        public string Text { get; set; }
        public int Color { get; set; }
        public TypeOfElement TypeOfElement { get; set; }
        public int Pos { get; set; }
        public int Value { get; set; }
        public float Sensitivity { get; set; }
        public int LockedPos { get; set; }
        public int MinVal { get; set; }
        public int MaxVal { get; set; }
        public List<Point> Points { get; set; }

        public PictureBox(int X, int Y, string ID, bool Visible, Bitmap image)
        {
            this.X = X;
            this.Y = Y;
            this.Width = (int)image.Width;
            this.Height = (int)image.Height;
            this.ID = ID;
            this.Visible = Visible;
            this.image = image;
            this.TypeOfElement = TypeOfElement.PictureBox;
        }
        public void Render(Bitmap canvas)
        {
            if(Visible == true)
            {
                if(image.Width == canvas.Width && X == 0)
                {
                    Array.Copy(image.RawData, 0, canvas.RawData, canvas.Width * (22 + Y), image.RawData.Length);
                }
                else
                {
                    ImprovedVBE.DrawImage(image, X, Y + 22, canvas);
                }
            }
        }

        public bool CheckClick(int X, int Y)
        {
            throw new NotImplementedException();
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
