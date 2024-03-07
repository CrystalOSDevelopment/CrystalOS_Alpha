using Cosmos.System;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Programming;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Kernel = CrystalOS_Alpha.Kernel;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;

namespace CrystalOSAlpha.Applications.FileSys
{
    class FileSystem : App
    {
        #region important
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int desk_ID {get; set; }
        public int AppID { get; set; }
        public string name {get; set;}

        public bool minimised { get; set; }
        public bool movable { get; set; }
        
        public Bitmap icon { get; set; }
        #endregion important

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

        public string Route = @"0:\";

        public List<Structure> Items = new List<Structure>();
        public Bitmap side;
        public Bitmap Main;

        public void App()
        {
            if (initial == true)
            {
                Buttons.Add(new Button_prop(4, 29, 70, 25, "Back", 1));
                Buttons.Add(new Button_prop(77, 29, 70, 25, "Forward", 1));

                Scroll.Add(new Scrollbar_Values(width - 22, 43, 20, 272, 0));

                initial = false;
            }
            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                Container = new Bitmap((uint)(width - 29), (uint)(height - 60), ColorDepth.ColorDepth32);

                side = new Bitmap(143, 306, ColorDepth.ColorDepth32);
                Main = new Bitmap(461, 272, ColorDepth.ColorDepth32);

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
                            case "Back":
                                if(Route.Length > 3)
                                {
                                    Route = Route.Remove(Route.LastIndexOf("\\"));
                                }
                                break;
                            case "Forward":
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

                Array.Fill(side.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                Array.Fill(Main.RawData, ImprovedVBE.colourToNumber(36, 36, 36));

                BitFont.DrawBitFontString(side, "ArialCustomCharset16", Color.LightGray, "Quick access", 3, 3);

                ImprovedVBE.DrawImageAlpha(side, 4, 65, canvas);
                ImprovedVBE.DrawImageAlpha(Main, 154, 65, canvas);

                canvas = Scrollbar.Render(canvas, Scroll[0]);

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Tabs", 163, 337);

                ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(36, 36, 36), 154, 351, 461, 20, false);

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
                    if (MouseManager.Y > y + scv.y + 42 + scv.Pos && MouseManager.Y < y + 22 + scv.y + scv.Pos + 42)
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
                if (MouseManager.Y > y + scv.y + 48 && MouseManager.Y < y + scv.y + scv.Height - 42 && scv.Clicked == true)
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
                    if (key.Key == ConsoleKeyEx.Enter)
                    {
                        Route = command.Replace(@"\", "\\");
                    }
                    else
                    {
                        command = Keyboard.HandleKeyboard(command, key);
                    }

                    Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                    temp = true;
                }
            }
            if(temp == true)
            {
                Items.Clear();
                foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(Route))
                {
                    if (d.mEntryType == DirectoryEntryTypeEnum.Directory)
                    {
                        Items.Add(new Structure(d.mName, d.mFullPath, Opt.Folder));
                    }
                }
                foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(Route))
                {
                    if (d.mEntryType == DirectoryEntryTypeEnum.File)
                    {
                        Items.Add(new Structure(d.mName, d.mFullPath, Opt.File));
                    }
                }
                Array.Fill(Main.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                TextBox.Box(window, 153, 29, 331, 20, ImprovedVBE.colourToNumber(60, 60, 60), command, Route, TextBox.Options.left);
                int x_off = 5;
                int y_off = 5;
                foreach(var entry in Items)
                {
                    entry.X = x_off;
                    entry.Y = y_off;
                    if(entry.Selected == true)
                    {
                        ImprovedVBE.DrawFilledRectangle(Main, ImprovedVBE.colourToNumber(69, 69, 69), x_off - 5, y_off - 5, 65, 65, false);
                    }
                    if(entry.type == Opt.Folder)
                    {
                        ImprovedVBE.DrawImageAlpha(Resources.Folder, x_off, y_off, Main);
                        if(entry.name.Length > 5)
                        {
                            BitFont.DrawBitFontString(Main, "ArialCustomCharset16", Color.White, entry.name.Remove(5) + "...", x_off, y_off + 60);
                        }
                        else
                        {
                            BitFont.DrawBitFontString(Main, "ArialCustomCharset16", Color.White, entry.name, x_off, y_off + 60);
                        }
                    }
                    if(entry.type == Opt.File)
                    {
                        ImprovedVBE.DrawImageAlpha(Resources.File, x_off, y_off, Main);
                        if (entry.name.Length > 5)
                        {
                            BitFont.DrawBitFontString(Main, "ArialCustomCharset16", Color.White, entry.name.Remove(5) + "...", x_off, y_off + 60);
                        }
                        else
                        {
                            BitFont.DrawBitFontString(Main, "ArialCustomCharset16", Color.White, entry.name.Remove(entry.name.Length - 4), x_off, y_off + 60);
                        }
                    }
                    x_off += 70;
                    if(x_off / 70 >= 6)
                    {
                        y_off += 85;
                        x_off = 5;
                    }
                }
                ImprovedVBE.DrawImageAlpha(Main, 154, 65, window);

                window = Scrollbar.Render(window, Scroll[0]);
                temp = false;
            }

