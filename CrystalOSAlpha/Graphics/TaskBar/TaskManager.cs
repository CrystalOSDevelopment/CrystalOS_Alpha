using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications.Calculator;
using CrystalOSAlpha.Applications.CarbonIDE;
using CrystalOSAlpha.Applications.Gameboy;
using CrystalOSAlpha.Applications.Minecraft;
using CrystalOSAlpha.Applications.Notepad;
using CrystalOSAlpha.Applications.Settings;
using CrystalOSAlpha.Applications.Solitare;
using CrystalOSAlpha.Applications.Terminal;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.Widgets;
using CrystalOSAlpha.UI_Elements;
using IL2CPU.API.Attribs;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace CrystalOSAlpha.Graphics.TaskBar
{
    class TaskManager
    {
        public static int Top = ImprovedVBE.height;
        public static int Left = ImprovedVBE.width / 2 + 60;
        public static int height = 50;
        public static int width = 50;
        public static int Time = 0;

        public static double X_offset = 10;

        public static bool resize = true;
        public static bool MenuOpened = false;
        public static bool Triggered = false;
        public static bool update = true;
        public static bool clicked = true;
        public static bool calendar = false;
        public static bool initial = true;
        public static bool disable = false;

        public static string DayOfWeek = "";
        public static string Text_Search = "";

        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.TaskBar.Icon.bmp")] public static byte[] Elephant;
        public static Bitmap icon;
        public static Bitmap Back;
        public static Bitmap TaskBar;
        public static Bitmap Extension_Dock;
        public static Bitmap Search_Box;

        public static List<Menu_Items> Items = new List<Menu_Items>();
        public static List<Button_prop> Buttons = new List<Button_prop>();

        public static void Main()
        {
            if (initial == true)
            {
                Buttons.Add(new Button_prop(5, 300, 410, 25, "More applications ->", 1));

                Buttons.Add(new Button_prop(5, 375, 80, 25, "Power off", 1));
                Buttons.Add(new Button_prop(90, 375, 80, 25, "Reboot", 1));
                Buttons.Add(new Button_prop(175, 375, 80, 25, "Log out", 1));

                initial = false;
            }
            if (resize == true)
            {
                icon = ImprovedVBE.ScaleImageStock(new Bitmap(Elephant), 36, 36);
                Back = Base.Widget_Back(420, 470, ImprovedVBE.colourToNumber(255, 255, 255));
                Back = ImprovedVBE.EnableTransparency(Back, (int)(Left + X_offset - 300), Top - 480, Back);

                TaskBar = Base.Widget_Back((int)(X_offset * 2), 50, ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B));
                TaskBar = ImprovedVBE.EnableTransparency(TaskBar, (int)(Left - X_offset), Top, TaskBar);

                Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                Extension_Dock = ImprovedVBE.EnableTransparency(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock);

                Search_Box = Base.Widget_Back(210, 36, ImprovedVBE.colourToNumber(255, 255, 255));
                Search_Box = ImprovedVBE.EnableTransparencyPreRGB(Search_Box, (int)(Left + X_offset - 200), Top - 460, Search_Box, 30, 30, 30, ImprovedVBE.cover);

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
                    if(X_offset < ImprovedVBE.width / 192 * 1.3 * 35)
                    {
                        Top -= 2;
                        X_offset += ImprovedVBE.width / 192 * 1.3;

                        TaskBar = Base.Widget_Back((int)(X_offset * 2), 50, ImprovedVBE.colourToNumber(255, 255, 255));
                        TaskBar = ImprovedVBE.EnableTransparency(TaskBar, (int)(Left - X_offset), Top, TaskBar);

                        Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                        Extension_Dock = ImprovedVBE.EnableTransparency(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock);

                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, 60, 5);
                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DayOfWeek + "," + DateTime.UtcNow.Day, (int)(Extension_Dock.Width / 2 - (DayOfWeek + "," + DateTime.UtcNow.Day).Length * 4), 20);//44
                    }
                }
                else
                {
                    if (X_offset > 10)
                    {
                        Top += 2;
                        X_offset -= ImprovedVBE.width / 192 * 1.3;

                        TaskBar = Base.Widget_Back((int)(X_offset * 2), 50, ImprovedVBE.colourToNumber(255, 255, 255));
                        TaskBar = ImprovedVBE.EnableTransparency(TaskBar, (int)(Left - X_offset), Top, TaskBar);

                        Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                        Extension_Dock = ImprovedVBE.EnableTransparency(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock);

                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, 60, 5);
                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DayOfWeek + "," + DateTime.UtcNow.Day, (int)(Extension_Dock.Width / 2 - (DayOfWeek + "," + DateTime.UtcNow.Day).Length * 4), 20);
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

                                m.Name = "Game...";
                                m.Source = "Gameboy";
                                m.Icon = ImprovedVBE.ScaleImageStock(Resources.Gameboy, 56, 56);
                                Items.Add(m);

                                m.Name = "Note...";
                                m.Source = "Notepad";
                                m.Icon = ImprovedVBE.ScaleImageStock(Resources.Notepad, 56, 56);
                                Items.Add(m);

                                m.Name = "Term...";
                                m.Source = "Terminal";
                                m.Icon = ImprovedVBE.ScaleImageStock(Resources.Terminal, 56, 56);
                                Items.Add(m);

                                m.Name = "File...";
                                m.Source = "Explorer";
                                m.Icon = ImprovedVBE.ScaleImageStock(Resources.Explorer, 56, 56);
                                Items.Add(m);

                                m.Name = "Webs...";
                                m.Source = "WebscapeNavigator";
                                m.Icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
                                Items.Add(m);

                                m.Name = "Carb...";
                                m.Source = "CarbonIDE";
                                m.Icon = ImprovedVBE.ScaleImageStock(Resources.IDE, 56, 56);
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
                Extension_Dock = ImprovedVBE.EnableTransparency(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock);
                BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, 60, 5);
                BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DayOfWeek + "," + DateTime.UtcNow.Day, (int)(Extension_Dock.Width / 2 - (DayOfWeek + "," + DateTime.UtcNow.Day).Length * 4), 20);

                Time = DateTime.UtcNow.Minute;
            }

            ImprovedVBE.DrawImageAlpha(Extension_Dock, (int)(Left - X_offset) - 175, Top, ImprovedVBE.cover);
            
            ImprovedVBE.DrawImageAlpha(icon, (int)(Left + X_offset - 41), Top + height / 2 - 18, ImprovedVBE.cover);
        }

        public static void Menu_Manager()
        {
            foreach (var button in Buttons)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > (int)(Left + X_offset - 300) + button.X && MouseManager.X < (int)(Left + X_offset - 300) + button.X + button.Width)
                    {
                        if (MouseManager.Y > Top - 480 + button.Y && MouseManager.Y < Top - 480 + button.Y + button.Height)
                        {
                            if (clicked == false)
                            {
                                button.Clicked = true;
                                update = true;
                                clicked = true;
                            }
                        }
                    }
                }
                if (button.Clicked == true && MouseManager.MouseState == MouseState.None)
                {
                    button.Clicked = false;
                }
            }

            if (MouseManager.MouseState == MouseState.None && clicked == true)
            {
                update = true;
                clicked = false;
            }

            if (update == true)
            {
                Back = Base.Widget_Back(420, 470, ImprovedVBE.colourToNumber(255, 255, 255));
                Back = ImprovedVBE.EnableTransparency(Back, (int)(Left + X_offset - 300), Top - 480, Back);

                Search_Box = Base.Widget_Back(210, 36, ImprovedVBE.colourToNumber(255, 255, 255));
                Search_Box = ImprovedVBE.EnableTransparencyPreRGB(Search_Box, (int)(Left + X_offset - 200), Top - 460, Search_Box, 30, 30, 30, ImprovedVBE.cover);
                if (Text_Search != "")
                {
                    BitFont.DrawBitFontString(Search_Box, "ArialCustomCharset16", Global_integers.c, Text_Search, 5, 9);
                }
                else
                {
                    BitFont.DrawBitFontString(Search_Box, "ArialCustomCharset16", Global_integers.c, "Type to search!", 5, 9);
                }
                ImprovedVBE.DrawImageAlpha(Search_Box, 100, 20, Back);
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Global_integers.c, "Recently opened", 10, 72);
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Global_integers.c, "Power Settings:", 10, 345);
                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        Button.Button_render(Back, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);
                        switch(button.Text)
                        {
                            case "Power off":
                                Power.Shutdown(); 
                                break;
                            case "Reboot":
                                Power.Reboot();
                                break;
                        }
                    }
                    else
                    {
                        Button.Button_render(Back, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                    }
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        button.Clicked = false;
                    }
                }
            }
            ImprovedVBE.DrawImageAlpha(Back, (int)(Left + X_offset - 300), Top - 480, ImprovedVBE.cover);

            int x = (int)(Left + X_offset - 290);
            int y = Top - 370;
            int co = 0;

            bool start = false;

            if(Text_Search.Length > 0)
            {
                start = true;
            }

            foreach(Menu_Items m in Items)
            {
                if (start == true)
                {
                    if (m.Source.ToLower().Contains(Text_Search.ToLower()))
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
                                    else if (m.Source == "Gameboy")
                                    {
                                        Core c = new Core();
                                        c.x = 100;
                                        c.y = 100;
                                        c.width = 162 * 3 - 4;
                                        c.height = 165 * 3 - 39 + 25;
                                        c.name = "Gameboy";
                                        c.z = 999;
                                        c.icon = m.Icon;
                                        TaskScheduler.Apps.Add(c);
                                        MenuOpened = false;
                                    }
                                    else if (m.Source == "Solitare")
                                    {
                                        Solitare c = new Solitare();
                                        c.x = 100;
                                        c.y = 100;
                                        c.width = 700;
                                        c.height = 420;
                                        c.name = "Soli...";
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
                }
                else
                {
                    if (MouseManager.MouseState == MouseState.Left)
                    {
                        if (MouseManager.X > x && MouseManager.X < x + 70)
                        {
                            if (MouseManager.Y > y && MouseManager.Y < y + 50)
                            {
                                if (m.Source == "Minecraft")
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
                                else if (m.Source == "Calculator")
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
                                else if (m.Source == "Gameboy")
                                {
                                    Core c = new Core();
                                    c.x = 100;
                                    c.y = 100;
                                    c.width = 162 * 3 - 4;
                                    c.height = 165 * 3 - 39 + 25;
                                    c.name = "Gameboy";
                                    c.z = 999;
                                    c.icon = m.Icon;
                                    TaskScheduler.Apps.Add(c);
                                    MenuOpened = false;
                                }
                                else if (m.Source == "Notepad")
                                {
                                    Notepad c = new Notepad();
                                    c.x = 100;
                                    c.y = 100;
                                    c.width = 700;
                                    c.height = 420;
                                    c.name = "Note...";
                                    c.z = 999;
                                    c.icon = m.Icon;
                                    TaskScheduler.Apps.Add(c);
                                    MenuOpened = false;
                                }
                                else if (m.Source == "Terminal")
                                {
                                    Terminal c = new Terminal();
                                    c.x = 100;
                                    c.y = 100;
                                    c.width = 700;
                                    c.height = 420;
                                    c.name = "Term...";
                                    c.z = 999;
                                    c.icon = m.Icon;
                                    TaskScheduler.Apps.Add(c);
                                    MenuOpened = false;
                                }
                                else if (m.Source == "Explorer")
                                {
                                    Applications.FileSys.FileSystem c = new Applications.FileSys.FileSystem();
                                    c.x = 100;
                                    c.y = 100;
                                    c.width = 650;
                                    c.height = 380;
                                    c.name = "File...";
                                    c.z = 999;
                                    c.icon = m.Icon;
                                    TaskScheduler.Apps.Add(c);
                                    MenuOpened = false;
                                }
                                else if (m.Source == "WebscapeNavigator")
                                {
                                    Applications.WebscapeNavigator.Webscape wn = new Applications.WebscapeNavigator.Webscape();
                                    wn.content = File.ReadAllText("0:\\index.html");
                                    wn.x = 100;
                                    wn.y = 100;
                                    wn.width = 700;
                                    wn.height = 420;
                                    wn.z = 999;
                                    wn.source = "example.com/index.html";
                                    wn.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
                                    wn.name = "Webs... - " + "example.com/index.html";

                                    TaskScheduler.Apps.Add(wn);
                                    MenuOpened = false;
                                }
                                else if (m.Source == "CarbonIDE")
                                {
                                    CarbonInit c = new CarbonInit();
                                    c.name = "CarbonIDE - Init";
                                    c.x = 100;
                                    c.y = 100;
                                    c.width = 700;
                                    c.height = 415;
                                    c.icon = ImprovedVBE.ScaleImageStock(Resources.IDE, 56, 56);
                                    c.z = 999;

                                    TaskScheduler.Apps.Add(c);
                                    MenuOpened = false;
                                }
                                Text_Search = "";
                                disable = true;
                            }
                        }
                    }
                    if (update == true)
                    {
                        ImprovedVBE.DrawImageAlpha(m.Icon, x - ((int)(Left + X_offset - 290)) + 12, y - (Top - 370) + 110, Back);
                        BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Global_integers.c, m.Name, x - ((int)(Left + X_offset - 290)) + 12, y - (Top - 370) + 170);
                    }
                    if (co < 4)
                    {
                        x += 75;
                        co++;
                    }
                    else
                    {
                        y += 90;
                        x = (int)(Left + X_offset - 290);
                        co = 0;
                    }
                }
            }
            update = false;
            KeyEvent k;
            if(KeyboardManager.TryReadKey(out k))
            {
                Text_Search = Keyboard.HandleKeyboard(Text_Search, k);
                update = true;
            }
        }
    }
    public struct Menu_Items
    {
        public Bitmap Icon { get; set;}
        public string Name { get; set;}
        public string Source { get; set;}
    }
}
