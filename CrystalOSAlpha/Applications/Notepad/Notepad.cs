using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOS_Alpha.Graphics.Widgets;
using CrystalOSAlpha.Applications.CarbonIDE;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;

namespace CrystalOSAlpha.Applications.Notepad
{
    class Notepad : App
    {
        public Notepad(int X, int Y, int Z, int Width, int Height, string Name, Bitmap Icon)
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

        public int desk_ID {get; set; }
        public int AppID { get; set; }
        public string name {get; set; }

        public bool minimised { get; set; }
        public bool movable { get; set; }
        
        public Bitmap icon { get; set; }
        #endregion important

        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public int Reg_Y = 0;
        public int CursorX = 0;
        public int CursorY = 0;

        public bool initial = true;
        public bool clicked = false;
        public bool once { get; set; }
        public bool temp = true;

        public string content = "";
        public string Buffered_Content = "";
        public string source = "";

        public Bitmap canvas;
        public Bitmap window { get; set; }
        public Bitmap Container;

        public List<UIElementHandler> UIElements = new List<UIElementHandler>();

        public void App()
        {
            if (initial == true)
            {
                UIElements.Add(new Button(5, 27, 90, 20, "New note", 1));
                UIElements.Add(new Button(100, 27, 170, 20, "Generate Lorem Ipsum", 1));
                UIElements.Add(new Button(280, 27, 90, 20, "Save", 1));

                UIElements.Add(new VerticalScrollbar(width - 22, 52, 20, height - 60, 20, 0, content.Split('\n').Length * 16, "ScrollRight"));

                content = ChuckNorrisFacts.LineBreak(content, 40);
                Buffered_Content = content;

                initial = false;
            }
            if (once == true)
            {
                Bitmap back = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                (canvas, back, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                Container = new Bitmap((uint)(width - 29), (uint)(height - 60), ColorDepth.ColorDepth32);
                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(60, 60, 60));

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                once = false;
                temp = true;
            }

            if (TaskScheduler.counter == TaskScheduler.Apps.Count - 1)
            {
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    (content, Buffered_Content, CursorX, CursorY) = CoreEditor.Editor(content, Buffered_Content, CursorX, CursorY, key);

                    UIElements.Find(d => d.ID == "ScrollRight").MaxVal = content.Split('\n').Length * 16 - (int)Container.Height;

                    Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                    //Problem that needs to be solved possibly before release: When going down with arrow key, it should go all the way to the bottom first and not just jump there
                    //Possible fix: IDK honestly...
                    switch (key.Key)
                    {
                        case ConsoleKeyEx.DownArrow:
                            if (CursorY * 16 >= Container.Height - 10)
                            {
                                UIElements.Find(d => d.ID == "ScrollRight").Value = Math.Clamp((CursorY - (int)Container.Height / 17) * 16, UIElements.Find(d => d.ID == "ScrollRight").MinVal, UIElements.Find(d => d.ID == "ScrollRight").MaxVal);
                                UIElements.Find(d => d.ID == "ScrollRight").Pos = (int)(UIElements.Find(d => d.ID == "ScrollRight").Value / UIElements.Find(d => d.ID == "ScrollRight").Sensitivity) + 20;
                            }
                            break;
                        case ConsoleKeyEx.UpArrow:
                            if (CursorY * 16 < UIElements.Find(d => d.ID == "ScrollRight").Value)
                            {
                                UIElements.Find(d => d.ID == "ScrollRight").Value = Math.Clamp(CursorY * 16, UIElements.Find(d => d.ID == "ScrollRight").MinVal, UIElements.Find(d => d.ID == "ScrollRight").MaxVal);
                                UIElements.Find(d => d.ID == "ScrollRight").Pos = (int)(UIElements.Find(d => d.ID == "ScrollRight").Value / UIElements.Find(d => d.ID == "ScrollRight").Sensitivity) + 20;
                            }
                            break;
                        case ConsoleKeyEx.Enter:
                            if (CursorY * 16 >= Container.Height - 10)
                            {
                                UIElements.Find(d => d.ID == "ScrollRight").Value = Math.Clamp(CursorY * 16, UIElements.Find(d => d.ID == "ScrollRight").MinVal, UIElements.Find(d => d.ID == "ScrollRight").MaxVal);
                                UIElements.Find(d => d.ID == "ScrollRight").Pos = (int)(UIElements.Find(d => d.ID == "ScrollRight").Value / UIElements.Find(d => d.ID == "ScrollRight").Sensitivity) + 20;
                            }
                            break;
                    }
                    temp = true;
                }
            }

