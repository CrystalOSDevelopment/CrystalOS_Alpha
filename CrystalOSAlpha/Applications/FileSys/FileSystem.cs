using Cosmos.System;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications.Terminal;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Programming;
using CrystalOSAlpha.Programming.CrystalSharp;
using CrystalOSAlpha.System32;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using Kernel = CrystalOS_Alpha.Kernel;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;

namespace CrystalOSAlpha.Applications.FileSys
{
    class FileSystem : App
    {
        public FileSystem(int X, int Y, int Z, int Width, int Height, string Name, Bitmap Icon)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
            this.width = Width;
            this.height = Height;
            this.name = Name;
            this.icon = Icon;
        }

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

        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public int Reg_Y = 0;
        public int offset = 0;
        public int index = 0;
        public int offset2 = 0;

        public string command = "";
        public string Route = @"0:\";
        public static string PathRightClick = "";
        public string ToCopy = "";
        public string ToCopyFilename = "";
        public string returnvalue = null;

        public bool initial = true;
        public bool clicked = false;
        public bool temp = true;
        public bool once { get; set; }
        public bool Cut = false;

        public Bitmap canvas;
        public Bitmap window { get; set; }
        public Bitmap Container;
        public Bitmap side;
        public Bitmap Main;
        public Bitmap Logo;

        public Structure Selected;

        public List<string> MItems = new List<string>();
        public List<string> History = new List<string>();
        public List<string> HistoryForward = new List<string>();
        public List<Structure> Items = new List<Structure>();
        public List<RightClick> rightClicks = new List<RightClick>();

        public List<UIElementHandler> UIElements = new List<UIElementHandler>();

