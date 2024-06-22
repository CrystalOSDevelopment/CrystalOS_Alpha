using Cosmos.System;
using Cosmos.System.FileSystem.Listing;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications.Terminal;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Programming.CrystalSharp;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using Kernel = CrystalOS_Alpha.Kernel;

namespace CrystalOSAlpha.Applications.CarbonIDE
{
    class CarbonIDE : App
    {
        public CarbonIDE() { }
        public CarbonIDE(int X, int Y, int Z, int Width, int Height, string Name, Bitmap Icon)
        {
            this.x = X;
            this.y = Y;
            this.z = Z;
            this.width = Width;
            this.height = Height;
            this.name = Name;
            this.icon = Icon;
        }

        #region important
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
        #endregion important

        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);

        public bool once { get; set; }
        public bool temp = true;
        public bool initial = true;
        public bool clicked = false;

        public string content = "";
        public string Buffered_Content = "";
        public string Path = "";

        public int CursorX = 0;
        public int CursorY = 0;

        public Bitmap window { get; set; }
        public Bitmap canvas;
        public Bitmap back_canvas;
        public Bitmap CodeContainer;
        public Bitmap Filetree;

        public List<UIElementHandler> UIElements = new List<UIElementHandler>();
        public List<CSharpFile> Files = new List<CSharpFile>();

        public void App()
        {
            if (initial == true)
            {
                if (!File.Exists(Path + "\\Extension.cs"))
                {
                    File.WriteAllText(Path + "\\Extension.cs",
                        "class Extension\n" +
                        "{\n" +
                        "    public static void TestVoid()\n" +
                        "    {\n" +
                        "        Console.WriteLine(\"Hello from TestVoid()\");\n" +
                        "    }\n" +
                        "    \n" +
                        "    public static void Testvoid2()\n" +
                        "    {\n" +
                        "        Console.WriteLine(\"Hello from Testvoid2()\");\n" +
                        "    }\n" +
                        "}");
                }
                if (!File.Exists(Path + "\\Main.cs"))
                {
                    #region TestCode
                    File.WriteAllText(Path + "\\Main.cs",
                        "class Demo\n" +
                        "{\n" +
                        "    public static void Main()\n" +
                        "    {\n" +
                        "        Console.WriteLine(\"Hello from Main()\");\n" +
                        "        Console.WriteLine(\"This is a test message\");\n" +
                        "        Console.WriteLine(\"Extra line\");\n" +
                        "        Console.Write(\"Extension: \");\n" +
                        "        Console.ReadLine();\n" +
                        "        Console.Clear();\n" +
                        "        Console.WriteLine(\"This line is visible after Clear()\");\n" +
                        "        string A = \"Hello\";\n" +
                        "        int B = 18;\n" +
                        "        bool C = true;\n" +
                        "        bool D = false;\n" +
                        "        float E = 1.5;\n" +
                        "        double F = 1.2221;\n" +
                        "        char G = 'A';\n" +
                        "        A = Math.Abs(-5 + 2);\n" +
                        "        D = false;\n" +
                        "        if(B == 18 && C)\n" +
                        "        {\n" +
                        "            Console.WriteLine(\"Condition B == 18 && C is true\");\n" +
                        "        }\n" +
                        "        else\n" +
                        "        {\n" +
                        "            Console.WriteLine(\"This shouldn't run\");\n" +
                        "        }\n" +
                        "        if(B != 18 && D == true)\n" +
                        "        {\n" +
                        "            Console.WriteLine(\"Condition B != 18 || D is false\");\n" +
                        "        }\n" +
                        "        else if(B == 18)\n" +
                        "        {\n" +
                        "            Console.WriteLine(\"From elseif!\");\n" +
                        "        }\n" +
                        "        else\n" +
                        "        {\n" +
                        "            Console.WriteLine(\"This should run\");\n" +
                        "        }\n" +
                        "        for(int i = 0; i < 10; i++)\n" +
                        "        {\n" +
                        "            Console.WriteLine(\"1\");\n" +
                        "        }\n" +
                        "        Random.Next(5, 11);\n" +
                        "        B += Console.ReadLine();\n" +
                        "        Console.WriteLine(\"Value of B: \" + B);\n" +
                        "        B *= 10;\n" +
                        "        Console.WriteLine(\"Value of B after multiply by 10: \" + B);\n" +
                        "    }\n" +
                        "    \n" +
                        "    public static void ExtraVoid()\n" +
                        "    {\n" +
                        "        Console.WriteLine(\"Hello from ExtraVoid()\");\n" +
                        "        Console.WriteLine(\"This shouldn't be executed!\");\n" +
                        "    }\n" +
                        "}");
                    #endregion TestCode
                    content = File.ReadAllText(Path + "\\Main.cs");
                    Buffered_Content = content;
                }
                else
                {
                    content = File.ReadAllText(Path + "\\Main.cs");
                    Buffered_Content = content;
                }

                #region Top bar
                UIElements.Add(new Button(5, 6, 60, 20, "New", 1, "New"));
                UIElements.Add(new Dropdown(72, 28, 115, 20, "CompileType", 
                    new List<values>
                    {
                        new values(true, "GUI", "CompileType"),
                        new values(false, "Terminal", "CompileType")
                    }));
                UIElements.Add(new Button(195, 6, 60, 20, "Run", 1, "Run"));
                UIElements.Add(new Button(270, 6, 60, 20, "Save", 1, "Save"));
                #endregion Top bar

                #region Code container
                UIElements.Add(new VerticalScrollbar(width - 348, 55, 20, height - 139, 20, 0, 1000, "TextScroll"));
                #endregion Code container

                #region FileTree
                UIElements.Add(new VerticalScrollbar(width - 26, 55, 20, 734, 20, 0, 1000, "FileTree"));
                #endregion FileTree
                name += " - " + Path;

                initial = false;
            }
            if (once == true)
            {
                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                CodeContainer = new Bitmap((uint)width - 331, (uint)height - 135, ColorDepth.ColorDepth32);
                Array.Fill(CodeContainer.RawData, ImprovedVBE.colourToNumber(200, 200, 200));
                ImprovedVBE.DrawFilledRectangle(CodeContainer, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)CodeContainer.Width - 4, (int)CodeContainer.Height - 4);
                ImprovedVBE.DrawImage(CodeContainer, 5, 53, canvas);

                Filetree = new Bitmap(314, 738, ColorDepth.ColorDepth32);
                Array.Fill(Filetree.RawData, ImprovedVBE.colourToNumber(200, 200, 200));
                ImprovedVBE.DrawFilledRectangle(Filetree, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Filetree.Width - 4, (int)Filetree.Height - 4);
                ImprovedVBE.DrawImage(Filetree, width - 318, 53, canvas);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                once = false;
            }

