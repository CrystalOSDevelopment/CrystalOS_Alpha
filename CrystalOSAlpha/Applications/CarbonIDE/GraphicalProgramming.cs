using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Graphics.Fonts;
using CrystalOSAlpha.Applications.FileSys;
using CrystalOSAlpha.Applications.Gameboy;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Widgets;
using CrystalOSAlpha.Programming;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }

        public Bitmap canvas;
        public Bitmap back_canvas;
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

        bool temp = true;

        string code = "";

        List<string> Elements = new List<string> { "Label", "Button", "TextBox", "Slider", "Scrollbar", "PictureBox", "CheckBox", "Radio button", "Progressbar", "Menutab", "Table" };

        public Table t = new Table(2, 7, 412, 600);
        public Window preview;
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

                code = CodeGenerator.Generate(code, "");

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

                initial = false;
            }

            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                back_canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);

                WindowCanvas = new Bitmap(1459, 670, ColorDepth.ColorDepth32);
                UIContainer = new Bitmap(1900, 269, ColorDepth.ColorDepth32);
                Propeties = new Bitmap(422, 674, ColorDepth.ColorDepth32);

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

                //window.RawData = canvas.RawData;
                back_canvas = canvas;
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
                                once = true;
                                clicked = true;
                            }
                        }
                    }
                }
                if (button.Clicked == true && MouseManager.MouseState == MouseState.None)
                {
                    once = true;
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
            }
            if (MouseManager.MouseState == MouseState.None && clicked == true)
            {
                temp = true;
                clicked = false;
            }

            if(TaskScheduler.Apps[^1] == this)
            {
                KeyEvent k;
                if(KeyboardManager.TryReadKey(out k))
                {
                    foreach(var v in t.Cells)
                    {
                        if(v.Selected == true && v.WriteProtected == false)
                        {
                            v.Content = Keyboard.HandleKeyboard(v.Content, k);
                            code = CodeGenerator.Generate(code, "Button btn = new Button(10, 30, 110, 25, \"Hello World\", 1, 1, 1);");
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
                #endregion Border

                if(MouseManager.MouseState == MouseState.Left)
                {
                    t.Select((int)(MouseManager.X - 1488), (int)(MouseManager.Y - 82));
                }

                #region Labeling
                BitFont.DrawBitFontString(WindowCanvas, "VerdanaCustomCharset24", Color.White, "Canvas:", 7, 10);
                BitFont.DrawBitFontString(UIContainer, "VerdanaCustomCharset24", Color.White, "UI Elements:", 7, 10);
                BitFont.DrawBitFontString(Propeties, "VerdanaCustomCharset24", Color.White, "Propeties:", 7, 10);

                //Showing options to different UI elements

                int Top = 50;
                int Left = 10;
                foreach(string s in Elements)
                {
                    BitFont.DrawBitFontString(UIContainer, "ArialCustomCharset16", Color.White, s, Left, Top);
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

                //Rendering Propeties table

                t.Render(Propeties, 10, 50);
                #endregion Labeling

                preview.App(WindowCanvas);

                #region Rendering
                ImprovedVBE.DrawImageAlpha(WindowCanvas, 10, 32, window);
                ImprovedVBE.DrawImageAlpha(UIContainer, 10, 721, window);
                ImprovedVBE.DrawImageAlpha(Propeties, 1488, 32, window);
                #endregion Rendering
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
}