        public void App()
        {
            if (initial == true)
            {
                if(Kernel.IsDiskSupport == false)
                {
                    throw new Exception("Failed to open FileManager:\nFilesystem not initialized!");
                }
                UIElements.Add(new Button(3, 2, 49, 25, "Home", 1, "Home"));
                UIElements.Add(new Button(59, 2, 105, 25, "Wastebasket", 1, "Wastebasket"));
                UIElements.Add(new Button(171, 2, 105, 25, "Security Area", 1, "Security Area"));
                UIElements.Add(new Button(3, 31, 20, 20, "<-", 1, "Back"));
                UIElements.Add(new Button(32, 31, 20, 20, "->", 1, "Forward"));
                UIElements.Add(new Button(59, 31, 35, 20, "Up", 1, "Up"));

                UIElements.Add(new VerticalScrollbar(width - 30, 77, 20, height - 85, 20, 0, 800, "ScrollRight"));

                UIElements.Add(new TextBox(283, 24, width - 320, 25, ImprovedVBE.colourToNumber(60, 60, 60), "", "Type here to search...", TextBox.Options.left, "Search"));
                UIElements.Add(new TextBox(99, 53, width - 109, 20, ImprovedVBE.colourToNumber(60, 60, 60), "0:\\", "0:\\", TextBox.Options.left, "Path"));

                Logo = ImprovedVBE.ScaleImageStock(new Bitmap(TaskManager.Elephant), 25, 25);

                initial = false;
            }
            if (once == true)
            {
                Bitmap back = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                (canvas, back, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);
                side = new Bitmap(118, (uint)height - 85, ColorDepth.ColorDepth32);
                Main = new Bitmap((uint)width - 163, (uint)height - 85, ColorDepth.ColorDepth32);

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

            if(TaskScheduler.Apps.FindAll(d => d.name.Contains("Error")).Count == 0)
            {
                foreach (var elements in UIElements)
                {
                    if (MouseManager.MouseState == MouseState.Left)
                    {
                        if (MouseManager.X > x + elements.X && MouseManager.X < x + elements.X + elements.Width)
                        {
                            if (MouseManager.Y > y + elements.Y && MouseManager.Y < y + elements.Y + elements.Height)
                            {
                                switch (elements.TypeOfElement)
                                {
                                    case TypeOfElement.Button:
                                        if (clicked == false)
                                        {
                                            elements.Clicked = true;
                                            temp = true;
                                            clicked = true;
                                        }
                                        break;
                                    case TypeOfElement.TextBox:
                                        foreach (var v in UIElements)
                                        {
                                            v.Clicked = false;
                                        }
                                        elements.Clicked = true;
                                        break;
                                }
                            }
                            else
                            {
                                if (elements.Clicked == true)
                                {
                                    temp = true;
                                    elements.Clicked = false;
                                    clicked = false;
                                }
                            }
                        }
                        else
                        {
                            if (elements.Clicked == true)
                            {
                                temp = true;
                                elements.Clicked = false;
                                clicked = false;
                            }
                        }
                    }
                    if (elements.TypeOfElement != TypeOfElement.TextBox && elements.Clicked == true && MouseManager.MouseState == MouseState.None)
                    {
                        temp = true;
                        elements.Clicked = false;
                        clicked = false;
                    }
                    if(elements.TypeOfElement == TypeOfElement.VerticalScrollbar)
                    {
                        if (elements.CheckClick((int)MouseManager.X - x, (int)MouseManager.Y - y))
                        {
                            temp = true;
                        }
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
                            Route = UIElements.Find(d => d.ID == "Path").Text;
                        }
                        else
                        {
                            foreach (var element in UIElements)
                            {
                                if (element.Clicked == true)
                                {
                                    element.Text = Keyboard.HandleKeyboard(element.Text, key);
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
                    temp = false;
                    Items.Clear();
                    try
                    {
                        foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(Route))
                        {
                            if (d.mEntryType == DirectoryEntryTypeEnum.Directory)
                            {
                                //Selecting if the file/directory matches the criteria of textbox 0
                                if (UIElements.Find(d => d.ID == "Search").Text == "")
                                {
                                    Items.Add(new Structure(d.mName, d.mFullPath, Opt.Folder));
                                }
                                else if(UIElements.Find(d => d.ID == "Search").Text != "" && !UIElements.Find(d => d.ID == "Search").Text.Contains("."))
                                {
                                    if (d.mName.ToLower().Contains(UIElements.Find(d => d.ID == "Search").Text.ToLower()))
                                    {
                                        Items.Add(new Structure(d.mName, d.mFullPath, Opt.Folder));
                                    }
                                }
                            }
                        }
                        foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(Route))
                        {
                            if (d.mEntryType == DirectoryEntryTypeEnum.File)
                            {
                                //Selecting if the file/directory matches the criteria of textbox 0
                                if (UIElements.Find(d => d.ID == "Search").Text == "" || UIElements.Find(d => d.ID == "Search").Text.StartsWith("*") && !UIElements.Find(d => d.ID == "Search").Text.Contains("."))
                                {
                                    Items.Add(new Structure(d.mName, d.mFullPath, Opt.File));
                                }
                                else if (UIElements.Find(d => d.ID == "Search").Text.Contains("."))
                                {
                                    string[] sides = UIElements.Find(d => d.ID == "Search").Text.Split(".");
                                    if (sides[0] == "*" && d.mName.ToLower().Contains("." + sides[1].ToLower()))
                                    {
                                        Items.Add(new Structure(d.mName, d.mFullPath, Opt.File));
                                    }
                                    else if (d.mName.ToLower().Contains(sides[0]) && d.mName.ToLower().Contains("." + sides[1].ToLower()))
                                    {
                                        Items.Add(new Structure(d.mName, d.mFullPath, Opt.File));
                                    }
                                }
                                else
                                {
                                    if (d.mName.ToLower().Contains(UIElements.Find(d => d.ID == "Search").Text.ToLower()))
                                    {
                                        Items.Add(new Structure(d.mName, d.mFullPath, Opt.File));
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        TaskScheduler.Apps.Add(new MsgBox(999, ImprovedVBE.width / 2 - 200, ImprovedVBE.height / 2 - 100, 400, 200, "Error!", ex.Message, icon));
                        Route = "0:\\";
                        UIElements.Find(d => d.ID == "Path").Text = "0:\\";
                    }
                    Array.Fill(Main.RawData, ImprovedVBE.colourToNumber(36, 36, 36));

                    Bitmap Folder = new Bitmap(30, 30, ColorDepth.ColorDepth32);
                    Array.Fill(Folder.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                    ImprovedVBE.DrawImageAlpha(ImprovedVBE.ScaleImageStock(Resources.Folder, 28, 28), 0, 0, Folder);

                    Bitmap File = new Bitmap(30, 30, ColorDepth.ColorDepth32);
                    Array.Fill(File.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                    ImprovedVBE.DrawImageAlpha(ImprovedVBE.ScaleImageStock(Resources.File, 28, 28), 0, 0, File);

                    int x_off = 14;
                    int y_off = 9 - UIElements.Find(d => d.ID == "ScrollRight").Value;
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
                            TransportedIcons.FirstFolder(Main, x_off, y_off);
                            //ImprovedVBE.DrawImage(Folder, x_off, y_off, Main);
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
                            //ImprovedVBE.DrawImage(File, x_off, y_off, Main);
                            TransportedIcons.FirstIcon(Main, x_off, y_off);
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
                        if(x_off >= Main.Width - 72)
                        {
                            y_off += 70;
                            x_off = 9;
                        }
                        indicator++;
                    }
                    ImprovedVBE.DrawImage(Main, 126, 77, window);
                    //Render scrollbar
                    foreach(var Box in UIElements)
                    {
                        Box.Render(window);
                        if(Box.TypeOfElement == TypeOfElement.TextBox)
                        {
                            switch (Box.ID)
                            {
                                case "Search":
                                    Box.Width = width - 320;
                                    break;
                                case "Path":
                                    Box.Width = width - 109;
                                    break;
                            }
                            Box.Render(window);
                        }
                        else if (Box.Clicked == true && Box.TypeOfElement == TypeOfElement.Button)
                        {
                            int Col = Box.Color;
                            Box.Color = Color.White.ToArgb();
                            Box.Render(window);
                            Box.Color = Col;

                            switch (Box.ID)
                            {
                                case "Home":
                                    UIElements.Find(d => d.ID == "Path").Text = "0:\\";
                                    Route = "0:\\";
                                    break;
                                case "Wastebasket":
                                    UIElements.Find(d => d.ID == "Path").Text = "0:\\User\\" + GlobalValues.Username + "\\Wastebasket";
                                    Route = "0:\\User\\" + GlobalValues.Username + "\\Wastebasket";
                                    break;
                                case "Security Area":
                                    UIElements.Find(d => d.ID == "Path").Text = "0:\\User\\" + GlobalValues.Username + "\\SecurityArea";
                                    Route = "0:\\User\\" + GlobalValues.Username + "\\SecurityArea";
                                    break;
                                case "Back":
                                    if (History.Count > 0)
                                    {
                                        HistoryForward.Add(History[^1]);
                                        History.RemoveAt(History.Count - 1);
                                        if (History.Count > 0)
                                        {
                                            UIElements.Find(d => d.ID == "Path").Text = History[^1];
                                            Route = History[^1];
                                        }
                                        else
                                        {
                                            History.Clear();
                                            History = new List<string>
                                        {
                                            "0:\\"
                                        };
                                            UIElements.Find(d => d.ID == "Path").Text = History[^1];
                                            Route = History[^1];
                                        }
                                    }
                                    break;
                                case "Forward":
                                    if (HistoryForward.Count > 0)
                                    {
                                        History.Add(HistoryForward[^1]);
                                        HistoryForward.RemoveAt(HistoryForward.Count - 1);
                                        UIElements.Find(d => d.ID == "Path").Text = History[^1];
                                        Route = History[^1];
                                    }
                                    break;
                            }
                        }
                        else if(Box.TypeOfElement == TypeOfElement.VerticalScrollbar)
                        {
                            Box.X = width - 30;
                            Box.Height = height - 85;
                            Box.Render(window);
                        }
                    }
                    ImprovedVBE.DrawImageAlpha(Logo, width - (int)Logo.Width - 5, 24, window);
                }

                #region Rename & Create file
                var Rename = WindowMessenger.Recieve("Rename", "FileSystem");
                var Create = WindowMessenger.Recieve("Create", "FileSystem");
                if (Rename != null)
                {
                    returnvalue = Rename.Message;
                    WindowMessenger.Message.RemoveAll(d => d.Message == Rename.Message);
                    Create = null;
                }
                if (Create != null)
                {
                    returnvalue = Create.Message;
                    WindowMessenger.Message.RemoveAll(d => d.Message == Create.Message);
                    Rename = null;
                }
                if (Rename != null)
                {
                    if(Rename.Message != "")
                    {
                        byte[] content = File.ReadAllBytes(Selected.fullPath);
                        File.Delete(Selected.fullPath);
                        string s = Selected.fullPath.Replace(Selected.name, "");
                        File.Create(s + returnvalue);
                        File.WriteAllBytes(s + returnvalue, content);
                    }
                    else
                    {
                        TaskScheduler.Apps.Add(new MsgBox(999, ImprovedVBE.width / 2 - 200, ImprovedVBE.height / 2 - 100, 400, 200, "Error", "Failed to rename file:\n    Wrong filename format", icon));
                    }
                    temp = true;
                }
                else if (Create != null)
                {
                    if(Create.Message != "")
                    {
                        if (Route.EndsWith("\\"))
                        {
                            File.Create(Route + Create.Message);
                        }
                        else
                        {
                            File.Create(Route + "\\" + Create.Message);
                        }
                    }
                    else
                    {
                        TaskScheduler.Apps.Add(new MsgBox(999, ImprovedVBE.width / 2 - 200, ImprovedVBE.height / 2 - 100, 400, 200, "Error", "Failed to create file:\n    Wrong filename format", icon));
                    }
                    temp = true;
                }
                returnvalue = null;
                #endregion Rename & Create file

                //Click Detection:
                if (MouseManager.MouseState == MouseState.Left)
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
                                            TaskScheduler.Apps.Add(new Terminal.Terminal(100, 100, 999, 500, 350, entry.fullPath.Split("\\")[^1], Resources.Terminal, TypeOfTerminal.Executable, CodeAssembler.AssembleCode(new List<string> { File.ReadAllText(entry.fullPath) })));
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
                                            Applications.Notepad.Notepad n = new Notepad.Notepad(100, 100, 999, 700, 420, "Note... - " + entry.name, ImprovedVBE.ScaleImageStock(Resources.Notepad, 56, 56));
                                            n.content = File.ReadAllText(entry.fullPath);
                                            n.source = entry.fullPath;
                                            n.Buffered_Content = n.content;

                                            TaskScheduler.Apps.Add(n);
                                        }
                                    }
                                    else if(entry.type == Opt.Folder)
                                    {
                                        UIElements.Find(d => d.ID == "Path").Text = entry.fullPath;
                                        Route = entry.fullPath;
                                        HistoryForward.Clear();
                                        History.Add(entry.fullPath);
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
                                UIElements.Find(d => d.ID == "Path").Text = i + ":\\";
                                Route = i + ":\\";
                                temp = true;
                            }
                        }
                        Top += 22;
                    }
                    //Click on shortcuts
                    Top = 27;
                    for (int i = 0; i < 4; i++)
                    {
                        if (MouseManager.X > x + 3 + 37 && MouseManager.X < x + 3 + side.Width)
                        {
                            if (MouseManager.Y > y + 77 + Top && MouseManager.Y < y + 77 + Top + 22)
                            {
                                switch (i)
                                {
                                    case 0:
                                        UIElements.Find(d => d.ID == "Path").Text = "0:\\User\\" + GlobalValues.Username + "\\Favorites";
                                        Route = "0:\\User\\" + GlobalValues.Username + "\\Favorites";
                                        temp = true;
                                        break;
                                    case 1:
                                        UIElements.Find(d => d.ID == "Path").Text = "0:\\User\\" + GlobalValues.Username + "\\Documents";
                                        Route = "0:\\User\\" + GlobalValues.Username + "\\Documents";
                                        temp = true;
                                        break;
                                    case 2:
                                        UIElements.Find(d => d.ID == "Path").Text = "0:\\User\\" + GlobalValues.Username + "\\Pictures";
                                        Route = "0:\\User\\" + GlobalValues.Username + "\\Pictures";
                                        temp = true;
                                        break;
                                    case 3:
                                        UIElements.Find(d => d.ID == "Path").Text = "0:\\User\\" + GlobalValues.Username + "\\Films";
                                        Route = "0:\\User\\" + GlobalValues.Username + "\\Films";
                                        temp = true;
                                        break;
                                }
                            }
                        }
                        Top += 15;
                    }
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
                                        "Rename",
                                        "Delete",
                                        "Create"
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
                                    Found = true;
                                    break;
                                case 1:
                                    //Paste a file
                                    if(UIElements.Find(d => d.ID == "Path").Text.Length > 3)
                                    {
                                        File.Create(UIElements.Find(d => d.ID == "Path").Text + "\\" + ToCopy.Split("\\")[^1]);
                                        File.WriteAllText(UIElements.Find(d => d.ID == "Path").Text + "\\" + ToCopy.Split("\\")[^1], File.ReadAllText(ToCopy));
                                        if(Cut == true)
                                        {
                                            File.Delete(ToCopy);
                                            Cut = false;
                                        }
                                    }
                                    else
                                    {
                                        File.Create(UIElements.Find(d => d.ID == "Path").Text + ToCopy.Split("\\")[^1]);
                                        File.WriteAllText(UIElements.Find(d => d.ID == "Path").Text + ToCopy.Split("\\")[^1], File.ReadAllText(ToCopy));
                                        if (Cut == true)
                                        {
                                            File.Delete(ToCopy);
                                            Cut = false;
                                        }
                                    }
                                    Found = true;
                                    break;
                                case 2:
                                    //Cut out a file
                                    ToCopy = Selected.fullPath;
                                    Cut = true;
                                    Found = true;
                                    break;
                                case 3:
                                    TaskScheduler.Apps.Add(new Rename(ImprovedVBE.width / 2 - 200, ImprovedVBE.height / 2 - 100, 400, 200, "Rename", "Rename file: " + Selected.name, icon));
                                    break;
                                case 4:
                                    //TODO: Ask the user if he actually wants to deleate it. And to place it into Wastebasket or to remove it entirely.
                                    File.Delete(Selected.fullPath);
                                    Found = true;
                                    break;
                                case 5:
                                    TaskScheduler.Apps.Add(new Rename(ImprovedVBE.width / 2 - 200, ImprovedVBE.height / 2 - 100, 400, 200, "Create", "File name: ", icon));
                                    break;
                            }
                            break;
                    }
                    if(v.Selected < 0)
                    {
                        Found = true;
                    }
                }
                Pointer++;
            }
            if(Found == true)
            {
                rightClicks.Clear();
                temp = true;
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
