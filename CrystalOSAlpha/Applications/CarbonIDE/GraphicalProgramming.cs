using Cosmos.System;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications.FileSys;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Programming.CrystalSharp.Graphics;
using CrystalOSAlpha.System32;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Xml.Linq;
using Kernel = CrystalOS_Alpha.Kernel;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;

namespace CrystalOSAlpha.Applications.CarbonIDE
{
    class GraphicalProgramming : App
    {
        #region Essential
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int desk_ID { get; set; }
        public int AppID { get; set; }
        public string name { get; set; }
        public string Path { get; set; }
        public string namedProject { get; set; }
        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }

        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        
        public Bitmap canvas;
        public Bitmap window { get; set; }

        public bool initial = true;
        public bool once { get; set; }
        public bool clicked = false;
        #endregion Essential

        #region Core variables
        public int StoredX = 0;
        public int StoredY = 0;
        public int Sel = 0;
        public int lineIndex = 0;
        public int cursorIndex = 0;
        public bool temp = true;

        public string code = "";
        public string Back_content = "";
        public string lineCount = "";
        public string ThatID = "";
        public static string Typo = "";
        public string ActiveDirectory = "";

        public List<string> Elements = new List<string> { "Label", "Button", "TextBox", "Slider", "Scrollbar", "PictureBox", "CheckBox", "Radio button", "Progressbar", "Menutab", "Table", "More >>" };
        public List<Structure> Items = new List<Structure>();

        public Window preview;

        public Bitmap WindowCanvas;
        public Bitmap back_canvas;
        public Bitmap UIContainer;
        public Bitmap Propeties;
        public Bitmap Container;
        public Bitmap Files;
        public Bitmap BuildLog;
        #endregion Core variables

        #region UI Elements
        public List<UIElementHandler> PropetiesTab = new List<UIElementHandler>();
        public List<UIElementHandler> FilesTab = new List<UIElementHandler>();
        public List<UIElementHandler> CodeContainer = new List<UIElementHandler>();
        #endregion UI Elements

        #region Compiler
        public List<string> Log = new List<string>();
        #endregion Compiler

