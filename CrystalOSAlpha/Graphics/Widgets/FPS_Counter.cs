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
        public bool once { get; set; }

        public Bitmap icon { get; set; }
        public Bitmap window { get; set; }

        public int FPS = 0;
        public int LastS = -1;
        public int Ticken = 0;
        public int x_dif = 10;
        public int y_dif = 10;
        public static int sizeDec = 0;

        public bool Get_Back = true;
        public bool mem = true;
        
        public Bitmap BackBuffer;
        public Bitmap Back;

        public void App()
        {
            string output = "FPS: " + FPS.ToString();
            if(Get_Back == true)
            {
                ImprovedVBE.Clear(true);
                if(x >= ImprovedVBE.width)
                {
                    sizeDec = 40;
                }
                if(BackBuffer == null)
                {
                    BackBuffer = Base.Widget_Back(200 - sizeDec, 200 - sizeDec, ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B));
                    Back = Base.Widget_Back(200 - sizeDec, 200 - sizeDec, ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B));
                    BackBuffer = ImprovedVBE.EnableTransparency(BackBuffer, x, y, BackBuffer);
                    Back = ImprovedVBE.EnableTransparency(Back, x, y, Back);
                }
                else
                {
                    Array.Copy(BackBuffer.RawData, Back.RawData, Back.RawData.Length);
                }
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", GlobalValues.c, output, ((100 - sizeDec / 2) - output.Length * 4), (int)(Back.Height / 2 - 8));//92
                Heap.Collect();
                ImprovedVBE.DrawImageAlpha(Back, x, y, ImprovedVBE.cover);
                ImprovedVBE.RequestRedraw = true;
                Get_Back = false;
            }
            else if (ImprovedVBE.RequestRedraw || SideNav.RequestDrawLocal == true)
            {
                ImprovedVBE.DrawImageAlpha(Back, x, y, ImprovedVBE.cover);
            }

            if (LastS == -1)
            {
                LastS = DateTime.UtcNow.Second;
            }
            if(Ticken % 50 == 0)
            {
                Heap.Collect();
            }
            if (DateTime.UtcNow.Second != LastS)
            {
                if (DateTime.UtcNow.Second > LastS)
                {
                    FPS = Ticken;
                    Get_Back = true;
                }
                LastS = DateTime.UtcNow.Second;
                Ticken = 0;
            }
            Ticken++;

            if (MouseManager.MouseState == MouseState.Left)
            {
                if (((MouseManager.X > x && MouseManager.X < x + Back.Width) && (MouseManager.Y > y && MouseManager.Y < y + Back.Height)))
                {
                    if (mem == false)
                    {
                        x_dif = (int)MouseManager.X - x;
                        y_dif = (int)MouseManager.Y - y;
                        mem = true;
                    }
                    x = (int)MouseManager.X - x_dif;
                    y = (int)MouseManager.Y - y_dif;
                    if (x + Back.Width > ImprovedVBE.width - 200)
                    {
                        if (sizeDec < 40)
                        {
                            Back = ImprovedVBE.ScaleImageStock(Back, (uint)(Back.Width - sizeDec), (uint)(Back.Height - sizeDec));
                            sizeDec += 10;
                            BackBuffer = null;
                            Get_Back = true;
                        }
                    }
                    else
                    {
                        if (sizeDec > 0)
                        {
                            Back = ImprovedVBE.ScaleImageStock(Back, (uint)(Back.Width - sizeDec), (uint)(Back.Height - sizeDec));
                            sizeDec -= 10;
                            BackBuffer = null;
                            Get_Back = true;
                        }
                    }
                }
                else
                {
                    if (x + Back.Width > ImprovedVBE.width - 200)
                    {
                        x = SideNav.X + 15;
                    }
                }
                if (mem == true)
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
                    y = (int)(TaskScheduler.Apps.IndexOf(this) * (Back.Height + 20) + 80);
                }
                if (mem == true)
                {
                    mem = false;
                    BackBuffer = null;
                    Get_Back = true;
                }
            }
        }

        public void RightClick()
        {

        }
    }
}
