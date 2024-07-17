using Cosmos.HAL.BlockDevice;
using Cosmos.System;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.Graphics;
using CrystalOS_Alpha;
using CrystalOSAlpha.Applications.FileSys;
using CrystalOSAlpha.Applications.Terminal;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Programming.CrystalSharp;
using CrystalOSAlpha.Programming.CrystalSharp.Graphics;
using CrystalOSAlpha.System32;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        public string Selected = "";
        public string ThatID = "";
        public static string Typo = "";

        public List<string> Elements = new List<string> { "Label", "Button", "TextBox", "Slider", "Scrollbar", "PictureBox", "CheckBox", "Radio button", "Progressbar", "Menutab", "Table" };
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
        #endregion UI Elements

        public void App()
        {
            if (initial == true)
            {
                //Read in the data
                name += " - " + Path.Remove(0, Path.LastIndexOf('\\') + 1);

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
                #endregion Propeties

                for (int i = 0; i < 278; i++)
                {
                    if (i.ToString().Length == 1)
                    {
                        lineCount += (i + 1) + "  \n";
                    }
                    else if (i.ToString().Length == 2)
                    {
                        lineCount += (i + 1) + " \n";
                    }
                    else
                    {
                        lineCount += (i + 1) + "\n";
                    }
                }

                Back_content = code;
                initial = false;
            }

            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);

                WindowCanvas = new Bitmap(1459 - 540, 674, ColorDepth.ColorDepth32);
                UIContainer = new Bitmap(395, 269, ColorDepth.ColorDepth32);
                Propeties = new Bitmap(422, 674, ColorDepth.ColorDepth32);
                Container = new Bitmap(540, 674, ColorDepth.ColorDepth32);
                Files = new Bitmap(1053, 269, ColorDepth.ColorDepth32);
                BuildLog = new Bitmap(422, 269, ColorDepth.ColorDepth32);

                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

                once = false;
                temp = true;
            }

            if (MouseManager.MouseState == MouseState.Left && TaskScheduler.Apps[^1] == this && clicked == false)
            {
                temp = true;
                if (MouseManager.Y < y + 32 + WindowCanvas.Height)
                {
                    StoredX = (int)MouseManager.X;
                    StoredY = (int)MouseManager.Y;
                }
            }
            else if(MouseManager.MouseState == MouseState.None && clicked)
            {
                temp = true;
                clicked = false;
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
                        break;
                    default:
                        temp = true;
                        break;
                }
            }

            if (preview.Code != code)
            {
                preview = new Window(20, 50, 999, 400, 300, 1, "New Window", false, icon, code);
                temp = true;
            }

            if (temp == true)
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
                        for(int i = 0; i < Elements.Count; i++)
                        {
                            if (MouseManager.MouseState == MouseState.Left && clicked == false)
                            {
                                if (MouseManager.X > 10 + XOffset && MouseManager.X < 190 + XOffset && MouseManager.Y > 721 + 35 + TaskManager.TaskBar.Height + YOffset && MouseManager.Y < 721 + TaskManager.TaskBar.Height + 35 + YOffset + 35)
                                {
                                    Sel = i;
                                    temp = true;
                                    clicked = true;
                                }
                            }
                            else
                            {
                                clicked = false;
                            }
                            if (Sel == i)
                            {
                                ImprovedVBE.DrawFilledRectangle(UIContainer, ImprovedVBE.colourToNumber(100, 100, 100), XOffset + 10, 30 + YOffset, 180, 35, false);
                            }
                            BitFont.DrawBitFontString(UIContainer, "ArialCustomCharset16", Color.White, Elements[i], XOffset + 10, 38 + YOffset);
                            if(i % 5 == 0 && i != 0)
                            {
                                XOffset += 190;
                                YOffset = 10;
                            }
                            else
                            {
                                YOffset += 35;
                            }
                        }
                        #endregion Elements

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
                        Items.Clear();
                        try
                        {
                            foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(Path.Remove(Path.LastIndexOf('\\'))))
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
                            foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(Path.Remove(Path.LastIndexOf('\\'))))
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
                            if (entry.fullPath == entry.fullPath)
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

                        if (MouseManager.MouseState == MouseState.Left)
                        {
                            //Going thru every item in Items
                            foreach (var entry in Items)
                            {
                                if (MouseManager.X > x + 126 + entry.X && MouseManager.X < x + 126 + entry.X + 35)
                                {
                                    if (MouseManager.Y > y + 77 + entry.Y && MouseManager.Y < y + 77 + entry.Y + 50 && entry.Y > 0 && entry.Y < Files.Height - 50)
                                    {
                                        if (clicked == false)
                                        {
                                            foreach (var v in Items)
                                            {
                                                v.Selected = false;
                                            }
                                            if (entry.type == Opt.File)
                                            {
                                                
                                            }
                                            else if (entry.type == Opt.Folder)
                                            {
                                                
                                            }
                                            entry.Selected = true;
                                            clicked = true;
                                            temp = true;
                                        }
                                    }
                                }
                            }
                        }
                        #endregion Filemanager
                        break;
                }
                #endregion Border

                //Render to propeties
                foreach (var Element in PropetiesTab)
                {
                    switch (Element.TypeOfElement)
                    {
                        case TypeOfElement.Table:
                            if(MouseManager.MouseState == MouseState.Left)
                            {
                                Element.CheckClick(1488, y + 32);
                                if(Element.MinVal >= 0)
                                {
                                    code = CodeGenerator.ModifyCode(code, Element.GetValue(Element.MinVal - 1, Element.MaxVal), Element.GetValue(Element.MinVal, Element.MaxVal));
                                }
                            }
                            switch (Element.MinVal >= 0)
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
                                                    code = CodeGenerator.ModifyCode(code, Element.GetValue(Element.MinVal - 1, Element.MaxVal), Element.GetValue(Element.MinVal, Element.MaxVal));
                                                    break;
                                                default:
                                                    switch (Element.Clicked)
                                                    {
                                                        case false:
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
                                                                    if (bool.TryParse(Element.GetValue(Element.MinVal, Element.MaxVal), out bool AlwaysOnTopVal))
                                                                    {
                                                                        Element.SetValue(Element.MinVal, Element.MaxVal, AlwaysOnTopVal.ToString(), false);
                                                                    }
                                                                    else
                                                                    {
                                                                        Element.SetValue(Element.MinVal, Element.MaxVal, "False", false);
                                                                    }
                                                                    break;
                                                                case "Window.Title":
                                                                    Element.SetValue(Element.MinVal, Element.MaxVal, Element.GetValue(Element.MinVal, Element.MaxVal), false);
                                                                    break;
                                                                case "Window.Titlebar":
                                                                    if (bool.TryParse(Element.GetValue(Element.MinVal, Element.MaxVal), out bool TitlebarVal))
                                                                    {
                                                                        Element.SetValue(Element.MinVal, Element.MaxVal, TitlebarVal.ToString(), false);
                                                                    }
                                                                    else
                                                                    {
                                                                        Element.SetValue(Element.MinVal, Element.MaxVal, "True", false);
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

                #region Labeling
                BitFont.DrawBitFontString(WindowCanvas, "VerdanaCustomCharset24", Color.White, "Canvas:", 7, 10);
                BitFont.DrawBitFontString(UIContainer, "VerdanaCustomCharset24", Color.White, "UI Elements:", 7, 10);
                BitFont.DrawBitFontString(Propeties, "VerdanaCustomCharset24", Color.White, "Propeties:", 7, 10);
                //BitFont.DrawBitFontString(Container, "VerdanaCustomCharset24", Color.White, "Code:", 7, 10);
                #endregion Labeling

                preview.App(WindowCanvas);

                #region Rendering
                //Top section
                ImprovedVBE.DrawImageAlpha(WindowCanvas, 10, 32, window);
                ImprovedVBE.DrawImageAlpha(Container, 933, 32, window);
                ImprovedVBE.DrawImageAlpha(Propeties, 1488, 32, window);

                //Bottom section
                ImprovedVBE.DrawImageAlpha(UIContainer, 10, 721, window);
                ImprovedVBE.DrawImageAlpha(Files, 420, 721, window);
                ImprovedVBE.DrawImageAlpha(BuildLog, 1488, 721, window);
                #endregion Rendering
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
