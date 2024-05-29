using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Applications.CarbonIDE;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics.Icons;
using CrystalOSAlpha.Graphics.TaskBar;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.System32.Installer
{
    class Greeting : App
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
        public Bitmap canvas;
        public Bitmap back_canvas;
        public bool initialize = true;
        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public List<UIElementHandler> UIElements = new List<UIElementHandler>();
        public Greeting(int X, int Y, int Z, int Width, int Height, string Name, Bitmap Icon)
        {
            x = X;
            y = Y;
            z = Z;
            width = Width;
            height = Height;
            name = Name;
            icon = Icon;
        }

        public void App()
        {
            if(initialize == true)
            {
                once = true;
                UIElements.Add(new Button(width - 215, height - 98, 192, 58, "Next", 1, "Next"));
                initialize = false;
            }
            if(once == true)
            {
                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset32", Color.White, "Thank you for choosing CrystalOS Alpha!", 30, 52);
                BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, "We're so glad that you want to try out CrystalOS Alpha!", 40, 142);
                BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, "To begin with the installation, click on \"Next\"", 40, 212);
                ImprovedVBE.DrawImageAlpha(ImprovedVBE.ScaleImageStock(new Bitmap(TaskManager.Elephant), 475, 450), width - 576, 151, canvas);
                
                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                once = false;
            }
            foreach (var Element in UIElements)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > x + Element.X && MouseManager.X < x + Element.X + Element.Width)
                    {
                        if (MouseManager.Y > y + Element.Y && MouseManager.Y < y + Element.Y + Element.Height)
                        {
                            Element.Color = ImprovedVBE.colourToNumber(255, 255, 255);
                            Element.Render(window);
                            TaskScheduler.Apps.Remove(this);
                        }
                    }
                }
                Element.Render(window);
            }
            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }

        public void RightClick()
        {
            
        }
    }
}
