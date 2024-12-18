﻿using Cosmos.Core.Memory;
using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.TaskBar;
using IL2CPU.API.Attribs;
using System;

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
        public bool once { get; set; }
        public Bitmap icon { get; set; }
        public Bitmap window { get; set; }

        public int x_dif = 10;
        public int y_dif = 10;
        public static int sizeDec = 0;

        public bool Get_Back = true;
        public bool mem = true;
        
        public Bitmap Back;

        #region images
        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.Widgets.Elephant.bmp")] public static byte[] Elephant;
        public static Bitmap Nr1;
        #endregion images
        public void App()
        {
            switch (Get_Back)
            {
                case true:
                    if (x >= ImprovedVBE.width)
                    {
                        sizeDec = 40;
                    }
                    Back = Base.Widget_Back(200 - sizeDec, 200 - sizeDec, ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B));
                    Back = ImprovedVBE.EnableTransparency(Back, x, y, Back);
                    Bitmap bmp = new Bitmap(Elephant);
                    Nr1 = ImprovedVBE.ScaleImageStock(bmp, (uint)(175 - sizeDec), (uint)(150 - sizeDec));
                    BitFont.DrawBitFontString(Back, "ArialCustomCharset16", System.Drawing.Color.White, "ImageViewer", 7, 2);
                    ImprovedVBE.DrawImageAlpha(Nr1, (int)((100 - sizeDec / 2) - (Nr1.Width / 2)), 25, Back);

                    width = (int)Back.Width;
                    height = (int)Back.Height;
                    Get_Back = false;
                    break;
                case false:
                    switch (ImprovedVBE.RequestRedraw || SideNav.RequestDrawLocal == true)
                    {
                        case true:
                            ImprovedVBE.DrawImageAlpha(Back, x, y, ImprovedVBE.cover);
                            break;
                    }
                    break;
            }
            //ImprovedVBE.DrawImageAlpha(Back, x, y, ImprovedVBE.cover);

            switch(MouseManager.MouseState)
            {
                case MouseState.Left:
                    if ((MouseManager.X > x && MouseManager.X < x + Back.Width) && (MouseManager.Y > y && MouseManager.Y < y + Back.Height))
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
                        }
                    }
                    if (mem == true)
                    {
                        x = (int)MouseManager.X - x_dif;
                        y = (int)MouseManager.Y - y_dif;
                    }
                    break;
                default:
                    if (x + Back.Width > ImprovedVBE.width - 200)
                    {
                        x = SideNav.X + 15;
                        y = (int)(TaskScheduler.Apps.IndexOf(this) * (Back.Height + 20) + 80);
                    }
                    if (mem == true)
                    {
                        mem = false;
                        Get_Back = true;
                    }
                    break;
            }
        }

        public void RightClick()
        {

        }
    }
}
