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

namespace CrystalOS_Alpha.Graphics.Widgets
{
    public class WeatherForecast : App
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
        public string City = "NaN";
        public string Date = "NaN";
        public int Temperature = 0;
        public string Description = "Description: ";
        public void App()
        {
            if(Line == null)
            {
                File.WriteAllText("0:\\Weather.txt", Kernel.getContent("", "Weather.html"));
                Line = File.ReadAllText("0:\\Weather.txt").Split("\n");
                for (int i = 0; i < Line.Length; i++)
                {
                    Line[i] = Line[i].Trim();
                    if (Line[i].Contains("Location:"))
                    {
                        City = Line[i].Split(":")[1].Trim().Split(",")[0];
                    }
                    else if (Line[i].Contains("Date:"))
                    {
                        Date = Line[i].Split(":")[1].Trim();
                    }
                    else if (Line[i].Contains("Temperature:"))
                    {
                        Temperature = (int)((float.Parse(Line[i].Split(":")[1]) - 32.0) * 5.0 / 9.0);
                    }
                    else if (Line[i].Contains("Description:"))
                    {
                        Description = "Description: " + Line[i].Split(":")[1].Trim();
                    }
                }
            }

            if (Get_Back == true)
            {
                if (x >= ImprovedVBE.width)
                {
                    sizeDec = 40;
                }
                Back = Base.Widget_Back(200 - sizeDec, 200 - sizeDec, ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B));
                Back = ImprovedVBE.EnableTransparency(Back, x, y, Back);
                BitFont.DrawBitFontString(Back, "VerdanaCustomCharset24", GlobalValues.c, City, 5, 5);
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", Color.LightSlateGray, Date, 5, 30);
                BitFont.DrawBitFontString(Back, "VerdanaCustomCharset24", GlobalValues.c, Temperature + " C", 120, 90);
                BitFont.DrawBitFontString(Back, "ArialCustomCharset16", GlobalValues.c, ChuckNorrisFacts.LineBreak(Description, 22), 7, 145);
                Get_Back = false;
            }
            else if (ImprovedVBE.RequestRedraw || SideNav.RequestDrawLocal == true)
            {
                ImprovedVBE.DrawImageAlpha(Back, x, y, ImprovedVBE.cover);
            }
            //ImprovedVBE.DrawImageAlpha(Back, x, y, ImprovedVBE.cover);

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
    }
}
