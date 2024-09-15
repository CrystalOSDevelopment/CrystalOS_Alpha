using Cosmos.System;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications.FileSys;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Programming.CrystalSharp;
using CrystalOSAlpha.Programming.CrystalSharp.Graphics;
using CrystalOSAlpha.System32;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using CrystalOSAlpha.UI_Elements.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection.Emit;
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
        public int CursorX = 0;
        public int CursorY = 0;

        public bool WaitForResponse = false;
        public bool temp = true;
        public bool Rerender = false;

        public string code = "";
        public string Back_content = "";
        public string lineCount = "";
        public string ThatID = "";
        public static string Typo = "";
        public string ActiveDirectory = "";
        public string WorkingFile = "Main.wlf";
        public string TableType = "WIndow";
        public string PropetiesType = "Window";
        public string ActiveSection = "Window";

        public List<string> Elements = new List<string> { "Label", "Button", "TextBox", "Slider", "PictureBox", "CheckBox", "Triangle", "Rectangle", "Circle", "Line", "Polygon" };
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
        public bool Building = false;
        public int BuildingPhase = 0;
        public List<string> Log = new List<string>();
        #endregion Compiler

        public void App()
        {
            if (initial == true)
            {
                Log = new List<string>(){ "CarbonIDE designed by Crystal Development.", "All rights reserved. Since 2023." };

                //Read in the data
                name += " - " + Path.Remove(0, Path.LastIndexOf('\\') + 1);

                ActiveDirectory = Path;

                //Set up the working directories, if it's not done already
                #region InitDirs
                //Directory.Delete("0:\\User\\Source\\Hello", true); <-- this will worth millions!!!
                switch (!Directory.Exists(Path + "\\Window_Layout"))
                {
                    case true:
                        Directory.CreateDirectory(Path + "\\Window_Layout");
                        break;
                }
                switch (!Directory.Exists(Path + "\\Scripts"))
                {
                    case true:
                        Directory.CreateDirectory(Path + "\\Scripts");
                        break;
                }
                #endregion InitDirs

                //Generate a starting code, if the file doesn't exist/empty
                #region InitCode
                switch (File.Exists(Path + "\\Window_Layout\\Main.wlf"))
                {
                    case true:
                        code = File.ReadAllText(Path + "\\Window_Layout\\Main.wlf");
                        if(code.Length == 0)
                        {
                            code = CodeGenerator.GenerateBase("");
                            File.WriteAllText(Path + "\\Window_Layout\\Main.wlf", code);
                        }
                        break;
                    case false:
                        code = CodeGenerator.GenerateBase("");
                        File.WriteAllText(Path + "\\Window_Layout\\Main.wlf", code);
                        break;
                }
                switch (File.Exists(Path + "\\Window_Layout\\Main2.wlf"))
                {
                    case true:
                        string test = File.ReadAllText(Path + "\\Window_Layout\\Main2.wlf");
                        if (test.Length == 0)
                        {
                            test = CodeGenerator.GenerateBase("2");
                            File.WriteAllText(Path + "\\Window_Layout\\Main2.wlf", test);
                        }
                        break;
                    case false:
                        string test1 = CodeGenerator.GenerateBase("2");
                        File.WriteAllText(Path + "\\Window_Layout\\Main2.wlf", test1);
                        break;
                }
                switch (File.Exists(Path + "\\Scripts\\Base.cs"))
                {
                    case true:
                        //If I get to the assembly part, this will be used to build the code into an app
                        //File.WriteAllText(Path + "\\Scripts\\Base.cs", 
                        //    "class Base\n" +
                        //    "{\n" +
                        //    "    public void BeforeRun()\n" +
                        //    "    {\n" +
                        //    "        //This is just a demo text\n" +
                        //    "    }\n" +
                        //    "\n" +
                        //    "    public void Run()\n" +
                        //    "    {\n" +
                        //    "        //This is like an infinite loop. Just like in COSMOS\n" +
                        //    "    }\n" +
                        //    "}");
                        break;
                    case false:
                        //If I get to the assembly part, this will be used to build the code into an app
                        File.WriteAllText(Path + "\\Scripts\\Base.cs",
                            "class Base\n" +
                            "{\n" +
                            "    public void BeforeRun()\n" +
                            "    {\n" +
                            "        //This executes only once at the start\n" +
                            "    }\n" +
                            "\n" +
                            "    public void Run()\n" +
                            "    {\n" +
                            "        //This is like an infinite loop. Just like in COSMOS\n" +
                            "    }\n" +
                            "}");
                        break;
                }
                switch (File.Exists(Path + "\\MKFILE.mkf"))
                {
                    case false:
                        File.WriteAllText(Path + "\\MKFILE.mkf", "INCLUDE:\nMain.wlf\nMain2.wlf\nBase.cs\nSGN: Y\nPBLSHR: " + GlobalValues.Username + "\n\nSTRTWNDW:Main2");
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
                PropetiesTab.Add(new Button(185, 18, 115, 25, "Edit MAKEFILE", 1, "MKFILE"));
                PropetiesTab.Add(new Button(317, -12, 95, 25, "UI elements", 1, "UI"));
                #endregion Propeties

                //Initialize the filemanager
                #region Filemanager
                //Filepath textbox
                FilesTab.Add(new TextBox(6, 228, 725, 35, ImprovedVBE.colourToNumber(60, 60, 60), Path, "0:\\", TextBox.Options.left, "SourcePath"));
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

            if(WaitForResponse == true)
            {
                var Response = WindowMessenger.Recieve("Element Selector", AppID.ToString());
                if (Response != null)
                {
                    TableType = "Element";
                    var Element = preview.UIElements[int.Parse(Response.Message)];
                    switch (Element.TypeOfElement)
                    {
                        case TypeOfElement.Button:
                            int ImndexOfTable = PropetiesTab.FindIndex(d => d.ID == "Propeties");
                            PropetiesTab[ImndexOfTable] = new Table(5, 80, 407, 350, 2, 9, "Propeties");
                            //Left side
                            PropetiesTab[ImndexOfTable].SetValue(0, 0, "Button.X", true);
                            PropetiesTab[ImndexOfTable].SetValue(0, 1, "Button.Y", true);
                            PropetiesTab[ImndexOfTable].SetValue(0, 2, "Button.Width", true);
                            PropetiesTab[ImndexOfTable].SetValue(0, 3, "Button.Height", true);
                            PropetiesTab[ImndexOfTable].SetValue(0, 4, "Button.Color", true);
                            PropetiesTab[ImndexOfTable].SetValue(0, 5, "Button.Text", true);
                            PropetiesTab[ImndexOfTable].SetValue(0, 6, "Button.ID", true);
                            PropetiesTab[ImndexOfTable].SetValue(0, 7, "Button.Visible", true);
                            PropetiesTab[ImndexOfTable].SetValue(0, 8, "Button.Tooltip", true);

                            //Right side
                            PropetiesTab[ImndexOfTable].SetValue(1, 0, Element.X.ToString(), false);
                            PropetiesTab[ImndexOfTable].SetValue(1, 1, Element.Y.ToString(), false);
                            PropetiesTab[ImndexOfTable].SetValue(1, 2, Element.Width.ToString(), false);
                            PropetiesTab[ImndexOfTable].SetValue(1, 3, Element.Height.ToString(), false);
                            PropetiesTab[ImndexOfTable].SetValue(1, 4, Element.Color.ToString(), false);
                            PropetiesTab[ImndexOfTable].SetValue(1, 5, Element.Text, false);
                            PropetiesTab[ImndexOfTable].SetValue(1, 6, Element.ID, false);
                            //PropetiesTab[ImndexOfTable].SetValue(1, 7, Element.Visible.ToString(), false);
                            //PropetiesTab[ImndexOfTable].SetValue(1, 8, Element.Tooltip, false);
                            break;
                        case TypeOfElement.Label:
                            int ImndexOfTable1 = PropetiesTab.FindIndex(d => d.ID == "Propeties");
                            PropetiesTab[ImndexOfTable1] = new Table(5, 80, 407, 300, 2, 7, "Propeties");
                            //Left side
                            PropetiesTab[ImndexOfTable1].SetValue(0, 0, "Label.X", true);
                            PropetiesTab[ImndexOfTable1].SetValue(0, 1, "Label.Y", true);
                            PropetiesTab[ImndexOfTable1].SetValue(0, 2, "Label.Color", true);
                            PropetiesTab[ImndexOfTable1].SetValue(0, 3, "Label.Text", true);
                            PropetiesTab[ImndexOfTable1].SetValue(0, 4, "Label.ID", true);
                            PropetiesTab[ImndexOfTable1].SetValue(0, 5, "Label.Visible", true);
                            PropetiesTab[ImndexOfTable1].SetValue(0, 6, "Label.Tooltip", true);

                            //Right side
                            PropetiesTab[ImndexOfTable1].SetValue(1, 0, Element.X.ToString(), false);
                            PropetiesTab[ImndexOfTable1].SetValue(1, 1, Element.Y.ToString(), false);
                            PropetiesTab[ImndexOfTable1].SetValue(1, 2, Element.Color.ToString(), false);
                            PropetiesTab[ImndexOfTable1].SetValue(1, 3, Element.Text, false);
                            PropetiesTab[ImndexOfTable1].SetValue(1, 4, Element.ID, false);
                            //PropetiesTab[ImndexOfTable1].SetValue(1, 5, Element.Visible.ToString(), false);
                            //PropetiesTab[ImndexOfTable1].SetValue(1, 6, Element.Tooltip, false);
                            break;
                        case TypeOfElement.TextBox:
                            int ImndexOfTable2 = PropetiesTab.FindIndex(d => d.ID == "Propeties");
                            PropetiesTab[ImndexOfTable2] = new Table(5, 80, 407, 300, 2, 9, "Propeties");
                            //Left side
                            PropetiesTab[ImndexOfTable2].SetValue(0, 0, "TextBox.X", true);
                            PropetiesTab[ImndexOfTable2].SetValue(0, 1, "TextBox.Y", true);
                            PropetiesTab[ImndexOfTable2].SetValue(0, 2, "TextBox.Width", true);
                            PropetiesTab[ImndexOfTable2].SetValue(0, 3, "TextBox.Height", true);
                            PropetiesTab[ImndexOfTable2].SetValue(0, 4, "TextBox.Color", true);
                            PropetiesTab[ImndexOfTable2].SetValue(0, 5, "TextBox.Text", true);
                            PropetiesTab[ImndexOfTable2].SetValue(0, 6, "TextBox.ID", true);
                            PropetiesTab[ImndexOfTable2].SetValue(0, 7, "TextBox.Visible", true);
                            PropetiesTab[ImndexOfTable2].SetValue(0, 8, "TextBox.Tooltip", true);

                            //Right side
                            PropetiesTab[ImndexOfTable2].SetValue(1, 0, Element.X.ToString(), false);
                            PropetiesTab[ImndexOfTable2].SetValue(1, 1, Element.Y.ToString(), false);
                            PropetiesTab[ImndexOfTable2].SetValue(1, 2, Element.Width.ToString(), false);
                            PropetiesTab[ImndexOfTable2].SetValue(1, 3, Element.Height.ToString(), false);
                            PropetiesTab[ImndexOfTable2].SetValue(1, 4, Element.Color.ToString(), false);
                            PropetiesTab[ImndexOfTable2].SetValue(1, 5, Element.Text, false);
                            PropetiesTab[ImndexOfTable2].SetValue(1, 6, Element.ID, false);
                            //PropetiesTab[ImndexOfTable2].SetValue(1, 7, Element.Visible.ToString(), false);
                            //PropetiesTab[ImndexOfTable2].SetValue(1, 8, Element.Tooltip, false);
                            //Propeties
                            break;
                        case TypeOfElement.Slider:
                            int ImndexOfTable3 = PropetiesTab.FindIndex(d => d.ID == "Propeties");
                            PropetiesTab[ImndexOfTable3] = new Table(5, 80, 407, 300, 2, 10, "Propeties");
                            //Left side
                            PropetiesTab[ImndexOfTable3].SetValue(0, 0, "Slider.X", true);
                            PropetiesTab[ImndexOfTable3].SetValue(0, 1, "Slider.Y", true);
                            PropetiesTab[ImndexOfTable3].SetValue(0, 2, "Slider.Width", true);
                            PropetiesTab[ImndexOfTable3].SetValue(0, 3, "Slider.Height", true);
                            PropetiesTab[ImndexOfTable3].SetValue(0, 4, "Slider.MinVal", true);
                            PropetiesTab[ImndexOfTable3].SetValue(0, 5, "Slider.MaxVal", true);
                            PropetiesTab[ImndexOfTable3].SetValue(0, 6, "Slider.Value", true);
                            PropetiesTab[ImndexOfTable3].SetValue(0, 7, "Slider.ID", true);
                            PropetiesTab[ImndexOfTable3].SetValue(0, 8, "Slider.Visible", true);
                            PropetiesTab[ImndexOfTable3].SetValue(0, 9, "Slider.Tooltip", true);

                            //Right side
                            PropetiesTab[ImndexOfTable3].SetValue(1, 0, Element.X.ToString(), false);
                            PropetiesTab[ImndexOfTable3].SetValue(1, 1, Element.Y.ToString(), false);
                            PropetiesTab[ImndexOfTable3].SetValue(1, 2, Element.Width.ToString(), false);
                            PropetiesTab[ImndexOfTable3].SetValue(1, 3, Element.Height.ToString(), false);
                            PropetiesTab[ImndexOfTable3].SetValue(1, 4, Element.MinVal.ToString(), false);
                            PropetiesTab[ImndexOfTable3].SetValue(1, 5, Element.MaxVal.ToString(), false);
                            PropetiesTab[ImndexOfTable3].SetValue(1, 6, Element.Value.ToString(), false);
                            PropetiesTab[ImndexOfTable3].SetValue(1, 7, Element.ID, false);
                            //PropetiesTab[ImndexOfTable3].SetValue(1, 8, Element.Visible.ToString(), false);
                            //PropetiesTab[ImndexOfTable3].SetValue(1, 9, Element.Tooltip, false);
                            break;
                        case TypeOfElement.PictureBox:
                            int ImndexOfTable4 = PropetiesTab.FindIndex(d => d.ID == "Propeties");
                            PropetiesTab[ImndexOfTable4] = new Table(5, 80, 407, 300, 2, 8, "Propeties");
                            //Left side
                            PropetiesTab[ImndexOfTable4].SetValue(0, 0, "PictureBox.X", true);
                            PropetiesTab[ImndexOfTable4].SetValue(0, 1, "PictureBox.Y", true);
                            PropetiesTab[ImndexOfTable4].SetValue(0, 2, "PictureBox.Width", true);
                            PropetiesTab[ImndexOfTable4].SetValue(0, 3, "PictureBox.Height", true);
                            PropetiesTab[ImndexOfTable4].SetValue(0, 4, "PictureBox.Source", true);
                            PropetiesTab[ImndexOfTable4].SetValue(0, 5, "PictureBox.ID", true);
                            PropetiesTab[ImndexOfTable4].SetValue(0, 6, "PictureBox.Visible", true);
                            PropetiesTab[ImndexOfTable4].SetValue(0, 7, "PictureBox.Tooltip", true);

                            //Right side
                            PropetiesTab[ImndexOfTable4].SetValue(1, 0, Element.X.ToString(), false);
                            PropetiesTab[ImndexOfTable4].SetValue(1, 1, Element.Y.ToString(), false);
                            PropetiesTab[ImndexOfTable4].SetValue(1, 2, Element.Width.ToString(), false);
                            PropetiesTab[ImndexOfTable4].SetValue(1, 3, Element.Height.ToString(), false);
                            PropetiesTab[ImndexOfTable4].SetValue(1, 4, "", false);
                            PropetiesTab[ImndexOfTable4].SetValue(1, 5, Element.ID, false);
                            PropetiesTab[ImndexOfTable4].SetValue(1, 6, "True", false);
                            //PropetiesTab[ImndexOfTable4].SetValue(1, 4, Element.Tooltip, false);
                            break;
                    }
                    WindowMessenger.Message.Remove(Response);
                    temp = true;
                    WaitForResponse = false;
                }
            }

            if((MouseManager.MouseState == MouseState.Left || clicked == true || MouseManager.ScrollDelta != 0) && TaskScheduler.Apps[^1] == this)
            {
                if(MouseManager.X <= 933 + Container.Width && MouseManager.X > 933 && MouseManager.Y > y + 32 && MouseManager.Y < y + 32 + Container.Height)
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
                                        Element.Value = Math.Clamp(Element.Value + MouseManager.ScrollDelta * 35, Element.MinVal, Element.MaxVal);
                                        Element.Pos = (int)(Element.Value / Element.Sensitivity) + 20;
                                        break;
                                }
                                break;
                            case false:
                                switch (Element.TypeOfElement)
                                {
                                    case TypeOfElement.HorizontalScrollbar:
                                        Element.Value = Math.Clamp(Element.Value + MouseManager.ScrollDelta * 35, Element.MinVal, Element.MaxVal);
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
                switch (clicked)
                {
                    case false:
                        if(MouseManager.Y < y + 32 + WindowCanvas.Height && MouseManager.Y > y + 32)
                        {
                            if (MouseManager.X < 10 + WindowCanvas.Width)
                            {
                                ActiveSection = "Window";
                            }
                            else if (MouseManager.X < 933 + Container.Width)
                            {
                                ActiveSection = "CodeContainer";
                            }
                            else if (MouseManager.X < 1488 + Propeties.Width)
                            {
                                ActiveSection = "Propeties";
                            }
                            temp = true;
                        }
                        else
                        {
                            if(MouseManager.X > 10 && MouseManager.X < 10 + UIContainer.Width)
                            {
                                ActiveSection = "UIContainer";
                            }
                            else if(MouseManager.X > 420 && MouseManager.X < 420 + Files.Width)
                            {
                                ActiveSection = "Files";
                            }
                            else if (MouseManager.X > 1488 && MouseManager.X < 1488 + BuildLog.Width)
                            {
                                ActiveSection = "BuildLog";
                            }
                            temp = true;
                         }
                        break;
                }
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
                                    code = CodeGenerator.AddUIElement(code, new PictureBox(StoredX, StoredY - 22, "PictureBox" + preview.UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.PictureBox).Count, true, new Bitmap((uint)WidthOfUI, (uint)HeightOfUI, ColorDepth.ColorDepth32)), "", true);
                                    break;
                                case 5:
                                    code = CodeGenerator.AddUIElement(code, new CheckBox(StoredX, StoredY - 42, WidthOfUI, HeightOfUI, true, "CheckBox" + preview.UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.CheckBox).Count, "CheckBox"));
                                    break;
                                case 6:
                                    code = CodeGenerator.AddUIElement(code, new Triangle(ImprovedVBE.colourToNumber(255, 0, 0), new List<Point> { }, true, true, "Triangle" + preview.UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.Triangle).Count), true.ToString());
                                    break;
                                case 7:
                                    code = CodeGenerator.AddUIElement(code, new UI_Elements.Shapes.Rectangle(ImprovedVBE.colourToNumber(245, 245, 220), new List<Point> { }, true, true, "Rectangle" + preview.UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.Rectangle).Count), true.ToString());
                                    break;
                                case 8:
                                    code = CodeGenerator.AddUIElement(code, new UI_Elements.Shapes.Circle(ImprovedVBE.colourToNumber(210, 105, 30), new List<Point> { }, true, true, "Circle" + preview.UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.Circle).Count), true.ToString());
                                    break;
                                case 9:
                                    code = CodeGenerator.AddUIElement(code, new UI_Elements.Shapes.Line(ImprovedVBE.colourToNumber(0, 255, 0), new List<Point> { }, true, true, "Line" + preview.UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.Line).Count), true.ToString());
                                    break;
                                case 10:
                                    code = CodeGenerator.AddUIElement(code, new UI_Elements.Shapes.Polygon(ImprovedVBE.colourToNumber(0, 255, 0), new List<Point> { }, true, true, "Polygon" + preview.UIElements.FindAll(d => d.TypeOfElement == TypeOfElement.Polygon).Count), true.ToString());
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
                        //Enable building
                        Building = true;
                        //Reseting the BuildPhase
                        BuildingPhase = 0;
                        break;
                    default:
                        if(ActiveSection == "CodeContainer")
                        {
                            if (ActiveSection == "CodeContainer")
                            {
                                (code, Back_content, CursorX, CursorY) = CrystalOSAlpha.Applications.CarbonIDE.CoreEditor.Editor(code, Back_content, CursorX, CursorY, KeyPress);
                            }
                        }
                        temp = true;
                        break;
                }
            }

            switch (Building)
            {
                case true:
                    switch (BuildingPhase)
                    {
                        case 0:
                            Log.Clear();
                            //Build the file
                            //For now, it only consists of the window layout
                            Log.Add("Building started...");
                            Log.Add("Saving currently opened file");
                            if (WorkingFile.EndsWith(".cs"))
                            {
                                File.WriteAllText(Path + "\\Scripts\\" + WorkingFile, code);
                            }
                            else
                            {
                                File.WriteAllText(Path + "\\Window_Layout\\" + WorkingFile, code);
                            }
                            Log.Add("Directory init");
                            if (!Directory.Exists(Path + "\\Bin"))
                            {
                                Directory.CreateDirectory(Path + "\\Bin");
                            }
                            Log.Add("Building output app");
                            string BuiltFile = new ExecutableCreator().CreateExecutable(Path, File.ReadAllText(Path + "\\MKFILE.mkf"));
                            BuiltFile = Core.RemoveCommentsAndBlankLines(BuiltFile);
                            File.Create(Path + "\\Bin\\" + Path.Remove(0, Path.LastIndexOf('\\') + 1) + ".app");
                            File.WriteAllText(Path + "\\Bin\\" + Path.Remove(0, Path.LastIndexOf('\\') + 1) + ".app", BuiltFile);
                            File.WriteAllText(Path + "\\Bin\\" + Path.Remove(0, Path.LastIndexOf('\\') + 1) + ".txt", BuiltFile);
                            BuildingPhase++;
                            break;
                        case 1:
                            Log.Add("Build successful!");
                            TaskScheduler.Apps.Add(new Window(100, 100, 999, 350, 200, 0, "Untitled", false, icon, File.ReadAllText(Path + "\\Bin\\" + Path.Remove(0, Path.LastIndexOf('\\') + 1) + ".app")));
                            BuildingPhase++;
                            Building = false;
                            break;
                    }

                    //Reset buildlog window
                    Array.Fill(BuildLog.RawData, ImprovedVBE.colourToNumber(36, 36, 36));//Background
                    ImprovedVBE.DrawFilledRectangle(BuildLog, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)BuildLog.Width - 4, (int)BuildLog.Height - 4, false);//Border

                    //Render out the debug window
                    string LogOutput = "";
                    foreach (string s in Log)
                    {
                        LogOutput += s + "\n";
                    }
                    BitFont.DrawBitFontString(BuildLog, "ArialCustomCharset16", Color.White, LogOutput, 10, 10);

                    //Render buildlog
                    ImprovedVBE.DrawImage(BuildLog, 1488, 721, window);
                    break;
            }

            if (preview.Code != code && !WorkingFile.EndsWith(".cs"))
            {
                Back_content = code;
                preview = new Window(20, 50, 999, 400, 300, 1, "New Window", false, icon, code);
                temp = true;
            }

            if (temp == true && clicked == false || Rerender)
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

                Array.Fill(BuildLog.RawData, ImprovedVBE.colourToNumber(36, 36, 36));//Background
                ImprovedVBE.DrawFilledRectangle(BuildLog, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)BuildLog.Width - 4, (int)BuildLog.Height - 4, false);//Border

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
                                if (MouseManager.X > 10 + XOffset && MouseManager.X < 190 + XOffset && MouseManager.Y > 721 + 30 + y + YOffset && MouseManager.Y < 721 + y + 30 + YOffset + 35)
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
                                //UIElements.Find(d => d.ID == "Path").Text = "0:\\";
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
                                                        System.IO.File.WriteAllText(Path + "\\Window_Layout\\" + WorkingFile, code);
                                                        code = System.IO.File.ReadAllText(entry.fullPath);
                                                        Back_content = code;
                                                        WorkingFile = entry.name;
                                                        break;
                                                    default:
                                                        switch(entry.name[^3..])
                                                        {
                                                            case ".cs":
                                                                System.IO.File.WriteAllText(Path + "\\Window_Layout\\" + WorkingFile, code);
                                                                code = System.IO.File.ReadAllText(entry.fullPath);
                                                                Back_content = code;
                                                                WorkingFile = entry.name;
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
                List<UIElementHandler> Temp = PropetiesTab;
                foreach (var Element in PropetiesTab)
                {
                    switch (Element.TypeOfElement)
                    {
                        case TypeOfElement.Table:
                            if (MouseManager.MouseState == MouseState.Left)
                            {
                                Element.CheckClick(1488, y + 32);
                            }
                            switch (TableType)
                            {
                                case "Element":
                                    switch (Element.MinVal > 0)
                                    {
                                        case true:
                                            switch (KeyPress)
                                            {
                                                case null:
                                                    code = CodeGenerator.ModifyUIElement(code, Element);
                                                    break;
                                                default:
                                                    switch (KeyPress.Key)
                                                    {
                                                        case ConsoleKeyEx.Enter:
                                                            code = CodeGenerator.ModifyUIElement(code, Element);
                                                            break;
                                                        default:
                                                            switch (Element.Clicked)//Element.Clicked means if it's write-protected or not
                                                            {
                                                                case false:
                                                                    KeyPressed = true;
                                                                    Element.SetValue(Element.MinVal, Element.MaxVal, Keyboard.HandleKeyboard(Element.GetValue(Element.MinVal, Element.MaxVal), KeyPress), false);
                                                                    switch (Element.GetValue(Element.MinVal - 1, Element.MaxVal).Split(".")[1])
                                                                    {
                                                                        #region Values
                                                                        case "X":
                                                                            if (int.TryParse(Element.GetValue(Element.MinVal, Element.MaxVal), out int XVal))
                                                                            {
                                                                                if (XVal < 0)
                                                                                {
                                                                                    Element.SetValue(Element.MinVal, Element.MaxVal, "0", false);
                                                                                }
                                                                                else
                                                                                {
                                                                                    Element.SetValue(Element.MinVal, Element.MaxVal, XVal.ToString(), false);
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                Element.SetValue(Element.MinVal, Element.MaxVal, "0", false);
                                                                            }
                                                                            break;
                                                                        case "Y":
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
                                                                            else
                                                                            {
                                                                                Element.SetValue(Element.MinVal, Element.MaxVal, "0", false);
                                                                            }
                                                                            break;
                                                                        case "Width":
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
                                                                            else
                                                                            {
                                                                                Element.SetValue(Element.MinVal, Element.MaxVal, "0", false);
                                                                            }
                                                                            break;
                                                                        case "Height":
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
                                                                            else
                                                                            {
                                                                                Element.SetValue(Element.MinVal, Element.MaxVal, "0", false);
                                                                            }
                                                                            break;
                                                                        case "Visible":
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
                                                                            #endregion Values
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
                                case "Window":
                                    if(MouseManager.MouseState == MouseState.Left)
                                    {
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
                            }
                            break;
                        case TypeOfElement.Button:
                            Element.CheckClick(1488, y + 32);
                            if (Element.Clicked)
                            {
                                switch (Element.ID)
                                {
                                    case "Window":
                                        Temp.Clear();
                                        Temp.Add(new Table(5, 80, 407, 300, 2, 7, "Propeties"));

                                        //Button
                                        PropetiesTab.Add(new Button(5, 612, 204, 35, "OnClick", 1, "Click"));
                                        PropetiesTab.Add(new Button(214, 612, 204, 35, "Hover", 1, "Hovering"));

                                        //Buttons for propety of window/UI elements
                                        PropetiesTab.Add(new Button(185, -12, 115, 25, "Window prop...", 1, "Window"));
                                        PropetiesTab.Add(new Button(185, 18, 115, 25, "Edit MAKEFILE", 1, "MKFILE"));
                                        PropetiesTab.Add(new Button(317, -12, 95, 25, "UI elements", 1, "UI"));

                                        //Reset table to adjust the window propeties
                                        Temp.Find(x => x.ID == "Propeties").SetValue(0, 0, "Window.X", true);
                                        Temp.Find(x => x.ID == "Propeties").SetValue(0, 1, "Window.Y", true);
                                        Temp.Find(x => x.ID == "Propeties").SetValue(0, 2, "Window.Width", true);
                                        Temp.Find(x => x.ID == "Propeties").SetValue(0, 3, "Window.Height", true);
                                        Temp.Find(x => x.ID == "Propeties").SetValue(0, 4, "Window.AlwaysOnTop", true);
                                        Temp.Find(x => x.ID == "Propeties").SetValue(0, 5, "Window.Title", true);
                                        Temp.Find(x => x.ID == "Propeties").SetValue(0, 6, "Window.Titlebar", true);

                                        Temp.Find(x => x.ID == "Propeties").SetValue(1, 0, preview.x.ToString(), false);
                                        Temp.Find(x => x.ID == "Propeties").SetValue(1, 1, preview.y.ToString(), false);
                                        Temp.Find(x => x.ID == "Propeties").SetValue(1, 2, preview.width.ToString(), false);
                                        Temp.Find(x => x.ID == "Propeties").SetValue(1, 3, preview.height.ToString(), false);
                                        Temp.Find(x => x.ID == "Propeties").SetValue(1, 4, preview.AlwaysOnTop.ToString(), false);
                                        Temp.Find(x => x.ID == "Propeties").SetValue(1, 5, preview.name, false);
                                        Temp.Find(x => x.ID == "Propeties").SetValue(1, 6, preview.HasTitlebar.ToString(), false);

                                        TableType = "Window";
                                        PropetiesType = "Window";
                                        break;
                                    case "UI":
                                        Temp.Clear();

                                        Temp.Add(new Table(5, 80, 407, 300, 2, 7, "Propeties"));

                                        //Buttons for propety of window/UI elements
                                        PropetiesTab.Add(new Button(185, -12, 115, 25, "Window prop...", 1, "Window"));
                                        PropetiesTab.Add(new Button(185, 18, 115, 25, "Edit MAKEFILE", 1, "MKFILE"));
                                        PropetiesTab.Add(new Button(317, -12, 95, 25, "UI elements", 1, "UI"));

                                        //Yes, this is a ToDo list, but it's already predefined how I wish to do it.
                                        //Create a new window that lists all the UI elements
                                        TaskScheduler.Apps.Add(new ElementSelector(749, 258, 423, 530, AppID, preview.UIElements, this.icon));
                                        //After the user selected one (by double clicking/enter key/ok button), the window will close after sending a message to this app defining which element is selected.
                                        //Since we can't leave it in here, because it'd hang the os, a boolean will be set to true, so it'll wait for a response.
                                        WaitForResponse = true;
                                        //The update the table to modify the correct values
                                        PropetiesType = "Element";
                                        break;
                                    case "MKFILE":
                                        Temp.Clear();
                                        //Buttons for propety of window/UI elements
                                        Temp.Add(new Button(185, -12, 115, 25, "Window prop...", 1, "Window"));
                                        Temp.Add(new Button(185, 18, 115, 25, "Edit MAKEFILE", 1, "MKFILE"));
                                        Temp.Add(new Button(317, -12, 95, 25, "UI elements", 1, "UI"));

                                        //Makefile propeties
                                        Temp.Add(new CheckBox(15, 66, 25, 25, true, "BuildDate", "Add build date"));
                                        Temp.Add(new TextBox(15, 129, 250, 25, ImprovedVBE.colourToNumber(60, 60, 60), "v1.0", "v1.0", TextBox.Options.left, "Version"));
                                        Temp.Add(new TextBox(15, 172, 250, 25, ImprovedVBE.colourToNumber(60, 60, 60), "\\Icon\\Icon.bmp", "*.bmp (50x50x32 Max)", TextBox.Options.left, "IconPath"));
                                        Temp.Add(new Button(15, 236, 250, 25, ".wlf files to include", 1, "MainFile"));
                                        Temp.Add(new CheckBox(15, 299, 20, 25, true, "Signed", "Signed"));
                                        Temp.Add(new CheckBox(15, 362, 20, 25, true, "Anonim", "Anonim publisher"));
                                        List<values> Vals = new List<values>();
                                        foreach(DirectoryEntry d in Kernel.fs.GetDirectoryListing(Path + "\\Window_Layout"))
                                        {
                                            if(d.mEntryType == DirectoryEntryTypeEnum.File)
                                            {
                                                Vals.Add(new values(false, d.mName, "StartWindow"));
                                            }
                                        }
                                        if(Vals.Count != 0)
                                        {
                                            Vals[^1].Highlighted = true;
                                        }
                                        Temp.Add(new Dropdown(15, 215, 250, 25, "StartWindow", Vals));
                                        PropetiesType = "MKFILE";
                                        break;
                                }
                            }
                            break;
                        case TypeOfElement.CheckBox:
                            if (clicked == false)
                            {
                                if (Element.CheckClick(1488, y + 32))
                                {
                                    foreach (UIElementHandler UI in PropetiesTab)
                                    {
                                        if (UI.TypeOfElement != TypeOfElement.CheckBox)
                                        {
                                            UI.Clicked = false;
                                        }
                                    }
                                    clicked = true;
                                }
                            }
                            break;
                        case TypeOfElement.DropDown:
                            Element.CheckClick(1488, y + 32);
                            break;
                    }
                    Element.Render(Propeties);
                }
                if(Temp != PropetiesTab)
                {
                    PropetiesTab = Temp;
                    Rerender = true;
                }
                else
                {
                    Rerender = false;
                }
                if(PropetiesType == "MKFILE")
                {
                    string DataToWrite =
                        (PropetiesTab.Find(d => d.ID == "BuildDate").Clicked ? "DATE: " + DateTime.Now.Year + "." + DateTime.Now.Month + "." + DateTime.Now.Day + "\n" : "") +
                        "VER: " + PropetiesTab.Find(d => d.ID == "Version").Text + "\n" +
                        "ICON: " + PropetiesTab.Find(d => d.ID == "IconPath").Text + "\n" +
                        "INCLUDE: \n" + 
                        "Main.wlf\n" + 
                        "Main2.wlf\n" + 
                        "Base.cs\n" + 
                        "SGN: " + (PropetiesTab.Find(d => d.ID == "Signed").Clicked ? "Y":"N") + "\n" + 
                        "PBLSHR: " + (PropetiesTab.Find(d => d.ID == "Anonim").Clicked ? GlobalValues.Username : "Anonim") + "\n" +
                        "STRTWNDW: " + PropetiesTab.Find(d => d.ID == "StartWindow").Text.Split(".")[0];

                    File.WriteAllText(Path + "\\MKFILE.mkf", DataToWrite);
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
