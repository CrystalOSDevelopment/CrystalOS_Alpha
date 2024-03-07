using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;

namespace CrystalOSAlpha.Applications.Terminal
{
    class Terminal : App
    {
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

        public List<Button_prop> Buttons = new List<Button_prop>();
        public List<Scrollbar_Values> Scroll = new List<Scrollbar_Values>();

        public bool initial = true;
        public bool clicked = false;
        public bool temp = true;
        public bool once = true;

        public int x_1 = 0;
        public int y_1 = 0;
        public int Reg_Y = 0;
        public int CurrentColor = ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B);

        public Bitmap canvas;
        public Bitmap window;
        public Bitmap Container;

        public string content = "Crystal-PC> ";

        public string command = "";

        public int offset = 0;
        public int offset2 = 0;

        public List<string> cmd_history = new List<string>();

        public int index = 0;

        public bool echo_off = false;

        public void App()
        {
            if (initial == true)
            {
                Buttons.Add(new Button_prop(5, 27, 90, 20, "Clear", 1));

                Scroll.Add(new Scrollbar_Values(width - 22, 30, 20, height - 60, 0));

                initial = false;
            }
            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                Container = new Bitmap((uint)(width - 29), (uint)(height - 60), ColorDepth.ColorDepth32);

                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(60, 60, 60));

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

                DrawGradientLeftToRight();

                ImprovedVBE.DrawFilledEllipse(canvas, width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                ImprovedVBE.DrawFilledEllipse(canvas, width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);

                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);

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
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                    }
                    if (MouseManager.MouseState == MouseState.None)
                    {
                        button.Clicked = false;
                    }
                }

                canvas = Scrollbar.Render(canvas, Scroll[0]);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                //window.RawData = canvas.RawData;
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

            foreach (var scv in Scroll)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {

                    if (MouseManager.Y > y + scv.y + 42 + scv.Pos && MouseManager.Y < y + scv.y + scv.Pos + 62)
                    {
                        if (MouseManager.X > x + scv.x + 2 && MouseManager.X < x + scv.x + scv.Width)
                        {
                            if (scv.Clicked == false)
                            {
                                scv.Clicked = true;
                                Reg_Y = (int)MouseManager.Y - y - scv.y - 42 - scv.Pos;
                            }
                        }
                        temp = true;
                    }
                }
                if (MouseManager.MouseState == MouseState.None && scv.Clicked == true)
                {
                    temp = true;
                    scv.Clicked = false;
                }
                if (scv.Clicked == true && MouseManager.MouseState == MouseState.None)
                {
                    scv.Clicked = false;
                }
                if (MouseManager.Y > y + scv.y + 48 && MouseManager.Y < y + height - 42 && scv.Clicked == true)
                {
                    if (scv.Pos >= 0 && scv.Pos <= scv.Height - 44)
                    {
                        scv.Pos = (int)MouseManager.Y - y - scv.y - 42 - Reg_Y;
                    }
                    else
                    {
                        if (scv.Pos < 0)
                        {
                            scv.Pos = 1;
                        }
                        else
                        {
                            scv.Pos = scv.Height - 44;
                        }
                    }
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
                            if(Scroll[0].Pos < Scroll[0].Height - 48 && content.Split("\n").Length > 21)
                            {
                                Scroll[0].Pos += CommandLibrary.offset * 8;
                            }
                            else if(Scroll[0].Pos < Scroll[0].Height - 48 == false)
                            {
                                offset2 += CommandLibrary.offset * 8;
                                offset += CommandLibrary.offset;
                            }
                            CommandLibrary.offset = 0;
                        }
                        command = "";
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

                    Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                    temp = true;
                }
            }

            if (temp == true)
            {
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                window = Scrollbar.Render(window, Scroll[0]);

                //string output = "";

                //output = content;
                /*
                if (output.Split('\n').Length > 23)
                {
                    int h = Scroll[0].Pos / 8;
                    for (int i = 0; i < h; i++)
                    {
                        int index = output.IndexOf('\n');
                        output = output.Remove(0, index + 1);
                    }
                }
                */
                if(content.Split('\n').Length > 21)
                {
                    content = content.Remove(0, Get_index_of_char(content, '\n', offset));
                }
                BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.White, content, 5, 5 - (Scroll[0].Pos + offset2) * 4);
                offset = 0;
                ImprovedVBE.DrawImageAlpha(Container, 5, 52, window);

                temp = false;
            }
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
