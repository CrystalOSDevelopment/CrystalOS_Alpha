using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Widgets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOS_Alpha.Graphics.Widgets
{
    public class FPS_Counter : WidgetHandling
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }
        public int FPS = 0;

        public int LastS = -1;
        public int Ticken = 0;
        public bool Get_Back = true;
        public Bitmap Back;

        public int x_dif = 10;
        public int y_dif = 10;
        public bool mem = true;

        public void Core()
        {
            string output = "FPS: " + FPS.ToString();
            if(Get_Back == true)
            {
                Back = Base.Widget_Back(200, 200, ImprovedVBE.colourToNumber(255, 255, 255));
                Back = ImprovedVBE.DrawImageAlpha2(Back, X, Y, Back);
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Global_integers.c, output, (100 - output.Length * 4), 92);
                Get_Back = false;
            }
            ImprovedVBE.DrawImageAlpha(Back, X, Y, ImprovedVBE.cover);

            if (LastS == -1)
            {
                LastS = DateTime.UtcNow.Second;
            }
            if (DateTime.UtcNow.Second - LastS != 0)
            {
                if (DateTime.UtcNow.Second > LastS)
                {
                    FPS = Ticken / (DateTime.UtcNow.Second - LastS);
                    Get_Back = true;
                }
                LastS = DateTime.UtcNow.Second;
                Ticken = 0;
            }
            Ticken++;

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
