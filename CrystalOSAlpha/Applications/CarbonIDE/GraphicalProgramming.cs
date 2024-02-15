using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOS_Alpha;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Programming;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
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

        public string name { get; set; }
        public string Path { get; set; }
        public string namedProject { get; set; }

        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }

        public Bitmap canvas;
        public Bitmap window;

        public int CurrentColor = ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B);
        public int x_1 = 0;

        public bool initial = true;
        public bool once = true;
        public bool clicked = false;
        #endregion Essential

        #region App UI
        public List<Button_prop> Button = new List<Button_prop>();
        public List<Slider> Slider = new List<Slider>();
        public List<CheckBox> CheckBox = new List<CheckBox>();
        public List<Dropdown> Dropdown = new List<Dropdown>();
        public List<Scrollbar_Values> Scroll = new List<Scrollbar_Values>();
        public List<TextBox> TextBox = new List<TextBox>();
        public List<label> Label = new List<label>();
        #endregion App UI

        #region Core variables
        public Bitmap WindowCanvas;
        public Bitmap UIContainer;
        public Bitmap Propeties;
        public Bitmap Container;

        bool temp = true;

        string code = "";

        public List<string> Elements = new List<string> { "Label", "Button", "TextBox", "Slider", "Scrollbar", "PictureBox", "CheckBox", "Radio button", "Progressbar", "Menutab", "Table" };
        public List<Elements> Full = new List<Elements>();
        public List<Elements> Used = new List<Elements>();

        public Table t = new Table(2, 7, 412, 600);
        public Window preview;

        public string Selected = "";

        public int StoredX = 0;
        public int StoredY = 0;

        public string ThatID = "";
        public static string Typo = "";

        public string lineCount = "";

        public string Back_content = "";
        public int cursorIndex = 0;
        public int lineIndex = 0;
        #endregion Core variables

        public void App()
        {
            //TODO: Make it suitable for all resolution
            if (initial == true)
            {
                //Separate the code into 3 segments:
                //1. Initial window UI element layout **Done! All implemented**
                //2. Loop of the window
                //3. UI element actions

                if(File.Exists(Path + ".app"))
                {
                    code = File.ReadAllText(Path + ".app");
                }
                else
                {
                    code = CodeGenerator.Generate(code, "");
                }


                preview = new Window(20, 50, 999, 400, 300, 1, "New Window", false, icon, code);

                #region Table Magic
                //Initialize the table
                t.Initialize();

                //Set values to the table
                t.SetValue(0, 0, "Window.X", true);
                t.SetValue(0, 1, "10", false);
                t.SetValue(1, 0, "Window.Y", true);
                t.SetValue(1, 1, "10", false);
                t.SetValue(2, 0, "Window.Width", true);
                t.SetValue(2, 1, "500", false);
                t.SetValue(3, 0, "Window.Height", true);
                t.SetValue(3, 1, "300", false);
                t.SetValue(4, 0, "Window.RGB", true);
                t.SetValue(4, 1, "60, 60, 60", false);
                t.SetValue(5, 0, "Window.AlwaysOnTop", true);
                t.SetValue(5, 1, "false", false);
                t.SetValue(6, 0, "Window.Titlebar", true);
                t.SetValue(6, 1, "true", false);
                #endregion Table Magic

                foreach(string s in Elements)
                {
                    Full.Add(new Applications.CarbonIDE.Elements(s, false));
                }

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

                Button.Add(new Button_prop(1609, 632, 180, 40, "OnClick", 1, "onclick"));

                Back_content = code;
                initial = false;
            }

            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);

                WindowCanvas = new Bitmap(1459, 674, ColorDepth.ColorDepth32);
                UIContainer = new Bitmap(1900, 269, ColorDepth.ColorDepth32);
                Propeties = new Bitmap(422, 674, ColorDepth.ColorDepth32);
                Container = new Bitmap(540, 674, ColorDepth.ColorDepth32);

                #region corners
                ImprovedVBE.DrawFilledEllipse(canvas, 10, 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, width - 11, 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, 10, height - 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, width - 11, height - 10, 10, 10, CurrentColor);

                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 0, 10, width, height - 20, false);
                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, 0, width - 10, 15, false);
                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, height - 15, width - 10, 15, false);
                #endregion corners

                #region Border
                Array.Fill(WindowCanvas.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(WindowCanvas, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)WindowCanvas.Width - 4, (int)WindowCanvas.Height - 4, false);

                Array.Fill(UIContainer.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(UIContainer, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)UIContainer.Width - 4, (int)UIContainer.Height - 4, false);

                Array.Fill(Propeties.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(Propeties, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Propeties.Width - 4, (int)Propeties.Height - 4, false);
                #endregion Border

                canvas = ImprovedVBE.DrawImageAlpha2(canvas, x, y, canvas, Color.FromArgb(CurrentColor).R, Color.FromArgb(CurrentColor).G, Color.FromArgb(CurrentColor).B, ImprovedVBE.cover);

                DrawGradientLeftToRight();

                ImprovedVBE.DrawFilledEllipse(canvas, width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                ImprovedVBE.DrawFilledEllipse(canvas, width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);

                foreach (var button in Button)
                {
                    if (button.Clicked == true)
                    {
                        UI_Elements.Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, ComplimentaryColor.Generate(button.Color).ToArgb(), button.Text);
                    }
                    else
                    {
                        UI_Elements.Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                    }
                }

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

                foreach (label l in Label)
                {
                    l.Label(window);
                }

                foreach (Slider s in Slider)
                {
                    s.Render(window);
                }

                foreach (var Box in TextBox)
                {
                    Box.Box(window, Box.X, Box.Y);
                }

                once = false;
                temp = true;
            }

            foreach (var button in Button)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > x + button.X && MouseManager.X < x + button.X + button.Width)
                    {
                        if (MouseManager.Y > y + button.Y && MouseManager.Y < y + button.Y + button.Height)
                        {
                            if (button.Clicked == false)
                            {
                                button.Clicked = true;
                                temp = true;
                                clicked = true;
                            }
                        }
                    }
                }
                if (button.Clicked == true && MouseManager.MouseState == MouseState.None)
                {
                    temp = true;
                    button.Clicked = false;
                    clicked = false;
                }
            }

            foreach (var slid in Slider)
            {
                int val = slid.Value;
                if (slid.CheckForClick(x, y))
                {
                    slid.Clicked = true;
                }
                if (slid.Clicked == true)
                {
                    slid.UpdateValue(x);
                    if (val != slid.Value)
                    {
                        once = true;
                    }
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        slid.Clicked = false;
                    }
                }
            }

            foreach (var Box in TextBox)
            {
                Box.Box(window, Box.X, Box.Y);
                if (Box.Clciked(x + Box.X, y + Box.Y) == true)
                {
                    foreach (var box2 in TextBox)
                    {
                        box2.Selected = false;
                    }
                    Box.Selected = true;
                }
            }

            if (MouseManager.MouseState == MouseState.Left && TaskScheduler.Apps[^1] == this && clicked == false)
            {
                temp = true;
                clicked = true;
                StoredX = (int)MouseManager.X;
                StoredY = (int)MouseManager.Y;
            }
            if (MouseManager.MouseState == MouseState.None && clicked == true)
            {
                temp = true;
                clicked = false;
                if(StoredX > 10 + preview.x && StoredX < 30 + preview.x + preview.width)
                {
                    if(StoredY > 32 + preview.y && StoredY < 82 + preview.y + preview.height)
                    {
                        if(Selected == "Button")
                        {
                            //code = CodeGenerator.Generate(code, "Button btn = new Button(10, 30, 110, 25, \"Hello World\", 1, 1, 1);");
                            int ex = StoredX - 10 - preview.x;
                            int epsz = StoredY - 54 - preview.y;
                            int W = (int)MouseManager.X - ex - 10 - preview.x;
                            int H = (int)MouseManager.Y - epsz - 54 - preview.y;
                            code = CodeGenerator.Generate(code, $"Button btn{preview.Button.Count + 1} = new Button({ex}, {epsz}, {W}, {H}, \"Hello World\", 1, 1, 1);");
                        }
                        else if (Selected == "Label")
                        {
                            //code = CodeGenerator.Generate(code, "Button btn = new Button(10, 30, 110, 25, \"Hello World\", 1, 1, 1);");
                            int ex = StoredX - 10 - preview.x;
                            int epsz = StoredY - 54 - preview.y;
                            int W = (int)MouseManager.X - ex - 10 - preview.x;
                            int H = (int)MouseManager.Y - epsz - 54 - preview.y;
                            code = CodeGenerator.Generate(code, $"Label lbl{preview.Label.Count + 1} = new Label({ex}, {epsz}, \"This is a label!\", 1, 1, 1);");
                        }
                        else if (Selected == "TextBox")
                        {
                            //code = CodeGenerator.Generate(code, "Button btn = new Button(10, 30, 110, 25, \"Hello World\", 1, 1, 1);");
                            int ex = StoredX - 10 - preview.x;
                            int epsz = StoredY - 54 - preview.y;
                            int W = (int)MouseManager.X - ex - 10 - preview.x;
                            int H = (int)MouseManager.Y - epsz - 54 - preview.y;
                            code = CodeGenerator.Generate(code, $"TextBox tBox{preview.TextBox.Count + 1} = new TextBox({ex}, {epsz}, {W}, {H}, 60, 60, 60, \"\", \"Textbox\");");
                        }
                        else if (Selected == "Slider")
                        {
                            //code = CodeGenerator.Generate(code, "Button btn = new Button(10, 30, 110, 25, \"Hello World\", 1, 1, 1);");
                            int ex = StoredX - 10 - preview.x;
                            int epsz = StoredY - 54 - preview.y;
                            int W = (int)MouseManager.X - ex - 10 - preview.x;
                            int H = (int)MouseManager.Y - epsz - 54 - preview.y;
                            code = CodeGenerator.Generate(code, $"Slider slider{preview.Slider.Count + 1} = new Slider({ex}, {epsz}, {W}, 255, 0);");
                        }
                        else if (Selected == "CheckBox")
                        {
                            //code = CodeGenerator.Generate(code, "Button btn = new Button(10, 30, 110, 25, \"Hello World\", 1, 1, 1);");
                            int ex = StoredX - 10 - preview.x;
                            int epsz = StoredY - 54 - preview.y;
                            int W = (int)MouseManager.X - ex - 10 - preview.x;
                            int H = (int)MouseManager.Y - epsz - 54 - preview.y;
                            code = CodeGenerator.Generate(code, $"CheckBox.NewCheckBox({ex}, {epsz}, {W}, {H}, false, \"\", \"\");");
                        }
                        Selected = "";
                        StoredX = 0;
                        StoredY = 0;
                    }
                }
                Back_content = code;
            }

            if(TaskScheduler.Apps[^1] == this)
            {
                KeyEvent k;
                if(KeyboardManager.TryReadKey(out k))
                {
                    if (k.Key == ConsoleKeyEx.F5)
                    {
                        TaskScheduler.Apps.Add(new Window(100, 100, 999, 350, 200, 0, "Later", false, icon, code));
                        File.WriteAllText(Path + ".app", code);
                        //Kernel.Clipboard = Path + "\\" + namedProject + ".app";
                    }
                    else
                    {
                        int counter = 0;
                        bool editing = false;
                        foreach(var v in t.Cells)
                        {
                            if(v.Selected == true && v.WriteProtected == false)
                            {
                                v.Content = Keyboard.HandleKeyboard(v.Content, k);
                                //code = CodeGenerator.Generate(code, "Button btn = new Button(10, 30, 110, 25, \"Hello World\", 1, 1, 1);");
                                if (t.Cells[counter - 1].Content.Contains("Window."))
                                {
                                    code = CodeGenerator.Generate(code, "this." + t.Cells[counter - 1].Content.Replace("Window.", "") + " = " + v.Content + ";");
                                    editing = true;
                                }
                                else
                                {
                                    if(Typo == "Button")
                                    {
                                        if(int.Parse(t.GetValue(1, 2)) > 10)
                                        {
                                            if(t.GetValue(1, 5).Split(',').Length == 3)
                                            {
                                                if(int.TryParse(t.GetValue(1, 5).Split(',')[2].Trim(), out int i))
                                                {
                                                    if(int.Parse(t.GetValue(1, 1)) >= 22)
                                                    {
                                                        code = CodeGenerator.Generate(code, $"Button {ThatID} = new Button({t.GetValue(1, 0)}, {int.Parse(t.GetValue(1, 1)) - 22}, {t.GetValue(1, 2)}, {t.GetValue(1, 3)}, \"{t.GetValue(1, 4)}\", {t.GetValue(1, 5)});");
                                                    }
                                                    else
                                                    {
                                                        code = CodeGenerator.Generate(code, $"Button {ThatID} = new Button({t.GetValue(1, 0)}, {t.GetValue(1, 1)}, {t.GetValue(1, 2)}, {t.GetValue(1, 3)}, \"{t.GetValue(1, 4)}\", {t.GetValue(1, 5)});");
                                                    }
                                                }
                                            }
                                        }
                                        editing = true;
                                    }
                                    else if(Typo == "Label")
                                    {
                                        if(t.GetValue(1, 3).Split(',').Length == 3)
                                        {
                                            if(int.TryParse(t.GetValue(1, 3).Split(',')[2].Trim(), out int i))
                                            {
                                                code = CodeGenerator.Generate(code, $"Label {ThatID} = new Label({t.GetValue(1, 0)}, {t.GetValue(1, 1)}, \"{t.GetValue(1, 2)}\", {t.GetValue(1, 3)});");
                                            }
                                        }
                                        editing = true;
                                    }
                                    else if (Typo == "Slider")
                                    {
                                        code = CodeGenerator.Generate(code, $"Slider {ThatID} = new Slider({t.GetValue(1, 0)}, {t.GetValue(1, 1)}, {t.GetValue(1, 2)}, {t.GetValue(1, 3)});");
                                        editing = true;
                                    }
                                }
                                //Kernel.Clipboard = code;
                                temp = true;
                            }
                            counter++;
                        }
                        Back_content = code;
                        if(editing == false)
                        {
                            (code, Back_content, cursorIndex, lineIndex) = CoreEditor.Editor(code, Back_content, cursorIndex, lineIndex, k);
                            temp = true;
                        }
                    }
                }
            }

            if(preview.Code != code)
            {
                //Kernel.Clipboard = code;
                preview = new Window(20, 50, 999, 400, 300, 1, "New Window", false, icon, code);
                temp = true;
            }

            if(temp == true)
            {
                //Copying canvas to window
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

                //Giving border to Elements
                #region Border
                Array.Fill(WindowCanvas.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(WindowCanvas, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)WindowCanvas.Width - 4, (int)WindowCanvas.Height - 4, false);

                Array.Fill(UIContainer.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(UIContainer, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)UIContainer.Width - 4, (int)UIContainer.Height - 4, false);

                Array.Fill(Propeties.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(Propeties, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Propeties.Width - 4, (int)Propeties.Height - 4, false);

                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(Container, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Container.Width - 4, (int)Container.Height - 4, false);
                ImprovedVBE.DrawFilledRectangle(Container, ImprovedVBE.colourToNumber(69, 69, 69), 2, 2, 35, (int)Container.Height - 4, false);
                #endregion Border

                if (MouseManager.MouseState == MouseState.Left)
                {
                    t.Select((int)(MouseManager.X - 1488), (int)(MouseManager.Y - 82));
                }

                #region Labeling
                BitFont.DrawBitFontString(WindowCanvas, "VerdanaCustomCharset24", Color.White, "Canvas:", 7, 10);
                BitFont.DrawBitFontString(UIContainer, "VerdanaCustomCharset24", Color.White, "UI Elements:", 7, 10);
                BitFont.DrawBitFontString(Propeties, "VerdanaCustomCharset24", Color.White, "Propeties:", 7, 10);
                //BitFont.DrawBitFontString(Container, "VerdanaCustomCharset24", Color.White, "Code:", 7, 10);

                //Showing options to different UI elements

                int Top = 50;
                int Left = 10;
                for(int i = 0; i < Full.Count; i++)
                {
                    if(MouseManager.MouseState == MouseState.Left)
                    {
                        if(MouseManager.X > 10 + Left && MouseManager.X < 10 + Left + 100)
                        {
                            if(MouseManager.Y > 721 + Top && MouseManager.Y < 721 + Top + 30)
                            {
                                foreach(var d in Full)
                                {
                                    d.Selected = false;
                                    Selected = Full[i].Name;
                                }
                                Full[i].Selected = true;
                                i = 0;
                                Top = 50;
                                Left = 10;
                            }
                        }
                    }
                    if (Full[i].Name != Selected)
                    {
                        Full[i].Selected = false;
                    }
                    if(Full[i].Selected == false)
                    {
                        BitFont.DrawBitFontString(UIContainer, "ArialCustomCharset16", Color.White, Full[i].Name, Left, Top);
                    }
                    else
                    {
                        BitFont.DrawBitFontString(UIContainer, "ArialCustomCharset16", Color.Black, Full[i].Name, Left, Top);
                    }
                    if(Top > 200)
                    {
                        Top = 50;
                        Left += 100;
                    }
                    else
                    {
                        Top += 30;
                    }
                }
                #endregion Labeling

                preview.App(WindowCanvas);

                Top = 50;
                Left = 900;
                foreach(var v in preview.Button)
                {
                    if(Used.Where(d => d.Name == v.ID).Count() == 0)
                    {
                        Used.Add(new Applications.CarbonIDE.Elements(v.ID, false, Applications.CarbonIDE.Elements.Types.Button));
                    }
                }
                foreach (var v in preview.Label)
                {
                    if (Used.Where(d => d.Name == v.ID).Count() == 0)
                    {
                        Used.Add(new Applications.CarbonIDE.Elements(v.ID, false, Applications.CarbonIDE.Elements.Types.Label));
                    }
                }
                foreach (var v in preview.Slider)
                {
                    if (Used.Where(d => d.Name == v.ID).Count() == 0)
                    {
                        Used.Add(new Applications.CarbonIDE.Elements(v.ID, false, Applications.CarbonIDE.Elements.Types.Slider));
                    }
                }
                foreach (var v in preview.CheckBox)
                {
                    if (Used.Where(d => d.Name == v.ID).Count() == 0)
                    {
                        Used.Add(new Applications.CarbonIDE.Elements(v.ID, false, Applications.CarbonIDE.Elements.Types.CheckBox));
                    }
                }
                for (int i = 0; i < Used.Count; i++)
                {
                    if (MouseManager.MouseState == MouseState.Left)
                    {
                        if (MouseManager.X > 10 + Left && MouseManager.X < 10 + Left + 100)
                        {
                            if (MouseManager.Y > 721 + Top && MouseManager.Y < 721 + Top + 30)
                            {
                                foreach (var d in Used)
                                {
                                    d.Selected = false;
                                    ThatID = Used[i].Name;
                                }
                                Used[i].Selected = true;

                                if (Used[i].T == Applications.CarbonIDE.Elements.Types.Button)
                                {
                                    t = new Table(2, 6, 412, 600);
                                    t.Initialize();
                                    t.SetValue(0, 0, "Button.X", true);
                                    t.SetValue(0, 1, preview.Button.Find(d => d.ID == Used[i].Name).X.ToString(), false);
                                    t.SetValue(1, 0, "Button.Y", true);
                                    t.SetValue(1, 1, preview.Button.Find(d => d.ID == Used[i].Name).Y.ToString(), false);
                                    t.SetValue(2, 0, "Button.Width", true);
                                    t.SetValue(2, 1, preview.Button.Find(d => d.ID == Used[i].Name).Width.ToString(), false);
                                    t.SetValue(3, 0, "Button.Height", true);
                                    t.SetValue(3, 1, preview.Button.Find(d => d.ID == Used[i].Name).Height.ToString(), false);
                                    t.SetValue(4, 0, "Button.Text", true);
                                    t.SetValue(4, 1, preview.Button.Find(d => d.ID == Used[i].Name).Text.ToString(), false);
                                    t.SetValue(5, 0, "Button.Color", true);
                                    int color = preview.Button.Find(d => d.ID == Used[i].Name).Color;
                                    t.SetValue(5, 1, Color.FromArgb(color).R + ", " + Color.FromArgb(color).G + ", " + Color.FromArgb(color).B, false);
                                    Typo = "Button";
                                }
                                else if (Used[i].T == Applications.CarbonIDE.Elements.Types.Label)
                                {
                                    t = new Table(2, 4, 412, 600);
                                    t.Initialize();
                                    t.SetValue(0, 0, "Label.X", true);
                                    t.SetValue(0, 1, preview.Label.Find(d => d.ID == Used[i].Name).X.ToString(), false);
                                    t.SetValue(1, 0, "Label.Y", true);
                                    t.SetValue(1, 1, preview.Label.Find(d => d.ID == Used[i].Name).Y.ToString(), false);
                                    t.SetValue(2, 0, "Label.Text", true);
                                    t.SetValue(2, 1, preview.Label.Find(d => d.ID == Used[i].Name).Text.ToString(), false);
                                    t.SetValue(3, 0, "Label.Color", true);
                                    int color = preview.Label.Find(d => d.ID == Used[i].Name).TextColor;
                                    t.SetValue(3, 1, Color.FromArgb(color).R + ", " + Color.FromArgb(color).G + ", " + Color.FromArgb(color).B, false);
                                    Typo = "Label";
                                }
                                else if (Used[i].T == Applications.CarbonIDE.Elements.Types.Slider)
                                {
                                    t = new Table(2, 4, 412, 600);
                                    t.Initialize();
                                    t.SetValue(0, 0, "Slider.X", true);
                                    t.SetValue(0, 1, preview.Slider.Find(d => d.ID == Used[i].Name).X.ToString(), false);
                                    t.SetValue(1, 0, "Slider.Y", true);
                                    t.SetValue(1, 1, preview.Slider.Find(d => d.ID == Used[i].Name).Y.ToString(), false);
                                    t.SetValue(2, 0, "Slieder.Width", true);
                                    t.SetValue(2, 1, preview.Slider.Find(d => d.ID == Used[i].Name).Width.ToString(), false);
                                    t.SetValue(3, 0, "Slider.Value", true);
                                    t.SetValue(3, 1, preview.Slider.Find(d => d.ID == Used[i].Name).Value.ToString(), false);
                                    Typo = "Slider";
                                }

                                i = 0;
                                Top = 50;
                                Left = 900;
                            }
                        }
                    }
                    if (Used[i].Selected == false)
                    {
                        BitFont.DrawBitFontString(UIContainer, "ArialCustomCharset16", Color.White, Used[i].Name, Left, Top);
                    }
                    else
                    {
                        BitFont.DrawBitFontString(UIContainer, "ArialCustomCharset16", Color.Black, Used[i].Name, Left, Top);
                    }
                    if (Top > 200)
                    {
                        Top = 50;
                        Left += 100;
                    }
                    else
                    {
                        Top += 30;
                    }
                }

                //Rendering Propeties table
                t.Render(Propeties, 10, 50);


                BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.Black, lineCount, 7, 7 - (0 * 4));//The line counter

                foreach (var button in Button)
                {
                    if (button.Clicked == true)
                    {
                        if (button.ID == "onclick")
                        {
                            if (!code.Contains("#OnClick " + ThatID))
                            {
                                code += "\n#OnClick " + ThatID + "\n{\n    \n}";
                            }
                        }
                        Back_content = code;
                    }
                }
                
                BitFont.DrawBitFontString(Container, "ArialCustomCharset16", new CarbonIDE().HighLight(Back_content), Back_content, 42, 7 - (0 * 4));//The actual code

                #region Rendering
                ImprovedVBE.DrawImageAlpha(WindowCanvas, 10, 32, window);
                ImprovedVBE.DrawImageAlpha(Container, 933, 32, window);
                ImprovedVBE.DrawImageAlpha(UIContainer, 10, 721, window);
                ImprovedVBE.DrawImageAlpha(Propeties, 1488, 32, window);
                #endregion Rendering

                foreach (var button in Button)
                {
                    if (button.Clicked == true)
                    {
                        UI_Elements.Button.Button_render(window, button.X, button.Y, button.Width, button.Height, ComplimentaryColor.Generate(button.Color).ToArgb(), button.Text);
                    }
                    else
                    {
                        UI_Elements.Button.Button_render(window, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                    }
                }

                temp = false;
            }

            Array.Copy(window.RawData, 0, ImprovedVBE.cover.RawData, 0, window.RawData.Length);
        }

        public int GetGradientColor(int x, int y, int width, int height)
        {
            int r = (int)((double)x / width * 255);
            int g = (int)((double)y / height * 255);
            int b = 255;

            return ImprovedVBE.colourToNumber(r, g, b);
        }
        public void DrawGradientLeftToRight()
        {
            int gradientColorStart = GetGradientColor(0, 0, width, height);
            int gradientColorEnd = GetGradientColor(width, 0, width, height);

            int rStart = Color.FromArgb(gradientColorStart).R;
            int gStart = Color.FromArgb(gradientColorStart).G;
            int bStart = Color.FromArgb(gradientColorStart).B;

            int rEnd = Color.FromArgb(gradientColorEnd).R;
            int gEnd = Color.FromArgb(gradientColorEnd).G;
            int bEnd = Color.FromArgb(gradientColorEnd).B;

            for (int i = 0; i < canvas.RawData.Length; i++)
            {
                if (x_1 == width - 1)
                {
                    x_1 = 0;
                }
                else
                {
                    x_1++;
                }
                int r = (int)((double)x_1 / width * (rEnd - rStart)) + rStart;
                int g = (int)((double)x_1 / width * (gEnd - gStart)) + gStart;
                int b = (int)((double)x_1 / width * (bEnd - bStart)) + bStart;
                if (canvas.RawData[i] != 0)
                {
                    canvas.RawData[i] = ImprovedVBE.colourToNumber(r, g, b);
                }
                if (i / width > 20)
                {
                    break;
                }
            }
            x_1 = 0;
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
            CheckBox
        }
    }
}
