using Cosmos.System;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Programming;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Kernel = CrystalOS_Alpha.Kernel;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;

namespace CrystalOSAlpha.Applications.FileSys
{
    class FileSystem : App
    {
        #region important
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int desk_ID {get; set; }
        public int AppID { get; set; }
        public string name {get; set;}

        public bool minimised { get; set; }
        public bool movable { get; set; }
        
        public Bitmap icon { get; set; }
        #endregion important

        public int CurrentColor = ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B);
        public int Reg_Y = 0;
        public int offset = 0;
        public int index = 0;
        public int offset2 = 0;

        public string command = "";
        public string Route = @"0:\";
        public static string PathRightClick = "";
        public string ToCopy = "";
        public string ToCopyFilename = "";

        public bool initial = true;
        public bool clicked = false;
        public bool temp = true;
        public bool once = true;
        public bool Cut = false;

        public Bitmap canvas;
        public Bitmap window { get; set; }
        public Bitmap Container;
        public Bitmap side;
        public Bitmap Main;
        public Bitmap Logo;

        public Structure Selected;

        public List<string> cmd_history = new List<string>();
        public List<string> MItems = new List<string>();
        public List<Button_prop> Buttons = new List<Button_prop>();
        public List<VerticalScrollbar> Scroll = new List<VerticalScrollbar>();
        public List<Structure> Items = new List<Structure>();
        public List<RightClick> rightClicks = new List<RightClick>();
        public List<TextBox> TextBoxes = new List<TextBox>();

