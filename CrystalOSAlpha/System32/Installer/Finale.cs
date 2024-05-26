using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Applications;
using CrystalOSAlpha.Applications.CarbonIDE;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.SystemApps;
using CrystalOSAlpha.UI_Elements;
using System.Drawing;
using System;
using System.Collections.Generic;

namespace CrystalOSAlpha.System32.Installer
{
    class Finale : App
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

        public bool initial = true;
        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public int RemainingTime = 5;
        public int Time = 0;
        public List<UIElementHandler> Elements = new List<UIElementHandler>();

        public Finale(int X, int Y, int Z, int Width, int Height, string Title, Bitmap Icon)
        {
            x = X;
            y = Y;
            z = Z;
            width = Width;
            height = Height;
            name = Title;
            icon = Icon;
        }

        public void App()
        {
            if (initial == true)
            {
                Time = DateTime.UtcNow.Second;
                Elements.Add(new Button(width - 215, height - 98, 192, 58, "Reboot", 1, "Next"));
                once = true;
                initial = false;
            }
            if (once == true)
            {
                (canvas, back_canvas, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset32", Color.White, "We're done!", 30, 52);
                BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, "Your computer will reboot in 5 seconds automatically!", 40, 142);
                BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, "After the reboot, you can enjoy your freshly installed CrystalOS Alpha.", 40, 212);

                Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                once = false;
            }
            foreach (var Element in Elements)
            {
                if (TaskScheduler.Apps.FindAll(d => d.name == "Error!").Count == 0)
                {
                    Element.Render(window);
                    if (MouseManager.MouseState == MouseState.Left)
                    {
                        if (MouseManager.X > x + Element.X && MouseManager.X < x + Element.X + Element.Width)
                        {
                            if (MouseManager.Y > y + Element.Y && MouseManager.Y < y + Element.Y + Element.Height)
                            {
                                switch (Element.TypeOfElement)
                                {
                                    case TypeOfElement.Button:
                                        int Col = Element.Color;
                                        Element.Color = ImprovedVBE.colourToNumber(255, 255, 255);
                                        Element.Render(window);
                                        Element.Clicked = true;
                                        Element.Color = Col;
                                        if (Element.ID == "Next")
                                        {
                                            Cosmos.System.Power.Reboot();
                                        }
                                        break;
                                }
                            }
                        }
                    }
                    else if (Element.TypeOfElement != TypeOfElement.TextBox)
                    {
                        Element.Clicked = false;
                    }
                }
            }
            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
            if(Time != DateTime.UtcNow.Second)
            {
                RemainingTime--;
                Time = DateTime.UtcNow.Second;
            }
            if(RemainingTime == 0)
            {
                Cosmos.System.Power.Reboot();
            }
        }

        public void RightClick()
        {
            
        }
    }
}
