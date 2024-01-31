using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications.FileSys;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;

namespace CrystalOSAlpha.Applications.CarbonIDE
{
    class CarbonInit : App
    {
        #region important
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int desk_ID {get; set; }

        public string name { get; set; }

        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }
        #endregion important

        public List<Button_prop> Buttons = new List<Button_prop>();
        public List<Scrollbar_Values> Scroll = new List<Scrollbar_Values>();

        public bool initial = true;
        public bool clicked = false;

        public int x_1 = 0;
        public int y_1 = 0;

        public Bitmap canvas;
        public Bitmap back_canvas;
        public bool once = true;
        public Bitmap window;
        public int CurrentColor = ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B);

        public string content = "";
        public string source = "";

        public bool temp = true;

        public int Reg_Y = 0;

        public List<Dropdown> dropdowns = new List<Dropdown>();
        public List<values> value = new List<values>();

        public int memory = 0;

        public string typeOf = "Opening";

        public string namedProject = "";
        public string SourceofProject = "0:\\User\\Source";

        public string activeTextbox = "name";

        public void App()
        {
            if (initial == true)
            {
                Buttons.Clear();
                if(typeOf == "Opening")
                {
                    Buttons.Add(new Button_prop(496, 129, 175, 60, "Create a new project", 1));
                    Buttons.Add(new Button_prop(496, 226, 175, 60, "Import from filesytem", 1));
                }
                else if(typeOf == "New Project")
                {
                    Buttons.Add(new Button_prop(537, 381, 70, 25, "Back", 1));
                    Buttons.Add(new Button_prop(620, 381, 70, 25, "Create", 1));
                }
                initial = false;
            }
            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32); //new int[width * height];
                back_canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32); //new int[width * height];
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);

                #region corners
                ImprovedVBE.DrawFilledEllipse(canvas, 10, 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, width - 11, 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, 10, height - 10, 10, 10, CurrentColor);
                ImprovedVBE.DrawFilledEllipse(canvas, width - 11, height - 10, 10, 10, CurrentColor);

                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 0, 10, width, height - 20, false);
                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, 0, width - 10, 15, false);
                ImprovedVBE.DrawFilledRectangle(canvas, CurrentColor, 5, height - 15, width - 10, 15, false);
                #endregion corners

                canvas = ImprovedVBE.DrawImageAlpha2(canvas, x, y, canvas);

                DrawGradientLeftToRight();

                ImprovedVBE.DrawFilledEllipse(canvas, width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                ImprovedVBE.DrawFilledEllipse(canvas, width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                //window.RawData = canvas.RawData;
                //back_canvas = canvas;
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
                                temp = true;
                                clicked = true;
                            }
                        }
                    }
                }
            }
            if (clicked == true && MouseManager.MouseState == MouseState.None)
            {
                foreach (var button in Buttons)
                {
                    button.Clicked = false;
                }
                temp = true;
                clicked = false;
            }

            if (temp == true)
            {
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

                if(typeOf == "Opening")
                {
                    string s = "Welcome to CarbonIDE!";

                    BitFont.DrawBitFontString(window, "VerdanaCustomCharset24", Color.White, s, (width / 2) - (BitFont.DrawBitFontString(back_canvas, "VerdanaCustomCharset24", Color.White, s, 0, 0) / 2), 30);

                    BitFont.DrawBitFontString(window, "VerdanaCustomCharset24", Color.White, "Recently Opened", 10, 65);
                }
                if (typeOf == "New Project")
                {
                    string s = "Creating a new project";

                    BitFont.DrawBitFontString(window, "VerdanaCustomCharset24", Color.White, s, (width / 2) - (BitFont.DrawBitFontString(back_canvas, "VerdanaCustomCharset24", Color.White, s, 0, 0) / 2), 30);

                    BitFont.DrawBitFontString(window, "ArialCustomCharset16", Color.White, "Project name:", 10, 65);

                    TextBox.Box(window, 10, 85, 200, 20, ImprovedVBE.colourToNumber(60, 60, 60), namedProject, "Example", TextBox.Options.left);

                    BitFont.DrawBitFontString(window, "ArialCustomCharset16", Color.White, "Project location:", 10, 110);

                    TextBox.Box(window, 10, 130, 200, 20, ImprovedVBE.colourToNumber(60, 60, 60), SourceofProject, "0:\\sources", TextBox.Options.left);
                }

                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        Button.Button_render(window, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);

                        switch (button.Text)
                        {
                            case "Create a new project":
                                typeOf = "New Project";
                                initial = true;
                                once = true;
                                break;
                            case "Import from filesytem":
                                
                                break;
                            case "Back":
                                typeOf = "Opening";
                                initial = true;
                                once = true;
                                break;
                            case "Create":
                                //Create the file structure
                                if(VMTools.IsVMWare == true)
                                {
                                    Directory.CreateDirectory(SourceofProject + "\\" + namedProject);
                                }
                                CarbonIDE ide = new CarbonIDE();
                                ide.x = 0;
                                ide.y = 0;
                                ide.width = ImprovedVBE.width;
                                ide.height = ImprovedVBE.height - 75;
                                ide.z = 999;
                                ide.icon = ImprovedVBE.ScaleImageStock(Resources.IDE, 56, 56);
                                ide.name = "CarbonIDE";
                                ide.Path = SourceofProject + "\\" + namedProject;

                                TaskScheduler.Apps.Add(ide);

                                TaskScheduler.Apps.Remove(this);
                                break;
                        }
                    }
                    else
                    {
                        Button.Button_render(window, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                    }
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        button.Clicked = false;
                    }
                }

                foreach (var Dropd in dropdowns)
                {
                    bool render = true;
                    if (MouseManager.MouseState == MouseState.Left)
                    {
                        if (MouseManager.X > x + Dropd.X + (Dropd.canv.Width - 30) && MouseManager.X < x + Dropd.X + Dropd.canv.Width)
                        {
                            if (MouseManager.Y > y + Dropd.Y && MouseManager.Y < y + Dropd.Y + Dropd.Height)
                            {
                                Dropdown d = Dropd;
                                dropdowns.Remove(d);
                                dropdowns.Add(d);
                                if (clicked == false)
                                {
                                    if (Dropd.Clicked == true)
                                    {
                                        Dropd.Clicked = false;
                                    }
                                    else
                                    {
                                        Dropd.Clicked = true;
                                        render = false;
                                    }
                                    clicked = true;
                                }
                            }
                        }
                    }
                    if (render == true)
                    {
                        Dropd.Draw(window, value);
                    }
                }

                temp = false;
            }

            #region Mechanical
            if (TaskScheduler.counter == TaskScheduler.Apps.Count - 1)
            {
                if(MouseManager.MouseState == MouseState.Left)
                {
                    if(MouseManager.X > x + 10 && MouseManager.X < x + 210)
                    {
                        if (MouseManager.Y > y + 85 && MouseManager.Y < y + 105)
                        {
                            activeTextbox = "name";
                        }
                        if (MouseManager.Y > y + 130 && MouseManager.Y < y + 150)
                        {
                            activeTextbox = "source";
                        }
                    }
                }
                KeyEvent k;
                if (KeyboardManager.TryReadKey(out k))
                {
                    if (k.Key == ConsoleKeyEx.Enter)
                    {
                        Buttons[1].Clicked = true;
                        clicked = true;
                        temp = true;
                    }
                    else
                    {
                        if(activeTextbox == "name")
                        {
                            namedProject = Keyboard.HandleKeyboard(namedProject, k);
                        }
                        else if(activeTextbox == "source")
                        {
                            SourceofProject = Keyboard.HandleKeyboard(SourceofProject, k);
                        }
                        temp = true;
                    }
                }
            }
            #endregion Mechanical

            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
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
