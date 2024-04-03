using Cosmos.Core.Memory;
using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Graphics.Widgets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOS_Alpha.Graphics.Widgets
{
    public class FPS_Counter : App
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int desk_ID {get; set;}

        public int AppID { get; set; }

        public string name {get; set;}

        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }

        public int FPS = 0;
        public int LastS = -1;
        public int Ticken = 0;
        public int x_dif = 10;
        public int y_dif = 10;
        public static int sizeDec = 0;

        public bool Get_Back = true;
        public bool mem = true;
        
        public Bitmap Back;

        public void App()
        {
            string output = "FPS: " + FPS.ToString();
            if(Get_Back == true)
            {
                Back = Base.Widget_Back(200 - sizeDec, 200 - sizeDec, ImprovedVBE.colourToNumber(255, 255, 255));
                Back = ImprovedVBE.EnableTransparency(Back, x, y, Back);
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Global_integers.c, output, ((100 - sizeDec / 2) - output.Length * 4), (int)(Back.Height / 2 - 8));//92
                Heap.Collect();
                Get_Back = false;
            }
            ImprovedVBE.DrawImageAlpha(Back, x, y, ImprovedVBE.cover);

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
                if (((MouseManager.X > x && MouseManager.X < x + Back.Width) && (MouseManager.Y > y && MouseManager.Y < y + Back.Height)) || mem == false)
                {
                    if (mem == true)
                    {
                        x_dif = (int)MouseManager.X - x;
                        y_dif = (int)MouseManager.Y - y;
                        mem = false;
                    }
                    x = (int)MouseManager.X - x_dif;
                    y = (int)MouseManager.Y - y_dif;
                    if (x + Back.Width > ImprovedVBE.width - 200)
                    {
                        if (sizeDec < 40)
                        {
                            Back = ImprovedVBE.ScaleImageStock(Back, (uint)(Back.Width - sizeDec), (uint)(Back.Height - sizeDec));
                            sizeDec += 10;
                            Get_Back = true;
                        }
                    }
                    else
                    {
                        if (sizeDec > 0)
                        {
                            Back = ImprovedVBE.ScaleImageStock(Back, (uint)(Back.Width - sizeDec), (uint)(Back.Height - sizeDec));
                            sizeDec -= 10;
                            Get_Back = true;
                        }
                    }
                }
                else
                {
                    if (x + Back.Width > ImprovedVBE.width - 200)
                    {
                        x = SideNav.X + 15;
                        y = SideNav.start_y;
                    }
                }
                SideNav.start_y += (int)Back.Height + 20;
                if (mem == false)
                {
                    x = (int)MouseManager.X - x_dif;
                    y = (int)MouseManager.Y - y_dif;
                }
            }
            else
            {
                if (x + Back.Width > ImprovedVBE.width - 200)
                {
                    x = SideNav.X + 15;
                    y = SideNav.start_y;
                    SideNav.start_y += (int)Back.Height + 20;
                }
            }
            if (mem == false && MouseManager.MouseState == MouseState.None)
            {
                mem = true;
                Get_Back = true;
            }
        }

        public void RightClick()
        {

        }
    }
}
