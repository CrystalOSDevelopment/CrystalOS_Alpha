﻿using Cosmos.Core;
using Cosmos.System;
using Cosmos.System.Graphics;
using Cosmos.System.Network.Config;
using Cosmos.System.Network.IPv4;
using Cosmos.System.Network.IPv4.TCP;
using Cosmos.System.Network.IPv4.UDP.DNS;
using CrystalOSAlpha.Applications.Calculator;
using CrystalOSAlpha.Graphics;
using CrystalOSAlpha.Graphics.Engine;
using CrystalOSAlpha.UI_Elements;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using TaskScheduler = CrystalOSAlpha.Graphics.TaskScheduler;

namespace CrystalOSAlpha.Applications.Settings
{
    class Settings : App
    {
        public int x { get; set; }
        public int y { get; set; }
        public int z { get; set; }
        public int width { get; set; }
        public int height { get; set; }

        public int desk_ID { get; set; }

        public string name { get; set; }

        public bool minimised { get; set; }
        public bool movable { get; set; }
        

        public Bitmap icon { get; set; }

        public bool initial = true;
        public bool clicked = false;

        public Bitmap canvas;
        public Bitmap back_canvas;
        public bool once = true;
        public Bitmap window;
        public List<Button_prop> Buttons = new List<Button_prop>();

        public List<Button_prop> Res = new List<Button_prop>();
        
        public int CurrentColor = ImprovedVBE.colourToNumber(Global_integers.R, Global_integers.G, Global_integers.B);

        public string ActiveD = "Display";

        public string customres = "";

        public void App()
        {
            if (initial == true)
            {
                Buttons.Clear();

                Buttons.Add(new Button_prop(5, 32, 98, 30, "Display", 1));
                Buttons.Add(new Button_prop(5, 73, 98, 30, "Sound", 1));
                Buttons.Add(new Button_prop(5, 114, 98, 30, "Networking", 1));
                Buttons.Add(new Button_prop(5, 155, 98, 30, "About OS", 1));

                switch (ActiveD)
                {
                    case "Display":
                        Buttons.Add(new Button_prop(303, 338, 98, 20, "Apply", 1));

                        Res.Add(new Button_prop(2, 2, 396, 30, "1920x1080x32", 1));
                        Res.Add(new Button_prop(2, 32, 396, 30, "1366x768x32", 1));
                        Res.Add(new Button_prop(2, 62, 396, 30, "1024x768x32", 1));
                        Res.Add(new Button_prop(2, 92, 396, 30, "800x600x32", 1));
                        Res.Add(new Button_prop(2, 122, 396, 30, "640x480x32", 1));
                        break;
                    case "Sound":
                        
                        break;
                    case "Networking":
                        
                        break;
                    case "About OS":
                        
                        break;
                }

                once = true;
                initial = false;
            }
            if (once == true)
            {
                canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32); //new int[width * height];
                back_canvas = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32); //new int[width * height];
                window = new Bitmap((uint)width, (uint)height, ColorDepth.ColorDepth32);

                #region corners
                DrawFilledEllipse(10, 10, 10, 10, CurrentColor);
                DrawFilledEllipse(width - 11, 10, 10, 10, CurrentColor);
                DrawFilledEllipse(10, height - 10, 10, 10, CurrentColor);
                DrawFilledEllipse(width - 11, height - 10, 10, 10, CurrentColor);

                DrawFilledRectangle(CurrentColor, 0, 10, width, height - 20);
                DrawFilledRectangle(CurrentColor, 5, 0, width - 10, 15);
                DrawFilledRectangle(CurrentColor, 5, height - 15, width - 10, 15);
                #endregion corners

                canvas = ImprovedVBE.DrawImageAlpha2(canvas, x, y, canvas);

                DrawGradientLeftToRight();

                DrawFilledEllipse(width - 13, 10, 8, 8, ImprovedVBE.colourToNumber(255, 0, 0));

                DrawFilledEllipse(width - 34, 10, 8, 8, ImprovedVBE.colourToNumber(227, 162, 37));

                BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, name, 2, 2);

                //Button.Button_render(canvas, 10, 70, 100, 25, 1, "Click");

