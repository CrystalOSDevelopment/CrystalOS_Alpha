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
        public static Bitmap TaskBar;
        public static Bitmap Extension_Dock;
        public static Bitmap Search_Box;
        public static Bitmap Backup;
        public static Bitmap Buffer;

        public static List<Menu_Items> Items = new List<Menu_Items>();
        public static List<Menu_Items> BackUP = new List<Menu_Items>();
        public static List<ItemUnifier> Itemunified = new List<ItemUnifier>();
        #region UI_Elements
        public static List<Button_prop> Buttons = new List<Button_prop>();
        public static List<VerticalScrollbar> Slider = new List<VerticalScrollbar>();
        public static List<CheckBox> CheckBox = new List<CheckBox>();
        public static List<Dropdown> Dropdown = new List<Dropdown>();
        public static List<VerticalScrollbar> Scroll = new List<VerticalScrollbar>();
        public static List<TextBox> TextBox = new List<TextBox>();
        public static List<label> Label = new List<label>();
        public static List<Table> Tables = new List<Table>();
        public static List<PictureBox> Picturebox = new List<PictureBox>();
        #endregion UI_Elements

        public static void Main()
        {
            switch (Global_integers.TaskBarType)
            {
                case "Classic":
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
                        Back = ImprovedVBE.EnableTransparencyPreRGB(Back, (int)(Left + X_offset - 300), Top - 480, Back, Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB, ImprovedVBE.cover);

                        TaskBar = Base.Widget_Back((int)(X_offset * 2), 50, ImprovedVBE.colourToNumber(Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB));
                        TaskBar = ImprovedVBE.EnableTransparencyPreRGB(TaskBar, (int)(Left - X_offset), Top, TaskBar, Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB, ImprovedVBE.cover);

                        Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                        Extension_Dock = ImprovedVBE.EnableTransparencyPreRGB(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock, Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB, ImprovedVBE.cover);

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
                    if (MenuOpened == false && calendar == false)
                    {
                        if (MouseManager.X > Left - ((ImprovedVBE.width / 192 * 1.3) * 35) - 175 && MouseManager.X < Left + ((ImprovedVBE.width / 192 * 1.3) * 35) && MouseManager.Y > ImprovedVBE.height - 70 && MouseManager.Y < ImprovedVBE.height)
                        {
                            if (X_offset < ImprovedVBE.width / 192 * 1.3 * 35)
                            {
                                Top -= 2;
                                X_offset += ImprovedVBE.width / 192 * 1.3;

                                TaskBar = Base.Widget_Back((int)(X_offset * 2), 50, ImprovedVBE.colourToNumber(255, 255, 255));
                                TaskBar = ImprovedVBE.EnableTransparencyPreRGB(TaskBar, (int)(Left - X_offset), Top, TaskBar, Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB, ImprovedVBE.cover);

                                Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                                Extension_Dock = ImprovedVBE.EnableTransparencyPreRGB(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock, Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB, ImprovedVBE.cover);

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
                                TaskBar = ImprovedVBE.EnableTransparencyPreRGB(TaskBar, (int)(Left - X_offset), Top, TaskBar, Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB, ImprovedVBE.cover);

                                Extension_Dock = Base.Widget_Back(150, 50, ImprovedVBE.colourToNumber(255, 255, 255));
                                Extension_Dock = ImprovedVBE.EnableTransparencyPreRGB(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock, Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB, ImprovedVBE.cover);

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
                            if (MouseManager.X > Left + X_offset - 38 && MouseManager.X < Left + X_offset)
                            {
                                if (MouseManager.Y > Top && MouseManager.Y < ImprovedVBE.height - 10)
                                {
                                    update = true;
                                    if (Triggered == false)
                                    {
                                        Items.Clear();

                                        Items = new List<Menu_Items>()
                                        {
                                            new Menu_Items
                                            {
                                                Name = "Mine...",
                                                Source = "Minecraft",
                                                Icon = ImprovedVBE.ScaleImageStock(ImageViewer.Nr1, Global_integers.IconWidth, Global_integers.IconHeight)
                                            },
                                            new Menu_Items
                                            {
                                                Name = "Calc...",
                                                Source = "Calculator",
                                                Icon = ImprovedVBE.ScaleImageStock(Resources.Calculator, Global_integers.IconWidth, Global_integers.IconHeight)
                                            },
                                            new Menu_Items
                                            {
                                                Name = "Carb...",
                                                Source = "CarbonIDE",
                                                Icon = ImprovedVBE.ScaleImageStock(Resources.IDE, Global_integers.IconWidth, Global_integers.IconHeight)
                                            },
                                            new Menu_Items
                                            {
                                                Name = "File...",
                                                Source = "Explorer",
                                                Icon = ImprovedVBE.ScaleImageStock(Resources.Explorer, Global_integers.IconWidth, Global_integers.IconHeight)
                                            },
                                            new Menu_Items
                                            {
                                                Name = "Game...",
                                                Source = "Gameboy",
                                                Icon = ImprovedVBE.ScaleImageStock(Resources.Gameboy, Global_integers.IconWidth, Global_integers.IconHeight)
                                            },
                                            new Menu_Items
                                            {
                                                Name = "Sett...",
                                                Source = "Settings",
                                                Icon = ImprovedVBE.ScaleImageStock(Resources.Settings, Global_integers.IconWidth, Global_integers.IconHeight)
                                            },
                                            new Menu_Items
                                            {
                                                Name = "Note...",
                                                Source = "Notepad",
                                                Icon = ImprovedVBE.ScaleImageStock(Resources.Notepad, Global_integers.IconWidth, Global_integers.IconHeight)
                                            },
                                            new Menu_Items
                                            {
                                                Name = "Term...",
                                                Source = "Terminal",
                                                Icon = ImprovedVBE.ScaleImageStock(Resources.Terminal, Global_integers.IconWidth, Global_integers.IconHeight)
                                            },
                                            new Menu_Items
                                            {
                                                Name = "Webs...",
                                                Source = "WebscapeNavigator",
                                                Icon = ImprovedVBE.ScaleImageStock(Resources.Web, Global_integers.IconWidth, Global_integers.IconHeight)
                                            }
                                        };

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
                            else
                            {

                            }
                        }
                        if (MenuOpened == true)
                        {
                            Menu_Manager();
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
                        Extension_Dock = ImprovedVBE.EnableTransparencyPreRGB(Extension_Dock, (int)(Left - X_offset) - 175, Top, Extension_Dock, Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB, ImprovedVBE.cover);
                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, 60, 5);
                        BitFont.DrawBitFontString(Extension_Dock, "ArialCustomCharset16", Global_integers.c, DayOfWeek + "," + DateTime.UtcNow.Day, (int)(Extension_Dock.Width / 2 - (DayOfWeek + "," + DateTime.UtcNow.Day).Length * 4), 20);

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
                            Bitmap temp = Base.Widget_Back(ImprovedVBE.width, 45, ImprovedVBE.colourToNumber(Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB));
                            //Does transparency stuff
                            ImprovedVBE.EnableTransparencyPreRGB(temp, 0, 0, TaskBar, Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB, ImprovedVBE.cover);
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
                            BitFont.DrawBitFontString(Backup, "ArialCustomCharset16", Global_integers.c, DateTime.UtcNow.Hour.ToString() + ":" + DateTime.UtcNow.Minute, ImprovedVBE.width - 80, 5);
                            BitFont.DrawBitFontString(Backup, "ArialCustomCharset16", Global_integers.c, DayOfWeek + "," + DateTime.UtcNow.Day, ImprovedVBE.width - 100, 20);
                            //Disable if condition to only execute once every minute -> Works efficiently
                            Time = DateTime.UtcNow.Minute;
                        }
                        //Rendering the TaskBar to the Screen
                        Array.Copy(Backup.RawData, 0, ImprovedVBE.cover.RawData, 0, Backup.RawData.Length);

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
                                        ClearLists();
                                    }
                                    else if(MenuOpened == true && clicked == false)
                                    {
                                        MenuOpened = false;
                                        clicked = true;
                                    }
                                }
                            }
                            if (MouseManager.X > ImprovedVBE.width - 100 && MouseManager.X < ImprovedVBE.width)
                            {
                                if(MouseManager.Y > 0 && MouseManager.Y < Backup.Height)
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
                        }
                        else if(MouseManager.MouseState == MouseState.None)
                        {
                            clicked = false;
                        }
                        if(MenuOpened == true)
                        {
                            Dynamic_Menu(ImprovedVBE.width / 2 - 200, 50, 400, 400);
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
            }
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
                Back = ImprovedVBE.EnableTransparencyPreRGB(Back, (int)(Left + X_offset - 300), Top - 480, Back, Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB, ImprovedVBE.cover);

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
                        switch (button.Text)
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

            if (Text_Search.Length > 0)
            {
                start = true;
            }

            foreach (Menu_Items m in Items)
            {
                if (start == true)
                {
                    if (m.Source.ToLower().Contains(Text_Search.ToLower()))
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
                        if (update == true)
                        {
                            ImprovedVBE.DrawImageAlpha(m.Icon, x - ((int)(Left + X_offset - 290)) + 12, 110, Back);
                            BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Global_integers.c, m.Name, x - ((int)(Left + X_offset - 290)) + 12, 170);
                        }
                        if (co < 5)
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
            if (KeyboardManager.TryReadKey(out k))
            {
                Text_Search = Keyboard.HandleKeyboard(Text_Search, k);
                update = true;
            }
        }
        public static void Dynamic_Menu(int X, int Y, int Width, int Height)
        {
            if(update == true)
            {
                //Get the Menu
                Back = Base.Widget_Back(Width, Height, ImprovedVBE.colourToNumber(255, 255, 255));
                Back = ImprovedVBE.EnableTransparencyPreRGB(Back, X, Y, Back, Global_integers.TaskBarR, Global_integers.TaskBarG, Global_integers.TaskBarB, ImprovedVBE.cover);
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
                        Itemunified.Add(new ItemUnifier(11, 280, ImprovedVBE.colourToNumber(255, 255, 255), $"Power options:", "ArialCustomCharset16", "Power"));
                        Itemunified.Add(new ItemUnifier(11, 359, ImprovedVBE.colourToNumber(255, 255, 255), $"Hello, {Global_integers.Username}!", "ArialCustomCharset16", "Uname"));
                    }
                    //Recently used apps if the used machine is under vmware
                    if(VMTools.IsVMWare == true)
                    {
                        Items = new List<Menu_Items>();
                        string FrequentApps = File.ReadAllText("0:\\System\\FrequentApps.sys");
                        string[] Sep = FrequentApps.Split("\n");
                        for(int i = 0; i < Sep.Length; i++)
                        {
                            switch (Sep[i])
                            {
                                case "Settings":
                                    Menu_Items Settings = new Menu_Items()
                                    {
                                        Name = "Settings",
                                        Source = "Settings",
                                        Icon = ImprovedVBE.ScaleImageStock(Resources.Settings, Global_integers.IconWidth, Global_integers.IconHeight)
                                    };
                                    Items.Add(Settings);
                                    break;
                                case "Gameboy":
                                    Menu_Items Gameboy = new Menu_Items()
                                    {
                                        Name = "Gameboy",
                                        Source = "Gameboy",
                                        Icon = ImprovedVBE.ScaleImageStock(Resources.Gameboy, Global_integers.IconWidth, Global_integers.IconHeight)
                                    };
                                    Items.Add(Gameboy);
                                    break;
                                case "FileSystem":
                                    Menu_Items FS = new Menu_Items
                                    {
                                        Name = "File Explorer",
                                        Source = "File Explorer",
                                        Icon = ImprovedVBE.ScaleImageStock(Resources.Explorer, Global_integers.IconWidth, Global_integers.IconHeight)
                                    };
                                    Items.Add(FS);
                                    break;
                            }
                        }
                        int X_Axis = 12;
                        int Y_Axis = 81;
                        foreach(var v in Items)
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
                            Icon = ImprovedVBE.ScaleImageStock(ImageViewer.Nr1, Global_integers.IconWidth, Global_integers.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "Calculator",
                            Source = "Calculator",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Calculator, Global_integers.IconWidth, Global_integers.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "CarbonIDE",
                            Source = "CarbonIDE",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.IDE, Global_integers.IconWidth, Global_integers.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "File Explorer",
                            Source = "File Explorer",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Explorer, Global_integers.IconWidth, Global_integers.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "Gameboy",
                            Source = "Gameboy",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Gameboy, Global_integers.IconWidth, Global_integers.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "Settings",
                            Source = "Settings",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Settings, Global_integers.IconWidth, Global_integers.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "Notepad",
                            Source = "Notepad",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Notepad, Global_integers.IconWidth, Global_integers.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "Terminal",
                            Source = "Terminal",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Terminal, Global_integers.IconWidth, Global_integers.IconHeight)
                        },
                        new Menu_Items
                        {
                            Name = "WebscapeNavigator",
                            Source = "WebscapeNavigator",
                            Icon = ImprovedVBE.ScaleImageStock(Resources.Web, Global_integers.IconWidth, Global_integers.IconHeight)
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
                                    Array.Fill(bmp.RawData, ImprovedVBE.colourToNumber(Global_integers.IconR, Global_integers.IconG, Global_integers.IconB));
                                    ImprovedVBE.DrawImageAlpha(Items[i].Icon, 0, 0, bmp);

                                    ImprovedVBE.DrawImage(bmp, 10, Helper - 10, Buffer);
                                    BitFont.DrawBitFontString(Buffer, "ArialCustomCharset16", Color.White, Items[i].Name, 80, Helper + 10);
                                }
                                Items[i].X = 10;
                                Items[i].Y = Helper - 10;
                                Helper += (int)Items[i].Icon.Height + 15;
                            }
                        }
                        //- Vertical scrollbar
                        Itemunified.Add(new ItemUnifier(375, 68, 20, 310, 20, 0, Helper));
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
                                    Array.Fill(bmp.RawData, ImprovedVBE.colourToNumber(Global_integers.IconR, Global_integers.IconG, Global_integers.IconB));
                                    ImprovedVBE.DrawImageAlpha(Items[i].Icon, 0, 0, bmp);

                                    ImprovedVBE.DrawImage(bmp, 10, Helper - 10, Buffer);
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
                    label.Label(Back);
                }
                //Renders every scrollbar
                foreach (var vscroll in Scroll)
                {
                    vscroll.Render(Back);
                }
                //Disables the if conditional, so it only executes when needed -> performance gain
                update = false;
            }
            //NOTE: So why is this necessary? COSMOS keeps tossing out the content of the Items list for some unknown reason and this has been happening for a while now... I'm getting mad...
            //If you know why is this happening or what am I doing wrong please let me know!
            if(Items.Count == 0 && Scroll.Count != 0)
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
                        }
                    }
                }
                if(button.Clicked == false)
                {
                    Button.Button_render(Back, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                }
                else
                {
                    Button.Button_render(Back, button.X, button.Y, button.Width, button.Height, ComplimentaryColor.Generate(button.Color).ToArgb(), button.Text);
                    if (clicked == false)
                    {
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

            //Activate click on MenuItems
            if(MouseManager.MouseState == MouseState.Left)
            {
                foreach(var v in Items)
                {
                    switch (ExtendedMenu)
                    {
                        case true:
                                if (MouseManager.X > X + v.X + 10 && (MouseManager.X < X + Width - v.X - 20 || MouseManager.X < X + v.X + v.Icon.Width))
                                {
                                    if (MouseManager.Y > Y + v.Y + 68 && MouseManager.Y < Y + v.Y + v.Icon.Height + 68 && v.Y > 0 && v.Y < Buffer.Height)
                                    {
                                        AppDecider(v.Source, v.Icon);
                                        ExtendedMenu = false;
                                        ClearLists();
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

            //Render the menu
            ImprovedVBE.DrawImageAlpha(Back, X, Y, ImprovedVBE.cover);
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
                    Calculator c = new Calculator();//These need some update to make it easier to work with
                    c.x = 100;
                    c.y = 100;
                    c.width = 200;
                    c.height = 380;
                    c.name = "Calc...";
                    c.z = 999;
                    c.icon = Icon;
                    TaskScheduler.Apps.Add(c);
                    MenuOpened = false;
                    break;
                case "File Explorer":
                    Applications.FileSys.FileSystem d = new Applications.FileSys.FileSystem();//Same here ;)
                    d.x = 100;
                    d.y = 100;
                    d.width = 650;
                    d.height = 380;
                    d.name = "File...";
                    d.z = 999;
                    d.icon = Icon;
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

                    r.icon = Icon;

                    TaskScheduler.Apps.Add(r);
                    MenuOpened = false;
                    break;
                case "Settings":
                    Settings settings = new Settings();
                    settings.x = 100;
                    settings.y = 100;
                    settings.width = 550;
                    settings.height = 380;
                    settings.name = "Sett...";
                    settings.z = 999;
                    settings.icon = Icon;
                    TaskScheduler.Apps.Add(settings);
                    MenuOpened = false;
                    break;
                case "Gameboy":
                    Core Gameboy = new Core();
                    Gameboy.x = 100;
                    Gameboy.y = 100;
                    Gameboy.width = 162 * 3 - 4;
                    Gameboy.height = 165 * 3 - 39 + 25;
                    Gameboy.name = "Gameboy";
                    Gameboy.z = 999;
                    Gameboy.icon = Icon;
                    TaskScheduler.Apps.Add(Gameboy);
                    MenuOpened = false;
                    break;
                case "Notepad":
                    Notepad Notepad = new Notepad();
                    Notepad.x = 100;
                    Notepad.y = 100;
                    Notepad.width = 700;
                    Notepad.height = 420;
                    Notepad.name = "Note...";
                    Notepad.z = 999;
                    Notepad.icon = Icon;
                    TaskScheduler.Apps.Add(Notepad);
                    MenuOpened = false;
                    break;
                case "Terminal":
                    Terminal Terminal = new Terminal();
                    Terminal.x = 100;
                    Terminal.y = 100;
                    Terminal.width = 700;
                    Terminal.height = 420;
                    Terminal.name = "Term...";
                    Terminal.z = 999;
                    Terminal.icon = Icon;
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
                    WebscapeNavigator.width = 700;
                    WebscapeNavigator.height = 420;
                    WebscapeNavigator.z = 999;
                    WebscapeNavigator.source = "example.com/index.html";
                    WebscapeNavigator.icon = ImprovedVBE.ScaleImageStock(Resources.Web, 56, 56);
                    WebscapeNavigator.name = "Webs... - " + "example.com/index.html";

                    TaskScheduler.Apps.Add(WebscapeNavigator);
                    MenuOpened = false;
                    break;
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
            TaskManager.Buttons.Add(new Button_prop(x, y - 22, width, height, content, color, id));
        }
        public ItemUnifier(int x, int y, int color, string content, string FonType, string id)
        {
            TaskManager.Label.Add(new label(x, y - 22, content, FonType, color, id));
        }
        public ItemUnifier(int x, int y, int width, int height, int position, int MinVal, int MaxVal)
        {
            TaskManager.Scroll.Add(new VerticalScrollbar(x, y, width, height, position, MinVal, MaxVal));
        }
    }
}