        public void App()
        {
            if (initial == true)
            {
                Buttons.Add(new Button_prop(3, 2, 49, 25, "Home", 1, "Home"));
                Buttons.Add(new Button_prop(59, 2, 105, 25, "Wastebasket", 1, "Wastebasket"));
                Buttons.Add(new Button_prop(171, 2, 105, 25, "Security Area", 1, "Security Area"));
                Buttons.Add(new Button_prop(3, 31, 20, 20, "<-", 1, "Back"));
                Buttons.Add(new Button_prop(32, 31, 20, 20, "->", 1, "Forward"));
                Buttons.Add(new Button_prop(59, 31, 35, 20, "Up", 1, "Up"));

                Scroll.Add(new VerticalScrollbar(width - 30, 77, 20, height - 85, 20, 0, 800));

                TextBoxes.Add(new TextBox(283, 24, 330, 25, ImprovedVBE.colourToNumber(60, 60, 60), "", "Type here to search...", TextBox.Options.left, "Search"));
                TextBoxes.Add(new TextBox(99, 53, 541, 20, ImprovedVBE.colourToNumber(60, 60, 60), "0:\\", "0:\\", TextBox.Options.left, "Path"));

                Logo = ImprovedVBE.ScaleImageStock(new Bitmap(TaskManager.Elephant), 25, 25);

                initial = false;
            }
            if (once == true)
            {
                Bitmap back = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                (canvas, back, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);
                side = new Bitmap(118, (uint)height - 85, ColorDepth.ColorDepth32);
                Main = new Bitmap(487, (uint)height - 85, ColorDepth.ColorDepth32);

                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);

                        switch (button.Text)
                        {
                            case "Back":
                                if(Route.Length > 3)
                                {
                                    Route = Route.Remove(Route.LastIndexOf("\\"));
                                }
                                break;
                            case "Forward":
                                break;
                        }
                    }
                    else
                    {
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                    }
                }

                Array.Fill(side.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                Array.Fill(Main.RawData, ImprovedVBE.colourToNumber(36, 36, 36));

                //Quick access menu
                BitFont.DrawBitFontString(side, "ArialCustomCharset16", Color.LightGray, "Quick access", 3, 3);
                //Favorites
                Color ForText = Color.Orange;
                BitFont.DrawBitFontString(side, "ArialCustomCharset16", ForText, "Favorites", 37, 27);
                BitFont.DrawBitFontString(side, "ArialCustomCharset16", ForText, "Documents", 37, 42);
                BitFont.DrawBitFontString(side, "ArialCustomCharset16", ForText, "Pictures", 37, 57);
                BitFont.DrawBitFontString(side, "ArialCustomCharset16", ForText, "Films", 37, 72);

                //This PC
                BitFont.DrawBitFontString(side, "ArialCustomCharset16", Color.LightGray, "This PC", 3, 98);
                //Draw out every drive
                int Top = 120;
                for(int i = 0; i < Kernel.fs.GetDisks().Count; i++)
                {
                    BitFont.DrawBitFontString(side, "ArialCustomCharset16", ForText, "Disk" + i + ":", 37, Top);
                    Top += 22;
                }

                ImprovedVBE.DrawImage(side, 3, 77, canvas);
                ImprovedVBE.DrawImage(Main, 126, 77, canvas);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                once = false;
                temp = true;
            }

            foreach (var button in Buttons)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > x + button.X && MouseManager.X < x + button.X + button.Width)
                    {
                        if (MouseManager.Y > y + button.Y && MouseManager.Y < y + button.Y + button.Height)
                        {
                            if (clicked == false)
                            {
                                button.Clicked = true;
                                once = true;
                                clicked = true;
                            }
                        }
                    }
                }
                else if(MouseManager.MouseState == MouseState.None)
                {
                    if(button.Clicked == true)
                    {
                        button.Clicked = false;
                        once = true;
                    }

                }
            }

            foreach (var vscroll in Scroll)
            {
                if (vscroll.CheckClick((int)MouseManager.X - x, (int)MouseManager.Y - y))
                {
                    temp = true;
                }
            }

            foreach (var Box in TextBoxes)
            {
                if (Box.Clciked(x + Box.X, y + Box.Y) == true && clicked == false)
                {
                    foreach (var box2 in TextBoxes)
                    {
                        box2.Selected = false;
                    }
                    clicked = true;
                    Box.Selected = true;
                }
            }

            if (MouseManager.MouseState == MouseState.None && clicked == true)
            {
                once = true;
                clicked = false;
            }

            if (TaskScheduler.Apps[^1] == this)
            {
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    if (key.Key == ConsoleKeyEx.Enter)
                    {
                        Route = TextBoxes[1].Text;
                    }
                    else
                    {
                        foreach (var box in TextBoxes)
                        {
                            if (box.Selected == true)
                            {
                                box.Text = Keyboard.HandleKeyboard(box.Text, key);
                                temp = true;
                            }
                        }
                    }
                    Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                    temp = true;
                }
            }
            if(temp == true)
            {
                Items.Clear();
                foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(Route))
                {
                    if (d.mEntryType == DirectoryEntryTypeEnum.Directory)
                    {
                        Items.Add(new Structure(d.mName, d.mFullPath, Opt.Folder));
                    }
                }
                foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(Route))
                {
                    if (d.mEntryType == DirectoryEntryTypeEnum.File)
                    {
                        Items.Add(new Structure(d.mName, d.mFullPath, Opt.File));
                    }
                }
                Array.Fill(Main.RawData, ImprovedVBE.colourToNumber(36, 36, 36));

                Bitmap Folder = new Bitmap(30, 30, ColorDepth.ColorDepth32);
                Array.Fill(Folder.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawImageAlpha(ImprovedVBE.ScaleImageStock(Resources.Folder, 28, 28), 0, 0, Folder);

                Bitmap File = new Bitmap(30, 30, ColorDepth.ColorDepth32);
                Array.Fill(File.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawImageAlpha(ImprovedVBE.ScaleImageStock(Resources.File, 28, 28), 0, 0, File);

                int x_off = 14;
                int y_off = 9 - Scroll[0].Value;
                int indicator = 0;
                foreach(var entry in Items)
                {
                    entry.X = x_off;
                    entry.Y = y_off;
                    if (entry.fullPath == Selected.fullPath)
                    {
                        ImprovedVBE.DrawFilledRectangle(Main, ImprovedVBE.colourToNumber(69, 69, 69), x_off - 5, y_off - 5, 65, 65, false);
                    }
                    if (entry.type == Opt.Folder)
                    {
                        ImprovedVBE.DrawImage(Folder, x_off, y_off, Main);
                        if(entry.name.Length > 5)
                        {
                            BitFont.DrawBitFontString(Main, "ArialCustomCharset16", Color.White, entry.name.Remove(5) + "...", x_off, y_off + 38);
                        }
                        else
                        {
                            BitFont.DrawBitFontString(Main, "ArialCustomCharset16", Color.White, entry.name, x_off, y_off + 38);
                        }
                    }
                    if(entry.type == Opt.File)
                    {
                        ImprovedVBE.DrawImage(File, x_off, y_off, Main);
                        if (entry.name.Length > 5)
                        {
                            BitFont.DrawBitFontString(Main, "ArialCustomCharset16", Color.White, entry.name.Remove(5) + "...", x_off, y_off + 38);
                        }
                        else
                        {
                            BitFont.DrawBitFontString(Main, "ArialCustomCharset16", Color.White, entry.name.Remove(entry.name.Length - 4), x_off, y_off + 38);
                        }
                    }
                    x_off += 62;
                    if(x_off / 62 >= 7)
                    {
                        y_off += 60;
                        x_off = 9;
                    }
                    indicator++;
                }
                ImprovedVBE.DrawImage(Main, 126, 77, window);
                //Render scrollbar
                foreach (var vscroll in Scroll)
                {
                    vscroll.Render(window);
                }
                foreach(var Box in TextBoxes)
                {
                    Box.Box(window, Box.X, Box.Y);
                }
                ImprovedVBE.DrawImageAlpha(Logo, 615, 24, window);
                temp = false;
            }

            //Click Detection:
            if(MouseManager.MouseState == MouseState.Left)
            {
                //Going thru every item in Items
                foreach(var entry in Items)
                {
                    if(MouseManager.X > x + 126 + entry.X && MouseManager.X < x + 126 + entry.X + 35)
                    {
                        if (MouseManager.Y > y + 77 + entry.Y && MouseManager.Y < y + 77 + entry.Y + 50 && entry.Y > 0 && entry.Y < Main.Height - 50)
                        {
                            if(clicked == false)
                            {
                                foreach(var v in Items)
                                {
                                    v.Selected = false;
                                }
                                if(entry.type == Opt.File)
                                {
                                    if (entry.name.ToLower().EndsWith(".html"))
                                    {
                                        WebscapeNavigator.Webscape wn = new WebscapeNavigator.Webscape();
                                        wn.content = File.ReadAllText(entry.fullPath);
                                        wn.x = 100;
                                        wn.y = 100;
                                        wn.width = 700;
                                        wn.height = 420;
                                        wn.z = 999;
                                        wn.source = entry.fullPath;
                                        wn.icon = ImprovedVBE.ScaleImageStock(Resources.Notepad, 56, 56);
                                        wn.name = "Webs... - " + entry.name;

                                        TaskScheduler.Apps.Add(wn);
                                    }
                                    else if (entry.name.ToLower().EndsWith(".cmd"))
                                    {
                                        CSharp c = new CSharp();
                                        c.Executor(File.ReadAllText(entry.fullPath));
                                    }
                                    else if (entry.name.ToLower().EndsWith(".app"))
                                    {
                                        TaskScheduler.Apps.Add(new Window(100, 100, 999, 350, 200, 0, "Untitled", false, icon, File.ReadAllText(entry.fullPath)));
                                    }
                                    else if (entry.name.ToLower().EndsWith(".bin"))
                                    {

                                    }
                                    else if (entry.name.ToLower().EndsWith(".bmp"))
                                    {
                                        //Time to write an image viewer app!
                                        //Not today loser!
                                    }
                                    else
                                    {
                                        Applications.Notepad.Notepad n = new Notepad.Notepad();
                                        n.content = File.ReadAllText(entry.fullPath);
                                        n.x = 100;
                                        n.y = 100;
                                        n.width = 700;
                                        n.height = 420;
                                        n.z = 999;
                                        n.source = entry.fullPath;
                                        n.icon = ImprovedVBE.ScaleImageStock(Resources.Notepad, 56, 56);
                                        n.name = "Note... - " + entry.name;

                                        TaskScheduler.Apps.Add(n);
                                    }
                                }
                                else if(entry.type == Opt.Folder)
                                {
                                    TextBoxes[1].Text = entry.fullPath;
                                    Route = entry.fullPath;
                                    temp = true;
                                }
                                entry.Selected = true;
                                clicked = true;
                                temp = true;
                            }
                        }
                    }
                }
                //Click on disk entries
                int Top = 120;
                for (int i = 0; i < Kernel.fs.GetDisks().Count; i++)
                {
                    if(MouseManager.X > x + 3 + 37 && MouseManager.X < x + 3 + side.Width)
                    {
                        if (MouseManager.Y > y + 77 + Top && MouseManager.Y < y + 77 + Top + 22)
                        {
                            TextBoxes[1].Text = i + ":\\";
                            Route = i + ":\\";
                            temp = true;
                        }
                    }
                    Top += 22;
                }
                //Click on shortcuts
                Top = 104;
                for (int i = 0; i < 4; i++)
                {
                    if (MouseManager.X > x + 3 + 37 && MouseManager.X < x + 3 + side.Width)
                    {
                        if (MouseManager.Y > y + 77 + Top && MouseManager.Y < y + 77 + Top + 22)
                        {
                            switch (i)
                            {
                                case 0:
                                    TextBoxes[1].Text = "0:\\User\\" + Global_integers.Username + "\\Favorites";
                                    Route = "0:\\User\\" + Global_integers.Username + "\\Favorites";
                                    temp = true;
                                    break;
                            }
                        }
                    }
                    Top += 22;
                }
            }
            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }

        public void RightClick()
        {
            //Adding the base of the contextmenu
            if (MouseManager.MouseState == MouseState.Right && clicked == false)
            {
                rightClicks.Clear();
                MItems.Clear();
                bool Find = false;
                foreach(var v in Items)
                {
                    v.Selected = false;
                    if(MouseManager.X > x + 126 + v.X && MouseManager.X < x + 126 + v.X + 50)
                    {
                        if (MouseManager.Y > y + 77 + v.Y && MouseManager.Y < y + 77 + v.Y + 50)
                        {
                            Find = true;
                            v.Selected = true;
                            temp = true;
                            if(v.type == Opt.File)
                            {
                                MItems = new List<string>
                                {
                                    "Open",
                                    "Open with:Extensive:",
                                    "Copy as path",
                                    "Add to Favorites",
                                    "Create shortcut",
                                    "Propeties",
                                    "Filesystem operations:Extensive:"
                                };
                                rightClicks.Add(new UI_Elements.RightClick((int)MouseManager.X, (int)MouseManager.Y, 200, 500, MItems, "FiMain"));
                            }
                            Selected = v;
                            ToCopyFilename = v.name;
                        }
                    }
                }
                if(Find == false)
                {
                    MItems = new List<string>
                                {
                                    "Open",
                                    "Open with:Extensive:",
                                    "Copy as path",
                                    "Add to Favorites",
                                    "Create shortcut",
                                    "Propeties",
                                    "Filesystem operations:Extensive:"
                                };
                    rightClicks.Add(new UI_Elements.RightClick((int)MouseManager.X, (int)MouseManager.Y, 200, 500, MItems, "FiMain"));
                    Selected = null;
                    temp = true;
                }
                if(Items.Count == 0)
                {
                    MItems = new List<string>
                                {
                                    "Open",
                                    "Open with:Extensive:",
                                    "Copy as path",
                                    "Add to Favorites",
                                    "Create shortcut",
                                    "Propeties",
                                    "Filesystem operations:Extensive:"
                                };
                    rightClicks.Add(new UI_Elements.RightClick((int)MouseManager.X, (int)MouseManager.Y, 200, 500, MItems, "FiMain"));
                }
                clicked = true;
            }
            //Reenabling right click
            else if(MouseManager.MouseState == MouseState.None && clicked == true)
            {
                clicked = false;
            }
            //Render the contextmenus + logic
            int Pointer = 0;
            bool Found = false;
            foreach(var v in rightClicks)
            {
                v.ProcessNRender();
                if(MouseManager.MouseState == MouseState.Left && clicked == false)
                {
                    switch (v.ID)
                    {
                        //Two of the main cases
                        case "FiMain":
                            switch (v.Selected)
                            {
                                case 0:
                                    //Open the chosen file/directory
                                    break;
                                case 1:
                                    MItems = new List<string>
                                    {
                                        "Notepad",
                                        "Binary reader",
                                    };
                                    if(!rightClicks.Exists(d => d.ID == "OpenWith"))
                                    {
                                        rightClicks.Add(new UI_Elements.RightClick(v.X + v.Width - 5, v.Y + v.Selected * 25 + 10, 200, 500, MItems, "OpenWith"));
                                        clicked = true;
                                    }
                                    else
                                    {
                                        rightClicks.RemoveRange(Pointer + 1, rightClicks.Count - Pointer - 1);
                                        clicked = true;
                                    }
                                    break;
                                case 2:
                                    //Add to Favorites
                                    break;
                                case 3:
                                    //TODO: Clipboard implementation
                                    //Copy the path
                                    break;
                                case 4:
                                    //Create a shortcut to the desktop
                                    break;
                                case 5:
                                    //Propeties of file/folders
                                    break;
                                case 6:
                                    MItems = new List<string>
                                    {
                                        "Copy",
                                        "Paste",
                                        "Cut",
                                        "Move",
                                        "Rename",
                                        "Delete"
                                    };
                                    if (!rightClicks.Exists(d => d.ID == "FIFSOPerations"))
                                    {
                                        rightClicks.Add(new UI_Elements.RightClick(v.X + v.Width - 5, v.Y + v.Selected * 25 + 10, 200, 500, MItems, "FIFSOPerations"));
                                        clicked = true;
                                    }
                                    else
                                    {
                                        rightClicks.RemoveRange(Pointer + 1, rightClicks.Count - Pointer - 1);
                                        clicked = true;
                                    }
                                    break;
                            }
                            break;

                        //Side options
                        case "OpenWith":
                            switch (v.Selected)
                            {
                                case 0:
                                    //Do it with file explorer
                                    break;
                                case 1:
                                    //Do it with notepad
                                    break;
                                case 2:
                                    //Do it with binary reader: Not implemented
                                    break;
                            }
                            break;
                        case "FIFSOPerations":
                            switch (v.Selected)
                            {
                                case 0:
                                    //Copy file name and path
                                    ToCopy = Selected.fullPath;
                                    break;
                                case 1:
                                    //Paste a file
                                    if(TextBoxes[1].Text.Length > 3)
                                    {
                                        File.Create(TextBoxes[1].Text + "\\" + ToCopy.Split("\\")[^1]);
                                        File.WriteAllText(TextBoxes[1].Text + "\\" + ToCopy.Split("\\")[^1], File.ReadAllText(ToCopy));
                                        if(Cut == true)
                                        {
                                            File.Delete(ToCopy);
                                            Cut = false;
                                        }
                                    }
                                    else
                                    {
                                        File.Create(TextBoxes[1].Text + ToCopy.Split("\\")[^1]);
                                        File.WriteAllText(TextBoxes[1].Text + ToCopy.Split("\\")[^1], File.ReadAllText(ToCopy));
                                        if (Cut == true)
                                        {
                                            File.Delete(ToCopy);
                                            Cut = false;
                                        }
                                    }
                                    break;
                                case 2:
                                    //Cut out a file
                                    ToCopy = Selected.fullPath;
                                    Cut = true;
                                    break;
                                case 5:
                                    //TODO: Ask the user if he actually wants to deleate it. And to place it into Wastebasket or to remove it entirely.
                                    File.Delete(Selected.fullPath);
                                    break;
                            }
                            break;
                    }
                    if(v.Selected < 0)
                    {
                        Found = true;
                    }
                    else
                    {
                        Found = false;
                    }
                }
                Pointer++;
            }
            if(Found == true)
            {
                rightClicks.Clear();
            }
        }
    }

    public class Structure
    {
        public string name { get; set;}
        public string fullPath { get; set;}
        public Opt type { get; set; }
        public int X { get; set;}
        public int Y { get; set;}
        public bool Selected { get; set;}
        public Structure(string name, string fullPath, Opt type)
        {
            this.name = name;
            this.fullPath = fullPath;
            this.type = type;
        }
    }

    public enum Opt
    {
        File,
        Folder
    }
}
