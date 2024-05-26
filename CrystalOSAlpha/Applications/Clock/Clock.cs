using Cosmos.System;
using Cosmos.System.Graphics;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.System32;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace CrystalOSAlpha.Applications.Clock
{
    class Clock : App
    {
        #region Window propeties
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
        #endregion Window propeties

        #region Extra
        public Bitmap canvas;

        public bool temp = true;
        public bool Clicked = false;
        public bool StartCount = false;
        public bool CountBack = false;

        public int CurrentColor = ImprovedVBE.colourToNumber(GlobalValues.R, GlobalValues.G, GlobalValues.B);
        public int Stopwatch = 0;
        public int CurrentTime = -1;
        public int RemainingTime = 0;

        public string Tab = "Clock";
        
        public List<int> StopWatchLaps = new List<int>();
        public List<Tuple<int, int>> Positions = new List<Tuple<int, int>>();
        public List<UIElementHandler> UIElements = new List<UIElementHandler>();
        #endregion Extra

        public void App()
        {
            int YPos = height / 2 - 8;
            string[] Options = { "Clock", "Stopwatch", "Timer" };
            if(width >= 350)
            {
                Options = new string[] { "Clock", "Stopwatch", "Timer", "Timezone" };
            }
            if (once == true)
            {
                #region Fix Windowsize
                if(width != 300)
                {
                    width = 300;
                }
                if(height != 300)
                {
                    height = 300;
                }
                #endregion Fix Windowsize

                Bitmap back = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);
                (canvas, back, window) = WindowGenerator.Generate(x, y, width, height, CurrentColor, name);

                UIElements.Clear();
                if(Tab == "Clock")
                {
                    int Radius = Math.Min(width, height) / 2 - 52;
                    YPos = height / 2 - 8;
                    ImprovedVBE.DrawFilledEllipse(canvas, width / 2, YPos, Radius, Radius, ImprovedVBE.colourToNumber(20, 20, 20));
                    for (int hour = 1; hour <= 12; hour++)
                    {
                        double angleInDegrees = 30 * hour - 90; // 30 degrees for each hour, -90 to start at 12 o'clock
                        double angleInRadians = angleInDegrees * Math.PI / 180;

                        // Calculate the position for the number
                        int numX = width / 2 + (int)((Radius - 15) * Math.Cos(angleInRadians));
                        int numY = YPos + (int)((Radius - 15) * Math.Sin(angleInRadians));

                        // Offset the position for centering the text
                        numX -= 8;
                        numY -= 12;

                        BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, hour.ToString(), numX, numY);
                    }

                    for (int minute = 1; minute <= 60; minute++)
                    {
                        if (minute % 5 == 0) continue; // Skip markers that align with hour numbers

                        double angleInDegrees = 6 * minute - 90; // 6 degrees for each minute marker, -90 to start at 12 o'clock
                        double angleInRadians = angleInDegrees * Math.PI / 180;

                        // Calculate the position for the minute marker
                        int markX = width / 2 + (int)((Radius - 5) * Math.Cos(angleInRadians));
                        int markY = YPos + (int)((Radius - 5) * Math.Sin(angleInRadians));

                        // Draw a filled rectangle for the minute marker
                        ImprovedVBE.DrawFilledRectangle(canvas, ImprovedVBE.colourToNumber(255, 255, 255), markX - 1, markY - 1, 3, 3); // Small square for minute markers
                    }
                }
                else if(Tab == "Stopwatch")
                {
                    UIElements.Add(new Button(10, height - 120, width / 2 - 20, 25, "Start/Stop", 1, "StartStop"));
                    UIElements.Add(new Button(width / 2 + 10, height - 120, width / 2 - 20, 25, "Mark time", 1, "Mark"));
                }
                else if(Tab == "Timer")
                {
                    UIElements.Add(new Button(10, height - 120, width / 2 - 20, 25, "Start/Stop", 1, "StartStopTimer"));
                    UIElements.Add(new Button(width / 2 + 10, height - 120, width / 2 - 20, 25, "Pause", 1, "Pause"));
                    UIElements.Add(new TextBox(10, 50, 200, 25, ImprovedVBE.colourToNumber(60, 60, 60), "", "00:00:00", TextBox.Options.left, "Time"));
                }

                int offset = 10;
                Positions.Clear();
                for (int i = 0; i < Options.Length; i++)
                {
                    int extract = offset;
                    if (i == 0)
                    {
                        offset += BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, Options[i], offset, height - 42) + 20;
                    }
                    else
                    {

                        offset += BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, Options[i], offset, height - 42) + 20;
                    }
                    Positions.Add(new Tuple<int, int>(extract, offset - extract - 20));
                }

                temp = true;
            }

            if (TaskScheduler.Apps[^1] == this)
            {
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    if (key.Key == ConsoleKeyEx.Enter)
                    {
                        string[] Vals = UIElements.Find(d => d.ID == "Time").Text.Split(":");
                        if (Vals.Length == 0)
                        {

                        }
                        else if (Vals.Length == 1)
                        {
                            RemainingTime = int.Parse(Vals[0]) * 3600;
                        }
                        else if (Vals.Length == 2)
                        {
                            RemainingTime = int.Parse(Vals[0]) * 3600 + int.Parse(Vals[1]) * 60;
                        }
                        else if (Vals.Length == 3)
                        {
                            RemainingTime = int.Parse(Vals[0]) * 3600 + int.Parse(Vals[1]) * 60 + int.Parse(Vals[2]);
                        }
                        CountBack = true;
                    }
                    else
                    {
                        foreach (var element in UIElements)
                        {
                            if (element.ID == "Time")
                            {
                                element.Text = Keyboard.HandleKeyboard(element.Text, key);
                                temp = true;
                            }
                        }
                    }
                    Array.Copy(canvas.RawData, 0, window.RawData, 0, canvas.RawData.Length);
                    temp = true;
                }
            }

            if (CurrentTime != DateTime.Now.Second)
            {
                temp = true;
                if(StartCount == true)
                {
                    Stopwatch++;
                }
                if(CountBack == true && RemainingTime > 0)
                {
                    RemainingTime--;
                }
                else
                {
                    CountBack = false;
                }

                CurrentTime = DateTime.Now.Second;
            }

            if (temp == true)
            {
                Array.Copy(canvas.RawData, window.RawData, canvas.RawData.Length);
                switch (Tab)
                {
                    case "Clock":
                        int Radius = Math.Min(width, height) / 2 - 52;
                        DrawHand(window, (int)(width / 2), YPos, 360 / 60 * DateTime.Now.Second, Radius);
                        DrawHand(window, (int)(width / 2), YPos, 360 / 60 * DateTime.Now.Minute, (int)(Radius - (Radius * 0.2f)));
                        if(DateTime.Now.Hour <= 12)
                        {
                            DrawHand(window, (int)(width / 2), YPos, 360 / 12 * DateTime.Now.Hour, (int)(Radius - (Radius * 0.4f)));
                        }
                        else if(DateTime.Now.Hour > 12)
                        {
                            int Temp = DateTime.Now.Hour - 12;
                            DrawHand(window, (int)(width / 2), YPos, 360 / 12 * Temp, (int)(Radius - (Radius * 0.4f)));
                        }
                        ImprovedVBE.DrawFilledRectangle(window, Color.Blue.ToArgb(), Positions[0].Item1, height - 48, Positions[0].Item2, 3);
                        break;
                    case "Stopwatch":
                        int Hour = Stopwatch / 3600;
                        int Minute = (Stopwatch - Hour * 3600) / 60;
                        int Second = Stopwatch - Hour * 3600 - Minute * 60;
                        string EllapsedTime = "";
                        if(Hour < 10)
                        {
                            EllapsedTime += "0" + Hour + ":";
                        }
                        else
                        {
                            EllapsedTime += Hour + ":";
                        }
                        if(Minute < 10)
                        {
                            EllapsedTime += "0" + Minute + ":";
                        }
                        else
                        {
                            EllapsedTime += Minute + ":";
                        }
                        if(Second < 10)
                        {
                            EllapsedTime += "0" + Second;
                        }
                        else
                        {
                            EllapsedTime += Second;
                        }
                        BitFont.DrawBitFontString(window, "VerdanaCustomCharset24", Color.White, EllapsedTime, width / 2 - 40, 70);
                        ImprovedVBE.DrawFilledRectangle(window, Color.Blue.ToArgb(), Positions[1].Item1, height - 48, Positions[1].Item2, 3);
                        break;
                    case "Timer":
                        int RemainingHour = RemainingTime / 3600;
                        int RemainingMinute = (RemainingTime - RemainingHour * 3600) / 60;
                        int RemainingSecond = RemainingTime - RemainingHour * 3600 - RemainingMinute * 60;
                        string RemainingTimeS = "";
                        if (RemainingHour < 10)
                        {
                            RemainingTimeS += "0" + RemainingHour + ":";
                        }
                        else
                        {
                            RemainingTimeS += RemainingHour + ":";
                        }
                        if (RemainingMinute < 10)
                        {
                            RemainingTimeS += "0" + RemainingMinute + ":";
                        }
                        else
                        {
                            RemainingTimeS += RemainingMinute + ":";
                        }
                        if (RemainingSecond < 10)
                        {
                            RemainingTimeS += "0" + RemainingSecond;
                        }
                        else
                        {
                            RemainingTimeS += RemainingSecond;
                        }
                        BitFont.DrawBitFontString(window, "ArialCustomCharset16", Color.White, "Starting time: HH:MM:SS", 10, 28);
                        BitFont.DrawBitFontString(window, "VerdanaCustomCharset24", Color.White, RemainingTimeS, width / 2 - 40, 100);
                        ImprovedVBE.DrawFilledRectangle(window, Color.Blue.ToArgb(), Positions[2].Item1, height - 48, Positions[2].Item2, 3);
                        break;
                    case "Timezone":
                        ImprovedVBE.DrawFilledRectangle(window, Color.Blue.ToArgb(), Positions[3].Item1, height - 48, Positions[3].Item2, 3);
                        break;
                }
                temp = false;
            }

            foreach (var element in UIElements)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > x + element.X && MouseManager.X < x + element.X + element.Width)
                    {
                        if (MouseManager.Y > y + element.Y && MouseManager.Y < y + element.Y + element.Height)
                        {
                            if (Clicked == false)
                            {
                                switch (element.TypeOfElement)
                                {
                                    case TypeOfElement.Button:
                                        element.Clicked = true;
                                        temp = true;
                                        Clicked = true;
                                        switch (element.ID)
                                        {
                                            case "StartStop":
                                                StartCount = !StartCount;
                                                break;
                                            case "Mark":
                                                StopWatchLaps.Add(Stopwatch);
                                                break;
                                            case "StartStopTimer":
                                                string[] Vals = UIElements.Find(d => d.ID == "Time").Text.Split(":");
                                                if (Vals.Length == 0)
                                                {

                                                }
                                                else if (Vals.Length == 1)
                                                {
                                                    RemainingTime = int.Parse(Vals[0]) * 3600;
                                                }
                                                else if (Vals.Length == 2)
                                                {
                                                    RemainingTime = int.Parse(Vals[0]) * 3600 + int.Parse(Vals[1]) * 60;
                                                }
                                                else if (Vals.Length == 3)
                                                {
                                                    RemainingTime = int.Parse(Vals[0]) * 3600 + int.Parse(Vals[1]) * 60 + int.Parse(Vals[2]);
                                                }
                                                CountBack = true;
                                                break;
                                            case "Pause":
                                                CountBack = !CountBack;
                                                break;
                                        }
                                        break;

                                    case TypeOfElement.TextBox:
                                        element.Clicked = true;
                                        temp = true;
                                        Clicked = true;
                                        break;
                                }
                            }
                        }
                    }
                }
                else if (MouseManager.MouseState == MouseState.None)
                {
                    if (element.Clicked == true)
                    {
                        element.Clicked = false;
                        once = true;
                    }

                }
                if (element.Clicked == true)
                {
                    switch (element.TypeOfElement)
                    {
                        case TypeOfElement.Button:
                            int Col = element.Color;
                            element.Color = Color.White.ToArgb();
                            element.Render(window);
                            element.Color = Col;
                            break;

                        case TypeOfElement.TextBox:
                            element.Render(window);
                            break;
                    }
                }
                else
                {
                    element.Render(window);
                }
            }

            bool ReEnable = false;
            if(MouseManager.MouseState == MouseState.Left && TaskScheduler.isResizing == false && Clicked == false)
            {
                for(int i = 0; i < Positions.Count; i++)
                {
                    if(MouseManager.X > x + Positions[i].Item1 && MouseManager.X < x + Positions[i].Item1 + Positions[i].Item2)
                    {
                        if(MouseManager.Y > y + height - 42 && MouseManager.Y < y + height)
                        {
                            if(once == false)
                            {
                                Tab = Options[i];
                                temp = true;
                                once = true;
                                ReEnable = true;
                                Clicked = true;
                            }
                        }
                    } 
                }
            }
            else if(Clicked == true && MouseManager.MouseState == MouseState.None)
            {
                Clicked = false;
            }
            if(ReEnable == false)
            {
                once = false;
            }
            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }

        public void RightClick()
        {
            
        }

        public void DrawHand(Bitmap Canvas, int xStart, int yStart, double angleInDegrees, int radius)
        {
            double angleInRadians = (angleInDegrees - 90) * (Math.PI / 180); // Start from 12 o'clock and rotate clockwise

            // Calculate the end point of the line on the circle's edge
            int endX = xStart + (int)(radius * Math.Cos(angleInRadians));
            int endY = yStart + (int)(radius * Math.Sin(angleInRadians));


            ImprovedVBE.DrawLine(Canvas, xStart, yStart, endX, endY, ImprovedVBE.colourToNumber(255, 0, 0));
        }
    }
}