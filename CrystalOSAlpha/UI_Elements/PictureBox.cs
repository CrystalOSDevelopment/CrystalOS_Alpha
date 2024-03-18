using Cosmos.System.Graphics;
using System;

namespace CrystalOSAlpha.UI_Elements
{
    class PictureBox
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string ID { get; set; }
        public bool Visible = true;
        public Bitmap image {get; set;}
        public PictureBox(int X, int Y, string ID, bool Visible, Bitmap image)
        {
            this.X = X;
            this.Y = Y;
            this.ID = ID;
            this.Visible = Visible;
            this.image = image;
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
    }
}
