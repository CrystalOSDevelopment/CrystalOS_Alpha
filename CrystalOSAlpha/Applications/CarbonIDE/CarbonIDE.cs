using Cosmos.HAL.Drivers.Video.SVGAII;
using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Programming;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.SymbolStore;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;

namespace CrystalOSAlpha.Applications.CarbonIDE
{
    class CarbonIDE : App
    {
        #region important
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

        public string TestCode = "string name = Console.ReadLine();\nif(name == \"John\")\n{\n    Console.WriteLine(\"Hi\");\n}\nelse if(name == \"Doe\")\n{\n    Console.WriteLine(\"Hello\");\n}\nelse if(name == \"Carol\")\n{\n    Console.WriteLine(\"Who even are you?\");\n}\nelse\n{\n    Console.WriteLine(\"No message for you!\");\n}\nfor(int i = 5; i < 15; i++)\n{\n    Console.WriteLine(i + \". Cycle\");\n}\nConsole.WriteLine(\"End of Program\");\nfor(int i = 0; i < 10; i++)\n{\n    Console.WriteLine(i + \". Cycle\");\n}\nConsole.WriteLine(\"End of Program\");";
        //public string content = "Console.WriteLine(\"Guess a number game!\");\nint num = Random.Next(0, 11);\nConsole.WriteLine(num);";
        public string Game = "Console.WriteLine(\"Guess a number game!\");\nbool match = false;\nint num = Random.Next(0, 11);\nConsole.WriteLine(\"You have 3 guesses!\");\nfor(int i = 0; i < 3; i++)\n{\n    Console.Write(i + \". Guess\");\n    int guessed = Console.ReadLine();\n    if(guessed == num)\n    {\n        Console.WriteLine(\"That's Correct\");\n        bool match = true;\n    }\n    else if(guessed > num)\n    {\n        Console.WriteLine(\"Guessed is bigger\");\n    }\n    else\n    {\n        Console.WriteLine(\"Guessed is smaller\");\n    }\n}\nConsole.WriteLine(\"The correct number was: \" + num);";
        public string TestWhile = "Console.Write(\"Start while loop? (y, n): \");\nstring input = Console.ReadLine();\nwhile(input == \"y\")\n{\n    for(int i = 0; i < 3; i++)\n    {\n        Console.WriteLine(\"Test\");\n    }\n    Console.Write(\"end of cycle, continue?(y, n): \");\n    string input = Console.ReadLine();\n}";
        public string Keyboard_Test = "ReadKey key;\nReadKey key2;\nif(key == key2)\n{\n    Console.WriteLine(\"Two keys are matching!\");\n}\nif(key == ConsoleKeyEx.U)\n{\n    Console.WriteLine(\"You pressed the Escape key!\");\n}";
        public string Window1 =
            "#Define Window_Main\n" +
            "{\n" +
            "    this.RGB = 255, 52, 126;\n" +
            "    this.Title = \"Demo title\";\n" +
            "    this.Width = 400;\n" +
            "    this.Height = 300;\n" +
            "    this.Titlebar = true;\n" +
            "    this.X = 500;\n" +
            "    this.Y = 500;\n" +
            "    this.AlwaysOnTop = true;\n" +
            "    Label test = new Label(10, 10, \"Hello World\", 255, 255, 255);\n" +
            "    Button btn = new Button(10, 30, 110, 25, \"Hello World\", 1, 1, 1);\n" +
            "    Slider slider = new Slider(10, 80, 255, 20);\n" +
            "}" +
            "\n" +
            "#OnClick btn\n" +
            "{\n" +
            "    if(10 == 10)\n" +
            "    {\n" +
            "        test.Content = \"New message\";\n" +
            "        test.Color = 1, 1, 1;\n" +
            "        test.X = 110;\n" +
            "        test.Y = 60;\n" +
            "        \n" +
            "        btn.Width = 90;\n" +
            "    }\n" +
            "}";

        public string Back_content = "";
        public string content = "";
        //public string content = "File.Create(\"0:\\\", \"Sample.app\");\nFile.WriteAllText(\"0:\\Sample.app\", \"Console.WriteLine(\\\"Hello, World!\\\");\");";
        //public string content = "";
        public string source = "";

        public bool temp = true;

        public int Reg_Y = 0;

        public Bitmap Container;
        public Bitmap Strucrure;

        public List<Dropdown> dropdowns = new List<Dropdown>();
        public List<values> value = new List<values>();

        public List<CSharpFile> FileTree = new List<CSharpFile>();

        public int memory = 0;

        public string lineCount = "";

        public int CurTop = 0;
        public int CurLeft = 0;