        public void App()
        {
            if (initial == true)
            {
                Log = new List<string>(){ "CarbonIDE designed by Crystal Development.", "All rights reserved. Since 2023." };

                //Read in the data
                name += " - " + Path.Remove(0, Path.LastIndexOf('\\') + 1);

                //Active directory set to the path of the working directory
                ActiveDirectory = Path.Remove(Path.LastIndexOf('\\'));

                //Set up the working directories, if it's not done already
                #region InitDirs
                //Directory.Delete("0:\\User\\Source\\Hello", true); <-- this will worth millions!!!
                switch (!Directory.Exists(Path.Remove(Path.LastIndexOf('\\')) + "\\Window_Layout"))
                {
                    case true:
                        Directory.CreateDirectory(Path.Remove(Path.LastIndexOf('\\')) + "\\Window_Layout");
                        break;
                }
                switch (!Directory.Exists(Path.Remove(Path.LastIndexOf('\\')) + "\\Scripts"))
                {
                    case true:
                        Directory.CreateDirectory(Path.Remove(Path.LastIndexOf('\\')) + "\\Scripts");
                        break;
                }
                #endregion InitDirs

                //Generate a starting code, if the file doesn't exist/empty
                #region InitCode
                switch (File.Exists(Path.Remove(Path.LastIndexOf('\\')) + "\\Window_Layout\\Main.wlf"))
                {
                    case true:
                        code = File.ReadAllText(Path.Remove(Path.LastIndexOf('\\')) + "\\Window_Layout\\Main.wlf");
                        if(code.Length == 0)
                        {
                            code = CodeGenerator.GenerateBase();
                            File.WriteAllText(Path.Remove(Path.LastIndexOf('\\')) + "\\Window_Layout\\Main.wlf", code);
                        }
                        break;
                    case false:
                        code = CodeGenerator.GenerateBase();
                        File.WriteAllText(Path.Remove(Path.LastIndexOf('\\')) + "\\Window_Layout\\Main.wlf", code);
                        break;
                }
                switch(File.Exists(Path.Remove(Path.LastIndexOf('\\')) + "\\MKFILE.mkf"))
                {
                    case true:
                        //If I get to the assembly part, this will be used to build the code into an app
                        break;
                    case false:
                        File.WriteAllText(Path.Remove(Path.LastIndexOf('\\')) + "\\MKFILE.mkf", "INCLUDE:\nMain.wlf\n\nSGN: Y\nPBLSHR: " + GlobalValues.Username);
                        break;
                }
                #endregion InitCode

                //Init the preview of the window. This helps to interpret informations easier without wasting lines and performance
                #region InitPreview
                preview = new Window(20, 50, 999, 400, 300, 1, "New Window", false, icon, code);
                Bitmap temp = new Bitmap(10, 10, ColorDepth.ColorDepth32);
                preview.App(temp);
                #endregion InitPreview

                //Initialize all tabs
                #region Propeties
                //Bringing table to existance
                PropetiesTab.Add(new Table(5, 80, 407, 300, 2, 7, "Propeties"));
                //Write-protecting top values
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(0, 0, "Window.X", true);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(0, 1, "Window.Y", true);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(0, 2, "Window.Width", true);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(0, 3, "Window.Height", true);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(0, 4, "Window.AlwaysOnTop", true);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(0, 5, "Window.Title", true);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(0, 6, "Window.Titlebar", true);

                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 0, preview.x.ToString(), false);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 1, preview.y.ToString(), false);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 2, preview.width.ToString(), false);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 3, preview.height.ToString(), false);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 4, preview.AlwaysOnTop.ToString(), false);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 5, preview.name, false);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 6, preview.HasTitlebar.ToString(), false);

                //Button
                PropetiesTab.Add(new Button(5, 612, 204, 35, "OnClick", 1, "Click"));
                PropetiesTab.Add(new Button(214, 612, 204, 35, "Hover", 1, "Hovering"));

                //Buttons for propety of window/UI elements
                PropetiesTab.Add(new Button(185, -12, 115, 25, "Window prop...", 1, "Window"));
                PropetiesTab.Add(new Button(317, -12, 95, 25, "UI elements", 1, "UI"));
                #endregion Propeties

                //Initialize the filemanager
                #region Filemanager
                //Filepath textbox
                FilesTab.Add(new TextBox(6, 228, 725, 35, ImprovedVBE.colourToNumber(60, 60, 60), ActiveDirectory, "0:\\", TextBox.Options.left, "SourcePath"));
                //Go button
                FilesTab.Add(new Button(756, 206, 75, 35, "Go", 1, "Go"));
                //Add new file button
                FilesTab.Add(new Button(856, 206, 75, 35, "Add", 1, "Add"));
                //Delete button
                FilesTab.Add(new Button(956, 206, 75, 35, "Delete", 1, "Delete"));
                #endregion Filemanager

                //Initialize the code container
                #region Container
                
                WindowCanvas = new Bitmap(1459 - 540, 674, ColorDepth.ColorDepth32);
                UIContainer = new Bitmap(395, 269, ColorDepth.ColorDepth32);
                Propeties = new Bitmap(422, 674, ColorDepth.ColorDepth32);
                Container = new Bitmap(540, 674, ColorDepth.ColorDepth32);
                Files = new Bitmap(1053, 269, ColorDepth.ColorDepth32);
                BuildLog = new Bitmap(422, 269, ColorDepth.ColorDepth32);

                CodeContainer.Add(new VerticalScrollbar((int)Container.Width - 20, 0, 20, (int)Container.Height, 0, 0, 1000, "VerticalScroll"));
                CodeContainer.Add(new HorizontalScrollbar(2, (int)Container.Height - 20, (int)Container.Width - 24, 20, 0, 0, 1000, "HorizontalScroll"));

                for (int i = 0; i < 278; i++)
                {
                    if ((i + 1).ToString().Length == 1)
                    {
                        lineCount += "  " + (i + 1) + "\n";
                    }
                    else if ((i + 1).ToString().Length == 2)
                    {
                        lineCount += " " + (i + 1) + "\n";
                    }
                    else
                    {
                        lineCount += (i + 1) + "\n";
                    }
                }

                Back_content = code;
                #endregion Container
                initial = false;
            }

            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);

                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

                once = false;
                temp = true;
            }

            if((MouseManager.MouseState == MouseState.Left || clicked == true || MouseManager.ScrollDelta != 0) && TaskScheduler.Apps[^1] == this)
            {
                //Data gathering from user input(s)
                foreach (var Element in CodeContainer)
                {
                    switch(!KeyboardManager.ShiftPressed)
                    {
                        case true:
                            switch(Element.TypeOfElement)
                            {
                                case TypeOfElement.VerticalScrollbar:
                                    Element.Value = Math.Clamp(Element.Value + MouseManager.ScrollDelta * 15, Element.MinVal, Element.MaxVal);
                                    Element.Pos = (int)(Element.Value / Element.Sensitivity) + 20;
                                    break;
                            }
                            break;
                        case false:
                            switch (Element.TypeOfElement)
                            {
                                case TypeOfElement.HorizontalScrollbar:
                                    Element.Value = Math.Clamp(Element.Value + MouseManager.ScrollDelta * 15, Element.MinVal, Element.MaxVal);
                                    Element.Pos = (int)(Element.Value / Element.Sensitivity) + 20;
                                    break;
                            }
                            break;
                    }
                    Element.CheckClick((int)MouseManager.X - 933, (int)MouseManager.Y - (y + 32));
                }

                //Clear the container
                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(36, 36, 36));//Background
                ImprovedVBE.DrawFilledRectangle(Container, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Container.Width - 4, (int)Container.Height - 4, false);//Border

                //Write the syntax highlighted code out
                BitFont.DrawBitFontString(Container, "ArialCustomCharset16", CarbonIDE.HighLight(Back_content), Back_content, 35 - CodeContainer.Find(d => d.ID == "HorizontalScroll").Value, 10 - CodeContainer.Find(d => d.ID == "VerticalScroll").Value);

                //Render the linecounter
                ImprovedVBE.DrawFilledRectangle(Container, ImprovedVBE.colourToNumber(100, 100, 100), 2, 2, 30, (int)Container.Height - 4, false);
                BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.Black, lineCount, 2, 10 - CodeContainer.Find(d => d.ID == "VerticalScroll").Value);

                //Actual rendering happens here
                foreach (var Element in CodeContainer)
                {
                    //Render out the scrollbars
                    Element.Render(Container);
                }

                //Render out to the main window
                ImprovedVBE.DrawImage(Container, 933, 32, window);
            }

            if (MouseManager.MouseState == MouseState.Left && TaskScheduler.Apps[^1] == this && clicked == false)
            {
                temp = true;
                if (MouseManager.Y < y + 32 + WindowCanvas.Height)
                {
                    StoredX = (int)MouseManager.X - 10 - preview.x - x;
                    StoredY = (int)MouseManager.Y - 32 - preview.y - y;
                }
            }
            else if(MouseManager.MouseState == MouseState.None && clicked)
            {
                temp = true;
                clicked = false;
                if(StoredX >= 0 && StoredY >= 22)
                {
                    if(StoredX < preview.width)
                    {
                        if(StoredY < preview.height)
                        {
                            int WidthOfUI = (int)Math.Abs((MouseManager.X - 10 - preview.x - StoredX - x));
                            int HeightOfUI = (int)Math.Abs((MouseManager.Y - 32 - preview.y - StoredY - y));
                            switch (Sel)
                            {
                                case 0:
                                    code = CodeGenerator.AddUIElement(code, new label(StoredX, StoredY - 22, "Text", ImprovedVBE.colourToNumber(255, 255, 255), "Label" + preview.UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.Label).Count));
                                    break;
                                case 1:
                                    code = CodeGenerator.AddUIElement(code, new Button(StoredX, StoredY - 42, WidthOfUI, HeightOfUI, "Button", 1, "Button" + preview.UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.Button).Count));
                                    break;
                                case 2:
                                    code = CodeGenerator.AddUIElement(code, new TextBox(StoredX, StoredY, WidthOfUI, HeightOfUI, ImprovedVBE.colourToNumber(255, 255, 255), "Text", "Placeholder", TextBox.Options.left, "TextBox" + preview.UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.TextBox).Count), "Placeholder");
                                    break;
                                case 3:
                                    code = CodeGenerator.AddUIElement(code, new Slider(StoredX, StoredY - 42, WidthOfUI, 10, 100, 50, "Slider" + preview.UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.Slider).Count));
                                    break;
                                case 4:
                                    //code = CodeGenerator.AddUIElement(code, new Scrollbar(StoredX, StoredY, WidthOfUI, HeightOfUI, 0, 100, 50, "Scrollbar" + preview.UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.Scrollbar).Count));
                                    break;
                                case 5:
                                    code = CodeGenerator.AddUIElement(code, new PictureBox(StoredX, StoredY, "PictureBox" + preview.UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.PictureBox).Count, true, new Bitmap(10, 10, ColorDepth.ColorDepth32)), "", true);
                                    break;
                            }
                            StoredX = -1;
                            StoredY = -1;
                        }
                    }
                }
            }

            KeyEvent KeyPress = null;
            if(KeyboardManager.TryReadKey(out KeyPress))
            {
                switch(KeyPress.Key)
                {
                    case ConsoleKeyEx.F5:
                        //Build the file
                        //For now, it only consists of the window layout
                        File.WriteAllText(Path.Remove(Path.LastIndexOf('\\')) + "\\Window_Layout\\Main.wlf", code);
                        if(!Directory.Exists(Path.Remove(Path.LastIndexOf('\\')) + "\\Bin"))
                        {
                            Directory.CreateDirectory(Path.Remove(Path.LastIndexOf('\\')) + "\\Bin");
                        }
                        File.Create(Path.Remove(Path.LastIndexOf('\\')) + "\\Bin\\" + Path.Remove(0, Path.LastIndexOf('\\') + 1) + ".app");
                        File.WriteAllText(Path.Remove(Path.LastIndexOf('\\')) + "\\Bin\\" + Path.Remove(0, Path.LastIndexOf('\\') + 1) + ".app", code);
                        TaskScheduler.Apps.Add(new Window(100, 100, 999, 350, 200, 0, "Untitled", false, icon, File.ReadAllText(Path.Remove(Path.LastIndexOf('\\')) + "\\Bin\\" + Path.Remove(0, Path.LastIndexOf('\\') + 1) + ".app")));
                        break;
                    default:
                        temp = true;
                        break;
                }
            }

            if (preview.Code != code)
            {
                Back_content = code;
                preview = new Window(20, 50, 999, 400, 300, 1, "New Window", false, icon, code);
                temp = true;
            }

            if (temp == true && clicked == false)
            {
                //In case later on need to enable multiple times in a row
                temp = false;

                //Copying canvas to window
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

                //Giving border to Elements
                #region Border
                Array.Fill(WindowCanvas.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(WindowCanvas, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)WindowCanvas.Width - 4, (int)WindowCanvas.Height - 4, false);

                Array.Fill(Propeties.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(Propeties, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Propeties.Width - 4, (int)Propeties.Height - 4, false);

                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(36, 36, 36));//Background
                ImprovedVBE.DrawFilledRectangle(Container, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Container.Width - 4, (int)Container.Height - 4, false);//Border

                switch (KeyPress)
                {
                    case null:
                        Array.Fill(UIContainer.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                        ImprovedVBE.DrawFilledRectangle(UIContainer, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)UIContainer.Width - 4, (int)UIContainer.Height - 4, false);

                        //Render out every choosable element
                        #region Elements
                        int XOffset = 0;
                        int YOffset = 10;
                        for (int i = 0; i < Elements.Count; i++)
                        {
                            if (MouseManager.MouseState == MouseState.Left && clicked == false)
                            {
                                if (MouseManager.X > 10 + XOffset && MouseManager.X < 190 + XOffset && MouseManager.Y > 721 + 30 + TaskManager.TaskBar.Height + YOffset && MouseManager.Y < 721 + TaskManager.TaskBar.Height + 30 + YOffset + 35)
                                {
                                    Sel = i;
                                    temp = true;
                                    clicked = true;
                                }
                            }
                            if (Sel == i)
                            {
                                ImprovedVBE.DrawFilledRectangle(UIContainer, ImprovedVBE.colourToNumber(100, 100, 100), XOffset + 5, 30 + YOffset, 180, 35, false);
                            }
                            BitFont.DrawBitFontString(UIContainer, "ArialCustomCharset16", Color.White, Elements[i], XOffset + 10, 38 + YOffset);

                            YOffset += 35;

                            // Move to the next column every 5 elements
                            if ((i + 1) % 6 == 0)
                            {
                                XOffset += 190;
                                YOffset = 10;
                            }
                        }
                        #endregion

                        //Render out Filemanager and Buildlog
                        //File box
                        Array.Fill(Files.RawData, ImprovedVBE.colourToNumber(36, 36, 36));//Background
                        ImprovedVBE.DrawFilledRectangle(Files, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Files.Width - 4, (int)Files.Height - 4, false);//Border
                        //Build log
                        Array.Fill(BuildLog.RawData, ImprovedVBE.colourToNumber(36, 36, 36));//Background
                        ImprovedVBE.DrawFilledRectangle(BuildLog, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)BuildLog.Width - 4, (int)BuildLog.Height - 4, false);//Border

                        #region Filemanager
                        //Filemanager
                        temp = false;
                        
                        if(Items.Count == 0)
                        {
                            try
                            {
                                foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(ActiveDirectory))
                                {
                                    if (d.mEntryType == DirectoryEntryTypeEnum.Directory)
                                    {
                                        //Selecting if the file/directory matches the criteria of textbox 0
                                        Items.Add(new Structure(d.mName, d.mFullPath, Opt.Folder));
                                        //if (UIElements.Find(d => d.ID == "Search").Text == "")
                                        //{
                                        //}
                                        //else if (UIElements.Find(d => d.ID == "Search").Text != "" && !UIElements.Find(d => d.ID == "Search").Text.Contains("."))
                                        //{
                                        //    if (d.mName.ToLower().Contains(UIElements.Find(d => d.ID == "Search").Text.ToLower()))
                                        //    {
                                        //        Items.Add(new Structure(d.mName, d.mFullPath, Opt.Folder));
                                        //    }
                                        //}
                                    }
                                }
                                foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(ActiveDirectory))
                                {
                                    if (d.mEntryType == DirectoryEntryTypeEnum.File)
                                    {
                                        //Selecting if the file/directory matches the criteria of textbox 0
                                        Items.Add(new Structure(d.mName, d.mFullPath, Opt.File));
                                        //if (UIElements.Find(d => d.ID == "Search").Text == "" || UIElements.Find(d => d.ID == "Search").Text.StartsWith("*") && !UIElements.Find(d => d.ID == "Search").Text.Contains("."))
                                        //{
                                        //}
                                        //else if (UIElements.Find(d => d.ID == "Search").Text.Contains("."))
                                        //{
                                        //    string[] sides = UIElements.Find(d => d.ID == "Search").Text.Split(".");
                                        //    if (sides[0] == "*" && d.mName.ToLower().Contains("." + sides[1].ToLower()))
                                        //    {
                                        //        Items.Add(new Structure(d.mName, d.mFullPath, Opt.File));
                                        //    }
                                        //    else if (d.mName.ToLower().Contains(sides[0]) && d.mName.ToLower().Contains("." + sides[1].ToLower()))
                                        //    {
                                        //        Items.Add(new Structure(d.mName, d.mFullPath, Opt.File));
                                        //    }
                                        //}
                                        //else
                                        //{
                                        //    if (d.mName.ToLower().Contains(UIElements.Find(d => d.ID == "Search").Text.ToLower()))
                                        //    {
                                        //        Items.Add(new Structure(d.mName, d.mFullPath, Opt.File));
                                        //    }
                                        //}
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                TaskScheduler.Apps.Add(new MsgBox(999, ImprovedVBE.width / 2 - 200, ImprovedVBE.height / 2 - 100, 400, 200, "Error!", ex.Message, icon));
                                //UIElements.Find(d => d.ID == "Path.Remove(Path.LastIndexOf('\\'))").Text = "0:\\";
                            }
                        }

                        Bitmap Folder = new Bitmap(30, 30, ColorDepth.ColorDepth32);
                        Array.Fill(Folder.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                        ImprovedVBE.DrawImageAlpha(ImprovedVBE.ScaleImageStock(Resources.Folder, 28, 28), 0, 0, Folder);

                        Bitmap File = new Bitmap(30, 30, ColorDepth.ColorDepth32);
                        Array.Fill(File.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                        ImprovedVBE.DrawImageAlpha(ImprovedVBE.ScaleImageStock(Resources.File, 28, 28), 0, 0, File);

                        int x_off = 14;
                        int y_off = 9;// - UIElements.Find(d => d.ID == "ScrollRight").Value;
                        int indicator = 0;
                        foreach (var entry in Items)
                        {
                            entry.X = x_off;
                            entry.Y = y_off;
                            if (MouseManager.MouseState == MouseState.Left)
                            {
                                if (MouseManager.X > x + 420 + entry.X && MouseManager.X < x + 420 + entry.X + 35)
                                {
                                    if (MouseManager.Y > y + 721 + entry.Y && MouseManager.Y < y + 721 + entry.Y + 50 && entry.Y > 0 && entry.Y < Files.Height - 50)
                                    {
                                        if (clicked == false)
                                        {
                                            if (entry.type == Opt.File)
                                            {
                                                entry.Selected = true;
                                                //If the file extension is .wlf, the preview will be shown of the window
                                                switch(entry.name[^4..])
                                                {
                                                    case ".wlf":
                                                        preview.Code = System.IO.File.ReadAllText(entry.fullPath);
                                                        break;
                                                    default:
                                                        switch(entry.name[^3..])
                                                        {
                                                            case ".cs":
                                                                code = System.IO.File.ReadAllText(entry.fullPath);
                                                                Back_content = code;
                                                                break;
                                                        }
                                                        break;
                                                }
                                                //If the file extension is .ccs, the code will be shown
                                            }
                                            else if (entry.type == Opt.Folder)
                                            {
                                                if(entry.Selected == true)
                                                {
                                                    ActiveDirectory = entry.fullPath;
                                                    FilesTab.Find(d => d.ID == "SourcePath").Text = ActiveDirectory;
                                                    Items.Clear();
                                                }
                                                else
                                                {

                                                }
                                            }
                                            foreach (var v in Items)
                                            {
                                                v.Selected = false;
                                            }
                                            entry.Selected = true;
                                            clicked = true;
                                            temp = true;
                                        }
                                    }
                                }
                            }
                            if (entry.Selected == true)
                            {
                                ImprovedVBE.DrawFilledRectangle(Files, ImprovedVBE.colourToNumber(69, 69, 69), x_off - 5, y_off - 5, 65, 65, false);
                            }
                            if (entry.type == Opt.Folder)
                            {
                                TransportedIcons.FirstFolder(Files, x_off, y_off);
                                //ImprovedVBE.DrawImage(Folder, x_off, y_off, Main);
                                if (entry.name.Length > 5)
                                {
                                    BitFont.DrawBitFontString(Files, "ArialCustomCharset16", Color.White, entry.name.Remove(5) + "...", x_off, y_off + 38);
                                }
                                else
                                {
                                    BitFont.DrawBitFontString(Files, "ArialCustomCharset16", Color.White, entry.name, x_off, y_off + 38);
                                }
                            }
                            if (entry.type == Opt.File)
                            {
                                //ImprovedVBE.DrawImage(File, x_off, y_off, Main);
                                TransportedIcons.FirstIcon(Files, x_off, y_off);
                                if (entry.name.Length > 5)
                                {
                                    BitFont.DrawBitFontString(Files, "ArialCustomCharset16", Color.White, entry.name.Remove(5) + "...", x_off, y_off + 38);
                                }
                                else
                                {
                                    BitFont.DrawBitFontString(Files, "ArialCustomCharset16", Color.White, entry.name.Remove(entry.name.Length - 4), x_off, y_off + 38);
                                }
                            }
                            x_off += 62;
                            if (x_off >= Files.Width - 72)
                            {
                                y_off += 70;
                                x_off = 9;
                            }
                            indicator++;
                        }

                        #endregion Filemanager
                        break;
                }
                #endregion Border

                bool KeyPressed = false;
                //Render to propeties
                foreach (var Element in PropetiesTab)
                {
                    switch (Element.TypeOfElement)
                    {
                        case TypeOfElement.Table:
                            if(MouseManager.MouseState == MouseState.Left)
                            {
                                Element.CheckClick(1488, y + 32);
                                if(Element.MinVal > 0)
                                {
                                    code = CodeGenerator.ModifyCode(code, Element.GetValue(Element.MinVal - 1, Element.MaxVal), Element.GetValue(Element.MinVal, Element.MaxVal));
                                }
                            }
                            switch (Element.MinVal > 0)
                            {
                                case true:
                                    switch (KeyPress)
                                    {
                                        case null:
                                            break;
                                        default:
                                            switch (KeyPress.Key)
                                            {
                                                case ConsoleKeyEx.Enter:
                                                    if(Element.MaxVal == 5)
                                                    {
                                                        code = CodeGenerator.ModifyCode(code, Element.GetValue(Element.MinVal - 1, Element.MaxVal), "\"" + Element.GetValue(Element.MinVal, Element.MaxVal) + "\"");
                                                    }
                                                    else
                                                    {
                                                        code = CodeGenerator.ModifyCode(code, Element.GetValue(Element.MinVal - 1, Element.MaxVal), Element.GetValue(Element.MinVal, Element.MaxVal));
                                                    }
                                                    break;
                                                default:
                                                    switch (Element.Clicked)//Element.Clicked means if it's write-protected or not
                                                    {
                                                        case false:
                                                            KeyPressed = true;
                                                            Element.SetValue(Element.MinVal, Element.MaxVal, Keyboard.HandleKeyboard(Element.GetValue(Element.MinVal, Element.MaxVal), KeyPress), false);
                                                            switch(Element.GetValue(Element.MinVal - 1, Element.MaxVal))
                                                            {
                                                                case "Window.X":
                                                                    if(int.TryParse(Element.GetValue(Element.MinVal, Element.MaxVal), out int XVal))
                                                                    {
                                                                        if(XVal < 0)
                                                                        {
                                                                            Element.SetValue(Element.MinVal, Element.MaxVal, "0", false);
                                                                        }
                                                                        else
                                                                        {
                                                                            Element.SetValue(Element.MinVal, Element.MaxVal, XVal.ToString(), false);
                                                                        }
                                                                    }
                                                                    break;
                                                                case "Window.Y":
                                                                    if (int.TryParse(Element.GetValue(Element.MinVal, Element.MaxVal), out int YVal))
                                                                    {
                                                                        if (YVal < 0)
                                                                        {
                                                                            Element.SetValue(Element.MinVal, Element.MaxVal, "0", false);
                                                                        }
                                                                        else
                                                                        {
                                                                            Element.SetValue(Element.MinVal, Element.MaxVal, YVal.ToString(), false);
                                                                        }
                                                                    }
                                                                    break;
                                                                case "Window.Width":
                                                                    if (int.TryParse(Element.GetValue(Element.MinVal, Element.MaxVal), out int WidthVal))
                                                                    {
                                                                        if (WidthVal < 0)
                                                                        {
                                                                            Element.SetValue(Element.MinVal, Element.MaxVal, "0", false);
                                                                        }
                                                                        else
                                                                        {
                                                                            Element.SetValue(Element.MinVal, Element.MaxVal, WidthVal.ToString(), false);
                                                                        }
                                                                    }
                                                                    break;
                                                                case "Window.Height":
                                                                    if (int.TryParse(Element.GetValue(Element.MinVal, Element.MaxVal), out int HeightVal))
                                                                    {
                                                                        if (HeightVal < 0)
                                                                        {
                                                                            Element.SetValue(Element.MinVal, Element.MaxVal, "0", false);
                                                                        }
                                                                        else
                                                                        {
                                                                            Element.SetValue(Element.MinVal, Element.MaxVal, HeightVal.ToString(), false);
                                                                        }
                                                                    }
                                                                    break;
                                                                case "Window.AlwaysOnTop":
                                                                    switch(KeyPress.Key)
                                                                    {
                                                                        case ConsoleKeyEx.T:
                                                                            Element.SetValue(Element.MinVal, Element.MaxVal, "True", false);
                                                                            break;
                                                                        case ConsoleKeyEx.F:
                                                                            Element.SetValue(Element.MinVal, Element.MaxVal, "False", false);
                                                                            break;
                                                                    }
                                                                    break;
                                                                case "Window.Title":
                                                                    Element.SetValue(Element.MinVal, Element.MaxVal, Element.GetValue(Element.MinVal, Element.MaxVal), false);
                                                                    break;
                                                                case "Window.Titlebar":
                                                                    switch (KeyPress.Key)
                                                                    {
                                                                        case ConsoleKeyEx.T:
                                                                            Element.SetValue(Element.MinVal, Element.MaxVal, "True", false);
                                                                            break;
                                                                        case ConsoleKeyEx.F:
                                                                            Element.SetValue(Element.MinVal, Element.MaxVal, "False", false);
                                                                            break;
                                                                    }
                                                                    break;
                                                            }
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case TypeOfElement.Button:
                            Element.CheckClick(1488, y + 32);
                            break;
                    }
                    Element.Render(Propeties);
                }

                //Render to FilesTab
                foreach (var Element in FilesTab)
                {
                    switch (Element.TypeOfElement)
                    {
                        case TypeOfElement.Button:
                            Element.CheckClick(420, y + 721);
                            switch (Element.Clicked)
                            {
                                case true:
                                    switch (Element.ID)
                                    {
                                        case "Go":
                                            try
                                            {
                                                if (Directory.Exists(FilesTab.Find(d => d.ID == "SourcePath").Text))
                                                {
                                                    ActiveDirectory = FilesTab.Find(d => d.ID == "SourcePath").Text;
                                                    Items.Clear();
                                                    temp = true;
                                                }
                                            }
                                            catch(Exception ex)
                                            {

                                            }
                                            break;
                                        case "Add":
                                            //TODO
                                            //Add file/folder dialog box
                                            break;
                                        case "Delete":
                                            //Delete file/folder dialog box
                                            try
                                            {
                                                var Item = Items.Find(x => x.Selected == true);
                                                switch(Item.type)
                                                {
                                                    case Opt.File:
                                                        File.Delete(Item.fullPath);
                                                        Items.Clear();
                                                        break;
                                                    case Opt.Folder:
                                                        Directory.Delete(Item.fullPath, true);
                                                        Items.Clear();
                                                        break;
                                                }
                                                temp = true;
                                            }
                                            catch(Exception ex)
                                            {
                                                
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                        case TypeOfElement.TextBox:
                            Element.CheckClick(420, y + 721);
                            switch (Element.Clicked)
                            {
                                case true:
                                    switch (KeyPress)
                                    {
                                        case null:
                                            break;
                                        default:
                                            switch (KeyPress.Key)
                                            {
                                                case ConsoleKeyEx.Enter:
                                                    switch (Element.ID)
                                                    {
                                                        case "SourcePath":
                                                            if (Directory.Exists(Element.Text))
                                                            {
                                                                ActiveDirectory = Element.Text;
                                                                Items.Clear();
                                                            }
                                                            temp = true;
                                                            break;
                                                    }
                                                    break;
                                                default:
                                                    switch (Element.ID)
                                                    {
                                                        case "SourcePath":
                                                            Element.Text = Keyboard.HandleKeyboard(Element.Text, KeyPress);
                                                            break;
                                                    }
                                                    break;
                                            }
                                            break;
                                    }
                                    break;
                            }
                            break;
                    }
                    Element.Render(Files);
                }

                #region Labeling
                BitFont.DrawBitFontString(WindowCanvas, "VerdanaCustomCharset24", Color.White, "Canvas:", 7, 10);
                BitFont.DrawBitFontString(UIContainer, "VerdanaCustomCharset24", Color.White, "UI Elements:", 7, 10);
                BitFont.DrawBitFontString(Propeties, "VerdanaCustomCharset24", Color.White, "Propeties:", 7, 10);
                //BitFont.DrawBitFontString(Container, "VerdanaCustomCharset24", Color.White, "Code:", 7, 10);

                //Writing out the selected .cs file content/code
                BitFont.DrawBitFontString(Container, "ArialCustomCharset16", CarbonIDE.HighLight(Back_content), Back_content, 35 - CodeContainer.Find(d => d.ID == "HorizontalScroll").Value, 10 - CodeContainer.Find(d => d.ID == "VerticalScroll").Value);

                //Render out the debug window
                string LogOutput = "";
                foreach (string s in Log)
                {
                    LogOutput += s + "\n";
                }
                BitFont.DrawBitFontString(BuildLog, "ArialCustomCharset16", Color.White, LogOutput, 10, 10);

                #endregion Labeling
                //Render the linecounter
                ImprovedVBE.DrawFilledRectangle(Container, ImprovedVBE.colourToNumber(100, 100, 100), 2, 2, 30, (int)Container.Height - 4, false);
                BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.Black, lineCount, 2, 10 - CodeContainer.Find(d => d.ID == "VerticalScroll").Value);

                //Render to Container
                foreach (var Element in CodeContainer)
                {
                    Element.Render(Container);
                }

                //Render the preview window
                preview.App(WindowCanvas);

                #region Rendering
                //Top section
                ImprovedVBE.DrawImage(WindowCanvas, 10, 32, window);
                ImprovedVBE.DrawImage(Container, 933, 32, window);
                ImprovedVBE.DrawImage(Propeties, 1488, 32, window);

                //Bottom section
                ImprovedVBE.DrawImage(UIContainer, 10, 721, window);
                ImprovedVBE.DrawImage(Files, 420, 721, window);
                ImprovedVBE.DrawImage(BuildLog, 1488, 721, window);
                #endregion Rendering

                //In case there was a click, but didn't touch anything, it'll stop temp from executing
                if (MouseManager.MouseState == MouseState.Left)
                {
                    clicked = true;
                }
            }

            //Renders the window to the screen
            if (GlobalValues.TaskBarType == "Nostalgia")
            {
                Array.Copy(window.RawData, 0, ImprovedVBE.cover.RawData, ImprovedVBE.width * TaskManager.TaskBar.Height, window.RawData.Length);
            }
            else
            {
                Array.Copy(window.RawData, 0, ImprovedVBE.cover.RawData, 0, window.RawData.Length);
            }
        }

        public void RightClick()
        {

        }
    }

    class Elements
    {
        public string Name { get; set; }
        public bool Selected { get; set; }
        public Types T {get; set; }
        public Elements(string Name, bool Selected)
        {
            this.Name = Name;
            this.Selected = Selected;
        }
        public Elements(string Name, bool Selected, Types t)
        {
            this.Name = Name;
            this.Selected = Selected;
            this.T = t;
        }
        public enum Types{
            Button,
            Label,
            Slider,
            TextBox,
            CheckBox,
            Table
        }
    }
}
