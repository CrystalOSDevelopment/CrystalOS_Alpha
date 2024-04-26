using Cosmos.System;
using Cosmos.System.Graphics;
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

        public List<Button_prop> Buttons = new List<Button_prop>();
        public List<VerticalScrollbar> Scroll = new List<VerticalScrollbar>();

        public void App()
        {
            if (initial == true)
            {
                Buttons.Add(new Button_prop(5, 27, 90, 20, "New note", 1));
                Buttons.Add(new Button_prop(100, 27, 170, 20, "Generate Lorem Ipsum", 1));
                Buttons.Add(new Button_prop(280, 27, 90, 20, "Save", 1));

                Scroll.Add(new VerticalScrollbar(width - 22, 52, 20, height - 60, 20, 0, content.Split('\n').Length * 16));

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
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);

                        switch(button.Text)
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
                                File.WriteAllText(source, content);
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

                    Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                    switch (key.Key)
                    {
                        case ConsoleKeyEx.DownArrow:
                            if(CursorY * 16 >= Container.Height - 10)
                            {
                                Scroll[0].Value = Math.Clamp((CursorY - (int)Container.Height / 17) * 16, Scroll[0].MinVal, Scroll[0].MaxVal);
                                Scroll[0].Pos = (int)(Scroll[0].Value / Scroll[0].Sensitivity) + 20;
                            }
                            break;
                        case ConsoleKeyEx.UpArrow:
                            if(CursorY * 16 < Scroll[0].Value)
                            {
                                Scroll[0].Value = Math.Clamp(CursorY * 16, Scroll[0].MinVal, Scroll[0].MaxVal);
                                Scroll[0].Pos = (int)(Scroll[0].Value / Scroll[0].Sensitivity) + 20;
                            }
                            break;
                        case ConsoleKeyEx.Enter:
                            if (CursorY * 16 >= Container.Height - 10)
                            {
                                Scroll[0].Value = Math.Clamp(CursorY * 16, Scroll[0].MinVal, Scroll[0].MaxVal);
                                Scroll[0].Pos = (int)(Scroll[0].Value / Scroll[0].Sensitivity) + 20;
                            }
                            break;
                    }
                    temp = true;
                }
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
                vscroll.x = width - 22;
                if(content.Split('\n').Length * 16 > Container.Height)
                {
                    vscroll.MaxVal = content.Split('\n').Length * 16 - (int)Container.Height;
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

            if(temp == true)
            {
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                if(Scroll[0].Value > 0)
                {
                    BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.White, Buffered_Content, 5, 0 - Scroll[0].Value);
                }
                else
                {
                    BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.White, Buffered_Content, 5, 5 - Scroll[0].Value);
                }
                ImprovedVBE.DrawImageAlpha(Container, 5, 52, window);

                foreach (var vscroll in Scroll)
                {
                    vscroll.Render(window);
                }

                temp = false;
            }
            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
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
