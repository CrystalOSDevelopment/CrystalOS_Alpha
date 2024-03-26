using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Widgets;
using CrystalOSAlpha.Programming;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        public int AppID { get; set; }

        public string name { get; set; }

        public bool minimised { get; set; }
        public bool movable { get; set; }
        public Bitmap icon { get; set; }

        public int Counter = 0;
        public int CurrentColor = ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B);
        public int part = 0;
        public int CycleCount = 0;

        public string Code = "#Define Window_Main\n{\n    this.Title = \"SubwaySim 24\";\n    this.X = 0;\n    this.Y = 0;\n    this.Width = 1920;\n    this.Height = 1005;\n    this.Titlebar = true;\n    this.RGB = 60, 60, 60;\n    //PictureBox Interior = new PictureBox(0, 0, \"1:\\SubwaySim24\\Assets\\Interior.bmp\", true);\n    //PictureBox Tunnel = new PictureBox(0, 0, \"1:\\SubwaySim24\\Assets\\Tunnel1.bmp\", true);\n    PictureBox Tunnel = new PictureBox(0, 0, 1920, 1080, true);\n\n    //Points\n    //Rails\n    Point p1 = new Point(770, 720);\n    Point p2 = new Point(1036, 353);\n    Point p3 = new Point(1038, 355);\n    Point p4 = new Point(790, 720);\n\n    Point p5 = new Point(1097, 720);\n    Point p6 = new Point(1079, 360);\n    Point p7 = new Point(1083, 363);\n    Point p8 = new Point(1116, 720);\n    //Station\n    Point First = new Point(885, 385);\n    Point Second = new Point(890, 388);\n    Point Third = new Point(869, 407);\n    Point Fourth = new Point(869, 466);\n    Point Fifth = new Point(563, 717);\n    Point Sixth = new Point(261, 719);\n    Point Seventh = new Point(231, 567);\n\n    Tunnel.Clear(42, 42, 42);\n    Tunnel.FilledPollygon(p1, p2, p3, p4);\n    Tunnel.FilledPollygon(p5, p6, p7, p8);\n    Tunnel.FilledPollygon(First, Second, Third, Fourth, Fifth, Sixth, Seventh);\n\n    //Game Graphics\n    //Interior.MergeOnto(Tunnel);\n    \n    Label VehicleName = new Label(125, 853, VehicleBrand, 255, 255, 255);\n    Label VehicleL = new Label(125, 880, VehicleLength, 255, 255, 255);\n    Label VehicleW = new Label(125, 907, VehicleWeight, 255, 255, 255);\n    Label VehicleMaxS = new Label(125, 934, VehicleMaxSpeed, 255, 255, 255);\n\n    Label VehicleS = new Label(345, 853, VehicleSpeed, 255, 255, 255);\n    Label VehicleT = new Label(345, 880, VehicleThrotle, 255, 255, 255);\n    Label VehicleB = new Label(345, 907, VehicleBreak, 255, 255, 255);\n\n    Label Time = new Label(1480, 25, Seconds, 255, 162, 0);\n    Label WaitTime = new Label(1480, 50, WaitInStation, 255, 162, 0);\n    Label Travelled = new Label(1480, 75, TravelledDistance, 255, 162, 0);\n    //Control UI\n    //Throtle\n    Button ThrotleUp = new Button(1660, 540, 225, 60, \"Throtle Up\", 1, 1, 1);\n    Button ThrotleDown = new Button(1660, 620, 225, 60, \"Throtle Down\", 1, 1, 1);\n    //Index\n    Button IndexL = new Button(1660, 463, 104, 60, \"Index Left\", 1, 1, 1);\n    Button IndexR = new Button(1781, 463, 104, 60, \"Index Right\", 1, 1, 1);\n    //Door handling\n    Button DoorL = new Button(1660, 385, 104, 60, \"Door Left\", 1, 1, 1);\n    Button DoorR = new Button(1781, 385, 104, 60, \"Door Right\", 1, 1, 1);\n    //Horn\n    Button Horn = new Button(1660, 308, 225, 60, \"Horn\", 1, 1, 1);\n}\n#Define Variables\n{\n    string VehicleBrand = \"Test subway\";\n    string VehicleLength = \"25 Meter\";\n    string VehicleWeight = \"34 Tonn\";\n    string VehicleMaxSpeed = \"120 KM/H\";\n    string VehicleGear = \"Neutral\";\n    string VehicleThrotle = \"0\";\n    string VehicleBreak = \"0\";\n    int VehicleSpeed = 0;\n    bool Horn = false;\n\n    //Game mechanics\n    int Seconds = 0;\n    int Now = DateTime.UtcNow.Second;\n    int WaitInStation = 8;\n    int TravelledDistance = 0;\n}\n#void Looping\n{\n    //Gametic\n    int CurrentSecond = DateTime.UtcNow.Second;\n    if(CurrentSecond != Now)\n    {\n        //Time spent in-game\n        Seconds += 1;\n        Now = CurrentSecond;\n        string temp = \"Ellapsed time: \" + Seconds + \"s\";\n        Time.Content = temp;\n\n        //Time spent waiting in station\n        if(WaitInStation > 0)\n        {\n            WaitInStation -= 1;\n            string countBack = \"You can leave the station after \" + WaitInStation + \" seconds.\";\n            WaitTime.Content = countBack;\n            WaitTime.Color = 255, 162, 0;\n        }\n        else\n        {\n            string countBack = \"You can now leave the station! Drive safe!\";\n            WaitTime.Content = countBack;\n            WaitTime.Color = 0, 255, 0;\n        }\n        //Measure distance in meter\n        int Dist = VehicleSpeed * 0.277778;\n        TravelledDistance += Dist;\n        string TravelledDist = \"Distance travelled: \" + TravelledDistance + \" meter(s)\";\n        Travelled.Content = TravelledDist;\n\n        //Moving tracks\n        P1.X += 10;\n        P2.X += 10;\n        Tunnel.Clear(42, 42, 42);\n        Tunnel.FilledPollygon(p1, p2, p3, p4);\n        Tunnel.FilledPollygon(p5, p6, p7, p8);\n        Tunnel.FilledPollygon(First, Second, Third, Fourth, Fifth, Sixth, Seventh);\n\n        //Game Graphics\n        //Interior.MergeOnto(Tunnel);\n    }\n    //End of Gametic\n    //Rendering\n    //End of Rendering\n}\n#OnClick ThrotleUp\n{\n    if(VehicleSpeed < 120)\n    {\n        VehicleSpeed += 10;\n        VehicleS.Content = VehicleSpeed;\n    }\n    if(VehicleSpeed > 80)\n    {\n        VehicleT.Content = \"5\";\n    }\n    if(VehicleSpeed < 80)\n    {\n        VehicleT.Content = \"4\";\n    }\n    if(VehicleSpeed < 60)\n    {\n        VehicleT.Content = \"3\";\n    }\n    if(VehicleSpeed < 40)\n    {\n        VehicleT.Content = \"2\";\n    }\n    if(VehicleSpeed < 20)\n    {\n        VehicleT.Content = \"1\";\n    }\n}\n#OnClick ThrotleDown\n{\n    if(0 < VehicleSpeed)\n    {\n        VehicleSpeed -= 10;\n        VehicleS.Content = VehicleSpeed;\n    }\n    if(VehicleSpeed > 80)\n    {\n        VehicleT.Content = \"5\";\n    }\n    if(VehicleSpeed < 80)\n    {\n        VehicleT.Content = \"4\";\n    }\n    if(VehicleSpeed < 60)\n    {\n        VehicleT.Content = \"3\";\n    }\n    if(VehicleSpeed < 40)\n    {\n        VehicleT.Content = \"2\";\n    }\n    if(VehicleSpeed < 20)\n    {\n        VehicleT.Content = \"1\";\n    }\n}";

        public bool initial = true;
        public bool once = true;
        public bool clicked = false;
        public bool HasTitlebar = true;
        public bool AlwaysOnTop = false;
        public bool temp = true;

        public Bitmap canvas;
        public Bitmap window;
        #endregion Essential

        #region UI_Elements
        public List<Button_prop> Button = new List<Button_prop>();
        public List<Slider> Slider = new List<Slider>();
        public List<CheckBox> CheckBox = new List<CheckBox>();
        public List<Dropdown> Dropdown = new List<Dropdown>();
        public List<Scrollbar_Values> Scroll = new List<Scrollbar_Values>();
        public List<TextBox> TextBox = new List<TextBox>();
        public List<label> Label = new List<label>();
        public List<Table> Tables = new List<Table>();
        public List<PictureBox> Picturebox = new List<PictureBox>();

        public List<Variables> Vars = new List<Variables>();

        public MenuBar mb;
        public bool HasMB = false;
        #endregion UI_Elements

        public List<string> Parts = new List<string>();
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
                    foreach (string s in Parts)
                    {
                        if (s.Contains("Menubar"))
                        {
                            mb = new GenerateMenubar().Generate(s);
                            HasMB = true;
                        }
                        else if (s.Contains("Define Variables"))
                        {
                            CSharp cs = new CSharp();
                            cs.Variables = Vars;
                            string[] lines2 = s.Split('\n');
                            for (int i = 2; i < lines2.Length; i++)
                            {
                                cs.Returning_methods(lines2[i]);
                            }
                            Vars = cs.Variables;
                        }
                    }
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
                            else if (trimmed.StartsWith("this.Icon"))
                            {
                                trimmed = trimmed.Replace("this.Icon=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                this.icon = new Bitmap(trimmed.Replace("\"", ""));
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
                                if (parts[1].Contains("\""))
                                {
                                    Label.Add(new label(int.Parse(values[0]), int.Parse(values[1]), parts[1].Substring(parts[1].IndexOf('\"') + 1, parts[1].LastIndexOf('\"') - parts[1].IndexOf('\"') - 1), ImprovedVBE.colourToNumber(int.Parse(values[^3]), int.Parse(values[^2]), int.Parse(values[^1])), name));
                                }
                                else
                                {
                                    string Value = "";
                                    foreach(var v in Vars)
                                    {
                                        if(v.S_Name == values[2])
                                        {
                                            Value = v.S_Value;
                                        }
                                        else if (v.I_Name == values[2])
                                        {
                                            Value = v.I_Value.ToString();
                                        }
                                    }
                                    Label.Add(new label(int.Parse(values[0]), int.Parse(values[1]), Value, ImprovedVBE.colourToNumber(int.Parse(values[^3]), int.Parse(values[^2]), int.Parse(values[^1])), name));
                                }
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
                            else if (trimmed.StartsWith("CheckBox"))
                            {
                                trimmed = trimmed.Replace("CheckBox.NewCheckBox(", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                //separate by =
                                //string[] parts = trimmed.Split('=');
                                //Store the name
                                //string name = parts[0];
                                //Read data from new()
                                string[] values = trimmed.Split(",");

                                CheckBox.Add(new UI_Elements.CheckBox(int.Parse(values[0]), int.Parse(values[1]) + 22, int.Parse(values[2]), int.Parse(values[3]), bool.Parse(values[4]), values[5].Replace("\"", ""), values[6].Remove(values[6].Length - 1).Remove(0, 1)));
                            }
                            else if (trimmed.StartsWith("Table"))
                            {
                                trimmed = trimmed.Replace("Table", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                //separate by =
                                string[] parts = trimmed.Split('=');
                                //Store the name
                                string name = parts[0];
                                //Read data from new()
                                string[] values = parts[1].Replace("new(", "").Split(",");

                                Tables.Add(new Table(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]), int.Parse(values[4]), int.Parse(values[5]), name, int.Parse(values[6]), int.Parse(values[7])));
                                Tables[^1].Initialize();
                            }
                            else if (trimmed.StartsWith("PictureBox"))
                            {
                                trimmed = trimmed.Replace("PictureBox", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                //separate by =
                                string[] parts = trimmed.Split('=');
                                //Store the name
                                string name = parts[0];
                                //Read data from new()
                                string[] values = parts[1].Replace("new(", "").Split(",");

                                if (trimmed.Contains("\""))
                                {
                                    if (int.TryParse(values[0], out int XAxis))
                                    {
                                        if (File.Exists(values[2].Replace("\"", "")))
                                        {
                                            Bitmap temp = new Bitmap(values[2].Replace("\"", ""));
                                            //bool.TryParse(values[2], out bool t);
                                            Picturebox.Add(new PictureBox(int.Parse(values[0]), int.Parse(values[1]), name, true, temp));
                                        }
                                    }
                                }
                                else
                                {
                                    Bitmap temp = new Bitmap(uint.Parse(values[2]), uint.Parse(values[3]), ColorDepth.ColorDepth32);
                                    Array.Fill(temp.RawData, 0);
                                    Picturebox.Add(new PictureBox(int.Parse(values[0]), int.Parse(values[1]), name, true, temp));
                                }
                            }
                            else
                            {
                                CSharp cs = new CSharp();
                                cs.Picturebox = Picturebox;
                                cs.Variables = Vars;
                                cs.Returning_methods(trimmed);
                                Picturebox = cs.Picturebox;
                                Vars = cs.Variables;
                            }
                        }
                    }
                }
                initial = false;
            }

            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
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

                canvas = ImprovedVBE.EnableTransparencyPreRGB(canvas, x, y, canvas, Color.FromArgb(CurrentColor).R, Color.FromArgb(CurrentColor).G, Color.FromArgb(CurrentColor).B, ImprovedVBE.cover);

                if(HasTitlebar == true)
                {
                    ImprovedVBE.DrawGradientLeftToRight(canvas);

                    ImprovedVBE.DrawFilledEllipse(canvas, width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                    ImprovedVBE.DrawFilledEllipse(canvas, width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                    BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);
                }

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                for (int i = 0; i < Parts.Count && part == 0; i++)
                {
                    if (Parts[i].Contains("#void Looping"))
                    {
                        part = i;
                    }
                }
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
                                temp = true;
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
                        temp = true;
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
                if (Box.Clciked(x + Box.X, y + Box.Y) == true && clicked == false)
                {
                    foreach (var box2 in TextBox)
                    {
                        box2.Selected = false;
                    }
                    clicked = true;
                    Box.Selected = true;
                }
            }

            foreach (var slid in CheckBox)
            {
                bool val = slid.Value;
                if (slid.CheckForClick(x, y))
                {
                    slid.Clicked = true;
                }
                if (slid.Clicked == true && MouseManager.MouseState == MouseState.None)
                {
                    if (slid.Value == false)
                    {
                        slid.Value = true;
                    }
                    else
                    {
                        slid.Value = false;
                    }
                    temp = true;
                    slid.Clicked = false;
                }
            }

            if(part != 0 && CycleCount > 50)
            {
                CSharp execLoop = new CSharp();
                execLoop.Button = Button;
                execLoop.Slider = Slider;
                execLoop.Label = Label;
                execLoop.Scroll = Scroll;
                execLoop.CheckBox = CheckBox;
                execLoop.TextBox = TextBox;
                execLoop.Dropdown = Dropdown;
                execLoop.Tables = Tables;
                execLoop.CurrentColor = CurrentColor;
                execLoop.Variables = Vars;
                execLoop.Picturebox = Picturebox;
                execLoop.window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                Array.Copy(window.RawData, 0, execLoop.window.RawData, 0, window.RawData.Length);

                Parts = Separate(Code);
                string[] lines2 = Parts[part].Split('\n');
                try
                {
                    for (int i = 3; i < lines2.Length - 1 && execLoop.Clipboard != "Terminate"; i++)
                    {
                        if (lines2[i].Contains("InjectCode(") && execLoop.Count == 0)
                        {
                            string line = lines2[i].Trim();
                            line = line.Replace("InjectCode(", "");
                            line = line.Remove(line.Length - 2).Replace("\"", "");
                            if(File.Exists(line))
                            {
                                string ToInject = File.ReadAllText(line);
                                string[] SplittedLines = ToInject.Split("\n");
                                for(int j = 0; j < SplittedLines.Length; j++)
                                {
                                    execLoop.Returning_methods(SplittedLines[j]);
                                    if (MouseManager.MouseState == MouseState.Left)
                                    {
                                        foreach (var button in Button)
                                        {
                                            if (MouseManager.X > x + button.X && MouseManager.X < x + button.X + button.Width)
                                            {
                                                if (MouseManager.Y > y + button.Y && MouseManager.Y < y + button.Y + button.Height)
                                                {
                                                    if (button.Clicked == false)
                                                    {
                                                        button.Clicked = true;
                                                        temp = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Kernel.Clipboard += "\nThe file doesn't exist";
                            }
                        }
                        else
                        {
                            execLoop.Returning_methods(lines2[i]);
                        }
                        if(MouseManager.MouseState == MouseState.Left)
                        {
                            foreach (var button in Button)
                            {
                                if (MouseManager.X > x + button.X && MouseManager.X < x + button.X + button.Width)
                                {
                                    if (MouseManager.Y > y + button.Y && MouseManager.Y < y + button.Y + button.Height)
                                    {
                                        if (button.Clicked == false)
                                        {
                                            button.Clicked = true;
                                            temp = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {
                    Parts = Separate(Code);
                }
                if (HasMB)
                {
                    var Message = WindowMessenger.Recieve("Submenu", AppID.ToString());
                    if(Message != null)
                    {
                        WindowMessenger.Message.RemoveAll(d => d.From == Message.From && d.Message == Message.Message);
                        if(Message.Message.Length != 0)
                        {
                            lines2 = Message.Message.Split('\n');
                            for (int i = 0; i < lines2.Length && execLoop.Clipboard != "Terminate"; i++)
                            {
                                execLoop.Returning_methods(lines2[i]);
                            }
                        }
                    }
                }
                Button = execLoop.Button;
                Slider = execLoop.Slider;
                Label = execLoop.Label;
                Scroll = execLoop.Scroll;
                CheckBox = execLoop.CheckBox;
                TextBox = execLoop.TextBox;
                Dropdown = execLoop.Dropdown;
                Vars = execLoop.Variables;
                if(Tables != execLoop.Tables)
                {
                    temp = true;
                }
                Tables = execLoop.Tables;
                if(CurrentColor != execLoop.CurrentColor)
                {
                    once = true;
                }
                if(execLoop.Clipboard == "Terminate")
                {
                    TaskScheduler.Apps.Remove(this);
                }
                CurrentColor = execLoop.CurrentColor;
                if(execLoop.NeedUpdate == true)
                {
                    temp = true;
                    execLoop.NeedUpdate = false;
                }
                Picturebox = execLoop.Picturebox;
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
                    foreach (var c in Tables)//For would be more efficient
                    {
                        foreach (var v in c.Cells)
                        {
                            if (v.Selected == true && v.WriteProtected == false)
                            {
                                v.Content = Keyboard.HandleKeyboard(v.Content, key);
                                temp = true;
                            }
                        }
                    }
                }
                if (MouseManager.MouseState == MouseState.Left)
                {
                    foreach (var v in Tables)
                    {
                        if(clicked == false)
                        {
                            if (v.Select2((int)MouseManager.X - x - v.X, (int)MouseManager.Y - y - v.Y))
                            {
                                foreach (var box in TextBox)
                                {
                                    if (box.Selected == true)
                                    {
                                        box.Selected = false;
                                    }
                                }
                                temp = true;
                                clicked = true;
                            }
                        }
                    }
                }
            }

            if (HasMB)
            {
                if(temp == true || mb.Render(window, AppID) == true)
                {
                    Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

                    foreach (var button in Button)
                    {
                        if (button.Clicked == true)
                        {
                            UI_Elements.Button.Button_render(window, button.X, button.Y, button.Width, button.Height, ComplimentaryColor.Generate(button.Color).ToArgb(), button.Text);

                            //Need to think about this one for a bit...
                            foreach (var p in Parts)
                            {
                                if (p.StartsWith("\n#OnClick " + button.ID + "\n"))
                                {
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
                        else
                        {
                            UI_Elements.Button.Button_render(window, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                        }
                    }

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

                    foreach (var Checkbox in CheckBox)
                    {
                        Checkbox.Render(window);
                    }

                    foreach (var T in Tables)
                    {
                        T.Render(window);
                    }

                    mb.Render(window, AppID);
                    temp = false;
                }
            }
            else
            {
                if (temp == true)
                {
                    foreach (var img in Picturebox)
                    {
                        //Array.Copy(img.RawData, 0, window.RawData, window.Width * 22, img.RawData.Length);
                        img.Render(window);
                    }

                    foreach (var button in Button)
                    {
                        if (button.Clicked == true)
                        {
                            UI_Elements.Button.Button_render(window, button.X, button.Y, button.Width, button.Height, ComplimentaryColor.Generate(button.Color).ToArgb(), button.Text);
                            if(clicked == false)
                            {
                                //Need to think about this one for a bit...
                                foreach (var p in Parts)
                                {
                                    if (p.Contains("#OnClick " + button.ID))//"#OnClick " + button.ID + "\n"
                                    {
                                        CSharp exec = new CSharp();
                                        exec.Button = Button;
                                        exec.Slider = Slider;
                                        exec.Label = Label;
                                        exec.Scroll = Scroll;
                                        exec.CheckBox = CheckBox;
                                        exec.TextBox = TextBox;
                                        exec.Dropdown = Dropdown;
                                        exec.window = window;
                                        exec.CurrentColor = CurrentColor;
                                        exec.Variables = Vars;
                                        Label.RemoveAll(d => d.ID == "Debug");
                                        string[] lines = p.Split('\n');
                                        try
                                        {
                                            for (int i = 3; i < lines.Length - 1 && exec.Clipboard != "Terminate"; i++)
                                            {
                                                if (lines[i].Contains("InjectCode("))
                                                {
                                                    string line = lines[i].Trim();
                                                    line = line.Replace("InjectCode(", "");
                                                    line = line.Remove(line.Length - 2).Replace("\"", "");
                                                    if (File.Exists(line))
                                                    {
                                                        string ToInject = File.ReadAllText(line);
                                                        string[] SplittedLines = ToInject.Split("\n");
                                                        for (int j = 0; j < SplittedLines.Length; j++)
                                                        {
                                                            exec.Returning_methods(SplittedLines[j]);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Kernel.Clipboard += "\nThe file doesn't exist";
                                                    }
                                                }
                                                else
                                                {
                                                    exec.Returning_methods(lines[i]);
                                                }
                                            }
                                        }
                                        catch
                                        {
                                            Parts = Separate(Code);
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
                                clicked = true;
                            }
                        }
                        else
                        {
                            UI_Elements.Button.Button_render(window, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                        }
                    }

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

                    foreach (var Checkbox in CheckBox)
                    {
                        Checkbox.Render(window);
                    }

                    foreach (var T in Tables)
                    {
                        T.Render(window);
                    }
                    temp = false;
                }
            }

            if (MouseManager.MouseState == MouseState.None)
            {
                clicked = false;
            }

            if (x == 0 && window.Width == ImprovedVBE.width)
            {
                Array.Copy(window.RawData, 0, ImprovedVBE.cover.RawData, y * ImprovedVBE.width, window.RawData.Length);
            }
            else
            {
                ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
            }
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
                                if (int.TryParse(trimmed, out int s))
                                {
                                    if (s > 50)
                                    {
                                        this.width = s;
                                    }
                                }
                            }
                            else if (trimmed.StartsWith("this.Height"))
                            {
                                trimmed = trimmed.Replace("this.Height=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                if (int.TryParse(trimmed, out int s))
                                {
                                    if (s > 50)
                                    {
                                        this.height = s;
                                    }
                                }
                            }
                            else if (trimmed.StartsWith("this.RGB"))
                            {
                                trimmed = trimmed.Replace("this.RGB=", "");
                                trimmed = trimmed.Remove(trimmed.Length - 1);
                                string[] values = trimmed.Split(',');
                                this.CurrentColor = ImprovedVBE.colourToNumber(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));
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
                            else if (trimmed.StartsWith("CheckBox"))
                            {
                                trimmed = trimmed.Replace("CheckBox.NewCheckBox(", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                //separate by =
                                //string[] parts = trimmed.Split('=');
                                //Store the name
                                //string name = parts[0];
                                //Read data from new()
                                string[] values = trimmed.Split(",");

                                CheckBox.Add(new UI_Elements.CheckBox(int.Parse(values[0]), int.Parse(values[1]) + 22, int.Parse(values[2]), int.Parse(values[3]), bool.Parse(values[4]), values[5].Replace("\"", ""), values[6].Remove(values[6].Length - 1).Remove(0, 1)));
                            }
                            else if (trimmed.StartsWith("Table"))
                            {
                                trimmed = trimmed.Replace("Table", "");
                                trimmed = trimmed.Remove(trimmed.Length - 2);
                                //separate by =
                                string[] parts = trimmed.Split('=');
                                //Store the name
                                string name = parts[0];
                                //Read data from new()
                                string[] values = parts[1].Replace("new(", "").Split(",");
                                //foreach(string s in values)
                                //{
                                //    Kernel.Clipboard += s + "\n";
                                //}

                                if(values.Length == 8)
                                {
                                    Tables.Add(new Table(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]), int.Parse(values[3]), int.Parse(values[4]), int.Parse(values[5]), name, int.Parse(values[6]), int.Parse(values[7])));
                                    Tables[^1].Initialize();
                                }
                            }
                        }
                    }
                }
                initial = false;
            }
            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
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

                canvas = ImprovedVBE.EnableTransparencyPreRGB(canvas, x, y, canvas, Color.FromArgb(CurrentColor).R, Color.FromArgb(CurrentColor).G, Color.FromArgb(CurrentColor).B, RenderTo);

                if (HasTitlebar == true)
                {
                    ImprovedVBE.DrawGradientLeftToRight(canvas);

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

                foreach (var Checkbox in CheckBox)
                {
                    Checkbox.Render(window);
                }

                foreach (var T in Tables)
                {
                    T.Render(window);
                }
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
                                temp = true;
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
            
            if (part != 0 && CycleCount > 40)
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
                    foreach(var c in Tables)//For would be more efficient
                    {
                        foreach (var v in c.Cells)
                        {
                            if (v.Selected == true && v.WriteProtected == false)
                            {
                                v.Content = Keyboard.HandleKeyboard(v.Content, key);
                            }
                        }
                    }
                }
                if(MouseManager.MouseState == MouseState.Left)
                {
                    foreach(var v in Tables)
                    {
                        v.Select2((int)MouseManager.X - x - v.X, (int)MouseManager.Y - y - v.Y);
                    }
                }
            }
            ImprovedVBE.DrawImageAlpha(window, x, y, RenderTo);
        }

        public List<string> Separate(string In)
        {
            List<string> parts = new List<string>();
            parts = CodeGenerator.ToList(In.Split("\n#"));
            for(int i = 0; i < parts.Count; i++)
            {
                if(i != 0)
                {
                    parts[i] = parts[i].Insert(0, "\n#");
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