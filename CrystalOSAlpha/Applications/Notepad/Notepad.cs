using Cosmos.System;
using Cosmos.System.Graphics;
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

        public int CurrentColor = ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B);
        public int Reg_Y = 0;

        public bool initial = true;
        public bool clicked = false;
        public bool once = true;
        public bool temp = true;

        public string content = "";
        public string source = "";

        public Bitmap canvas;
        public Bitmap window;
        public Bitmap Container;

        public List<Button_prop> Buttons = new List<Button_prop>();
        public List<Scrollbar_Values> Scroll = new List<Scrollbar_Values>();

        public void App()
        {
            if (initial == true)
            {
                Buttons.Add(new Button_prop(5, 27, 90, 20, "New note", 1));
                Buttons.Add(new Button_prop(100, 27, 170, 20, "Generate Lorem Ipsum", 1));
                Buttons.Add(new Button_prop(280, 27, 90, 20, "Save", 1));

                Scroll.Add(new Scrollbar_Values(width - 22, 30, 20, height - 60, 0));

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

                canvas = Scrollbar.Render(canvas, Scroll[0]);

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
                if(MouseManager.Y > y + scv.y + 48 && MouseManager.Y < y + height - 42 && scv.Clicked == true)
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
                    int length = content.Length;
                    content = Keyboard.HandleKeyboard(content, key);
                    int length2 = content.Length;

                    Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                    temp = true;
                }
            }

            if(temp == true)
            {
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                Array.Fill(Container.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                window = Scrollbar.Render(window, Scroll[0]);

                string output = "";

                output = content;
                if(output.Split('\n').Length > 23)
                {
                    int h = Scroll[0].Pos / 8;
                    for(int i = 0; i < h; i++)
                    {
                        int index = output.IndexOf('\n');
                        output = output.Remove(0, index + 1);
                    }
                }
                if(output.Split('\n').Length > 23)
                {
                    output = output.Remove(Get_index_of_char(output, '\n', 23));
                }
                BitFont.DrawBitFontString(Container, "ArialCustomCharset16", Color.White, output, 5, 5 - Scroll[0].Pos);
                ImprovedVBE.DrawImageAlpha(Container, 5, 52, window);
                
                temp = false;
            }
            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
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
