using Kernel = CrystalOS_Alpha.Kernel;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;
using Cosmos.System;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications.FileSys;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Programming;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

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
        public int AppID { get; set; }
        public string name { get; set; }

        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }
        #endregion important

        public int CurrentColor = ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B);
        public int Reg_Y = 0;
        public int memory = 0;

        public string content = "";
        public string source = "";
        public string namedProject = "";
        public string typeOf = "Opening";
        public string SourceofProject = "0:\\User\\Source";
        public string activeTextbox = "name";

        public bool initial = true;
        public bool clicked = false;
        public bool once = true;
        public bool temp = true;

        public Bitmap canvas;
        public Bitmap back_canvas;
        public Bitmap window { get; set; }

        public List<Button_prop> Buttons = new List<Button_prop>();
        public List<Scrollbar_Values> Scroll = new List<Scrollbar_Values>();
        public List<Dropdown> dropdowns = new List<Dropdown>();
        public List<values> value = new List<values>();
        public List<Structure> CSharpFiles = new List<Structure>();

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

                    dropdowns.Add(new Dropdown(10, 167, 325, 25, "Options"));

                    value.Add(new values(true, "Program mode: Terminal", "Options"));
                    value.Add(new values(false, "Program mode: Graphical", "Options"));
                }

                foreach (DirectoryEntry dir in Kernel.fs.GetDirectoryListing("0:\\User\\Source"))
                {
                    if (dir.mEntryType == DirectoryEntryTypeEnum.Directory)
                    {
                        if (File.Exists(dir.mFullPath + "\\" + dir.mName + ".sln"))
                        {
                            if(!CSharpFiles.Contains(new Structure(dir.mName, dir.mFullPath, Opt.Folder)))
                            {
                                CSharpFiles.Add(new Structure(dir.mName, dir.mFullPath, Opt.Folder));
                            }
                        }
                    }
                }

                initial = false;
            }
            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                back_canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
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

                canvas = ImprovedVBE.EnableTransparency(canvas, x, y, canvas);

                ImprovedVBE.DrawGradientLeftToRight(canvas);

                ImprovedVBE.DrawFilledEllipse(canvas, width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                ImprovedVBE.DrawFilledEllipse(canvas, width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);

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

            if (TaskScheduler.Apps[^1] == this)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    temp = true;
                }
            }

            if (temp == true)
            {
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

                if(typeOf == "Opening")
                {
                    string s = "Welcome to CarbonIDE!";

                    BitFont.DrawBitFontString(window, "VerdanaCustomCharset24", Color.White, s, (width / 2) - (BitFont.DrawBitFontString(back_canvas, "VerdanaCustomCharset24", Color.White, s, 0, 0) / 2), 30);

                    BitFont.DrawBitFontString(window, "VerdanaCustomCharset24", Color.White, "Recently Opened", 10, 65);

                    //View all folders in Source folder
                    int top = 105;
                    int left = 15;
                    foreach (var v in CSharpFiles)
                    {
                        BitFont.DrawBitFontString(window, "VerdanaCustomCharset24", Color.White,v.name, left, top);

                        BitFont.DrawBitFontString(window, "ArialCustomCharset16", Color.LightGray, v.fullPath, left + 10, top + 25);

                        if(MouseManager.MouseState == MouseState.Left)
                        {
                            if(MouseManager.X > x + left && MouseManager.X < x + left + 200)
                            {
                                if (MouseManager.Y > y + top && MouseManager.Y < y + top + 50)
                                {
                                    string[] content = File.ReadAllLines(v.fullPath + "\\" + v.name + ".sln");
                                    if(content.Length > 0)
                                    {
                                        if (content[0] == "Program_mode = Terminal")
                                        {
                                            CarbonIDE ide = new CarbonIDE();
                                            ide.x = 0;
                                            ide.y = 0;
                                            ide.width = ImprovedVBE.width;
                                            ide.height = ImprovedVBE.height - 75;
                                            ide.z = 999;
                                            ide.icon = ImprovedVBE.ScaleImageStock(Resources.IDE, 56, 56);
                                            ide.name = "CarbonIDE";
                                            ide.Path = v.fullPath + "\\" + v.name;
                                            ide.namedProject = v.name;

                                            TaskScheduler.Apps.Add(ide);

                                            TaskScheduler.Apps.Remove(this);
                                        }
                                        else if (content[0] == "Program_mode = Graphical")
                                        {
                                            GraphicalProgramming ide = new GraphicalProgramming();
                                            ide.x = 0;
                                            ide.y = 0;
                                            ide.width = ImprovedVBE.width;
                                            ide.height = ImprovedVBE.height - 75;
                                            ide.z = 999;
                                            ide.icon = ImprovedVBE.ScaleImageStock(Resources.IDE, 56, 56);
                                            ide.name = "CarbonIDE";
                                            ide.Path = v.fullPath + "\\" + v.name;
                                            ide.namedProject = v.name;

                                            TaskScheduler.Apps.Add(ide);

                                            TaskScheduler.Apps.Remove(this);
                                        }
                                    }
                                }
                            }
                        }

                        top += 50;
                    }
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
                                    File.Create(SourceofProject + "\\" + namedProject + "\\" + namedProject + ".sln");
                                    if (value[0].Highlighted == true)
                                    {
                                        File.WriteAllText(SourceofProject + "\\" + namedProject + "\\" + namedProject + ".sln", "Program_mode = Terminal");
                                        File.Create(SourceofProject + "\\" + namedProject + "\\" + namedProject + ".cmd");

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
                                    }
                                    else if (value[1].Highlighted == true)
                                    {
                                        File.WriteAllText(SourceofProject + "\\" + namedProject + "\\" + namedProject + ".sln", "Program_mode = Graphical");
                                        File.Create(SourceofProject + "\\" + namedProject + "\\" + namedProject + ".app");
                                        File.WriteAllText(SourceofProject + "\\" + namedProject + "\\" + namedProject + ".app", CodeGenerator.Generate("", ""));
                                        
                                        GraphicalProgramming ide = new GraphicalProgramming();
                                        ide.x = 0;
                                        ide.y = 0;
                                        ide.width = ImprovedVBE.width;
                                        ide.height = ImprovedVBE.height - 75;
                                        ide.z = 999;
                                        ide.icon = ImprovedVBE.ScaleImageStock(Resources.IDE, 56, 56);
                                        ide.name = "CarbonIDE";
                                        ide.Path = SourceofProject + "\\" + namedProject;
                                        ide.namedProject = namedProject;

                                        TaskScheduler.Apps.Add(ide);

                                        TaskScheduler.Apps.Remove(this);
                                    }
                                }
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

                int ind = 0;
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
                                dropdowns.RemoveAt(ind);
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
                    ind++;
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

                foreach (var Dropd in dropdowns)
                {
                    if (MouseManager.MouseState == MouseState.Left)
                    {
                        if (MouseManager.X > x + Dropd.X + (Dropd.canv.Width - 30) && MouseManager.X < x + Dropd.X + Dropd.canv.Width)
                        {
                            if (MouseManager.Y > y + Dropd.Y && MouseManager.Y < y + Dropd.Y + Dropd.Height)
                            {
                                Dropdown d = Dropd;
                                dropdowns.Remove(Dropd);
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

                                    }
                                    clicked = true;
                                }
                            }
                        }
                    }
                    if (Dropd.Clicked != false)
                    {
                        if (MouseManager.X > x + Dropd.X && MouseManager.X < x + Dropd.X + Dropd.canv.Width - 30)
                        {
                            if (MouseManager.Y > y + Dropd.Y + Dropd.Height && MouseManager.Y < y + Dropd.Y + Dropd.Height + 100)
                            {
                                int top = (int)(MouseManager.Y - y - Dropd.Y - Dropd.Height);
                                int discardable = 0;
                                int select = 1;
                                if (top < 20)
                                {
                                    select = 1;
                                }
                                else if (top > 20 && top < 40)
                                {
                                    select = 2;
                                }
                                else if (top > 40 && top < 60)
                                {
                                    select = 3;
                                }
                                else if (top > 60 && top < 80)
                                {
                                    select = 4;
                                }
                                if (select != memory)
                                {
                                    foreach (var val in value)
                                    {
                                        if (val.ID == Dropd.ID)
                                        {
                                            val.Highlighted = false;
                                            discardable++;
                                        }
                                        if (discardable == select && val.ID == Dropd.ID)
                                        {
                                            if (val.Highlighted == false)
                                            {
                                                val.Highlighted = true;
                                                temp = true;
                                            }
                                        }
                                    }
                                    memory = select;
                                }

                                if (MouseManager.MouseState == MouseState.Left)
                                {
                                    Dropd.Clicked = false;
                                }
                            }
                        }
                    }
                    Dropd.Render(x, y);
                }
            }
            #endregion Mechanical

            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }
        public void RightClick()
        {

        }
    }
}
