using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrystalOSAlpha.Applications.WebscapeNavigator
{
    class Webrendering
    {
        public static Bitmap DrawBoard;
        public static List<string> Tags = new List<string> { "<a" };
        public static List<Links> Link = new List<Links>();
        public static Bitmap Render(Bitmap input, string source)
        {
            DrawBoard = new Bitmap(input.Width, 1500, ColorDepth.ColorDepth32);
            Array.Fill(DrawBoard.RawData, ImprovedVBE.colourToNumber(255, 255, 255));
            int X_index = 5;
            int Y_index = 5;
            List<string> WaitToClose = new List<string> { "body" };
            foreach (string line in source.Split('\n'))
            {
                string mod = line.Trim();
                mod = mod.Replace("<br>", "\n");
            Top:
                if (mod.StartsWith("<h1>") || WaitToClose[^1] == "<h1>")
                {
                    mod = mod.Replace("<h1>", "");
                    if (!mod.Contains("</h1>"))
                    {
                        WaitToClose.Add("<h1>");
                    }
                    else
                    {
                        if (WaitToClose[^1] == "<h1>")
                        {
                            WaitToClose.RemoveAt(WaitToClose.Count - 1);
                        }
                        mod = mod.Replace("</h1>", "");
                    }
                    BitFont.DrawBitFontString(DrawBoard, "VerdanaCustomCharset24", Color.Black, mod, X_index, Y_index);
                    Y_index += 27;
                }
                if (mod.StartsWith("<p>") || WaitToClose[^1] == "<p>")
                {
                    mod = mod.Replace("<p>", "");
                    if (!mod.Contains("</p"))
                    {
                        WaitToClose.Add("<p>");
                    }
                    else
                    {
                        if (WaitToClose[^1] == "<p>")
                        {
                            WaitToClose.RemoveAt(WaitToClose.Count - 1);
                        }
                        mod = mod.Replace("</p>", "");
                        Y_index += 18;
                    }
                    bool found = false;
                    foreach (string s in Tags)
                    {
                        if (mod.Contains(s))
                        {
                            found = true;
                            goto Top;
                        }
                    }
                    if (found == false)
                    {
                    }

                    BitFont.DrawBitFontString(DrawBoard, "ArialCustomCharset16", Color.Black, mod, X_index, Y_index);
                }
                if (mod.StartsWith("<a") || WaitToClose[^1] == "<a>")
                {
                    string href = mod.Remove(0, mod.IndexOf("h") + 6);
                    href = href.Substring(0, href.IndexOf("\""));
                    int first = mod.IndexOf("<");
                    int last = mod.LastIndexOf(">");
                    try
                    {
                        if(first > 0)
                        {
                            string temp = mod.Substring(0, first);
                            X_index += BitFont.DrawBitFontString(DrawBoard, "ArialCustomCharset16", Color.Black, temp, X_index, Y_index) + 5;
                        }
                        mod = mod.Substring(first, last);
                        int a = mod.IndexOf(">");
                        mod = mod.Remove(0, a + 1);
                    }
                    catch (Exception e)
                    {
                        X_index += BitFont.DrawBitFontString(DrawBoard, "ArialCustomCharset16", Color.Red, e.Message, X_index, Y_index) + 5;
                    }


                    if (!mod.Contains("</a"))
                    {
                        WaitToClose.Add("<a>");
                    }
                    else
                    {
                        if (WaitToClose[^1] == "<a>")
                        {
                            WaitToClose.RemoveAt(WaitToClose.Count - 1);
                        }
                        mod = mod.Replace("</a", "");
                    }
                    bool found = false;
                    foreach (string s in Tags)
                    {
                        if (mod.Contains(s))
                        {
                            found = true;
                            goto Top;
                        }
                    }
                    if (found == false)
                    {
                        int t = X_index;
                        X_index += BitFont.DrawBitFontString(DrawBoard, "ArialCustomCharset16", Color.Blue, mod, X_index, Y_index) + 5;
                        Link.Add(new Links(t, Y_index, X_index - t, href));
                    }

                    X_index = 5;
                    Y_index += 18;
                }
            }
            Array.Copy(DrawBoard.RawData, 0, input.RawData, 0, input.RawData.Length);
            return input;
        }
    }

    class Links
    {
        public int X { get; set;}
        public int Y { get; set;}
        public int Width { get; set;}
        public string link { get; set;}
        public Links(int x, int y, int width, string link)
        {
            X = x;
            Y = y;
            Width = width;
            this.link = link;
        }
    }
}
