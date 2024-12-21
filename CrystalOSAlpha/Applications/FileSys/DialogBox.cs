using Cosmos.System;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;
using Kernel = CrystalOS_Alpha.Kernel;
using CrystalOSAlpha.System32;

namespace CrystalOSAlpha.Applications.FileSys
{
    class DialogBox : App
    {
        #region Essential
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int desk_ID {get; set; }
        public int AppID { get; set; }
        public string name { get; set; }

        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }
        #endregion Essential

        #region UI_Elements
        public List<UIElementHandler> UIElements = new List<UIElementHandler>();
        #endregion UI_Elements

        #region Extra
        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        
        public string message = "";
        public string Path = "0:\\";
        public string SourceFile = "";
        public string SourceFileTemp = "0:\\";
        
        public bool initial = true;
        public bool once { get; set; }
        public bool clicked = false;
        public bool temp = false;

        public Bitmap canvas;
        public Bitmap window { get; set; }
        public Bitmap Container;
        public Bitmap QuickAccess;

        public List<Structure> Items = new List<Structure>();
        #endregion Extra

        public DialogBox(int x, int y, int z, int width, int height, int desk_ID, string name, bool minimised, Bitmap icon)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.width = width;
            this.height = height;
            this.desk_ID = desk_ID;
            this.name = name;
            this.minimised = minimised;
            this.icon = icon;
        }

        public void App()
        {
            if (SourceFile == null)
            {
                TaskScheduler.Apps.Remove(this);
            }
            if (initial == true)
            {
                UIElements.Add(new Button(11, 40, 90, 25, "Back", 1));
                UIElements.Add(new Button(108, 40, 90, 25, "Forward", 1));
                UIElements.Add(new Button(686, 502, 90, 25, "Open/Create", 1));
                UIElements.Add(new Button(783, 502, 90, 25, "Cancel", 1));

                UIElements.Add(new UI_Elements.TextBox(230, 40, 440, 25, ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B), "0:\\", "", UI_Elements.TextBox.Options.left, "Main"));
                UIElements.Add(new UI_Elements.TextBox(680, 40, 193, 25, ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B), "", "Search", UI_Elements.TextBox.Options.left, "SearchBox"));
                UIElements.Add(new UI_Elements.TextBox(332, 502, 338, 25, ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B), "", "Name", UI_Elements.TextBox.Options.left, "Fname"));

                initial = false;
            }
            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                QuickAccess = new Bitmap(187, 440, ColorDepth.ColorDepth32);
                Container = new Bitmap(648, 400, ColorDepth.ColorDepth32);

                Array.Fill(QuickAccess.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(QuickAccess, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)QuickAccess.Width - 4, (int)QuickAccess.Height - 4, false);
                
                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(Container, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Container.Width - 4, (int)Container.Height - 4, false);

                #region corners
                ImprovedVBE.DrawFilledEllipse(canvas, 10, 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, width - 11, 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, 10, height - 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, width - 11, height - 10, 10, 10, CurrentColor);

                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 0, 10, width, height - 20, false);
                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, 0, width - 10, 15, false);
                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, height - 15, width - 10, 15, false);
                #endregion corners

                canvas = ImprovedVBE.EnableTransparency(canvas, x, y, canvas);

                ImprovedVBE.DrawGradientLeftToRight(canvas);

                ImprovedVBE.DrawFilledEllipse(canvas, width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                ImprovedVBE.DrawFilledEllipse(canvas, width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                once = false;
                temp = true;
            }

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
                                    foreach(var v in UIElements)
                                    {
                                        v.Clicked = false;
                                    }
                                    elements.Clicked = true;
                                    break;
                            }
                        }
                        else
                        {
                            if(elements.Clicked == true)
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
                if(elements.TypeOfElement == TypeOfElement.Button && elements.Clicked == true && MouseManager.MouseState == MouseState.None)
                {
                    temp = true;
                    elements.Clicked = false;
                    clicked = false;
                }
            }

            if (TaskScheduler.Apps[^1] == this)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    foreach (var entry in Items)
                    {
                        if (MouseManager.X > x + 225 + entry.X && MouseManager.X < x + 225 + entry.X + 60)
                        {
                            if (MouseManager.Y > y + 87 + entry.Y && MouseManager.Y < y + 87 + entry.Y + 60)
                            {
                                if (clicked == false)
                                {
                                    if (entry.type == Opt.File)
                                    {
                                        if (entry.name.EndsWith(".cs"))
                                        {
                                            //Send the values to the CarbonIDE
                                            UIElements.Find(d => d.ID == "Fname").Text = entry.fullPath;
                                        }
                                        else if (entry.name.EndsWith(".cmd"))
                                        {
                                            UIElements.Find(d => d.ID == "Fname").Text = entry.fullPath;
                                        }
                                        else if (entry.name.EndsWith(".app"))
                                        {
                                            UIElements.Find(d => d.ID == "Fname").Text = entry.fullPath;
                                        }
                                        else if (entry.name.EndsWith(".html"))
                                        {
                                            UIElements.Find(d => d.ID == "Fname").Text = entry.fullPath;
                                        }
                                        else if (entry.name.EndsWith(".txt"))
                                        {
                                            UIElements.Find(d => d.ID == "Fname").Text = entry.fullPath;
                                        }
                                        else
                                        {
                                            //Unsupported file type
                                        }
                                        temp = true;
                                    }
                                    else if (entry.type == Opt.Folder)
                                    {
                                        UIElements.Find(d => d.ID == "Main").Text = entry.fullPath;
                                        temp = true;
                                    }
                                }
                            }
                        }
                    }
                }

                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    if(key.Key == ConsoleKeyEx.Enter)
                    {
                        if(UIElements.Find(d => d.ID == "Main").Clicked == true)
                        {
                            Path = UIElements.Find(d => d.ID == "Main").Text;
                            UIElements.Find(d => d.ID == "Main").Clicked = false;
                            temp = true;
                        }
                        else if (UIElements.Find(d => d.ID == "SearchBox").Clicked == true)
                        {
                            
                        }
                        else if (UIElements.Find(d => d.ID == "Fname").Clicked == true)
                        {
                            WindowMessenger.Send(new WindowMessage(UIElements.Find(d => d.ID == "Main").Text + "\\" + UIElements.Find(d => d.ID == "Fname").Text, "Create new", "CarbonIDE"));
                            TaskScheduler.Apps.Remove(this);
                        }
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
                }
            }

            if (temp == true)
            {
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

                Array.Fill(QuickAccess.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(QuickAccess, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)QuickAccess.Width - 4, (int)QuickAccess.Height - 4, false);

                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(Container, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Container.Width - 4, (int)Container.Height - 4, false);

                if(UIElements.Find(d => d.ID == "Main").Clicked == false)
                {
                    Items.Clear();
                    foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(UIElements.Find(d => d.ID == "Main").Text))
                    {
                        if (d.mEntryType == DirectoryEntryTypeEnum.Directory)
                        {
                            Items.Add(new Structure(d.mName, d.mFullPath, Opt.Folder));
                        }
                    }
                    foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(UIElements.Find(d => d.ID == "Main").Text))
                    {
                        if (d.mEntryType == DirectoryEntryTypeEnum.File)
                        {
                            Items.Add(new Structure(d.mName, d.mFullPath, Opt.File));
                        }
                    }
                }

                foreach (var button in UIElements)
                {
                    if (button.Clicked == true && button.TypeOfElement == TypeOfElement.Button)
                    {
                        int Col = button.Color;
                        button.Color = Color.White.ToArgb();
                        button.Render(window);
                        button.Color = Col;

                        switch (button.Text)
                        {
                            case "Cancel":
                                TaskScheduler.Apps.Remove(this);
                                break;
                            case "Open/Create":
                                WindowMessenger.Send(new WindowMessage(Path + "\\" + UIElements.Find(d => d.ID == "Fname").Text, "Create new", "CarbonIDE"));
                                TaskScheduler.Apps.Remove(this);
                                break;
                            case "Back":
                                
                                break;
                            case "Forward":
                                
                                break;
                        }
                    }
                    //else if(button.TypeOfElement == TypeOfElement.TextBox)
                    //{
                    //    if (button.ID == "Fname")
                    //    {
                    //        //button.Text = SourceFileTemp.Remove(0, SourceFileTemp.LastIndexOf('\\') + 1);
                    //    }
                    //    if (button.ID == "Main")
                    //    {
                    //        //button.Text = Path;
                    //    }
                    //    button.Render(window);
                    //}
                    else
                    {
                        button.Render(window);
                    }
                }

                int x_off = 15;
                int y_off = 15;
                foreach (var entry in Items)
                {
                    entry.X = x_off;
                    entry.Y = y_off;
                    if (entry.type == Opt.Folder)
                    {
                        ImprovedVBE.DrawImageAlpha(Resources.Folder, x_off, y_off, Container);
                        if (entry.name.Length > 5)
                        {
                            BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.White, entry.name.Remove(5) + "...", x_off, y_off + 60);
                        }
                        else
                        {
                            BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.White, entry.name, x_off, y_off + 60);
                        }
                    }
                    if (entry.type == Opt.File)
                    {
                        ImprovedVBE.DrawImageAlpha(Resources.File, x_off, y_off, Container);
                        if (entry.name.Length > 5)
                        {
                            BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.White, entry.name.Remove(5) + "...", x_off, y_off + 60);
                        }
                        else
                        {
                            BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.White, entry.name.Remove(entry.name.Length - 4), x_off, y_off + 60);
                        }
                    }
                    x_off += 70;
                    if (x_off / 70 >= 6)
                    {
                        y_off += 85;
                        x_off = 5;
                    }
                }

                BitFont.DrawBitFontString(window, "ArialCustomCharset16", Color.White, "File name:", 225, 505);

                ImprovedVBE.DrawImageAlpha(QuickAccess, 11, 87, window);
                ImprovedVBE.DrawImageAlpha(Container, 225, 87, window);

                temp = false;
            }
        }

        public void RightClick()
        {

        }
    }
}
