using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOS_Alpha;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.Widgets;
using CrystalOSAlpha.Programming;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using Kernel = CrystalOS_Alpha.Kernel;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;

namespace CrystalOSAlpha.SystemApps
{
    class Window : App
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
        public bool HasTitlebar = true;
        public bool AlwaysOnTop = false;
        #endregion Essential

        #region UI_Elements
        public List<Button_prop> Button = new List<Button_prop>();
        public List<Slider> Slider = new List<Slider>();
        public List<CheckBox> CheckBox = new List<CheckBox>();
        public List<Dropdown> Dropdown = new List<Dropdown>();
        public List<Scrollbar_Values> Scroll = new List<Scrollbar_Values>();
        public List<TextBox> TextBox = new List<TextBox>();
        public List<label> Label = new List<label>();
        #endregion UI_Elements

        public string Code = "";
        public List<string> Parts;

        public int CycleCount = 0;

        public void App()
        {
            if (AlwaysOnTop == true)
            {
                z = 1000;
            }
            if (initial == true)
            {
                //Separate the code into 3 segments:
                //1. Initial window UI element layout **Done! All implemented**
                //2. Loop of the window
                //3. UI element actions
                Parts = Separate(Code);
                if(Parts.Count == 0)
                {
                    TaskScheduler.Apps.Add(new MsgBox(999, 100, 100, 400, 200, "Error!", "Failed to execute program!\nNo executable code was found!", ImprovedVBE.ScaleImageStock(ImageViewer.Nr1, 56, 56)));
                    TaskScheduler.Apps.Remove(this);
                }
                else
                {
                    if (!Parts[0].Trim().StartsWith("#Define Window_Main"))
                    {
                        TaskScheduler.Apps.Add(new MsgBox(999, 100, 100, 400, 200, "Error!", "Failed to Initialize window!\nNo propeties were found!", ImprovedVBE.ScaleImageStock(ImageViewer.Nr1, 56, 56)));
                        TaskScheduler.Apps.Remove(this);
                    }
                    else
                    {
                        string[] Lines = Parts[0].Split('\n');
                        for(int i = 0; i < Lines.Length; i++)
                        {
                            string trimmed = WhitespaceRemover.Remover(Lines[i]);

                            if (trimmed.StartsWith("this.Titlebar"))
                            {
                                trimmed = trimmed.Replace("this.Titlebar=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.HasTitlebar = bool.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("this.Title"))
                            {
                                trimmed = trimmed.Replace("this.Title=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                trimmed = trimmed.Remove(0, 1);
                                this.name = trimmed;
                            }
                            else if (trimmed.StartsWith("this.Width"))
                            {
                                trimmed = trimmed.Replace("this.Width=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.width = int.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("this.Height"))
                            {
                                trimmed = trimmed.Replace("this.Height=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.height = int.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("this.RGB"))
                            {
                                trimmed = trimmed.Replace("this.RGB=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                string[] values = trimmed.Split(',');
                                this.CurrentColor = ImprovedVBE.colourToNumber(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
                            }
                            else if (trimmed.StartsWith("this.X"))
                            {
                                trimmed = trimmed.Replace("this.X=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.x = int.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("this.Y"))
                            {
                                trimmed = trimmed.Replace("this.Y=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.y = int.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("this.Z"))
                            {
                                trimmed = trimmed.Replace("this.Z=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.z = int.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("this.AlwaysOnTop"))
                            {
                                trimmed = trimmed.Replace("this.AlwaysOnTop=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.AlwaysOnTop = bool.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("Label"))
                            {
                                trimmed = trimmed.Replace("Label", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                //separate by =
                                string[] parts = trimmed.Split('=');
                                //Store the name
                                string name = parts[0];
                                //Read data from new()
                                string[] values = parts[1].Replace("new(", "").Split(",");
                                //Store the values
                                //Label.Add(new label(int.Parse(values[0]), int.Parse(values[1]), values[2].Remove(values[2].Length - 1).Remove(0, 1), ImprovedVBE.colourToNumber(int.Parse(values[3]), int.Parse(values[4]), int.Parse(values[5])), name));
                                //Kernel.Clipboard += int.Parse(values[0]) + int.Parse(values[1]) + values[2].Remove(values[2].Length - 1).Remove(0, 1) + ImprovedVBE.colourToNumber(int.Parse(values[^3]), int.Parse(values[^2]), int.Parse(values[^1])) + name;
                                Label.Add(new label(int.Parse(values[0]), int.Parse(values[1]), parts[1].Substring(parts[1].IndexOf('\"') + 1, parts[1].LastIndexOf('\"') - parts[1].IndexOf('\"') - 1), ImprovedVBE.colourToNumber(int.Parse(values[^3]), int.Parse(values[^2]), int.Parse(values[^1])), name));
                            }
                            else if (trimmed.StartsWith("Button"))
                            {
                                trimmed = trimmed.Replace("Button", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                //separate by =
                                string[] parts = trimmed.Split('=');
                                //Store the name
                                string name = parts[0];
                                //Read data from new()
                                string[] values = parts[1].Replace("new(", "").Split(",");
                                //Store the values
                                //Label.Add(new label(int.Parse(values[0]), int.Parse(values[1]), values[2].Remove(values[2].Length - 1).Remove(0, 1), ImprovedVBE.colourToNumber(int.Parse(values[3]), int.Parse(values[4]), int.Parse(values[5])), name));
                                Button.Add(new Button_prop(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]), parts[1].Substring(parts[1].IndexOf('\"') + 1, parts[1].LastIndexOf('\"') - parts[1].IndexOf('\"') - 1), ImprovedVBE.colourToNumber(int.Parse(values[^3]), int.Parse(values[^2]), int.Parse(values[^1])), name));
                            }
                            else if (trimmed.StartsWith("Slider"))
                            {
                                trimmed = trimmed.Replace("Slider", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                //separate by =
                                string[] parts = trimmed.Split('=');
                                //Store the name
                                string name = parts[0];
                                //Read data from new()
                                string[] values = parts[1].Replace("new(", "").Split(",");

                                Slider.Add(new UI_Elements.Slider(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]), name));
                            }
                            else if (trimmed.StartsWith("TextBox"))
                            {
                                trimmed = trimmed.Replace("TextBox", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                //separate by =
                                string[] parts = trimmed.Split('=');
                                //Store the name
                                string name = parts[0];
                                //Read data from new()
                                string[] values = parts[1].Replace("new(", "").Split(",");

                                TextBox.Add(new UI_Elements.TextBox(int.Parse(values[0]), int.Parse(values[1]) + 22, int.Parse(values[2]), int.Parse(values[3]), ImprovedVBE.colourToNumber(int.Parse(values[4]), int.Parse(values[5]), int.Parse(values[6])), values[7].Remove(values[7].Length - 1).Remove(0, 1), values[8].Remove(values[8].Length - 1).Remove(0, 1), UI_Elements.TextBox.Options.left, name));
                                //TextBox.Add(new UI_Elements.TextBox(10, 100, 110, 25, ImprovedVBE.colourToNumber(60, 60, 60), "", "Dummy Text", UI_Elements.TextBox.Options.left, "Demo"));
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

                canvas = ImprovedVBE.DrawImageAlpha2(canvas, x, y, canvas, Color.FromArgb(CurrentColor).R, Color.FromArgb(CurrentColor).G, Color.FromArgb(CurrentColor).B, ImprovedVBE.cover);

                if(HasTitlebar == true)
                {
                    DrawGradientLeftToRight();

                    ImprovedVBE.DrawFilledEllipse(canvas, width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                    ImprovedVBE.DrawFilledEllipse(canvas, width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                    BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);
                }

                foreach (var button in Button)
                {
                    if (button.Clicked == true)
                    {
                        UI_Elements.Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, ComplimentaryColor.Generate(button.Color).ToArgb(), button.Text);

                        //Need to think about this one for a bit...
                        //foreach(var p in Parts)
                        //{
                        //    if(p.Contains("OnClick " + button.ID + "\n"))
                        //    {
                        //        //Proof that it works to this point!
                        //        //Label.RemoveAll(d => d.ID == "Debug");
                        //        //Label.Add(new label(10, 60, Parts[1], ImprovedVBE.colourToNumber(255, 255, 255), "Debug"));

                        //        CSharp exec = new CSharp();
                        //        exec.Button = Button;
                        //        exec.Slider = Slider;
                        //        exec.Label = Label;
                        //        exec.Scroll = Scroll;
                        //        exec.CheckBox = CheckBox;
                        //        exec.TextBox = TextBox;
                        //        exec.Dropdown = Dropdown;
                        //        exec.window = canvas;
                        //        exec.CurrentColor = CurrentColor;

                        //        Label.RemoveAll(d => d.ID == "Debug");
                        //        string[] lines = p.Split('\n');
                        //        for(int i = 2; i < lines.Length - 1; i++)
                        //        {
                        //            //Label.Add(new label(10, 60, exec.Returning_methods(lines[i]), ImprovedVBE.colourToNumber(255, 255, 255), "Debug"));
                        //            exec.Returning_methods(lines[i]);
                        //        }
                        //        Button = exec.Button;
                        //        Slider = exec.Slider;
                        //        Label = exec.Label;
                        //        Scroll = exec.Scroll;
                        //        CheckBox = exec.CheckBox;
                        //        TextBox = exec.TextBox;
                        //        Dropdown = exec.Dropdown;
                        //        if (CurrentColor != exec.CurrentColor)
                        //        {
                        //            once = true;
                        //            CurrentColor = exec.CurrentColor;
                        //            goto top;
                        //        }
                        //        Array.Copy(exec.window.RawData, 0, window.RawData, 0, exec.window.RawData.Length);
                        //    }
                        //}
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

                foreach(Slider s in Slider)
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
                                foreach (var p in Parts)
                                {
                                    if (p.Contains("OnClick " + button.ID + "\n"))
                                    {
                                        //Proof that it works to this point!
                                        //Label.RemoveAll(d => d.ID == "Debug");
                                        //Label.Add(new label(10, 60, Parts[1], ImprovedVBE.colourToNumber(255, 255, 255), "Debug"));

                                        CSharp exec = new CSharp();
                                        exec.Button = Button;
                                        exec.Slider = Slider;
                                        exec.Label = Label;
                                        exec.Scroll = Scroll;
                                        exec.CheckBox = CheckBox;
                                        exec.TextBox = TextBox;
                                        exec.Dropdown = Dropdown;
                                        exec.window = canvas;
                                        exec.CurrentColor = CurrentColor;

                                        Label.RemoveAll(d => d.ID == "Debug");
                                        string[] lines = p.Split('\n');
                                        for (int i = 2; i < lines.Length - 1; i++)
                                        {
                                            //Label.Add(new label(10, 60, exec.Returning_methods(lines[i]), ImprovedVBE.colourToNumber(255, 255, 255), "Debug"));
                                            exec.Returning_methods(lines[i]);
                                        }
                                        Button = exec.Button;
                                        Slider = exec.Slider;
                                        Label = exec.Label;
                                        Scroll = exec.Scroll;
                                        CheckBox = exec.CheckBox;
                                        TextBox = exec.TextBox;
                                        Dropdown = exec.Dropdown;
                                        if (CurrentColor != exec.CurrentColor)
                                        {
                                            once = true;
                                        }
                                        if (exec.Clipboard == "Terminate")
                                        {
                                            TaskScheduler.Apps.Remove(this);
                                        }
                                        CurrentColor = exec.CurrentColor;
                                        Array.Copy(exec.window.RawData, 0, window.RawData, 0, exec.window.RawData.Length);
                                    }
                                }
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

            int part = 0;
            for(int i = 0; i < Parts.Count && part == 0; i++)
            {
                if (Parts[i].Contains("#void Looping\n"))
                {
                    part = i;
                }
            }

            //Kernel.Clipboard = part.ToString();

            if(part != 0 && CycleCount > 5)
            {
                CSharp execLoop = new CSharp();
                execLoop.Button = Button;
                execLoop.Slider = Slider;
                execLoop.Label = Label;
                execLoop.Scroll = Scroll;
                execLoop.CheckBox = CheckBox;
                execLoop.TextBox = TextBox;
                execLoop.Dropdown = Dropdown;
                execLoop.CurrentColor = CurrentColor;
                execLoop.window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                Array.Copy(window.RawData, 0, execLoop.window.RawData, 0, window.RawData.Length);
                string[] lines2 = Parts[part].Split('\n');
                for (int i = 2; i < lines2.Length && execLoop.Clipboard != "Terminate"; i++)
                {
                    execLoop.Returning_methods(lines2[i]);
                }
                Button = execLoop.Button;
                Slider = execLoop.Slider;
                Label = execLoop.Label;
                Scroll = execLoop.Scroll;
                CheckBox = execLoop.CheckBox;
                TextBox = execLoop.TextBox;
                Dropdown = execLoop.Dropdown;
                if(CurrentColor != execLoop.CurrentColor)
                {
                    once = true;
                }
                if(execLoop.Clipboard == "Terminate")
                {
                    TaskScheduler.Apps.Remove(this);
                }
                CurrentColor = execLoop.CurrentColor;
                Array.Copy(execLoop.window.RawData, 0, window.RawData, 0, execLoop.window.RawData.Length);
                CycleCount = 0;
            }
            else
            {
                CycleCount++;
            }

            if (TaskScheduler.Apps[^1] == this)
            {
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    foreach(var box in TextBox)
                    {
                        if(box.Selected == true)
                        {
                            box.Text = Keyboard.HandleKeyboard(box.Text, key);
                        }
                    }
                }
            }
            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }

        public void App(Bitmap RenderTo)
        {
            if (AlwaysOnTop == true)
            {
                z = 1000;
            }
            if (initial == true)
            {
                //Separate the code into 3 segments:
                //1. Initial window UI element layout **Done! All implemented**
                //2. Loop of the window
                //3. UI element actions
                Parts = Separate(Code);
                if (Parts.Count == 0)
                {
                    TaskScheduler.Apps.Add(new MsgBox(999, 100, 100, 400, 200, "Error!", "Failed to execute program!\nNo executable code was found!", ImprovedVBE.ScaleImageStock(ImageViewer.Nr1, 56, 56)));
                    TaskScheduler.Apps.Remove(this);
                }
                else
                {
                    if (!Parts[0].Trim().StartsWith("#Define Window_Main"))
                    {
                        TaskScheduler.Apps.Add(new MsgBox(999, 100, 100, 400, 200, "Error!", "Failed to Initialize window!\nNo propeties were found!", ImprovedVBE.ScaleImageStock(ImageViewer.Nr1, 56, 56)));
                        TaskScheduler.Apps.Remove(this);
                    }
                    else
                    {
                        string[] Lines = Parts[0].Split('\n');
                        for (int i = 0; i < Lines.Length; i++)
                        {
                            string trimmed = WhitespaceRemover.Remover(Lines[i]);

                            if (trimmed.StartsWith("this.Titlebar"))
                            {
                                trimmed = trimmed.Replace("this.Titlebar=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.HasTitlebar = bool.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("this.Title"))
                            {
                                trimmed = trimmed.Replace("this.Title=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                trimmed = trimmed.Remove(0, 1);
                                this.name = trimmed;
                            }
                            else if (trimmed.StartsWith("this.Width"))
                            {
                                trimmed = trimmed.Replace("this.Width=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.width = int.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("this.Height"))
                            {
                                trimmed = trimmed.Replace("this.Height=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.height = int.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("this.RGB"))
                            {
                                trimmed = trimmed.Replace("this.RGB=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                string[] values = trimmed.Split(',');
                                this.CurrentColor = ImprovedVBE.colourToNumber(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
                            }
                            else if (trimmed.StartsWith("this.X"))
                            {
                                trimmed = trimmed.Replace("this.X=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.x = int.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("this.Y"))
                            {
                                trimmed = trimmed.Replace("this.Y=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.y = int.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("this.Z"))
                            {
                                trimmed = trimmed.Replace("this.Z=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.z = int.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("this.AlwaysOnTop"))
                            {
                                trimmed = trimmed.Replace("this.AlwaysOnTop=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.AlwaysOnTop = bool.Parse(trimmed);
                            }
                            else if (trimmed.StartsWith("Label"))
                            {
                                trimmed = trimmed.Replace("Label", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                //separate by =
                                string[] parts = trimmed.Split('=');
                                //Store the name
                                string name = parts[0];
                                //Read data from new()
                                string[] values = parts[1].Replace("new(", "").Split(",");
                                //Store the values
                                //Label.Add(new label(int.Parse(values[0]), int.Parse(values[1]), values[2].Remove(values[2].Length - 1).Remove(0, 1), ImprovedVBE.colourToNumber(int.Parse(values[3]), int.Parse(values[4]), int.Parse(values[5])), name));
                                
                                //Kernel.Clipboard += int.Parse(values[0]) + int.Parse(values[1]) + values[2].Remove(values[2].Length - 1).Remove(0, 1) + ImprovedVBE.colourToNumber(int.Parse(values[^3]), int.Parse(values[^2]), int.Parse(values[^1])) + name;
                                Label.Add(new label(int.Parse(values[0]), int.Parse(values[1]), parts[1].Substring(parts[1].IndexOf('\"') + 1, parts[1].LastIndexOf('\"') - parts[1].IndexOf('\"') - 1), ImprovedVBE.colourToNumber(int.Parse(values[^3]), int.Parse(values[^2]), int.Parse(values[^1])), name));

                                //Label.Add(new label(int.Parse(values[0]), int.Parse(values[1]), values[2].Remove(values[2].Length - 1).Remove(0, 1), ImprovedVBE.colourToNumber(int.Parse(values[3]), int.Parse(values[4]), int.Parse(values[5])), name));
                            }
                            else if (trimmed.StartsWith("Button"))
                            {
                                trimmed = trimmed.Replace("Button", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                //separate by =
                                string[] parts = trimmed.Split('=');
                                //Store the name
                                string name = parts[0];
                                //Read data from new()
                                string[] values = parts[1].Replace("new(", "").Split(",");
                                //Store the values
                                //Label.Add(new label(int.Parse(values[0]), int.Parse(values[1]), values[2].Remove(values[2].Length - 1).Remove(0, 1), ImprovedVBE.colourToNumber(int.Parse(values[3]), int.Parse(values[4]), int.Parse(values[5])), name));
                                Button.Add(new Button_prop(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]), parts[1].Substring(parts[1].IndexOf('\"') + 1, parts[1].LastIndexOf('\"') - parts[1].IndexOf('\"') - 1), ImprovedVBE.colourToNumber(int.Parse(values[^3]), int.Parse(values[^2]), int.Parse(values[^1])), name));
                                //Button.Add(new Button_prop(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]), values[4].Remove(values[4].Length - 1).Remove(0, 1), ImprovedVBE.colourToNumber(int.Parse(values[5]), int.Parse(values[6]), int.Parse(values[7])), name));
                            }
                            else if (trimmed.StartsWith("Slider"))
                            {
                                trimmed = trimmed.Replace("Slider", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                //separate by =
                                string[] parts = trimmed.Split('=');
                                //Store the name
                                string name = parts[0];
                                //Read data from new()
                                string[] values = parts[1].Replace("new(", "").Split(",");

                                Slider.Add(new UI_Elements.Slider(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]), name));
                            }
                            else if (trimmed.StartsWith("TextBox"))
                            {
                                trimmed = trimmed.Replace("TextBox", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                //separate by =
                                string[] parts = trimmed.Split('=');
                                //Store the name
                                string name = parts[0];
                                //Read data from new()
                                string[] values = parts[1].Replace("new(", "").Split(",");

                                TextBox.Add(new UI_Elements.TextBox(int.Parse(values[0]), int.Parse(values[1]) + 22, int.Parse(values[2]), int.Parse(values[3]), ImprovedVBE.colourToNumber(int.Parse(values[4]), int.Parse(values[5]), int.Parse(values[6])), values[7].Remove(values[7].Length - 1).Remove(0, 1), values[8].Remove(values[8].Length - 1).Remove(0, 1), UI_Elements.TextBox.Options.left, name));
                                //TextBox.Add(new UI_Elements.TextBox(10, 100, 110, 25, ImprovedVBE.colourToNumber(60, 60, 60), "", "Dummy Text", UI_Elements.TextBox.Options.left, "Demo"));
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

                canvas = ImprovedVBE.DrawImageAlpha2(canvas, x, y, canvas, Color.FromArgb(CurrentColor).R, Color.FromArgb(CurrentColor).G, Color.FromArgb(CurrentColor).B, RenderTo);

                if (HasTitlebar == true)
                {
                    DrawGradientLeftToRight();

                    ImprovedVBE.DrawFilledEllipse(canvas, width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                    ImprovedVBE.DrawFilledEllipse(canvas, width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                    BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);
                }

                foreach (var button in Button)
                {
                    if (button.Clicked == true)
                    {
                        UI_Elements.Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, ComplimentaryColor.Generate(button.Color).ToArgb(), button.Text);

                        //Need to think about this one for a bit...
                        foreach (var p in Parts)
                        {
                            if (p.Contains("OnClick " + button.ID + "\n"))
                            {
                                //Proof that it works to this point!
                                //Label.RemoveAll(d => d.ID == "Debug");
                                //Label.Add(new label(10, 60, Parts[1], ImprovedVBE.colourToNumber(255, 255, 255), "Debug"));

                                CSharp exec = new CSharp();
                                exec.Button = Button;
                                exec.Slider = Slider;
                                exec.Label = Label;
                                exec.Scroll = Scroll;
                                exec.CheckBox = CheckBox;
                                exec.TextBox = TextBox;
                                exec.Dropdown = Dropdown;
                                exec.window = canvas;

                                Label.RemoveAll(d => d.ID == "Debug");
                                string[] lines = p.Split('\n');
                                for (int i = 2; i < lines.Length - 1; i++)
                                {
                                    //Label.Add(new label(10, 60, exec.Returning_methods(lines[i]), ImprovedVBE.colourToNumber(255, 255, 255), "Debug"));
                                    exec.Returning_methods(lines[i]);
                                }
                                Button = exec.Button;
                                Slider = exec.Slider;
                                Label = exec.Label;
                                Scroll = exec.Scroll;
                                CheckBox = exec.CheckBox;
                                TextBox = exec.TextBox;
                                Dropdown = exec.Dropdown;
                                Array.Copy(exec.window.RawData, 0, window.RawData, 0, exec.window.RawData.Length);
                            }
                        }
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

            int part = 0;
            for (int i = 0; i < Parts.Count && part == 0; i++)
            {
                if (Parts[i].Contains("#void Looping\n"))
                {
                    part = i;
                }
            }

            //Kernel.Clipboard = part.ToString();
            
            if (part != 0 && CycleCount > 5)
            {
                CSharp execLoop = new CSharp();
                execLoop.Button = Button;
                execLoop.Slider = Slider;
                execLoop.Label = Label;
                execLoop.Scroll = Scroll;
                execLoop.CheckBox = CheckBox;
                execLoop.TextBox = TextBox;
                execLoop.Dropdown = Dropdown;
                execLoop.window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                Array.Copy(window.RawData, 0, execLoop.window.RawData, 0, window.RawData.Length);
                string[] lines2 = Parts[part].Split('\n');
                for (int i = 2; i < lines2.Length && execLoop.Clipboard != "Terminate"; i++)
                {
                    execLoop.Returning_methods(lines2[i]);
                }
                Button = execLoop.Button;
                Slider = execLoop.Slider;
                Label = execLoop.Label;
                Scroll = execLoop.Scroll;
                CheckBox = execLoop.CheckBox;
                TextBox = execLoop.TextBox;
                Dropdown = execLoop.Dropdown;
                Array.Copy(execLoop.window.RawData, 0, window.RawData, 0, execLoop.window.RawData.Length);
                CycleCount = 0;
            }
            else
            {
                CycleCount++;
            }

            if (TaskScheduler.Apps[^1] == this)
            {
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    foreach (var box in TextBox)
                    {
                        if (box.Selected == true)
                        {
                            box.Text = Keyboard.HandleKeyboard(box.Text, key);
                        }
                    }
                }
            }
            ImprovedVBE.DrawImageAlpha(window, x, y, RenderTo);
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

        public List<string> Separate(string In)
        {
            List<string> parts = new List<string>();

            string current = "";
            int openedRemainingBrackets = 0;
            bool Started = false;
            for(int i = 0; i < In.Length; i++)
            {
                current += In[i];
                if (In[i] == '{')
                {
                    openedRemainingBrackets++;
                    Started = true;
                }
                else if (In[i] == '}')
                {
                    openedRemainingBrackets--;
                }
                if(i != 0 && openedRemainingBrackets == 0 && Started == true)
                {
                    parts.Add(current);
                    Started = false;
                    current = "";
                }
            }
            return parts;
        }

        public Window(int x, int y, int z, int width, int height, int desk_ID, string name, bool minimised, Bitmap icon, string Code)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.width = width;
            this.height = height;
            this.desk_ID = desk_ID;
            this.name = name;
            this.minimised = minimised;
            this.icon = icon;
            this.Code = Code;
        }
    }
}