            if (TaskScheduler.counter == TaskScheduler.Apps.Count - 1)
            {
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    (content, Buffered_Content, CursorX, CursorY) = CoreEditor.Editor(content, Buffered_Content, CursorX, CursorY, key);

                    //UIElements.Find(d => d.ID == "TextScroll").MaxVal = content.Split('\n').Length * 16 - (int)CodeContainer.Height;

                    Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                    //Problem that needs to be solved possibly before release: When going down with arrow key, it should go all the way to the bottom first and not just jump there
                    //Possible fix: IDK honestly...
                    switch (key.Key)
                    {
                        case ConsoleKeyEx.DownArrow:
                            if (CursorY * 16 >= CodeContainer.Height - 10)
                            {
                                UIElements.Find(d => d.ID == "TextScroll").Value = Math.Clamp((CursorY - (int)CodeContainer.Height / 17) * 16, UIElements.Find(d => d.ID == "TextScroll").MinVal, UIElements.Find(d => d.ID == "TextScroll").MaxVal);
                                UIElements.Find(d => d.ID == "TextScroll").Pos = (int)(UIElements.Find(d => d.ID == "TextScroll").Value / UIElements.Find(d => d.ID == "TextScroll").Sensitivity) + 20;
                            }
                            break;
                        case ConsoleKeyEx.UpArrow:
                            if (CursorY * 16 < UIElements.Find(d => d.ID == "TextScroll").Value)
                            {
                                UIElements.Find(d => d.ID == "TextScroll").Value = Math.Clamp(CursorY * 16, UIElements.Find(d => d.ID == "TextScroll").MinVal, UIElements.Find(d => d.ID == "TextScroll").MaxVal);
                                UIElements.Find(d => d.ID == "TextScroll").Pos = (int)(UIElements.Find(d => d.ID == "TextScroll").Value / UIElements.Find(d => d.ID == "TextScroll").Sensitivity) + 20;
                            }
                            break;
                        case ConsoleKeyEx.Enter:
                            if (CursorY * 16 >= CodeContainer.Height - 10)
                            {
                                UIElements.Find(d => d.ID == "TextScroll").Value = Math.Clamp(CursorY * 16, UIElements.Find(d => d.ID == "TextScroll").MinVal, UIElements.Find(d => d.ID == "TextScroll").MaxVal);
                                UIElements.Find(d => d.ID == "TextScroll").Pos = (int)(UIElements.Find(d => d.ID == "TextScroll").Value / UIElements.Find(d => d.ID == "TextScroll").Sensitivity) + 20;
                            }
                            break;
                    }
                    temp = true;
                }
                if(MouseManager.MouseState == MouseState.Left && clicked == false) 
                {
                    if (MouseManager.X > width - 318 && MouseManager.X < width - 318 + Filetree.Width)
                    {
                        if (MouseManager.Y > y + 53 && MouseManager.Y < y + 53 + Filetree.Height)
                        {
                            temp = true;
                            clicked = true;
                        }
                    }
                }
                else
                {
                    clicked = false;
                }
            }

            if (temp == true)
            {
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);

                #region CodeContainer
                Array.Fill(CodeContainer.RawData, ImprovedVBE.colourToNumber(200, 200, 200));
                ImprovedVBE.DrawFilledRectangle(CodeContainer, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)CodeContainer.Width - 4, (int)CodeContainer.Height - 4);
                ImprovedVBE.DrawFilledRectangle(CodeContainer, ImprovedVBE.colourToNumber(90, 90, 90), 2, 2, 25, (int)CodeContainer.Height - 4);
                int StartY = UIElements.Find(d => d.ID == "TextScroll").Value;

                //TODO: Cache the code so it doesn't have to be re-rendered every frame
                if (UIElements.Find(d => d.ID == "TextScroll").Value > 0)
                {
                    for (int Top = 0; Top < Buffered_Content.Split('\n').Length; Top++)
                    {
                        BitFont.DrawBitFontString(CodeContainer, "ArialCustomCharset16", Color.White, (Top + 1).ToString(), 5, 0 - UIElements.Find(d => d.ID == "TextScroll").Value + Top * 16);
                    }

                    BitFont.DrawBitFontString(CodeContainer, "ArialCustomCharset16", HighLight(Buffered_Content), Buffered_Content, 30, 0 - UIElements.Find(d => d.ID == "TextScroll").Value);
                }
                else
                {
                    for (int Top = 0; Top < Buffered_Content.Split('\n').Length; Top++)
                    {
                        BitFont.DrawBitFontString(CodeContainer, "ArialCustomCharset16", Color.White, (Top + 1).ToString(), 5, 5 - UIElements.Find(d => d.ID == "TextScroll").Value + Top * 16);
                    }

                    BitFont.DrawBitFontString(CodeContainer, "ArialCustomCharset16", HighLight(Buffered_Content), Buffered_Content, 30, 5 - UIElements.Find(d => d.ID == "TextScroll").Value);
                }

                ImprovedVBE.DrawImage(CodeContainer, 5, 53, window);

                #endregion CodeContainer

                #region FileTree
                Filetree = new Bitmap(314, 738, ColorDepth.ColorDepth32);
                Array.Fill(Filetree.RawData, ImprovedVBE.colourToNumber(200, 200, 200));
                ImprovedVBE.DrawFilledRectangle(Filetree, ImprovedVBE.colourToNumber(50, 50, 50), 2, 2, (int)Filetree.Width - 4, (int)Filetree.Height - 4);

                Bitmap Title = new Bitmap(Filetree.Width - 24, 35, ColorDepth.ColorDepth32);
                Array.Fill(Title.RawData, ImprovedVBE.colourToNumber(50, 50, 50));
                ImprovedVBE.DrawFilledRectangle(Title, ImprovedVBE.colourToNumber(200, 200, 200), 5, (int)Title.Height - 3, (int)Title.Width - 10, 2);
                BitFont.DrawBitFontString(Title, "VerdanaCustomCharset24", Color.White, Path.Split('\\')[^1], 2, 2);

                var Slider = UIElements.Find(d => d.ID == "FileTree");
                int TopY = 0;
                if(Files.Count == 0)
                {
                    foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(Path))
                    {
                        Files.Add(new CSharpFile(d));
                        if (d.mName.Contains("Main"))
                        {
                            Files[^1].selected = true;
                        }
                    }
                }
                foreach (CSharpFile d in Files)
                {
                    if (!d.Name.mName.EndsWith("sln") && !d.Name.mName.EndsWith("cmd"))
                    {
                        if(MouseManager.MouseState == MouseState.Left)
                        {
                            if(MouseManager.X > width - 318 && MouseManager.X < width - 318 + Filetree.Width)
                            {
                                if (MouseManager.Y > y + 53 + 40 + TopY * 26 && MouseManager.Y < y + 53 + 40 + TopY * 26 + 26)
                                {
                                    foreach (CSharpFile f in Files)
                                    {
                                        if (f.selected == true)
                                        {
                                            File.WriteAllText(f.Name.mFullPath, content);
                                            f.selected = false;
                                        }
                                    }
                                    d.selected = true;
                                    content = File.ReadAllText(d.Name.mFullPath);
                                    Buffered_Content = content;
                                }
                            }
                        }
                        if(d.selected == true)
                        {
                            ImprovedVBE.DrawFilledRectangle(Filetree, ImprovedVBE.colourToNumber(100, 100, 100), 2, 40 - Slider.Value + TopY * 26, (int)Filetree.Width - 4, 31);
                        }
                        BitFont.DrawBitFontString(Filetree, "ArialCustomCharset16", Color.White, d.Name.mName, 15, 45 - Slider.Value + TopY * 26);
                        TopY++;
                    }
                }
                ImprovedVBE.DrawImage(Title, 2, 2, Filetree);

                ImprovedVBE.DrawImage(Filetree, width - 318, 53, window);
                #endregion FileTree
                temp = false;
            }

            foreach (var element in UIElements)
            {
                if(element.TypeOfElement == TypeOfElement.DropDown)
                {
                    bool StoreClick = element.Clicked;
                    element.CheckClick(x, y);
                    if (StoreClick != element.Clicked)
                    {
                        temp = true;
                    }
                }
                element.Render(window);
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > x + element.X && MouseManager.X < x + element.X + element.Width)
                    {
                        if (MouseManager.Y > y + element.Y && MouseManager.Y < y + element.Y + element.Height)
                        {
                            switch (element.TypeOfElement)
                            {
                                case TypeOfElement.Button:
                                    int Col = element.Color;
                                    element.Color = Color.White.ToArgb();
                                    element.Render(window);
                                    element.Color = Col;
                                    if (element.Clicked == false)
                                    {
                                        switch (element.ID)
                                        {
                                            case "Run":
                                                //Take the file(s) and make it segmented into fast accessable chunks
                                                List<string> list = new List<string>();
                                                foreach (DirectoryEntry d in Kernel.fs.GetDirectoryListing(Path))
                                                {
                                                    if (d.mName.EndsWith("cs"))
                                                    {
                                                        list.Add(d.mFullPath);
                                                    }
                                                }
                                                var Assembled = CodeAssembler.AssembleCode(list);

                                                TaskScheduler.Apps.Add(new Terminal.Terminal(100, 100, 999, 500, 350, Path.Split("\\")[^1] + ".cmd", Resources.Terminal, TypeOfTerminal.Executable, Assembled));
                                                break;
                                        }
                                    }
                                    break;
                                case TypeOfElement.DropDown:
                                    element.CheckClick(x, y);
                                    break;
                            }
                            element.Clicked = true;
                        }
                    }
                }
                else if(element.TypeOfElement == TypeOfElement.Button && MouseManager.MouseState == MouseState.None)
                {
                    element.Clicked = false;
                }
                if (element.TypeOfElement == TypeOfElement.VerticalScrollbar)
                {
                    if (element.CheckClick((int)MouseManager.X - x, (int)MouseManager.Y - y))
                    {
                        temp = true;
                    }
                }
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

        private static readonly Dictionary<string, Color> keywordColors = new Dictionary<string, Color>
    {
        {"Console", Color.Green},
        {"Math", Color.Green},
        {"Random", Color.Green},
        {"File", Color.Green},
        {"ReadKey", Color.LightGreen},
        {"ConsoleKeyEx", Color.LightGreen},
        {"int", Color.Blue},
        {"string", Color.Blue},
        {"bool", Color.Blue},
        {"float", Color.Blue},
        {"double", Color.Blue},
        {"public", Color.Blue},
        {"static", Color.Blue},
        {"void", Color.Blue},
        {"namespace", Color.Blue},
        {"class", Color.Blue},
        {"new", Color.Blue},
        {"true", Color.Blue},
        {"false", Color.Blue},
        {"this", Color.Blue},
        {"if", Color.DeepPink},
        {"for", Color.DeepPink},
        {"while", Color.DeepPink},
        {"else", Color.DeepPink},
        {"Label", Color.DarkSalmon},
        {"Button", Color.DarkSalmon},
        {"Slider", Color.DarkSalmon},
        {"TextBox", Color.DarkSalmon}
    };

        public Color[] HighLight(string source)
        {
            Color[] colors = new Color[source.Length];
            Array.Fill(colors, Color.White);

            StringBuilder extra = new StringBuilder();
            string state = "Ended";
            bool comment = false;
            int index = 0;

            for (int i = 0; i < source.Length; i++)
            {
                char c = source[i];

                if (state == "Ended")
                {
                    if (c == '\n')
                    {
                        comment = false;
                    }
                    if (".\n ={}+-|,".Contains(c))
                    {
                        if (!comment)
                        {
                            extra.Clear();
                        }
                    }
                    else
                    {
                        extra.Append(c);
                        if (c == '\"')
                        {
                            state = "Started";
                        }
                    }
                }
                else if (state == "Started")
                {
                    extra.Append(c);
                    if (c == '\"' && !extra.ToString().EndsWith("\\\"") && extra.ToString().Count(t => t == '\"') % 2 == 0)
                    {
                        state = "Ended";
                    }
                }

                string extraString = extra.ToString();

                if (keywordColors.ContainsKey(extraString))
                {
                    Color color = keywordColors[extraString];
                    for (int j = index - extraString.Length + 1; j <= index; j++)
                    {
                        colors[j] = color;
                    }
                    extra.Clear();
                }
                else if (extraString.Length > 2)
                {
                    if (extraString.EndsWith("(") && state == "Ended")
                    {
                        for (int j = index - extraString.Length + 1; j < index; j++)
                        {
                            colors[j] = Color.Yellow;
                        }
                        extra.Clear();
                    }
                    if (extraString.StartsWith("//"))
                    {
                        for (int j = index - extraString.Length + 1; j <= index; j++)
                        {
                            colors[j] = Color.LightGreen;
                        }
                        comment = true;
                    }
                }

                if (extraString.EndsWith("\""))
                {
                    int quoteCount = extraString.Count(ch => ch == '\"');
                    if (quoteCount % 2 == 0)
                    {
                        for (int j = index - extraString.Length + 1; j <= index; j++)
                        {
                            colors[j] = Color.Orange;
                        }
                        extra.Clear();
                    }
                }

                if (c != '\n')
                {
                    index++;
                }
            }
            return colors;
        }
    }

    class CSharpFile
    {
        public DirectoryEntry Name { get; set; }
        public bool selected { get; set; }

        public CSharpFile(DirectoryEntry d)
        {
            Name = d;
            selected = false;
        }
    }
}