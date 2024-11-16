using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications.Calculator;
using CrystalOSAlpha.Applications.CarbonIDE;
using CrystalOSAlpha.Applications.Clock;
using CrystalOSAlpha.Applications.CrystalStore;
using CrystalOSAlpha.Applications.Gameboy;
using CrystalOSAlpha.Applications.MediaCenter;
using CrystalOSAlpha.Applications.Minecraft;
using CrystalOSAlpha.Applications.Notepad;
using CrystalOSAlpha.Applications.PatternGenerator;
using CrystalOSAlpha.Applications.Settings;
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
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha.Graphics.TaskBar
{
    class TaskManager
    {
        public static int Top = ImprovedVBE.height;
        public static int Left = ImprovedVBE.width / 2 + 60;
        public static int height = 50;
        public static int width = 50;
        public static int Time = 99;

        public static double X_offset = 10;

        public static bool resize = true;
        public static bool MenuOpened = false;
        public static bool Triggered = false;
        public static bool update = true;
        public static bool clicked = true;
        public static bool calendar = false;
        public static bool initial = true;
        public static bool disable = false;
        public static bool ExtendedMenu = false;

        public static string DayOfWeek = "";
        public static string Text_Search = "";

        [ManifestResourceStream(ResourceName = "CrystalOSAlpha.Graphics.TaskBar.Icon.bmp")] public static byte[] Elephant;
        public static Bitmap icon;
        public static Bitmap Back;
        public static Bitmap Back_Buffer = null;
        public static Bitmap TaskBar;
        public static Bitmap Extension_Dock;
        public static Bitmap Search_Box;
        public static Bitmap Backup;
        public static Bitmap Buffer;

        public static List<Menu_Items> Items = new List<Menu_Items>();
        public static List<Menu_Items> BackUP = new List<Menu_Items>();
        public static List<ItemUnifier> Itemunified = new List<ItemUnifier>();
        #region UI_Elements
        public static List<Button> Buttons = new List<Button>();
        public static List<VerticalScrollbar> Slider = new List<VerticalScrollbar>();
        public static List<CheckBox> CheckBox = new List<CheckBox>();
        public static List<Dropdown> Dropdown = new List<Dropdown>();
        public static List<VerticalScrollbar> Scroll = new List<VerticalScrollbar>();
        public static List<TextBox> TextBox = new List<TextBox>();
        public static List<label> Label = new List<label>();
        public static List<Table> Tables = new List<Table>();
        public static List<PictureBox> Picturebox = new List<PictureBox>();
        #endregion UI_Elements

        public static int Count = -1;

        public static void Main()
        {
            switch (GlobalValues.TaskBarType)
            {
                case "Classic":
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        clicked = false;
                    }
                    if (resize == true)
                    {
                        icon = ImprovedVBE.ScaleImageStock(new Bitmap(Elephant), 36, 36);
                        Back = Base.Widget_Back(420, 470, ImprovedVBE.colourToNumber(255, 255, 255));
                        Back = ImprovedVBE.EnableTransparencyPreRGB(Back, (int)(Left + X_offset - 300), Top - 480, Back, GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB, ImprovedVBE.cover);

                        TaskBar = Base.Widget_Back((int)(X_offset * 2), 50, ImprovedVBE.colourToNumber(GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB));
                        TaskBar = ImprovedVBE.EnableTransparencyPreRGB(TaskBar, (int)(Left - X_offset), Top, TaskBar, GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB, ImprovedVBE.cover);

                        Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                        Extension_Dock = ImprovedVBE.EnableTransparencyPreRGB(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock, GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB, ImprovedVBE.cover);

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

                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", GlobalValues.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, 60, 5);
                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", GlobalValues.c, DayOfWeek + "," + DateTime.UtcNow.Day, (int)(Extension_Dock.Width / 2 - (DayOfWeek + "," + DateTime.UtcNow.Day).Length * 4), 20);
                        resize = false;
                    }
                    if (MenuOpened == false && calendar == false)
                    {
                        if (MouseManager.X > Left - ((ImprovedVBE.width / 192 * 1.3) * 35) - 175 && MouseManager.X < Left + ((ImprovedVBE.width / 192 * 1.3) * 35) && MouseManager.Y > ImprovedVBE.height - 70 && MouseManager.Y < ImprovedVBE.height)
                        {
                            if (X_offset < ImprovedVBE.width / 192 * 1.3 * 35)
                            {
                                Top -= 2;
                                X_offset += ImprovedVBE.width / 192 * 1.3;

                                TaskBar = Base.Widget_Back((int)(X_offset * 2), 50, ImprovedVBE.colourToNumber(255, 255, 255));
                                TaskBar = ImprovedVBE.EnableTransparencyPreRGB(TaskBar, (int)(Left - X_offset), Top, TaskBar, GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB, ImprovedVBE.cover);

                                Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                                Extension_Dock = ImprovedVBE.EnableTransparencyPreRGB(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock, GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB, ImprovedVBE.cover);

                                BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", GlobalValues.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, 60, 5);
                                BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", GlobalValues.c, DayOfWeek + "," + DateTime.UtcNow.Day, (int)(Extension_Dock.Width / 2 - (DayOfWeek + "," + DateTime.UtcNow.Day).Length * 4), 20);//44
                            }
                        }
                        else
                        {
                            if (X_offset > 10)
                            {
                                Top += 2;
                                X_offset -= ImprovedVBE.width / 192 * 1.3;

                                TaskBar = Base.Widget_Back((int)(X_offset * 2), 50, ImprovedVBE.colourToNumber(255, 255, 255));
                                TaskBar = ImprovedVBE.EnableTransparencyPreRGB(TaskBar, (int)(Left - X_offset), Top, TaskBar, GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB, ImprovedVBE.cover);

                                Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                                Extension_Dock = ImprovedVBE.EnableTransparencyPreRGB(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock, GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB, ImprovedVBE.cover);

                                BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", GlobalValues.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, 60, 5);
                                BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", GlobalValues.c, DayOfWeek + "," + DateTime.UtcNow.Day, (int)(Extension_Dock.Width / 2 - (DayOfWeek + "," + DateTime.UtcNow.Day).Length * 4), 20);
                            }
                        }
                        if (Triggered == true && MouseManager.MouseState == MouseState.None)
                        {
                            Triggered = false;
                        }
                        if (MouseManager.MouseState == MouseState.Left)
                        {
                            if (MouseManager.X > Left + X_offset - 38 && MouseManager.X < Left + X_offset)
                            {
                                if (MouseManager.Y > Top && MouseManager.Y < ImprovedVBE.height - 10)
                                {
                                    if (Triggered == false)
                                    {
                                        update = true;
                                        MenuOpened = true;
                                        clicked = true;
                                        ExtendedMenu = false;
                                        Back_Buffer = null;
                                        ClearLists();
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
                                        update = true;
                                        calendar = true;
                                        clicked = true;
                                        Calendar.get_Render = true;
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
                                    if (Triggered == false)
                                    {
                                        Items.Clear();
                                        if (MenuOpened == false)
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
                                        if (calendar == false)
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
                        }
                        if (MenuOpened == true)
                        {
                            Dynamic_Menu((int)(Left + X_offset - 290), Top - 410, 400, 400);
                        }
                        else if (calendar == true)
                        {
                            Calendar.Calendar_Widget((int)(Left - X_offset) - 275, Top - 330);
                        }
                    }

                    ImprovedVBE.DrawImageAlpha(TaskBar, (int)(Left - X_offset), Top, ImprovedVBE.cover);

                    if (Top < ImprovedVBE.height - 10)
                    {
                        TaskScheduler.Render_Icons();
                    }

                    if (Time != DateTime.UtcNow.Minute)
                    {
                        Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                        Extension_Dock = ImprovedVBE.EnableTransparencyPreRGB(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock, GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB, ImprovedVBE.cover);
                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", GlobalValues.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, 60, 5);
                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", GlobalValues.c, DayOfWeek + "," + DateTime.UtcNow.Day, (int)(Extension_Dock.Width / 2 - (DayOfWeek + "," + DateTime.UtcNow.Day).Length * 4), 20);

                        Time = DateTime.UtcNow.Minute;
                    }

                    ImprovedVBE.DrawImageAlpha(Extension_Dock, (int)(Left - X_offset) - 175, Top, ImprovedVBE.cover);

                    ImprovedVBE.DrawImageAlpha(icon, (int)(Left + X_offset - 41), Top + height / 2 - 18, ImprovedVBE.cover);
                break;

                case "Nostalgia":
                        if(resize == true)
                        {
                            //CrystalOS Alpha Logo
                            icon = ImprovedVBE.ScaleImageStock(new Bitmap(Elephant), 36, 36);
                            //Set the canvas for TaskBar
                            TaskBar = new Bitmap((uint)ImprovedVBE.width, 45, ColorDepth.ColorDepth32);
                            //Copy background from wallpaper for faster rounded corner rendering
                            Array.Copy(ImprovedVBE.cover.RawData, 0, TaskBar.RawData, 0, ImprovedVBE.width * 45);
                            //Draws the "Rounded rectangle" to the canvas
                            Bitmap temp = Base.Widget_Back(ImprovedVBE.width, 45, ImprovedVBE.colourToNumber(GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB));
                            //Does transparency stuff
                            ImprovedVBE.EnableTransparencyPreRGB(temp, 0, 0, TaskBar, GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB, ImprovedVBE.cover);
                            //Draw CrystalOS Alpha logo to the taskbar
                            ImprovedVBE.DrawImageAlpha(icon, (int)(ImprovedVBE.width / 2 - icon.Width / 2), 5, TaskBar);
                            //Having a backup copy for date/time display
                            Backup = new Bitmap((uint)ImprovedVBE.width, 45, ColorDepth.ColorDepth32);
                            //Copy the TaskBar to Backup
                            Array.Copy(TaskBar.RawData, Backup.RawData, TaskBar.RawData.Length);
                            //Day of week indentifier
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

                            //Disable the if condition to only execute this part once -> Works efficiently
                            resize = false;
                            Time = 99;
                        }
                        if(Time != DateTime.UtcNow.Minute)
                        {
                            //Clear the TaskBar with the Backup
                            Array.Copy(TaskBar.RawData, Backup.RawData, TaskBar.RawData.Length);
                            //Write out the time/date
                            if(DateTime.UtcNow.Hour < 10)
                            {
                                if(DateTime.UtcNow.Minute < 10)
                                {
                                    BitFont.DrawBitFontString(Backup, "ArialCustomCharset16", GlobalValues.c, "0" + DateTime.UtcNow.Hour.ToString() + ":0" + DateTime.UtcNow.Minute, ImprovedVBE.width - 80, 5);
                                }
                                else
                                {
                                    BitFont.DrawBitFontString(Backup, "ArialCustomCharset16", GlobalValues.c, "0" + DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, ImprovedVBE.width - 80, 5);
                                }
                            }
                            else
                            {
                                if (DateTime.UtcNow.Minute < 10)
                                {
                                    BitFont.DrawBitFontString(Backup, "ArialCustomCharset16", GlobalValues.c, DateTime.UtcNow.Hour.ToString() + ":0" + DateTime.UtcNow.Minute, ImprovedVBE.width - 80, 5);
                                }
                                else
                                {
                                    BitFont.DrawBitFontString(Backup, "ArialCustomCharset16", GlobalValues.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, ImprovedVBE.width - 80, 5);
                                }
                            }
                            BitFont.DrawBitFontString(Backup, "ArialCustomCharset16", GlobalValues.c, DayOfWeek + "," + DateTime.UtcNow.Day, ImprovedVBE.width - 100, 20);
                            //Disable if condition to only execute once every minute -> Works efficiently
                            Time = DateTime.UtcNow.Minute;
                            Array.Copy(Backup.RawData, 0, ImprovedVBE.cover.RawData, 0, Backup.RawData.Length);
                        }
                        //Rendering the TaskBar to the Screen
                        if(ImprovedVBE.RequestRedraw == true)
                        {
                            Array.Copy(Backup.RawData, 0, ImprovedVBE.cover.RawData, 0, Backup.RawData.Length);
                        }

                        //Opening the menu
                        if(MouseManager.MouseState == MouseState.Left)
                        {
                            if(MouseManager.X > (int)(ImprovedVBE.width / 2 - icon.Width / 2) && MouseManager.X < (int)(ImprovedVBE.width / 2 + icon.Width / 2))
                            {
                                if (MouseManager.Y > 0 && MouseManager.Y < 45)
                                {
                                    if (MenuOpened == false && clicked == false)
                                    {
                                        update = true;
                                        MenuOpened = true;
                                        clicked = true;
                                        ExtendedMenu = false;
                                        Back_Buffer = null;
                                        ClearLists();
                                    }
                                    else if(MenuOpened == true && clicked == false)
                                    {
                                        MenuOpened = false;
                                        update = true;
                                        Back_Buffer = null;
                                        clicked = true;
                                    }
                                }
                            }
                            if (MouseManager.X > ImprovedVBE.width - 100 && MouseManager.X < ImprovedVBE.width)
                            {
                                if(MouseManager.Y > Backup.Height / 2 && MouseManager.Y < Backup.Height)
                                {
                                    if (calendar == false && clicked == false)
                                    {
                                        update = true;
                                        calendar = true;
                                        clicked = true;
                                        Calendar.get_Render = true;
                                    }
                                    else if (calendar == true && clicked == false)
                                    {
                                        calendar = false;
                                        clicked = true;
                                    }
                                }
                            }
                            if (MouseManager.X > ImprovedVBE.width - 100 && MouseManager.X < ImprovedVBE.width)
                            {
                                if (MouseManager.Y > 0 && MouseManager.Y < Backup.Height / 2)
                                {
                                    calendar = false;
                                    MenuOpened = false;
                                    if (TaskScheduler.Apps.FindAll(d => d.name == "Clock").Count == 0)
                                    {
                                        Clock clock = new Clock(ImprovedVBE.width - 305, (int)Backup.Height + 5, 999, 300, 300, "Clock", ImprovedVBE.ScaleImageStock(Resources.Clock, 56, 56));
                                        TaskScheduler.Apps.Add(clock);

                                        TaskScheduler.Apps[^1].once = true;
                                        clicked = true;
                                    }
                                }
                            }
                        }
                        else if(MouseManager.MouseState == MouseState.None)
                        {
                            clicked = false;
                        }
                        if(MenuOpened == true)
                        {
                            //Dynamic_Menu(ImprovedVBE.width / 2 - 200, 50, 400, 400);
                            calendar = false;
                        }
                        //Add calendar here
                        if(calendar == true)
                        {
                            Calendar.Calendar_Widget(ImprovedVBE.width - 345, (int)Backup.Height + 5);
                            MenuOpened = false;
                        }

                        //Render app buttons to the menubar
                        TaskScheduler.Render_Icons();
                    break;

                case "Mirage DE":

                    break;
            }
        }
        public static void Dynamic_Menu(int X, int Y, int Width, int Height)
        {
            //Activate click on MenuItems
            if(MouseManager.MouseState == MouseState.Left)
            {
                if(MouseManager.X > ImprovedVBE.width / 2 - icon.Width / 2 && MouseManager.X < ImprovedVBE.width / 2 + icon.Width)
                {
                    if (MouseManager.Y > 0 && MouseManager.Y < TaskBar.Height)
                    {
                        update = true;
                        Back_Buffer = null;
                    }
                }
                foreach (var v in Items)
                {
                    if(clicked == false)
                    {
                        switch (ExtendedMenu)
                        {
                            case true:
                                if(Scroll[0].Clicked == false)
                                {
                                    if (MouseManager.X > X + v.X + 10 && (MouseManager.X < X + Width - v.X - 20 || MouseManager.X < X + v.X + v.Icon.Width))
                                    {
                                        if (MouseManager.Y > Y + v.Y + 68 && MouseManager.Y < Y + v.Y + v.Icon.Height + 68 && v.Y > 0 && v.Y < Buffer.Height)
                                        {
                                            AppDecider(v.Source, v.Icon);
                                            ExtendedMenu = false;
                                            ClearLists();
                                        }
                                    }
                                }
                                break;
                            case false:
                                    if (MouseManager.X > X + v.X && MouseManager.X < X + v.X + v.Icon.Width)
                                    {
                                        if (MouseManager.Y > Y + v.Y && MouseManager.Y < Y + v.Y + v.Icon.Height + 18)
                                        {
                                            AppDecider(v.Source, v.Icon);
                                        }
                                    }
                                break;
                        }
                    }
                }
            }

            if(update == true)
            {
                if(Back_Buffer == null)
                {
                    //Get the Menu
                    Back = Base.Widget_Back(Width, Height, ImprovedVBE.colourToNumber(255, 255, 255));
                    Back = ImprovedVBE.EnableTransparencyPreRGB(Back, X, Y, Back, GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB, ImprovedVBE.cover);
                    Back_Buffer = Base.Widget_Back(Width, Height, ImprovedVBE.colourToNumber(255, 255, 255));
                    Back_Buffer = ImprovedVBE.EnableTransparencyPreRGB(Back_Buffer, X, Y, Back_Buffer, GlobalValues.TaskBarR, GlobalValues.TaskBarG, GlobalValues.TaskBarB, ImprovedVBE.cover);
                }
                else
                {
                    Array.Copy(Back_Buffer.RawData, Back.RawData, Back_Buffer.RawData.Length);
                }
                //Render Main Menu only
                if(ExtendedMenu == false)
                {
                    //Add menu UI_Elements
                    if(Itemunified.Count == 0)
                    {
                        Itemunified.Add(new ItemUnifier(11, 228, 379, 25, 1, "All apps ->", "More"));
                        Itemunified.Add(new ItemUnifier(11, 310, 109, 25, 1, "Power off", "Poweroof"));
                        Itemunified.Add(new ItemUnifier(131, 310, 109, 25, 1, "Restart", "Rest"));
                        Itemunified.Add(new ItemUnifier(281, 310, 109, 25, 1, "Log off", "Logoff"));
                        Itemunified.Add(new ItemUnifier(111, 14, ImprovedVBE.colourToNumber(255, 255, 255), "CrystalOS Alpha", "VerdanaCustomCharset24", "Banner"));
                        Itemunified.Add(new ItemUnifier(16, 53, ImprovedVBE.colourToNumber(255, 255, 255), "Recently used apps:", "ArialCustomCharset16", "RecentApps"));
                        Itemunified.Add(new ItemUnifier(11, 280, ImprovedVBE.colourToNumber(255, 255, 255), "Power options:", "ArialCustomCharset16", "Power"));
                        Itemunified.Add(new ItemUnifier(11, 359, ImprovedVBE.colourToNumber(255, 255, 255), "Hello, " + GlobalValues.Username + "!", "ArialCustomCharset16", "Uname"));
                    }
                    //Recently used apps if the used machine is under vmware
                    if(VMTools.IsVMWare == true && Kernel.fs.Disks.Count != 0)
                    {
                        Items = new List<Menu_Items>();
                        string FrequentApps = File.ReadAllText("0:\\System\\FrequentApps.sys");
                        string[] Sep = FrequentApps.Split("\n");
                        for (int i = 0; i < Sep.Length; i++)
                        {
                            switch (Sep[i])
                            {
                                case "Settings":
                                    Menu_Items Settings = new Menu_Items()
                                    {
                                        Name = "Settings",
                                        Source = "Settings",
                                        Icon = ImprovedVBE.ScaleImageStock(Resources.Settings, GlobalValues.IconWidth, GlobalValues.IconHeight)
                                    };
                                    Items.Add(Settings);
                                    break;
                                case "Gameboy":
                                    Menu_Items Gameboy = new Menu_Items()
                                    {
                                        Name = "Gameboy",
                                        Source = "Gameboy",
                                        Icon = ImprovedVBE.ScaleImageStock(Resources.Gameboy, GlobalValues.IconWidth, GlobalValues.IconHeight)
                                    };
                                    Items.Add(Gameboy);
                                    break;
                                case "FileSystem":
                                    Menu_Items FS = new Menu_Items
                                    {
                                        Name = "File Explorer",
                                        Source = "File Explorer",
                                        Icon = ImprovedVBE.ScaleImageStock(Resources.Explorer, GlobalValues.IconWidth, GlobalValues.IconHeight)
                                    };
                                    Items.Add(FS);
                                    break;
                            }
                        }
                        int X_Axis = 12;
                        int Y_Axis = 81;
                        foreach (var v in Items)
                        {
                            ImprovedVBE.DrawImageAlpha(v.Icon, X_Axis, Y_Axis, Back);
                            BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Color.White, v.Name, X_Axis, (int)(Y_Axis + v.Icon.Height + 5));
                            v.X = X_Axis;
                            v.Y = Y_Axis;
                            X_Axis += 80;
                        }
                    }
                }
                //Change the Menu layout to every application
                else if(ExtendedMenu == true)
                {
                    //Adding all MenuItems
                    Items = new List<Menu_Items>()
                    {
                        new Menu_Items
                        {
                            Name = "Minecraft",
                            Source = "Minecraft",
                            Icon = ImprovedVBE.ScaleImageStock(ImageViewer.Nr1, GlobalValues.IconWidth, GlobalValues.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "Calculator",
                            Source = "Calculator",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Calculator, GlobalValues.IconWidth, GlobalValues.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "CarbonIDE",
                            Source = "CarbonIDE",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.IDE, GlobalValues.IconWidth, GlobalValues.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "File Explorer",
                            Source = "File Explorer",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Explorer, GlobalValues.IconWidth, GlobalValues.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "Gameboy",
                            Source = "Gameboy",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Gameboy, GlobalValues.IconWidth, GlobalValues.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "Settings",
                            Source = "Settings",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Settings, GlobalValues.IconWidth, GlobalValues.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "Notepad",
                            Source = "Notepad",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Notepad, GlobalValues.IconWidth, GlobalValues.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "Terminal",
                            Source = "Terminal",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Terminal, GlobalValues.IconWidth, GlobalValues.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "WebscapeNavigator",
                            Source = "WebscapeNavigator",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Web, GlobalValues.IconWidth, GlobalValues.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "Clock",
                            Source = "Clock",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Clock, GlobalValues.IconWidth, GlobalValues.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "Pattern Generator",
                            Source = "PatternGenerator",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.PTG, GlobalValues.IconWidth, GlobalValues.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "Entertainment",
                            Source = "Entertainment",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.CrystalVideo, GlobalValues.IconWidth, GlobalValues.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "CrystalStore",
                            Source = "CrystalStore",
                            Icon = ImprovedVBE.ScaleImageStock(ImprovedVBE.ScaleImageStock(new Bitmap(Elephant), 50, 50), GlobalValues.IconWidth, GlobalValues.IconHeight)
                        }
                    };

                    //Sorting the applications
                    Items = SortMenuItems.SortMenuItemsAlphabetically(Items);

                    //Adding UI elements only once
                    if(Itemunified.Count == 0)
                    {
                        //Set up the canvas to render all the apps onto
                        Buffer = new Bitmap(357, 310, ColorDepth.ColorDepth32);
                        //Adding UI elements to the menu
                        //- Back button 
                        Itemunified.Add(new ItemUnifier(16, 10, 30, 30, 1, "<-", "Back"));
                        //- Banner and subtext
                        Itemunified.Add(new ItemUnifier(111, 14, ImprovedVBE.colourToNumber(255, 255, 255), "CrystalOS Alpha", "VerdanaCustomCharset24", "Banner"));
                        Itemunified.Add(new ItemUnifier(16, 53, ImprovedVBE.colourToNumber(255, 255, 255), "Extended app list:", "ArialCustomCharset16", "RecentApps"));
                        //Drawing "every" app to the screen to determine the MaxValue of scrollbar
                        int Helper = 10 + 0 * 25 - 0;
                        for (int i = 0; i < Items.Count; i++)
                        {
                            if (Items[i].Name.Length == 1)
                            {
                                if (Helper > 0 && Helper < Buffer.Height)
                                {
                                    BitFont.DrawBitFontString(Buffer, "VerdanaCustomCharset24", Color.White, Items[i].Name, 10, Helper);
                                }
                                Helper += 35;
                            }
                            else
                            {
                                if (Helper + Items[i].Icon.Height > 0 && Helper < Buffer.Height)
                                {
                                    Bitmap bmp = new Bitmap(Items[i].Icon.Width, Items[i].Icon.Height, ColorDepth.ColorDepth32);
                                    if (GlobalValues.IconR + GlobalValues.IconG + GlobalValues.IconB != 0)
                                    {
                                        Array.Fill(bmp.RawData, ImprovedVBE.colourToNumber(GlobalValues.IconR, GlobalValues.IconG, GlobalValues.IconB));
                                        ImprovedVBE.DrawImageAlpha(Items[i].Icon, 0, 0, bmp);
                                        ImprovedVBE.DrawImage(bmp, 10, Helper - 10, Buffer);
                                    }
                                    else
                                    {
                                        Array.Copy(Items[i].Icon.RawData, bmp.RawData, bmp.RawData.Length);
                                        ImprovedVBE.DrawImageAlpha(bmp, 10, Helper - 10, Buffer);
                                    }
                                    BitFont.DrawBitFontString(Buffer, "ArialCustomCharset16", Color.White, Items[i].Name, 80, Helper + 10);
                                }
                                Items[i].X = 10;
                                Items[i].Y = Helper - 10;
                                Helper += (int)Items[i].Icon.Height + 15;
                            }
                        }
                        //- Vertical scrollbar
                        Itemunified.Add(new ItemUnifier(375, 68, 20, 310, 20, 0, Helper - (int)Buffer.Height));
                    }
                    else
                    {
                        //Clearing the Buffer with a fresh copy of Back
                        for (int i = 0; i < Buffer.Height; i++)
                        {
                            Array.Copy(Back.RawData, (i + 68) * Back.Width + 10, Buffer.RawData, i * Buffer.Width, Buffer.Width);
                        }

                        //Drawing "every" app to the screen
                        int Helper = 10 + 0 * 25 - Scroll[0].Value;
                        for (int i = 0; i < Items.Count; i++)
                        {
                            if (Items[i].Name.Length == 1)
                            {
                                if(Helper > 0 && Helper < Buffer.Height)
                                {
                                    BitFont.DrawBitFontString(Buffer, "VerdanaCustomCharset24", Color.White, Items[i].Name, 10, Helper);
                                }
                                Helper += 35;
                            }
                            else
                            {
                                if(Helper + Items[i].Icon.Height > 0 && Helper < Buffer.Height)
                                {
                                    Bitmap bmp = new Bitmap(Items[i].Icon.Width, Items[i].Icon.Height, ColorDepth.ColorDepth32);
                                    if(GlobalValues.IconR + GlobalValues.IconG + GlobalValues.IconB != 0)
                                    {
                                        Array.Fill(bmp.RawData, ImprovedVBE.colourToNumber(GlobalValues.IconR, GlobalValues.IconG, GlobalValues.IconB));
                                        ImprovedVBE.DrawImageAlpha(Items[i].Icon, 0, 0, bmp);
                                        ImprovedVBE.DrawImage(bmp, 10, Helper - 10, Buffer);
                                    }
                                    else
                                    {
                                        Array.Copy(Items[i].Icon.RawData, bmp.RawData, bmp.RawData.Length);
                                        ImprovedVBE.DrawImageAlpha(bmp, 10, Helper - 10, Buffer);
                                    }
                                    BitFont.DrawBitFontString(Buffer, "ArialCustomCharset16", Color.White, Items[i].Name, 80, Helper + 10);
                                }
                                Items[i].X = 10;
                                Items[i].Y = Helper - 10;
                                Helper += (int)Items[i].Icon.Height + 15;
                            }
                        }
                    }

                    //Backup list because COSMOS tends to throw lists away for some unknown reason
                    BackUP = Items;
                    //Render it to the menu itself
                    ImprovedVBE.DrawImageAlpha(Buffer, 10, 68, Back);
                }
                //Renders every label
                foreach (var label in Label)
                {
                    label.Render(Back);
                }
                //Renders every scrollbar
                foreach (var vscroll in Scroll)
                {
                    vscroll.Render(Back);
                }
                //Renders every button
                foreach(var button in Buttons)
                {
                    button.Render(Back);
                }
                //Disables the if conditional, so it only executes when needed -> performance gain
                update = false;
            }
            //NOTE: So why is this necessary? COSMOS keeps tossing out the content of the Items list for some unknown reason and this has been happening for a while now... I'm getting mad...
            //If you know why is this happening or what am I doing wrong please let me know!
            if(Items.Count == 0 || Scroll.Count != 0)
            {
                //Adding all MenuItems
                Items = BackUP;
            }

            //Check buttons for any presses
            foreach (var button in Buttons)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > X + button.X && MouseManager.X < X + button.X + button.Width)
                    {
                        if (MouseManager.Y > Y + button.Y && MouseManager.Y < Y + button.Y + button.Height)
                        {
                            button.Clicked = true;
                            switch (button.ID)
                            {
                                case "More":
                                    ExtendedMenu = true;
                                    update = true;
                                    ClearLists();
                                    break;
                                case "Back":
                                    ExtendedMenu = false;
                                    update = true;
                                    ClearLists();
                                    break;
                                case "Poweroof":
                                    Power.Shutdown();
                                    break;
                                case "Rest":
                                    Power.Reboot();
                                    break;
                                case "Logoff":
                                    //TODO: User Identification
                                    break;
                            }
                            clicked = true;
                        }
                    }
                }
                if (button.Clicked == true && MouseManager.MouseState == MouseState.None)
                {
                    button.Clicked = false;
                    clicked = false;
                }
            }
            //Check scrollbars for any movement
            foreach (var vscroll in Scroll)
            {
                if (vscroll.CheckClick((int)MouseManager.X - X, (int)MouseManager.Y - Y))
                {
                    update = true;
                }
            }

            //Render the menu if it's opened
            if(MenuOpened == true && Count == -1 && ImprovedVBE.RequestRedraw)
            {
                ImprovedVBE.DrawImageAlpha(Back, X, Y, ImprovedVBE.cover);
            }
        }

        public static void ClearLists()
        {
            Items.Clear();
            Itemunified.Clear();

            Buttons.Clear();
            Slider.Clear();
            CheckBox.Clear();
            Dropdown.Clear();
            Scroll.Clear();
            TextBox.Clear();
            Label.Clear();
            Tables.Clear();
            Picturebox.Clear();
        }
        public static void AppDecider(string Source, Bitmap Icon)
        {
            switch (Source)
            {
                case "Calculator":
                    Calculator c = new Calculator(100, 100, 999, 200, 380, "Calculator", Icon);//These need some update to make it easier to work with
                    TaskScheduler.Apps.Add(c);
                    MenuOpened = false;
                    break;
                case "File Explorer":
                    Applications.FileSys.FileSystem d = new Applications.FileSys.FileSystem(100, 100, 999, 650, 380, "File Explorer", Icon);//Same here ;)
                    TaskScheduler.Apps.Add(d);
                    MenuOpened = false;
                    break;
                case "Minecraft":
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
                    r.name = "Minecraft";
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

                    r.icon = Icon;

                    TaskScheduler.Apps.Add(r);
                    MenuOpened = false;
                    break;
                case "Settings":
                    Settings settings = new Settings(100, 100, 999, 550, 380, "Settings", Icon);
                    TaskScheduler.Apps.Add(settings);
                    MenuOpened = false;
                    break;
                case "Gameboy":
                    Core Gameboy = new Core(100, 100, 999, 162 * 3 - 4, 165 * 3 - 39 + 25, "Gameboy", Icon);
                    TaskScheduler.Apps.Add(Gameboy);
                    MenuOpened = false;
                    break;
                case "Notepad":
                    Notepad Notepad = new Notepad(100, 100, 999, 700, 420, "Notepad", Icon);
                    TaskScheduler.Apps.Add(Notepad);
                    MenuOpened = false;
                    break;
                case "Terminal":
                    Terminal Terminal = new Terminal(100, 100, 999, 700, 420, "Terminal", Icon);
                    TaskScheduler.Apps.Add(Terminal);
                    MenuOpened = false;
                    break;
                case "CarbonIDE":
                    CarbonInit CarbonIDE = new CarbonInit();
                    CarbonIDE.name = "CarbonIDE - Init";
                    CarbonIDE.x = 100;
                    CarbonIDE.y = 100;
                    CarbonIDE.width = 700;
                    CarbonIDE.height = 415;
                    CarbonIDE.icon = ImprovedVBE.ScaleImageStock(Resources.IDE, 56, 56);
                    CarbonIDE.z = 999;

                    TaskScheduler.Apps.Add(CarbonIDE);
                    MenuOpened = false;
                    break;
                case "WebscapeNavigator":
                    Applications.WebscapeNavigator.Webscape WebscapeNavigator = new Applications.WebscapeNavigator.Webscape();
                    if (File.Exists("0:\\index.html"))
                    {
                        WebscapeNavigator.content = File.ReadAllText("0:\\index.html");
                    }
                    WebscapeNavigator.x = 100;
                    WebscapeNavigator.y = 100;
                    WebscapeNavigator.width = 738;
                    WebscapeNavigator.height = 488;
                    WebscapeNavigator.z = 999;
                    WebscapeNavigator.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
                    WebscapeNavigator.name = "Webscape Navigator";

                    TaskScheduler.Apps.Add(WebscapeNavigator);
                    MenuOpened = false;
                    break;
                case "Clock":
                    if (TaskScheduler.Apps.FindAll(d => d.name == "Clock").Count == 0)
                    {
                        Clock clock = new Clock(ImprovedVBE.width - 305, (int)Backup.Height + 5, 999, 300, 300, "Clock", ImprovedVBE.ScaleImageStock(Resources.Clock, 56, 56));
                        TaskScheduler.Apps.Add(clock);
                        MenuOpened = false;
                    }
                    break;
                case "PatternGenerator":
                    PatternGenerator PTG = new PatternGenerator(10, 100, 999, 375, 307, "Pattern Generator", ImprovedVBE.ScaleImageStock(Resources.PTG, 56, 56));
                    TaskScheduler.Apps.Add(PTG);
                    MenuOpened = false;
                    break;
                case "Entertainment":
                    MediaCenter mc = new MediaCenter(10, 100, 999, 862, 490, "Media center", ImprovedVBE.ScaleImageStock(Resources.CrystalVideo, 56, 56));
                    TaskScheduler.Apps.Add(mc);
                    MenuOpened = false;
                    break;
                case "CrystalStore":
                    CrystalStore cs = new CrystalStore();
                    cs.x = 100;
                    cs.y = 100;
                    cs.width = 800;
                    cs.height = 600;
                    cs.z = 999;
                    cs.icon = ImprovedVBE.ScaleImageStock(new Bitmap(Elephant), 56, 56);
                    cs.name = "CrystalStore";

                    TaskScheduler.Apps.Add(cs);
                    MenuOpened = false;
                    break;
            }
            if (MenuOpened == false)
            {
                TaskScheduler.Apps[^1].once = true;
                clicked = true;
            }

        }
    }
    public class Menu_Items
    {
        public Bitmap Icon { get; set;}
        public string Name { get; set;}
        public string Source { get; set;}
        public int X { get; set;}
        public int Y { get; set;}
    }
    public class ItemUnifier
    {
        public ItemUnifier(int x, int y, int width, int height, int color, string content, string id)
        {
            TaskManager.Buttons.Add(new Button(x, y - 22, width, height, content, color, id));
        }
        public ItemUnifier(int x, int y, int color, string content, string FonType, string id)
        {
            TaskManager.Label.Add(new label(x, y - 22, content, FonType, color, id));
        }
        public ItemUnifier(int x, int y, int width, int height, int position, int MinVal, int MaxVal)
        {
            TaskManager.Scroll.Add(new VerticalScrollbar(x, y, width, height, position, MinVal, MaxVal, ""));
        }
    }
}
