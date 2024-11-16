using Cosmos.System.Graphics;
using Cosmos.System;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Widgets;
using System.Drawing;

namespace CrystalOSAlpha.Graphics.TaskBar
{
    public class SideNav
    {
        public static int X = ImprovedVBE.width;
        public static int Y = 0;
        public static int start_y = 40;
        public static int KernelCycle = 1;

        public static bool Get_Back = true;
        public static bool RequestDrawLocal = false;

        public static Bitmap Back;
        public static string temp = "";

        public static void Core()
        {
            if(TaskScheduler.Apps.FindAll(d => d.minimised == false).Count == 5 || TaskScheduler.Apps.FindAll(d => d.minimised == false).Count == 3)
            {
                if(temp != GlobalValues.TaskBarType)
                {
                    Get_Back = true;
                    temp = GlobalValues.TaskBarType;
                }
                switch (GlobalValues.TaskBarType)
                {
                    case "Classic":
                        start_y = 40;
                        if (MouseManager.X > ImprovedVBE.width - 200 && MouseManager.X < ImprovedVBE.width)
                        {
                            if (MouseManager.Y > Y && MouseManager.Y < Y + Back.Height - 70)
                            {
                                if (X > ImprovedVBE.width - 200)
                                {
                                    X -= 5;
                                    ImprovedVBE.Clear(true);
                                }
                            }
                        }
                        else
                        {
                            if (X < ImprovedVBE.width)
                            {
                                X += 5;
                            }
                        }
                        if (Get_Back == true)
                        {
                            Back = Base.Widget_Back(200, ImprovedVBE.height - 1, ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B));
                            Back = ImprovedVBE.EnableTransparency(Back, X - 200, Y, Back);
                            BitFont.DrawBitFontString(Back, "VerdanaCustomCharset24", Color.White, "Widgets", 2, 5);
                            Get_Back = false;
                        }
                        if(X < ImprovedVBE.width - 2 && ImprovedVBE.RequestRedraw)
                        {
                            ImprovedVBE.DrawImageAlpha(Back, X, Y, ImprovedVBE.cover);
                        }
                        break;
                    case "Nostalgia":
                        start_y = 87;
                        if (MouseManager.X > ImprovedVBE.width - 200 && MouseManager.X < ImprovedVBE.width && TaskManager.calendar == false)
                        {
                            if (MouseManager.Y > Y + 47 && MouseManager.Y < Y + Back.Height)
                            {
                                if (X > ImprovedVBE.width - 200)
                                {
                                    X -= 5;
                                    ImprovedVBE.Clear(true);
                                }
                            }
                            else
                            {
                                if (X < ImprovedVBE.width)
                                {
                                    ImprovedVBE.Clear(true);
                                    X += 5;
                                }
                            }
                        }
                        else
                        {
                            if (X < ImprovedVBE.width)
                            {
                                ImprovedVBE.Clear(true);
                                X += 5;
                            }
                            else
                            {
                                RequestDrawLocal = false;
                            }
                        }
                        if (Get_Back == true)
                        {
                            Back = Base.Widget_Back(200, ImprovedVBE.height - 47, ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B));
                            Back = ImprovedVBE.EnableTransparency(Back, X - 200, Y + 47, Back);
                            BitFont.DrawBitFontString(Back, "VerdanaCustomCharset24", Color.White, "Widgets", 2, 5);
                            Get_Back = false;
                        }
                        if (X < ImprovedVBE.width - 2 && TaskManager.calendar == false && ImprovedVBE.RequestRedraw || RequestDrawLocal)
                        {
                            if(MouseManager.MouseState == MouseState.Left)
                            {
                                ImprovedVBE.DrawImageAlpha(Back, X, Y + 47, ImprovedVBE.cover);
                            }
                            else
                            {
                                if(KernelCycle % 3 == 0)
                                {
                                    KernelCycle = 1;
                                    RequestDrawLocal = false;
                                }
                                else
                                {
                                    ImprovedVBE.DrawImageAlpha(Back, X, Y + 47, ImprovedVBE.cover);
                                    KernelCycle++;
                                }
                            }
                            if(ImprovedVBE.RequestRedraw == true)
                            {
                                RequestDrawLocal = true;
                            }
                        }
                        break;
                }
            }
        }
    }
}
