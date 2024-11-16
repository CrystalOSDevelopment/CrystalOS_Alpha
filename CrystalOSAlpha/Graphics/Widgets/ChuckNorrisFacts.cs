using Cosmos.Core.Memory;
using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.Graphics.Widgets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace CrystalOS_Alpha.Graphics.Widgets
{
    public class ChuckNorrisFacts : App
    {
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
        public bool once { get; set; }

        public Bitmap icon { get; set; }
        public Bitmap window { get; set; }

        public int x_dif = 10;
        public int y_dif = 10;
        public static int sizeDec = 0;

        public bool Get_Back = true;
        public bool mem = true;

        public Bitmap Back;
        public string[] Line = null;
        public string City = "Null";
        public void App()
        {
            if(Line == null)
            {
                File.WriteAllText("0:\\CNFact.txt", Kernel.getContent("", "CNFacts.html"));
                Line = File.ReadAllText("0:\\CNFact.txt").Split("\n");
                for (int i = 0; i < Line.Length; i++)
                {
                    Line[i] = Line[i].Trim();
                    if (Line[i].Contains("Value:"))
                    {
                        City = Line[i].Remove(0, Line[i].IndexOf(":") + 1).Trim();
                    }
                }
                City = LineBreak(City, 22);
            }

            if (Get_Back == true)
            {
                if (x >= ImprovedVBE.width)
                {
                    sizeDec = 40;
                }
                Back = Base.Widget_Back(200 - sizeDec, 200 - sizeDec, ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B));
                Back = ImprovedVBE.EnableTransparency(Back, x, y, Back);
                BitFont.DrawBitFontString(Back, "VerdanaCustomCharset24", GlobalValues.c, "Chuck Norris\nFacts", 5, 5);
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Color.White, City, 5, 55);
                Get_Back = false;
                ImprovedVBE.DrawImageAlpha(Back, x, y, ImprovedVBE.cover);
            }
            else if (ImprovedVBE.RequestRedraw)
            {
                ImprovedVBE.DrawImageAlpha(Back, x, y, ImprovedVBE.cover);
            }

            if (MouseManager.MouseState == MouseState.Left)
            {
                if (((MouseManager.X > x && MouseManager.X < x + Back.Width) && (MouseManager.Y > y && MouseManager.Y < y + Back.Height)))
                {
                    if (mem == false)
                    {
                        x_dif = (int)MouseManager.X - x;
                        y_dif = (int)MouseManager.Y - y;
                        mem = true;
                    }
                    x = (int)MouseManager.X - x_dif;
                    y = (int)MouseManager.Y - y_dif;
                    if (x + Back.Width > ImprovedVBE.width - 200)
                    {
                        if (sizeDec < 40)
                        {
                            Back = ImprovedVBE.ScaleImageStock(Back, (uint)(Back.Width - sizeDec), (uint)(Back.Height - sizeDec));
                            sizeDec += 10;
                            Get_Back = true;
                        }
                    }
                    else
                    {
                        if (sizeDec > 0)
                        {
                            Back = ImprovedVBE.ScaleImageStock(Back, (uint)(Back.Width - sizeDec), (uint)(Back.Height - sizeDec));
                            sizeDec -= 10;
                            Get_Back = true;
                        }
                    }
                }
                else
                {
                    if (x + Back.Width > ImprovedVBE.width - 200)
                    {
                        x = SideNav.X + 15;
                    }
                }
                if (mem == true)
                {
                    x = (int)MouseManager.X - x_dif;
                    y = (int)MouseManager.Y - y_dif;
                }
            }
            else
            {
                if (x + Back.Width > ImprovedVBE.width - 200)
                {
                    x = SideNav.X + 15;
                    y = (int)(TaskScheduler.Apps.IndexOf(this) * (Back.Height + 20) + 80);
                }
                if (mem == true)
                {
                    mem = false;
                    Get_Back = true;
                }
            }
        }

        public void RightClick()
        {

        }

        public static string LineBreak(string input, int maxLineWidth)
        {
            StringBuilder output = new StringBuilder();
            string[] words = input.Split(' ');  // Split the input string by spaces
            StringBuilder currentLine = new StringBuilder();

            foreach (string word in words)
            {
                // If adding this word would exceed the maximum width
                if (currentLine.Length + word.Length + 1 > maxLineWidth)
                {
                    // Append the current line to the output and start a new line
                    output.AppendLine(currentLine.ToString());
                    currentLine.Clear();
                }

                // Add the word to the current line
                if (currentLine.Length > 0)
                {
                    currentLine.Append(' ');  // Add a space before the next word
                }
                currentLine.Append(word);
            }

            // Append the last line if it exists
            if (currentLine.Length > 0)
            {
                output.AppendLine(currentLine.ToString());
            }

            return output.ToString();
        }
    }
}
