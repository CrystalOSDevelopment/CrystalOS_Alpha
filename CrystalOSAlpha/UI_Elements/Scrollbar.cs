using Cosmos.System.Graphics;

namespace CrystalOSAlpha.UI_Elements
{
    class Scrollbar
    {
        public static Bitmap Render(Bitmap canvas, Scrollbar_Values scv)
        {
            ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(36, 36, 36), scv.x, scv.y + 22, scv.Width, scv.Height, false);
            if(scv.Clicked == false)
            {
                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(50, 50, 50), scv.x + 2, scv.y + 42 + scv.Pos, scv.Width - 4, 20, false);
            }
            else
            {
                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(100, 100, 100), scv.x + 2, scv.y + 42 + scv.Pos, scv.Width - 4, 20, false);
            }

            return canvas;
        }
    }

    class Scrollbar_Values
    {
        public int x { get; set; }
        public int y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Pos { get; set; }
        public bool Clicked { get; set; }

        public Scrollbar_Values(int x, int y, int width, int height, int Pos)
        {
            this.x = x;
            this.y = y;
            this.Width = width;
            this.Height = height;
            this.Pos = Pos;
        }
        public Scrollbar_Values(int x, int y, int width, int height, int Pos, bool Clicked)
        {
            this.x = x;
            this.y = y;
            this.Width = width;
            this.Height = height;
            this.Pos = Pos;
            this.Clicked = Clicked;
        }
    }
}
