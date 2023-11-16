using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Graphics.Widgets
{
    class ImageViewer : WidgetHandling
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public bool Get_Back = true;
        public Bitmap Back;
        public int x_dif = 10;
        public int y_dif = 10;
        public bool mem = true;

        #region images
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Widgets.Elephant.bmp")] public static byte[] Elephant;
        public static Bitmap Nr1;
        #endregion images
        public void Core()
        {
            if (Get_Back == true)
            {
                Back = Base.Widget_Back(200, 200, ImprovedVBE.colourToNumber(255, 255, 255));
                Back = ImprovedVBE.DrawImageAlpha2(Back, X, Y, Back);
                Bitmap bmp = new Bitmap(Elephant);
                Nr1 = ImprovedVBE.ScaleImageStock(bmp, 175, 150);
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", System.Drawing.Color.White, "ImageViewer", 7, 2);
                Get_Back = false;
            }
            ImprovedVBE.DrawImageAlpha(Back, X, Y, ImprovedVBE.cover);
            ImprovedVBE.DrawImage(Nr1, (int)(X + (100 - (Nr1.Width / 2))), Y + 25);

            if (MouseManager.MouseState == MouseState.Left)
            {
                if (MouseManager.X > X && MouseManager.X < X + Back.Width)
                {
                    if (MouseManager.Y > Y && MouseManager.Y < Y + Back.Height)
                    {
                        if (mem == true)
                        {
                            x_dif = (int)MouseManager.X - X;
                            y_dif = (int)MouseManager.Y - Y;
                            mem = false;
                        }
                        X = (int)MouseManager.X - x_dif;
                        Y = (int)MouseManager.Y - y_dif;
                    }
                }
                if (mem == false)
                {
                    X = (int)MouseManager.X - x_dif;
                    Y = (int)MouseManager.Y - y_dif;
                }
            }
            if (mem == false && MouseManager.MouseState == MouseState.None)
            {
                mem = true;
                Get_Back = true;
            }
        }
    }
}