            foreach (var elements in UIElements)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > x + elements.X && MouseManager.X < x + elements.X + elements.Width)
                    {
                        if (MouseManager.Y > y + elements.Y && MouseManager.Y < y + elements.Y + elements.Height)
                        {
                            switch (elements.TypeOfElement)
                            {
                                case TypeOfElement.Button:
                                    if (clicked == false)
                                    {
                                        elements.Clicked = true;
                                        temp = true;
                                        clicked = true;
                                    }
                                    break;
                                case TypeOfElement.TextBox:
                                    foreach (var v in UIElements)
                                    {
                                        v.Clicked = false;
                                    }
                                    elements.Clicked = true;
                                    break;
                            }
                        }
                        else
                        {
                            if (elements.Clicked == true)
                            {
                                temp = true;
                                elements.Clicked = false;
                                clicked = false;
                            }
                        }
                    }
                    else
                    {
                        if (elements.Clicked == true)
                        {
                            temp = true;
                            elements.Clicked = false;
                            clicked = false;
                        }
                    }
                }
                if (elements.TypeOfElement != TypeOfElement.TextBox && elements.Clicked == true && MouseManager.MouseState == MouseState.None)
                {
                    temp = true;
                    elements.Clicked = false;
                    clicked = false;
                }
                if (elements.TypeOfElement == TypeOfElement.VerticalScrollbar)
                {
                    if (elements.CheckClick((int)MouseManager.X - x, (int)MouseManager.Y - y))
                    {
                        temp = true;
                    }
                }
            }

            if (temp == true)
            {
                temp = false;
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                if(UIElements.Find(d => d.ID == "ScrollRight").Value > 0)
                {
                    BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.White, Buffered_Content, 5, 0 - UIElements.Find(d => d.ID == "ScrollRight").Value);
                }
                else
                {
                    BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.White, Buffered_Content, 5, 5 - UIElements.Find(d => d.ID == "ScrollRight").Value);
                }
                ImprovedVBE.DrawImageAlpha(Container, 5, 52, window);

                foreach (var Box in UIElements)
                {
                    if (Box.Clicked == true && Box.TypeOfElement == TypeOfElement.Button && clicked == false)
                    {
                        int Col = Box.Color;
                        Box.Color = Color.White.ToArgb();
                        Box.Render(window);
                        Box.Color = Col;

                        switch (Box.Text)
                        {
                            case "New note":
                                content = "";
                                break;
                            case "Generate Lorem Ipsum":
                                content = LoremIpsum(20, 80, 5, 20, 1);
                                (content, Buffered_Content, CursorX, CursorY) = CoreEditor.Update(content, Buffered_Content, CursorX, CursorY);
                                temp = true;
                                break;
                            case "Save":
                                if(CrystalOS_Alpha.Kernel.IsDiskSupport == true)
                                {
                                    File.WriteAllText(source, content);
                                }
                                break;
                        }
                        clicked = true;
                    }
                    else if (Box.TypeOfElement == TypeOfElement.VerticalScrollbar)
                    {
                        Box.X = width - 22;
                        Box.Height = height - 60;
                        Box.Render(window);
                    }
                    else
                    {
                        Box.Render(window);
                    }
                }
            }

            if (ImprovedVBE.RequestRedraw == true)
            {
                switch (GlobalValues.TaskBarType)
                {
                    case "Classic":
                        ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
                        break;
                    case "Nostalgia":
                        ImprovedVBE.DrawImage(window, x, y, ImprovedVBE.cover);
                        break;
                }
            }

            //ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }

        public void RightClick()
        {

        }

        static string LoremIpsum(int minWords, int maxWords, int minSentences, int maxSentences, int numParagraphs)
        {

            var words = new[]{"lorem", "ipsum", "dolor", "sit", "amet", "consectetuer",
        "adipiscing", "elit", "sed", "diam", "nonummy", "nibh", "euismod",
        "tincidunt", "ut", "laoreet", "dolore", "magna", "aliquam", "erat"};

            var rand = new Random();
            int numSentences = rand.Next(maxSentences - minSentences)
                + minSentences + 1;
            int numWords = rand.Next(maxWords - minWords) + minWords + 1;

            StringBuilder result = new StringBuilder();

            int charcount = 0;

            for (int p = 0; p < numParagraphs; p++)
            {
                for (int s = 0; s < numSentences; s++)
                {
                    for (int w = 0; w < numWords; w++)
                    {
                        if (w > 0) { result.Append(" "); }
                        string ss = words[rand.Next(words.Length)];
                        if(charcount > 55)
                        {
                            result.Append("\n");
                            charcount = 0;
                        }
                        result.Append(ss);
                        charcount += ss.Length;
                    }
                    result.Append(". ");
                }
                result.Append("\n");
            }

            return result.ToString();
        }

        public int Get_index_of_char(string source, char c, int index)
        {
            int counter = 0;
            int index_out = 0;
            for(int i = 0; counter < index || i < source.Length; i++)
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
