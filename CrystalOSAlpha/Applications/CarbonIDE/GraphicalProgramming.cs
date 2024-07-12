using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Programming;
using CrystalOSAlpha.System32;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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

        public Window preview;

        public Bitmap WindowCanvas;
        public Bitmap back_canvas;
        public Bitmap UIContainer;
        public Bitmap Propeties;
        public Bitmap Container;
        #endregion Core variables

        #region UI Elements
        public List<UIElementHandler> PropetiesTab = new List<UIElementHandler>();
        #endregion UI Elements

        public void App()
        {
            if (initial == true)
            {
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

                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 0, "10", false);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 1, "10", false);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 2, "450", false);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 3, "300", false);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 4, "false", false);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 5, "Test", false);
                PropetiesTab.Find(x => x.ID == "Propeties").SetValue(1, 6, "false", false);
                #endregion Propeties

                if (File.Exists(Path + ".app"))
                {
                    code = File.ReadAllText(Path + ".app");
                }
                else
                {
                    code = CodeGenerator.Generate(code, "");
                }

                code = CodeGenerator.Generate(code, "");
                preview = new Window(20, 50, 999, 400, 300, 1, "New Window", false, icon, code);

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
                UIContainer = new Bitmap(1900, 269, ColorDepth.ColorDepth32);
                Propeties = new Bitmap(422, 674, ColorDepth.ColorDepth32);
                Container = new Bitmap(540, 674, ColorDepth.ColorDepth32);

                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

                once = false;
                temp = true;
            }

            if (MouseManager.MouseState == MouseState.Left && TaskScheduler.Apps[^1] == this && clicked == false)
            {
                if (MouseManager.Y < 32 + WindowCanvas.Height)
                {
                    temp = true;
                    clicked = true;
                    StoredX = (int)MouseManager.X;
                    StoredY = (int)MouseManager.Y;
                }
            }
            else if(MouseManager.MouseState == MouseState.None)
            {
                clicked = false;
            }

            KeyEvent KeyPress = null;
            if(KeyboardManager.TryReadKey(out KeyPress))
            {
                temp = true;
            }

            //if (preview.Code != code)
            //{
            //    preview = new Window(20, 50, 999, 400, 300, 1, "New Window", false, icon, code);
            //    temp = true;
            //}

            if (temp == true)
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

                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(36, 36, 36));//Background
                ImprovedVBE.DrawFilledRectangle(Container, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Container.Width - 4, (int)Container.Height - 4, false);//Border
                #endregion Border

                //Render to propeties
                foreach(var Element in PropetiesTab)
                {
                    if(MouseManager.MouseState == MouseState.Left)
                    {
                        Element.CheckClick(1488, y + 32);
                    }
                    switch (Element.MinVal >= 0)
                    {
                        case true:
                            switch (KeyPress)
                            {
                                case null:
                                    break;
                                default:
                                    switch (Element.Clicked)
                                    {
                                        case false:
                                            Element.SetValue(Element.MinVal, Element.MaxVal, Keyboard.HandleKeyboard(Element.GetValue(Element.MinVal, Element.MaxVal), KeyPress), false);
                                            break;
                                    }
                                    break;
                            }
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

                //preview.App(WindowCanvas);

                #region Rendering
                ImprovedVBE.DrawImageAlpha(WindowCanvas, 10, 32, window);
                ImprovedVBE.DrawImageAlpha(Container, 933, 32, window);
                ImprovedVBE.DrawImageAlpha(UIContainer, 10, 721, window);
                ImprovedVBE.DrawImageAlpha(Propeties, 1488, 32, window);
                #endregion Rendering

                temp = false;
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
