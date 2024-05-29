using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.System32;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;

namespace CrystalOSAlpha.Applications.Terminal
{
    class Terminal : App
    {
        public Terminal(int X, int Y, int Z, int Width, int Height, string Name, Bitmap Icon)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
            this.width = Width;
            this.height = Height;
            this.name = Name;
            this.icon = Icon;
        }

        #region Core_Values
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public int desk_ID {get; set; }
        public int AppID { get; set; }
        public string name {get; set; }
        public bool minimised { get; set; }
        public bool movable { get; set; }
        
        public Bitmap icon { get; set; }
        #endregion Core_Values

        public int offset = 0;
        public int offset2 = 0;
        public int index = 0;
        public int Reg_Y = 0;
        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);

        public string content = "Crystal-PC> ";

        public string command = "";
        public bool initial = true;
        public bool clicked = false;
        public bool temp = true;
        public bool once { get; set; }
        public bool echo_off = false;

        public Bitmap canvas;
        public Bitmap window { get; set; }
        public Bitmap Container;

        public List<UIElementHandler> Buttons = new List<UIElementHandler>();
        public List<UIElementHandler> Scroll = new List<UIElementHandler>();
        public List<string> cmd_history = new List<string>();

        public void App()
        {
            if (initial == true)
            {
                Buttons.Add(new Button(5, 27, 90, 20, "Clear", 1));

                Scroll.Add(new VerticalScrollbar(width - 22, 52, 20, height - 60, 20, 0, 1000, ""));

                initial = false;
            }
            if (once == true)
            {
                Bitmap back = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                (canvas, back, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                Container = new Bitmap((uint)(width - 29), (uint)(height - 60), ColorDepth.ColorDepth32);
                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(60, 60, 60));

                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        int Col = button.Color;
                        button.Color = Color.White.ToArgb();
                        button.Render(canvas);
                        button.Color = Col;

                        switch (button.Text)
                        {
                            case "Clear":
                                content = "Crystal-PC> ";
                                command = "";
                                offset = 0;
                                offset2 = 0;
                                break;
                        }
                    }
                    else
                    {
                        button.Render(canvas);
                    }
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        button.Clicked = false;
                    }
                }

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
                if (clicked == true && MouseManager.MouseState == MouseState.None)
                {
                    once = true;
                    button.Clicked = false;
                    clicked = false;
                }
            }

            foreach (var vscroll in Scroll)
            {
                vscroll.Height = height - 60;
                vscroll.X = width - 22;
                if (vscroll.CheckClick((int)MouseManager.X - x, (int)MouseManager.Y - y))
                {
                    temp = true;
                }
            }

            if (TaskScheduler.counter == TaskScheduler.Apps.Count - 1)
            {
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    if(key.Key == ConsoleKeyEx.Enter)
                    {
                        if(command == "clear")
                        {
                            if(echo_off == true)
                            {

                            }
                            else
                            {
                                content = "Crystal-PC> ";
                            }
                            offset = 0;
                            offset2 = 0;
                        }
                        else
                        {
                            cmd_history.Add(command);
                            if(echo_off == true)
                            {
                                content += "\n" + CommandLibrary.Execute(command);
                            }
                            else
                            {
                                content += "\n" + CommandLibrary.Execute(command) + "\nCrystal-PC> ";
                            }
                            index = cmd_history.Count;
                            if(cmd_history.Count > 20)
                            {
                                cmd_history.RemoveAt(0);
                            }
                        }
                        command = "";
                        while(content.Split('\n').Length * 16 > Container.Height * 5)
                        {
                            content = content.Remove(0, content.IndexOf("\n") + 1);
                        }
                        if (content.Split('\n').Length * 16 >= Container.Height - 10)
                        {
                            Scroll[0].Value = Math.Clamp(content.Split('\n').Length * 16, Scroll[0].MinVal, Scroll[0].MaxVal);
                            Scroll[0].Pos = (int)(Scroll[0].Value / Scroll[0].Sensitivity) + 20;
                        }
                    }
                    else if(key.Key == ConsoleKeyEx.UpArrow)
                    {
                        int l = command.Length;
                        if (command.Length != 0)
                        {
                            content = content.Remove(content.Length - l);
                        }
                        if (index > 0)
                        {
                            index--;
                        }
                        command = cmd_history[index];
                        content += command;
                    }
                    else if (key.Key == ConsoleKeyEx.DownArrow)
                    {
                        int l = command.Length;
                        if(command.Length != 0)
                        {
                            content = content.Remove(content.Length - l);
                        }
                        if(index < cmd_history.Count - 1)
                        {
                            index++;
                        }
                        command = cmd_history[index];
                        content += command;
                    }
                    else
                    {
                        int length = command.Length;
                        command = Keyboard.HandleKeyboard(command, key);
                        content = content.Remove(content.Length - length);
                        content += command;
                    }

                    foreach (var vscroll in Scroll)
                    {
                        vscroll.Height = height - 60;
                        vscroll.X = width - 22;
                        if (content.Split('\n').Length * 16 > Container.Height)
                        {
                            vscroll.MaxVal = content.Split('\n').Length * 16 - (int)Container.Height;
                            vscroll.Pos = (int)(Scroll[0].MaxVal / Scroll[0].Sensitivity) + 20;
                            temp = true;
                        }
                        else
                        {
                            vscroll.MaxVal = 0;
                        }
                        if (vscroll.CheckClick((int)MouseManager.X - x, (int)MouseManager.Y - y))
                        {
                            temp = true;
                        }
                    }

                    Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                    temp = true;
                }
            }

            if (temp == true)
            {
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                foreach (var vscroll in Scroll)
                {
                    vscroll.Render(window);
                }
                if (Scroll[0].Value > 0)
                {
                    BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.White, content, 5, (-3) - Scroll[0].Value);
                }
                else
                {
                    BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.White, content, 5, 5 - Scroll[0].Value);
                }
                ImprovedVBE.DrawImageAlpha(Container, 5, 52, window);

                temp = false;
            }
            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }
        public void RightClick()
        {

        }
        public int Get_index_of_char(string source, char c, int index)
        {
            int counter = 0;
            int index_out = 0;
            for (int i = 0; counter < index && i < source.Length; i++)
            {
                if (source[i] == c)
                {
                    counter++;
                }
                index_out = i;
            }
            return index_out;
        }
    }
}
