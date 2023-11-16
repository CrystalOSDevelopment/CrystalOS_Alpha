using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications.Calculator;
using CrystalOSAlpha.Applications.Minecraft;
using CrystalOSAlpha.Applications.Settings;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.Widgets;
using IL2CPU.API.Attribs;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Graphics.TaskBar
{
    class TaskManager
    {
        public static int Top = ImprovedVBE.height;
        public static int Left = ImprovedVBE.width / 2 + 60;

        public static int height = 50;
        public static int width = 50;

        public static double X_offset = 10;

        public static bool resize = true;

        public static bool MenuOpened = false;

        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.TaskBar.Icon.bmp")] public static byte[] Elephant;
        public static Bitmap icon;
        public static Bitmap Back;
        public static Bitmap TaskBar;
        public static Bitmap Extension_Dock;

        public static Bitmap Search_Box;

        public static List<Menu_Items> Items = new List<Menu_Items>();
        public static bool Triggered = false;
        public static string DayOfWeek = "";

        public static bool update = true;
        public static bool calendar = false;

        public static int Time = 0;
        public static void Main()
        {
            if(resize == true)
            {
                icon = ImprovedVBE.ScaleImageStock(new Bitmap(Elephant), 36, 36);
                Back = Base.Widget_Back(420, 470, ImprovedVBE.colourToNumber(255, 255, 255));
                Back = ImprovedVBE.DrawImageAlpha2(Back, (int)(Left + X_offset - 300), Top - 480, Back);

                TaskBar = Base.Widget_Back((int)(X_offset * 2), 50, ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B));
                TaskBar = ImprovedVBE.DrawImageAlpha2(TaskBar, (int)(Left - X_offset), Top, TaskBar);

                Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                Extension_Dock = ImprovedVBE.DrawImageAlpha2(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock);

                Search_Box = Base.Widget_Back(210, 36, ImprovedVBE.colourToNumber(255, 255, 255));
                Search_Box = ImprovedVBE.DrawImageAlpha2(Search_Box, (int)(Left + X_offset - 200), Top - 460, Search_Box, 40, 40, 40);

                switch (DateTime.Now.DayOfWeek)
                {
                    case System.DayOfWeek.Monday:
                        DayOfWeek = "Monday";
                        break;
                    case System.DayOfWeek.Tuesday:
                        DayOfWeek = "Tuesday";
                        break;
                    case System.DayOfWeek.Wednesday:
                        DayOfWeek = "Wednesday";
                        break;
                    case System.DayOfWeek.Thursday:
                        DayOfWeek = "Thursday";
                        break;
                    case System.DayOfWeek.Friday:
                        DayOfWeek = "Friday";
                        break;
                    case System.DayOfWeek.Saturday:
                        DayOfWeek = "Saturday";
                        break;
                    case System.DayOfWeek.Sunday:
                        DayOfWeek = "Sunday";
                        break;
                }

                BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, 60, 5);
                BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DayOfWeek + "," + DateTime.UtcNow.Day, (int)(Extension_Dock.Width / 2 - (DayOfWeek + "," + DateTime.UtcNow.Day).Length * 4), 20);
                resize = false;
            }
            if(MenuOpened == false && calendar == false)
            {
                if(MouseManager.X > Left - ((ImprovedVBE.width / 192 * 1.3) * 35) - 175 && MouseManager.X < Left + ((ImprovedVBE.width / 192 * 1.3) * 35) && MouseManager.Y > ImprovedVBE.height - 70 && MouseManager.Y < ImprovedVBE.height)
                {
                    if(X_offset < ImprovedVBE.width / 192 * 1.3 * 35) //ImprovedVBE.height - 70
                    {
                        Top -= 2;
                        X_offset += ImprovedVBE.width / 192 * 1.3;//10

                        TaskBar = Base.Widget_Back((int)(X_offset * 2), 50, ImprovedVBE.colourToNumber(255, 255, 255));
                        TaskBar = ImprovedVBE.DrawImageAlpha2(TaskBar, (int)(Left - X_offset), Top, TaskBar);

                        Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                        Extension_Dock = ImprovedVBE.DrawImageAlpha2(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock);

                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, 60, 5);
                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DayOfWeek + "," + DateTime.UtcNow.Day, (int)(Extension_Dock.Width / 2 - (DayOfWeek + "," + DateTime.UtcNow.Day).Length * 4), 20);//44
                        /*
                        ImprovedVBE.DrawFilledRectangle(ImprovedVBE.colourToNumber(255, 255, 255), Left - X_offset, Top, X_offset * 2, height, true);
                        ImprovedVBE.DrawFilledEllipse(Left - X_offset, Top + height / 2 - 1, height / 2, height / 2, ImprovedVBE.colourToNumber(255, 255, 255));
                        ImprovedVBE.DrawFilledEllipse(Left+ X_offset, Top + height / 2 - 1, height / 2, height / 2, ImprovedVBE.colourToNumber(255, 255, 255));
                        */

                    }
                }
                else
                {
                    if (X_offset > 10)
                    {
                        Top += 2;
                        X_offset -= ImprovedVBE.width / 192 * 1.3;

                        TaskBar = Base.Widget_Back((int)(X_offset * 2), 50, ImprovedVBE.colourToNumber(255, 255, 255));
                        TaskBar = ImprovedVBE.DrawImageAlpha2(TaskBar, (int)(Left - X_offset), Top, TaskBar);

                        Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                        Extension_Dock = ImprovedVBE.DrawImageAlpha2(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock);

                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, 60, 5);
                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DayOfWeek + "," + DateTime.UtcNow.Day, (int)(Extension_Dock.Width / 2 - (DayOfWeek + "," + DateTime.UtcNow.Day).Length * 4), 20);

                        /*
                        ImprovedVBE.DrawFilledRectangle(ImprovedVBE.colourToNumber(255, 255, 255), Left - X_offset, Top, X_offset * 2, height, true);
                        ImprovedVBE.DrawFilledEllipse(Left - X_offset, Top + height / 2 - 1, height / 2, height / 2, ImprovedVBE.colourToNumber(255, 255, 255));
                        ImprovedVBE.DrawFilledEllipse(Left + X_offset, Top + height / 2 - 1, height / 2, height / 2, ImprovedVBE.colourToNumber(255, 255, 255));
                        */
                    }
                }
                if (Triggered == true && MouseManager.MouseState == MouseState.None)
                {
                    Triggered = false;
                }
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if(MouseManager.X > Left + X_offset - 38 && MouseManager.X < Left + X_offset)
                    {
                        if(MouseManager.Y > Top && MouseManager.Y < ImprovedVBE.height - 10)
                        {
                            update = true;
                            if (Triggered == false)
                            {
                                Items.Clear();

                                Menu_Items m = new Menu_Items();
                                m.Name = "Mine...";
                                m.Source = "Minecraft";
                                m.Icon = ImprovedVBE.ScaleImageStock(ImageViewer.Nr1, 56, 56);
                                Items.Add(m);

                                m.Name = "Calc...";
                                m.Source = "Calculator";
                                m.Icon = ImprovedVBE.ScaleImageStock(Resources.Calculator, 56, 56);
                                Items.Add(m);

                                m.Name = "Sett...";
                                m.Source = "Settings";
                                m.Icon = ImprovedVBE.ScaleImageStock(Resources.Settings, 56, 56);
                                Items.Add(m);

                                calendar = false;
                                MenuOpened = true;
                                resize = true;
                                Triggered = true;
                            }
                        }
                    }
                    if (MouseManager.X > Left - X_offset - 175 && MouseManager.X < Left - X_offset - 20)
                    {
                        if (MouseManager.Y > Top && MouseManager.Y < ImprovedVBE.height - 10)
                        {
                            update = true;
                            if (Triggered == false)
                            {
                                MenuOpened = false;
                                calendar = true;
                                Triggered = true;
                            }
                        }
                    }
                }
            }
            else
            {
                if (Triggered == true && MouseManager.MouseState == MouseState.None)
                {
                    Triggered = false;
                }
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > Left && MouseManager.X < Left + X_offset)
                    {
                        if (MouseManager.Y > Top && MouseManager.Y < ImprovedVBE.height - 10)
                        {
                           if(Triggered == false)
                           {
                                Items.Clear();
                                if(MenuOpened == false)
                                {
                                    MenuOpened = true;
                                    calendar = false;
                                }
                                else
                                {
                                    MenuOpened = false;
                                }
                                Triggered = true;
                           }
                        }
                    }
                    else if (MouseManager.X > Left - X_offset - 175 && MouseManager.X < Left - X_offset - 20)
                    {
                        if (MouseManager.Y > Top && MouseManager.Y < ImprovedVBE.height - 10)
                        {
                            update = true;
                            if (Triggered == false)
                            {
                                if(calendar == false)
                                {
                                    calendar = true;
                                    MenuOpened = false;
                                }
                                else
                                {
                                    calendar = false;
                                }
                                Triggered = true;
                            }
                        }
                    }
                    else
                    {
                        //MenuOpened = false;
                        //calendar = false;
                    }
                }
                if(MenuOpened == true)
                {
                    Menu_Manager();
                }
                else if(calendar == true)
                {
                    Calendar.Calendar_Widget((int)(Left - X_offset) - 275, Top - 330);
                }
            }


            ImprovedVBE.DrawImageAlpha(TaskBar, (int)(Left - X_offset), Top, ImprovedVBE.cover);

            if(Top < ImprovedVBE.height - 10)
            {
                TaskScheduler.Render_Icons();
            }

            if (Time != DateTime.UtcNow.Minute)
            {
                Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                Extension_Dock = ImprovedVBE.DrawImageAlpha2(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock);
                BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, 60, 5);
                BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DayOfWeek + "," + DateTime.UtcNow.Day, (int)(Extension_Dock.Width / 2 - (DayOfWeek + "," + DateTime.UtcNow.Day).Length * 4), 20);

                Time = DateTime.UtcNow.Minute;
            }

            ImprovedVBE.DrawImageAlpha(Extension_Dock, (int)(Left - X_offset) - 175, Top, ImprovedVBE.cover);
            
            /*
            ImprovedVBE.DrawFilledRectangle(ImprovedVBE.colourToNumber(255, 255, 255), Left - X_offset, Top, X_offset * 2, height, false);
            ImprovedVBE.DrawFilledEllipse(Left - X_offset, Top + height / 2 - 1, height / 2, height / 2, ImprovedVBE.colourToNumber(255, 255, 255));
            ImprovedVBE.DrawFilledEllipse(Left + X_offset, Top + height / 2 - 1, height / 2, height / 2, ImprovedVBE.colourToNumber(255, 255, 255));
            */
            ImprovedVBE.DrawImageAlpha(icon, (int)(Left + X_offset - 41), Top + height / 2 - 18, ImprovedVBE.cover);
        }

        public static void Menu_Manager()
        {
            ImprovedVBE.DrawImageAlpha(Search_Box, 100, 20, Back);
            ImprovedVBE.DrawImageAlpha(Back, (int)(Left + X_offset - 300), Top - 480, ImprovedVBE.cover);
            //ImprovedVBE.DrawImageAlpha(Search_Box, Left + X_offset - 200, Top - 460, ImprovedVBE.cover);
            //ImprovedVBE._DrawACSIIString("Type to search!", Left + X_offset - 190, Top - 450, ImprovedVBE.colourToNumber(0, 0, 0));
            if(update == true)
            {
                BitFont.DrawBitFontString(Search_Box, "ArialCustomCharset16", Global_integers.c, "Type to search!", 5, 9);
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Global_integers.c, "Recently opened", 10, 62);
            }
            int x = (int)(Left + X_offset - 290);
            int y = Top - 370;//358
            int co = 0;
            foreach(Menu_Items m in Items)
            {
                if(MouseManager.MouseState == MouseState.Left)
                {
                    if(MouseManager.X > x && MouseManager.X < x + 70)
                    {
                        if (MouseManager.Y > y && MouseManager.Y < y + 50)
                        {
                            if(m.Source == "Minecraft")
                            {
                                Bitmap layer = new Bitmap(140, 140, ColorDepth.ColorDepth32);
                                Array.Fill(layer.RawData, 1);
                                Bitmap layer2 = new Bitmap(140, 140, ColorDepth.ColorDepth32);
                                Bitmap layer3 = new Bitmap(140, 140, ColorDepth.ColorDepth32);
                                Bitmap layer4 = new Bitmap(140, 140, ColorDepth.ColorDepth32);
                                Bitmap layer5 = new Bitmap(140, 140, ColorDepth.ColorDepth32);
                                Bitmap layer6 = new Bitmap(140, 140, ColorDepth.ColorDepth32);
                                Bitmap layer7 = new Bitmap(140, 140, ColorDepth.ColorDepth32);
                                Bitmap layer8 = new Bitmap(140, 140, ColorDepth.ColorDepth32);
                                Bitmap layer9 = new Bitmap(140, 140, ColorDepth.ColorDepth32);
                                Bitmap layer10 = new Bitmap(140, 140, ColorDepth.ColorDepth32);

                                Render r = new Render();
                                r.x = 600;
                                r.y = 100;
                                r.desk_ID = 0;
                                r.width = 800;
                                r.height = 420;
                                r.name = "Mine...";
                                r.minimised = false;
                                r.z = 999;
                                r.Layers.Add(layer);
                                r.Layers.Add(layer2);

                                r.Layers.Add(layer3);
                                r.Layers.Add(layer4);
                                r.Layers.Add(layer5);
                                r.Layers.Add(layer6);
                                r.Layers.Add(layer7);
                                r.Layers.Add(layer8);
                                r.Layers.Add(layer9);
                                r.Layers.Add(layer10);

                                r.icon = m.Icon;

                                TaskScheduler.Apps.Add(r);

                                MenuOpened = false;
                            }
                            else if(m.Source == "Calculator")
                            {
                                Calculator c = new Calculator();
                                c.x = 100;
                                c.y = 100;
                                c.width = 200;
                                c.height = 380;
                                c.name = "Calc...";
                                c.z = 999;
                                c.icon = m.Icon;
                                TaskScheduler.Apps.Add(c);
                                MenuOpened = false;
                            }
                            else if (m.Source == "Settings")
                            {
                                Settings c = new Settings();
                                c.x = 100;
                                c.y = 100;
                                c.width = 550;
                                c.height = 380;
                                c.name = "Sett...";
                                c.z = 999;
                                c.icon = m.Icon;
                                TaskScheduler.Apps.Add(c);
                                MenuOpened = false;
                            }
                        }
                    }
                }
                if(update == true)
                {
                    ImprovedVBE.DrawImageAlpha(m.Icon, x - ((int)(Left + X_offset - 290)) + 12, 110, Back);
                    BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Global_integers.c, m.Name, x - ((int)(Left + X_offset - 290)) + 12, 170);
                }
                /*
                if (x < Left + X_offset - 290 + Back.Width)
                {
                    x += 70;
                }
                else
                {
                    y += 50;
                    x = (int)(Left + X_offset - 290);
                }
                */
                if(co < 5)
                {
                    x += 70;
                    co++;
                }
                else
                {
                    y += 50;
                    x = (int)(Left + X_offset - 290);
                    co = 0;
                }
            }
            update = false;
        }
    }
    public struct Menu_Items
    {
        public Bitmap Icon { get; set;}
        public string Name { get; set;}
        public string Source { get; set;}
    }
}
