using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.TaskBar;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Graphics.Widgets
{
    class ImageViewer : App
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int desk_ID { get; set; }
        public int AppID { get; set; }
        public string name { get; set; }

        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }

        public bool Get_Back = true;
        public Bitmap Back;
        public int x_dif = 10;
        public int y_dif = 10;
        public bool mem = true;

        #region images
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Widgets.Elephant.bmp")] public static byte[] Elephant;
        public static Bitmap Nr1;
        #endregion images
        public static int sizeDec = 0;
        public void App()
        {
            if (Get_Back == true)
            {
                Back = Base.Widget_Back(200 - sizeDec, 200 - sizeDec, ImprovedVBE.colourToNumber(255, 255, 255));
                Back = ImprovedVBE.DrawImageAlpha2(Back, x, y, Back);
                Bitmap bmp = new Bitmap(Elephant);
                Nr1 = ImprovedVBE.ScaleImageStock(bmp, (uint)(175 - sizeDec), (uint)(150 - sizeDec));
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", System.Drawing.Color.White, "ImageViewer", 7, 2);
                ImprovedVBE.DrawImageAlpha(Nr1, (int)((100 - sizeDec / 2) - (Nr1.Width / 2)), 25, Back);

                width = (int)Back.Width;
                height = (int)Back.Height;
                Get_Back = false;
            }
            ImprovedVBE.DrawImageAlpha(Back, x, y, ImprovedVBE.cover);

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
                    if(x + Back.Width > ImprovedVBE.width - 200)
                    {
                        if(sizeDec < 40)
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
    }
}