            //Click Detection:
            if(MouseManager.MouseState == MouseState.Left)
            {
                foreach(var entry in Items)
                {
                    if(MouseManager.X > x + 154 + entry.X && MouseManager.X < x + 154 + entry.X + 60)
                    {
                        if (MouseManager.Y > y + 65 + entry.Y && MouseManager.Y < y + 65 + entry.Y + 60)
                        {
                            if(clicked == false)
                            {
                                foreach(var v in Items)
                                {
                                    v.Selected = false;
                                }
                                if(entry.type == Opt.File)
                                {
                                    if (entry.name.ToLower().EndsWith(".html"))
                                    {
                                        WebscapeNavigator.Webscape wn = new WebscapeNavigator.Webscape();
                                        wn.content = File.ReadAllText(entry.fullPath);
                                        wn.x = 100;
                                        wn.y = 100;
                                        wn.width = 700;
                                        wn.height = 420;
                                        wn.z = 999;
                                        wn.source = entry.fullPath;
                                        wn.icon = ImprovedVBE.ScaleImageStock(Resources.Notepad, 56, 56);
                                        wn.name = "Webs... - " + entry.name;

                                        TaskScheduler.Apps.Add(wn);
                                    }
                                    else if (entry.name.ToLower().EndsWith(".cmd"))
                                    {
                                        CSharp c = new CSharp();
                                        c.Executor(File.ReadAllText(entry.fullPath));
                                    }
                                    else if (entry.name.ToLower().EndsWith(".app"))
                                    {
                                        TaskScheduler.Apps.Add(new Window(100, 100, 999, 350, 200, 0, "Untitled", false, icon, File.ReadAllText(entry.fullPath)));
                                    }
                                    else if (entry.name.ToLower().EndsWith(".bin"))
                                    {

                                    }
                                    else if (entry.name.ToLower().EndsWith(".bmp"))
                                    {
                                        //Time to write an image viewer app!
                                        //Kernel.image = new Bitmap(entry.fullPath);
                                    }
                                    else
                                    {
                                        Applications.Notepad.Notepad n = new Notepad.Notepad();
                                        n.content = File.ReadAllText(entry.fullPath);
                                        n.x = 100;
                                        n.y = 100;
                                        n.width = 700;
                                        n.height = 420;
                                        n.z = 999;
                                        n.source = entry.fullPath;
                                        n.icon = ImprovedVBE.ScaleImageStock(Resources.Notepad, 56, 56);
                                        n.name = "Note... - " + entry.name;

                                        TaskScheduler.Apps.Add(n);
                                    }
                                }
                                else if(entry.type == Opt.Folder)
                                {
                                    Route = entry.fullPath;
                                    temp = true;
                                }
                                entry.Selected = true;
                                clicked = true;
                                temp = true;
                            }
                        }
                    }
                }
            }
            if(MouseManager.MouseState == MouseState.None && clicked == true)
            {
                clicked = false;
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
    }

    public class Structure
    {
        public string name { get; set;}
        public string fullPath { get; set;}
        public Opt type { get; set; }
        public int X { get; set;}
        public int Y { get; set;}
        public bool Selected { get; set;}
        public Structure(string name, string fullPath, Opt type)
        {
            this.name = name;
            this.fullPath = fullPath;
            this.type = type;
        }
    }

    public enum Opt
    {
        File,
        Folder
    }
}