                foreach (var button in Buttons)
                {
                    if (button.Clicked == true)
                    {
                        Button.Button_render(canvas, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);
                        switch (button.Text)
                        {
                            case "Display":
                                ActiveD = button.Text;
                                initial = true;
                                break;
                            case "Sound":
                                ActiveD = button.Text;
                                initial = true;
                                break;
                            case "Networking":
                                ActiveD = button.Text;
                                initial = true;
                                break;
                            case "About OS":
                                ActiveD = button.Text;
                                initial = true;
                                break;
                        }
                        button.Clicked = false;
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

                switch (ActiveD)
                {
                    case "Display":
                        BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, ActiveD, width - BitFont.DrawBitFontString(back_canvas, "VerdanaCustomCharset24", Color.White, ActiveD, 0, 0) - 5, 24);
                        
                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Current resolution: " + ImprovedVBE.width + "x" + ImprovedVBE.height + "x32", 128, 48);
                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Available resolutions:", 128, 73);
                        //Add a resolution selector here

                        Bitmap res = new Bitmap(400, 210, ColorDepth.ColorDepth32);
                        Array.Fill(res.RawData, ImprovedVBE.colourToNumber(36, 36, 36));
                        ImprovedVBE.DrawFilledRectangle(res, ImprovedVBE.colourToNumber(60, 60, 60), 2, 2, (int)(res.Width - 4), (int)(res.Height - 4), false);

                        #region res buttons
                        foreach(var button in Res)
                        {
                            if (button.Clicked == true)
                            {
                                Button.Button_render(res, button.X, button.Y, button.Width, button.Height, Color.White.ToArgb(), button.Text);
                                switch (button.Text)
                                {
                                    case "1920x1080x32":
                                        ImprovedVBE.width = 1920;
                                        ImprovedVBE.height = 1080;

                                        MouseManager.ScreenWidth = 1920;
                                        MouseManager.ScreenHeight = 1080;
                                        ImprovedVBE.Res = true;
                                        break;
                                    case "1600x900x32":
                                        ImprovedVBE.width = 1600;
                                        ImprovedVBE.height = 900;

                                        MouseManager.ScreenWidth = 1600;
                                        MouseManager.ScreenHeight = 900;
                                        ImprovedVBE.Res = true;
                                        break;
                                    case "1366x768x32":
                                        ImprovedVBE.width = 1366;
                                        ImprovedVBE.height = 768;

                                        MouseManager.ScreenWidth = 1366;
                                        MouseManager.ScreenHeight = 768;
                                        ImprovedVBE.Res = true;
                                        break;
                                    case "1024x768x32":
                                        ImprovedVBE.width = 1024;
                                        ImprovedVBE.height = 768;

                                        MouseManager.ScreenWidth = 1024;
                                        MouseManager.ScreenHeight = 768;
                                        ImprovedVBE.Res = true;
                                        break;
                                    case "800x600x32":
                                        ImprovedVBE.width = 800;
                                        ImprovedVBE.height = 600;

                                        MouseManager.ScreenWidth = 800;
                                        MouseManager.ScreenHeight = 600;
                                        ImprovedVBE.Res = true;
                                        break;
                                    case "640x480x32":
                                        ImprovedVBE.width = 640;
                                        ImprovedVBE.height = 480;

                                        MouseManager.ScreenWidth = 640;
                                        MouseManager.ScreenHeight = 480;
                                        ImprovedVBE.Res = true;
                                        break;
                                }
                                button.Clicked = false;
                            }
                            else
                            {
                                Button.Button_render(res, button.X, button.Y, button.Width, button.Height, button.Color, button.Text);
                            }
                            if (MouseManager.MouseState == MouseState.None)
                            {
                                button.Clicked = false;
                            }
                        }
                        #endregion

                        ImprovedVBE.DrawImageAlpha(res, 128, 100, canvas);

                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, " Use custom resolution:", 128, 317);

                        TextBox.Box(canvas, 128, 338, 157, 20, ImprovedVBE.colourToNumber(60, 60, 60), customres, "Example: 800x600x32", TextBox.Options.left);

                        break;
                    case "Sound":
                        BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, ActiveD, width - BitFont.DrawBitFontString(back_canvas, "VerdanaCustomCharset24", Color.White, ActiveD, 0, 0) - 5, 24);
                        break;
                    case "Networking":
                        BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, ActiveD, width - BitFont.DrawBitFontString(back_canvas, "VerdanaCustomCharset24", Color.White, ActiveD, 0, 0) - 5, 24);

                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Attempting to ping google.com - 8.8.8.8", 128, 75);

                        int PacketSent = 0;
                        int PacketReceived = 0;
                        int PacketLost = 0;
                        int PercentLoss;

                        Address source;
                        Address destination = Address.Parse("8.8.8.8");

                        if (destination != null)
                        {
                            source = IPConfig.FindNetwork(destination);
                        }
                        else //Make a DNS request if it's not an IP
                        {
                            var xClient = new DnsClient();
                            xClient.Connect(DNSConfig.DNSNameservers[0]);
                            xClient.SendAsk("google.com");
                            destination = xClient.Receive();
                            xClient.Close();

                            if (destination == null)
                            {
                                
                            }

                            source = IPConfig.FindNetwork(destination);
                        }

                        try
                        {
                            var xClient = new ICMPClient();
                            xClient.Connect(destination);

                            for (int i = 0; i < 4; i++)
                            {
                                xClient.SendEcho();

                                PacketSent++;

                                var endpoint = new EndPoint(Address.Zero, 0);

                                int second = xClient.Receive(ref endpoint, 4000);

                                if (second == -1)
                                {
                                    BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Failed to recieve ICMP packet: Timeout\n\nNetwork status: Offline", 128, 102);
                                    PacketLost++;
                                }
                                else
                                {
                                    if (second < 1)
                                    {
                                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Successfuly recieved ICMP packet: " + second + "\n\nNetwork status: Online", 128, 102);
                                        //Kernel.console.WriteLine("Reply received from " + endpoint.Address.ToString() + " time < 1s");
                                    }
                                    else if (second >= 1)
                                    {
                                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Successfuly recieved ICMP packet: " + second + "\n\nNetwork status: Online", 128, 102);
                                        //Kernel.console.WriteLine("Reply received from " + endpoint.Address.ToString() + " time " + second + "s");
                                    }

                                    PacketReceived++;
                                }
                            }

                            xClient.Close();
                        }
                        catch
                        {
                            
                        }

                        PercentLoss = 25 * PacketLost;

                        
                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, NetworkConfiguration.CurrentAddress.ToString(), 128, 48);
                        break;
                    case "About OS":
                        BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, ActiveD, width - BitFont.DrawBitFontString(back_canvas, "VerdanaCustomCharset24", Color.White, ActiveD, 0, 0) - 5, 24);
                        
                        BitFont.DrawBitFontString(canvas, "VerdanaCustomCharset24", Color.White, "CrystalOS Alpha Edition", 128, 58);

                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Build release: 2023111221", 128, 95);
                        string cpuname = CPU.GetCPUBrandString();
                        if(cpuname.Length > 20)
                        {
                            cpuname = cpuname.Insert(20, "\n");
                        }
                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "System Processor: " + cpuname, 128, 122);
                        BitFont.DrawBitFontString(canvas, "ArialCustomCharset16", Color.White, "Amount of RAM: " + CPU.GetAmountOfRAM() + "MB", 128, 155);
                        break;
                }

                window.RawData = canvas.RawData;
                back_canvas = canvas;
                once = false;
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
            }

            foreach (var button in Res)
            {
                if (MouseManager.MouseState == MouseState.Left)
                {
                    if (MouseManager.X > x + button.X + 128 && MouseManager.X < x + button.X + button.Width + 128)
                    {
                        if (MouseManager.Y > y + button.Y + 100 && MouseManager.Y < y + button.Y + button.Height + 100)
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
            }

            if(TaskScheduler.counter == TaskScheduler.Apps.Count - 1)
            {
                KeyEvent key;
                if (KeyboardManager.TryReadKey(out key))
                {
                    customres = Keyboard.HandleKeyboard(customres, key);
                    once = true;
                }
            }

            if (MouseManager.MouseState == MouseState.None && clicked == true)
            {
                once = true;
                clicked = false;
            }

            ImprovedVBE.DrawImageAlpha(window, x, y, ImprovedVBE.cover);
        }

        public int x_1 = 0;
        public int y_1 = 0;

        public void DrawFilledEllipse(int xCenter, int yCenter, int yR, int xR, int color)
        {
            for (int y = -yR; y <= yR; y++)
            {
                for (int x = -xR; x <= xR; x++)
                {
                    if (x * x * yR * yR + y * y * xR * xR <= yR * yR * xR * xR)
                    {
                        if (xCenter + x > 0 && xCenter + x < width - 1 && yCenter + y > 0 && yCenter + y < height)
                        {
                            canvas.RawData[(yCenter + y) * width + xCenter + x] = color;
                        }
                    }
                }
            }
        }

        public void DrawFilledRectangle(int color, int X, int Y, int Width, int Height)
        {
            if (X < width && Y < height)
            {
                int[] line = new int[Width];
                if (X < 0)
                {
                    line = new int[Width + X];
                }
                else if (X + Width > width)
                {
                    line = new int[Width - (X + Width - width)];
                }
                Array.Fill(line, color);

                for (int i = Y; i < Y + Height; i++)
                {
                    Array.Copy(line, 0, canvas.RawData, i * width + X, line.Length);
                }
            }

        }
        public int GetGradientColor(int x, int y, int width, int height)
        {
            int r = (int)((double)x / width * 255);
            int g = (int)((double)y / height * 255);
            int b = 255;

            return ImprovedVBE.colourToNumber(r, g, b);
        }
        public void DrawGradientLeftToRight()
        {
            int gradientColorStart = GetGradientColor(0, 0, width, height);
            int gradientColorEnd = GetGradientColor(width, 0, width, height);

            int rStart = Color.FromArgb(gradientColorStart).R;
            int gStart = Color.FromArgb(gradientColorStart).G;
            int bStart = Color.FromArgb(gradientColorStart).B;

            int rEnd = Color.FromArgb(gradientColorEnd).R;
            int gEnd = Color.FromArgb(gradientColorEnd).G;
            int bEnd = Color.FromArgb(gradientColorEnd).B;

            for (int i = 0; i < canvas.RawData.Length; i++)
            {
                if (x_1 == width - 1)
                {
                    x_1 = 0;
                }
                else
                {
                    x_1++;
                }
                int r = (int)((double)x_1 / width * (rEnd - rStart)) + rStart;
                int g = (int)((double)x_1 / width * (gEnd - gStart)) + gStart;
                int b = (int)((double)x_1 / width * (bEnd - bStart)) + bStart;
                if (canvas.RawData[i] != 0)
                {
                    canvas.RawData[i] = ImprovedVBE.colourToNumber(r, g, b);
                }
                if (i / width > 20)
                {
                    break;
                }
            }
            x_1 = 0;
        }
    }
}