        public void App()
        {
            if (initial == true)
            {
                Buttons.Add(new Button_prop(5, 27, 60, 20, "File", 1));

                Dropdown d = new Dropdown();
                d.X = 72;
                d.Y = 27;
                d.Width = 115;
                d.Height = 20;
                d.ID = "Type";
                dropdowns.Add(d);

                value.Add(new values(false, "C# console", "Type"));
                value.Add(new values(true, "C# GUI", "Type"));

                Buttons.Add(new Button_prop(195, 27, 60, 20, "Run", 1));

                Scroll.Add(new Scrollbar_Values(width - 347, 30, 20, height - 60, 0));

                Scroll.Add(new Scrollbar_Values(width - 25, 30, 20, height - 267, 0));

                //Starting to init line counting
                for(int i = 0; i < 278; i++)
                {
                    if (i.ToString().Length == 1)
                    {
                        lineCount += (i + 1) + "  \n";
                    }
                    else if(i.ToString().Length == 2)
                    {
                        lineCount += (i + 1) + " \n";
                    }
                    else
                    {
                        lineCount += (i + 1) + "\n";
                    }
                }

                FileTree.Add(new CSharpFile("Tests.cs", TestCode));
                FileTree.Add(new CSharpFile("Game.cs", Game));
                FileTree.Add(new CSharpFile("WhileLoops.cs", TestWhile));
                FileTree.Add(new CSharpFile("Keyboard.cs", Keyboard_Test));
                FileTree.Add(new CSharpFile("Window_Demo.cs", Window1));

                initial = false;
            }
            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32); //new int[width * height];
                back_canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32); //new int[width * height];
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                Container = new Bitmap((uint)(width - 332), (uint)(height - 60), ColorDepth.ColorDepth32);//(width - 356), (uint)(height - 60)
                Strucrure = new Bitmap((uint)(314), (uint)height - 267, ColorDepth.ColorDepth32);

                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(60, 60, 60));
                Array.Fill(Strucrure.RawData, ImprovedVBE.colourToNumber(60, 60, 60));

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

                //Button.Button_render(canvas, 10, 70, 100, 25, 1, "Click");

                canvas = Scrollbar.Render(canvas, Scroll[0]);
                canvas = Scrollbar.Render(canvas, Scroll[1]);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                //window.RawData = canvas.RawData;
                back_canvas = canvas;
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
                foreach(var button in Buttons)
                {
                    button.Clicked = false;
                }
                temp = true;
                clicked = false;
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
                                Reg_Y = (int)MouseManager.Y - y - scv.y - 22 - scv.Pos;
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
                if (MouseManager.Y > y + scv.y + 48 && MouseManager.Y < y + scv.y + scv.Height - 22 && scv.Clicked == true)
                {
                    if (scv.Pos >= 0 && scv.Pos <= scv.Height - 24)
                    {
                        scv.Pos = (int)MouseManager.Y - y - scv.y - 22 - Reg_Y;
                    }
                    else
                    {
                        if (scv.Pos < 0)
                        {
                            scv.Pos = 1;
                        }
                        else
                        {
                            scv.Pos = scv.Height - 24;
                        }
                    }
                }
            }

            if (TaskScheduler.counter == TaskScheduler.Apps.Count - 1)
            {
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    Back_content = content;
                    string[] lines = Back_content.Split("\n");
                    if(key.Key == ConsoleKeyEx.DownArrow)
                    {
                        if(CurTop < lines.Length - 1)
                        {
                            CurTop++;
                            lines[CurTop] = lines[CurTop] + "|";

                            Back_content = "";

                            for(int i = 0; i < lines.Length; i++)
                            {
                                if(i == lines.Length - 1)
                                {
                                    Back_content += lines[i];
                                }
                                else
                                {
                                    Back_content += lines[i] + "\n";
                                }
                            }
                        }
                        else
                        {
                            lines[CurTop] = lines[CurTop] + "|";

                            Back_content = "";

                            for (int i = 0; i < lines.Length; i++)
                            {
                                if (i == lines.Length - 1)
                                {
                                    Back_content += lines[i];
                                }
                                else
                                {
                                    Back_content += lines[i] + "\n";
                                }
                            }
                        }
                    }
                    else if (key.Key == ConsoleKeyEx.F5)
                    {
                        if (value[0].Highlighted == true)
                        {
                            CSharp c = new CSharp();
                            c.Executor(content);
                        }
                        if (value[1].Highlighted == true)
                        {
                            TaskScheduler.Apps.Add(new Window(100, 100, 999, 350, 200, 0, "Later", false, icon, content));
                        }
                    }
                    else if(KeyboardManager.ControlPressed == true)
                    {
                        if (key.Key == ConsoleKeyEx.S)
                        {
                            //save file
                        }
                    }
                    else if (key.Key == ConsoleKeyEx.UpArrow)
                    {
                        if (CurTop >= 1)
                        {
                            CurTop--;
                            lines[CurTop] = lines[CurTop] + "|";

                            Back_content = "";

                            for (int i = 0; i < lines.Length; i++)
                            {
                                if (i == lines.Length - 1)
                                {
                                    Back_content += lines[i];
                                }
                                else
                                {
                                    Back_content += lines[i] + "\n";
                                }
                            }
                        }
                        else
                        {
                            lines[CurTop] = lines[CurTop] + "|";

                            Back_content = "";

                            for (int i = 0; i < lines.Length; i++)
                            {
                                if (i == lines.Length - 1)
                                {
                                    Back_content += lines[i];
                                }
                                else
                                {
                                    Back_content += lines[i] + "\n";
                                }
                            }
                        }
                    }
                    else
                    {
                        int length = content.Length;
                        content = Keyboard.HandleKeyboard(content, key);
                        int length2 = content.Length;
                        Back_content = content;

                        lines = Back_content.Split("\n");

                        lines[CurTop] = lines[CurTop] + "|";

                        Back_content = "";

                        for (int i = 0; i < lines.Length; i++)
                        {
                            if (i == lines.Length - 1)
                            {
                                Back_content += lines[i];
                            }
                            else
                            {
                                Back_content += lines[i] + "\n";
                            }
                        }
                    }
                    Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                    temp = true;
                }
            }

            if (temp == true)
            {
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                
                #region Code container
                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(Container, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Container.Width - 4, (int)Container.Height - 4, false);
                ImprovedVBE.DrawFilledRectangle(Container, ImprovedVBE.colourToNumber(69, 69, 69), 2, 2, 35, (int)Container.Height - 4, false);
                #endregion Code container

                #region FileTree
                Array.Fill(Strucrure.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                ImprovedVBE.DrawFilledRectangle(Strucrure, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Strucrure.Width - 4, (int)Strucrure.Height - 4, false);

                int top = 15;
                int left = 15;

                BitFont.DrawBitFontString(Strucrure, "ArialCustomCharset16", Color.White, "Solution explorer", 15, top - 4);//The title
                ImprovedVBE.DrawFilledRectangle(Strucrure, ImprovedVBE.colourToNumber(90, 90, 90), 7, top + 20, (int)Strucrure.Width - 34, 2, false);

                top += 28;

                foreach(var v in FileTree)
                {
                    v.List_Y = top;
                    v.List_X = left;
                    if (v.selected)
                    {
                        ImprovedVBE.DrawFilledRectangle(Strucrure, ImprovedVBE.colourToNumber(20, 20, 20), left - 10, top - 3, (int)(Strucrure.Width - 28), 23, false);
                        ImprovedVBE.DrawFilledRectangle(Strucrure, ImprovedVBE.colourToNumber(90, 90, 90), left - 8, top - 1, (int)(Strucrure.Width - 32), 19, false);
                    }
                    BitFont.DrawBitFontString(Strucrure, "ArialCustomCharset16", Color.White, v.Name, left, top - Scroll[1].Pos);//Filename
                    top += 25;
                }

                #endregion FileTree

                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        Button.Button_render(window, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);

                        switch (button.Text)
                        {
                            case "File":
                                
                                break;
                            case "Run":
                                //BitFont.DrawBitFontString(window, "ArialCustomCharset16", Color.White, CSharp.Executor(content), 500, 500);
                                if (value[0].Highlighted == true)
                                {
                                    CSharp c = new CSharp();
                                    c.Executor(content);
                                }
                                if (value[1].Highlighted == true)
                                {
                                    TaskScheduler.Apps.Add(new Window(100, 100, 999, 350, 200, 0, "Later", false, icon, content));
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

                BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.Black, lineCount, 7, 7 - (Scroll[0].Pos * 4));//The line counter

                BitFont.DrawBitFontString(Container, "ArialCustomCharset16", HighLight(Back_content), Back_content, 42, 7 - (Scroll[0].Pos * 4));//The actual code

                ImprovedVBE.DrawImageAlpha(Container, 5, 52, window);
                ImprovedVBE.DrawImageAlpha(Strucrure, width - 319, 52, window);

                window = Scrollbar.Render(window, Scroll[0]);
                window = Scrollbar.Render(window, Scroll[1]);

                temp = false;
            }

            if(MouseManager.MouseState == MouseState.Left)
            {
                foreach(var v in FileTree)
                {
                    if(MouseManager.X > x + width - 319 + v.List_X && MouseManager.X < x + width - 10)
                    {
                        if (MouseManager.Y > y + 52 + v.List_Y && MouseManager.Y < y + 52 + v.List_Y + 22)
                        {
                            foreach(var v2 in FileTree)
                            {
                                v2.selected = false;
                            }
                            content = v.Content;
                            Back_content = v.Content;
                            v.selected = true;
                            temp = true;
                        }
                    }
                }
            }
            //ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
            Array.Copy(window.RawData, 0, ImprovedVBE.cover.RawData, 0, window.RawData.Length);
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
                            temp = true;
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
                                temp = true;
                            }
                        }
                    }
                }
                Dropd.Render(x, y);
            }
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
            for (int i = 0; counter < index || i < source.Length; i++)
            {
                if (source[i] == c)
                {
                    counter++;
                }
                index_out = i;
            }
            return index_out;
        }

        public Color[] HighLight(string source)
        {
            Color[] colors = new Color[source.Length];
            Array.Fill(colors, Color.White);

            string Extra = "";
            int index = 0;
            bool was_String = false;
            string state = "Ended";

            int qCount = 0;

            bool comment = false;
            int sepCounter = 0;
            foreach(char c in source)
            {
                if(state == "Ended")
                {
                    if(c == '\n')
                    {
                        comment = false;
                    }
                    if (c == '.' || c == '\n' || c == ' ' || c == '=' || c == '{' || c == '}' || c == '+' || c == '-' || c == ',' || c == '|')
                    {
                        if(comment == false)
                        {
                            Extra = "";
                        }
                    }
                    else
                    {
                        Extra += c;
                        if (c == '\"')
                        {
                            state = "Started";
                        }
                    }
                }
                else if(state == "Started")
                {
                    Extra += c;
                    if (c == '\"' && !Extra.EndsWith("\\\"") && Extra.Count(t => t == '\"') % 2 == 0)
                    {
                        state = "Ended";
                    }
                }
                
                switch(Extra)
                {
                    case "Console":
                        for(int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Green;
                        }
                        //Extra = "";
                        break;
                    case "Math":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Green;
                        }
                        Extra = "";
                        break;
                    case "Random":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Green;
                        }
                        Extra = "";
                        break;
                    case "File":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Green;
                        }
                        Extra = "";
                        break;
                    case "ReadKey":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.LightGreen;
                        }
                        Extra = "";
                        break;
                    case "ConsoleKeyEx":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.LightGreen;
                        }
                        Extra = "";
                        break;
                    case "int":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "string":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "bool":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "float":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "double":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "public":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "static":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "void":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "namespace":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "class":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "new":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "true":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "false":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "this":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.Blue;
                        }
                        Extra = "";
                        break;
                    case "if(":
                        for (int i = index - Extra.Length + 1; i < index; i++)
                        {
                            colors[i] = Color.DeepPink;
                        }
                        Extra = "";
                        break;
                    case "for(":
                        for (int i = index - Extra.Length + 1; i < index; i++)
                        {
                            colors[i] = Color.DeepPink;
                        }
                        Extra = "";
                        break;
                    case "while(":
                        for (int i = index - Extra.Length + 1; i < index; i++)
                        {
                            colors[i] = Color.DeepPink;
                        }
                        Extra = "";
                        break;
                    case "else":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.DeepPink;
                        }
                        Extra = "";
                        break;
                    //For UI Elements
                    case "Label":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.DarkSalmon;
                        }
                        Extra = "";
                        break;
                    case "Button":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.DarkSalmon;
                        }
                        Extra = "";
                        break;
                    case "Slider":
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.DarkSalmon;
                        }
                        Extra = "";
                        break;
                    default:
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.LightBlue;
                        }
                        break;
                }
                if(Extra.Length > 2)
                {
                    if (Extra.EndsWith("(") && state == "Ended")
                    {
                        for (int i = index - Extra.Length + 1; i < index; i++)
                        {
                            colors[i] = Color.Yellow;
                        }
                        Extra = "";
                    }
                    if (Extra.StartsWith("//"))
                    {
                        for (int i = index - Extra.Length + 1; i <= index; i++)
                        {
                            colors[i] = Color.LightGreen;
                        }
                        comment = true;
                    }
                }
                switch (Extra[^1])
                {
                    case '\"':
                        qCount++;
                        if (qCount % 2 == 0)
                        {
                            for (int i = index - Extra.Length + 1; i <= index; i++)
                            {
                                colors[i] = Color.Orange;
                            }
                            Extra = "";
                            was_String = true;
                        }
                        break;
                }
                if(c == '\n')
                {
                    
                }
                else
                {
                    index++;
                }
                sepCounter++;
            }
            return colors;
        }
    }

    class CSharpFile
    {
        public string Name { get; set; }
        public string Content { get; set; }
        public int List_X { get; set; }
        public int List_Y { get; set; }
        public bool selected { get; set; }

        public CSharpFile(string name, string content)
        {
            Name = name;
            Content = content;
        }
    }
}
